using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Xml;
using NLog;
using XmlResolver.Catalog.Entry;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace XmlResolver.Loaders {
    public class XmlLoader : ICatalogLoader {
        private readonly object _syncLock = new object();
        protected static ResolverLogger logger = new ResolverLogger(LogManager.GetCurrentClassLogger());

        private static readonly HashSet<string> CATALOG_ELEMENTS
            = new() {"group", "public", "system", "rewriteSystem",
                "delegatePublic", "delegateSystem", "uri", "rewriteURI", "delegateURI",
                "nextCatalog", "uriSuffix", "systemSuffix"};

        private static readonly HashSet<string> TR9401_ELEMENTS
            = new() {"doctype", "document", "dtddecl", "entity", "linktype", "notation", "sgmldecl"};

        private readonly Dictionary<Uri,EntryCatalog> _catalogMap = new();
        private static XmlResolver _loaderResolver = null;

        private readonly Stack<Entry> _parserStack = new();
        private readonly Stack<bool> _preferPublicStack = new();
        private readonly Stack<Uri> _baseUriStack = new();

        private bool _preferPublic = true;
        private bool _archivedCatalogs = true;
        private EntryCatalog _catalog = null;
        private Locator _locator = null;

        public XmlResolver LoaderResolver {
            get
            {
                if (_loaderResolver == null) {
                    XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
                    config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
                    config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, false);
                    config.AddAssemblyCatalog("XmlResolver.catalog.xml", Assembly.GetExecutingAssembly());
                    _loaderResolver = new XmlResolver(config);
                }

                return _loaderResolver;
            }
        }
        
        public EntryCatalog LoadCatalog(Uri caturi) {
            lock (_syncLock) {
                if (_catalogMap.ContainsKey(caturi)) {
                    return _catalogMap[caturi];
                }

                Stream stream = null;
                try {
                    stream = ResourceAccess.GetStream(caturi);
                }
                catch (Exception ex) {
                    logger.Debug("Failed to load catalog {0}: {1}", caturi.ToString(), ex.Message);
                    _catalog = new EntryCatalog(caturi, null, false);
                    _catalogMap.Add(caturi, _catalog);
                    return _catalog;
                }

                return _LoadCatalog(caturi, stream);
            }
        }

        public EntryCatalog LoadCatalog(Uri caturi, Stream data) {
            lock (_syncLock) {
                if (_catalogMap.ContainsKey(caturi)) {
                    return _catalogMap[caturi];
                }

                return _LoadCatalog(caturi, data);
            }
        }

        private EntryCatalog _LoadCatalog(Uri caturi, Stream data) {
            return _LoadCatalog(caturi, caturi, data);
        }
        
        private EntryCatalog _LoadCatalog(Uri caturi, Uri baseUri, Stream data) {
            _catalog = null;
            _locator = null;
            _parserStack.Clear();
            _baseUriStack.Clear();
            _baseUriStack.Push(baseUri);
            _preferPublicStack.Push(_preferPublic);

            if (baseUri == null || caturi.Equals(baseUri)) {
                logger.Debug("Attempting to load catalog: {0}", caturi.ToString());
            }
            else {
                logger.Debug("Attempting to load catalog: {0} (with base URI: {1})", caturi.ToString(), baseUri.ToString());
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;
            settings.DtdProcessing = DtdProcessing.Ignore; // FIXME: ???
            settings.XmlResolver = LoaderResolver.GetXmlResolver();

            try {
                using (XmlReader reader = XmlReader.Create(data, settings)) {
                    while (reader.Read()) {
                        switch (reader.NodeType) {
                            case XmlNodeType.Element:
                                bool empty = reader.IsEmptyElement;
                                StartElement(reader);
                                if (empty) {
                                    EndElement(reader);
                                }

                                break;
                            case XmlNodeType.EndElement:
                                EndElement(reader);
                                break;
                            default:
                                break;
                        }
                    }
                }

                _catalogMap.Add(caturi, _catalog);
                return _catalog;
            }
            catch (Exception ex)  {
                logger.Debug("Failed to parse catalog {0}: {1}", caturi.ToString(), ex.Message);

                if (_archivedCatalogs) {
                    _catalog = ArchiveCatalog(caturi);
                    // It will already have been added to the map
                }
                else {
                    _catalog = new EntryCatalog(caturi, null, false);
                    _catalogMap.Add(caturi,_catalog);
                }
                
                return _catalog;
            }
        }

        private void StartElement(XmlReader reader) {
            if (_parserStack.Count == 0) {
                if (reader.NamespaceURI.Equals(ResolverConstants.CATALOG_NS)
                    && reader.LocalName.Equals("catalog")) {
                    string id = reader.GetAttribute("id", "");
                    string prefer = reader.GetAttribute("prefer", "");
                    if (prefer != null) {
                        _preferPublicStack.Push("public".Equals(prefer));
                        if (!"public".Equals(prefer) && !"system".Equals(prefer)) {
                            logger.Debug("Prefer on {0} is neither 'system' nor 'public': {1}",
                                    reader.LocalName, prefer);
                        }
                    }

                    _catalog = new EntryCatalog(_baseUriStack.Peek(), id, _preferPublicStack.Peek());
                    
                    if (reader is IXmlLineInfo && ((IXmlLineInfo) reader).HasLineInfo()) {
                        _locator = new Locator();
                        _catalog.SetLocator(_locator);
                    }

                    _parserStack.Push(_catalog);
                } else {
                    logger.Debug("Catalog document is not an XML Catalog (ignored): {0}", reader.Name);
                    _parserStack.Push(new EntryNull());
                }

                Uri baseUri = _baseUriStack.Peek();
                if (reader.GetAttribute("xml:base") != null) {
                    baseUri = UriUtils.Resolve(baseUri, reader.GetAttribute("xml:base"));
                }

                _baseUriStack.Push(baseUri);
                _preferPublicStack.Push(_preferPublicStack.Peek());
                return;
            }

            Entry top = _parserStack.Peek();
            if (top.GetEntryType() == Entry.EntryType.NULL) {
                PushNull();
            } else {
                if (ResolverConstants.CATALOG_NS.Equals(reader.NamespaceURI)) {
                    // Technically, the TR9401 extension elements should be in the TR9401 namespace,
                    // but I'm willing to bet lots of folks get that wrong. Be liberal in what mumble mumble...
                    if (CATALOG_ELEMENTS.Contains(reader.LocalName) || TR9401_ELEMENTS.Contains(reader.LocalName)) {
                        CatalogElement(reader);
                    } else {
                        logger.Debug("Unexpected catalog element (ignored): {0}", reader.LocalName);
                        PushNull();
                    }
                } else if (ResolverConstants.TR9401_NS.Equals(reader.NamespaceURI)) {
                    if (TR9401_ELEMENTS.Contains(reader.LocalName)) {
                        CatalogElement(reader);
                    } else {
                        logger.Debug("Unexpected catalog element (ignored): {0}", reader.LocalName);
                        PushNull();
                    }
                } else {
                    PushNull();
                }
            }
        }
        
        private void EndElement(XmlReader reader) {
            _parserStack.Pop();
            _baseUriStack.Pop();
            _preferPublicStack.Pop();
        }

        private void CatalogElement(XmlReader reader) {
            string id = reader.GetAttribute("id", "");
            string name = reader.GetAttribute("name", "");
            string uri = reader.GetAttribute("uri", "");
            string caturi = reader.GetAttribute("catalog", "");
            string start, prefix, suffix, publicId;

            Uri baseUri = _baseUriStack.Peek();
            if (reader.GetAttribute("xml:base") != null) {
                baseUri = UriUtils.Resolve(baseUri, reader.GetAttribute("xml:base"));
            }

            if (_locator != null) {
                IXmlLineInfo li = (IXmlLineInfo) reader;
                _locator.BaseUri = baseUri;
                _locator.LineNumber = li.LineNumber;
                _locator.LinePosition = li.LinePosition;
            }

            bool preferPublic = _preferPublicStack.Peek();

            Entry entry = null;

            switch (reader.LocalName) {
                case "group":
                    String prefer = reader.GetAttribute("prefer", "");
                    if (prefer != null) {
                        preferPublic = "public".Equals(prefer);
                        if (!"public".Equals(prefer) && !"system".Equals(prefer)) {
                            logger.Debug("Prefer on {0} is neither 'system' nor 'public': {1}",
                                reader.LocalName, prefer);
                        }
                    }

                    entry = _catalog.AddGroup(baseUri, id, preferPublic);
                    break;
                case "public":
                    // In XML, there will always be a system identifier.
                    publicId = PublicId.Normalize(reader.GetAttribute("publicId", ""));
                    entry = _catalog.AddPublic(baseUri, id, publicId, uri, preferPublic);
                    break;
                case "system":
                    String systemId = reader.GetAttribute("systemId", "");
                    entry = _catalog.AddSystem(baseUri, id, systemId, uri);
                    break;
                case "rewriteSystem":
                    start = reader.GetAttribute("systemIdStartString", "");
                    prefix = reader.GetAttribute("rewritePrefix", "");
                    entry = _catalog.AddRewriteSystem(baseUri, id, start, prefix);
                    break;
                case "systemSuffix":
                    suffix = reader.GetAttribute("systemIdSuffix", "");
                    entry = _catalog.AddSystemSuffix(baseUri, id, suffix, uri);
                    break;
                case "delegatePublic":
                    start = PublicId.Normalize(reader.GetAttribute("publicIdStartString", ""));
                    entry = _catalog.AddDelegatePublic(baseUri, id, start, caturi, preferPublic);
                    break;
                case "delegateSystem":
                    start = reader.GetAttribute("systemIdStartString", "");
                    entry = _catalog.AddDelegateSystem(baseUri, id, start, caturi);
                    break;
                case "uri":
                    String nature = reader.GetAttribute("nature", ResolverConstants.RDDL_NS);
                    String purpose = reader.GetAttribute("purpose", ResolverConstants.RDDL_NS);
                    entry = _catalog.AddUri(baseUri, id, name, uri, nature, purpose);
                    break;
                case "uriSuffix":
                    suffix = reader.GetAttribute("uriSuffix", "");
                    entry = _catalog.AddUriSuffix(baseUri, id, suffix, uri);
                    break;
                case "rewriteURI":
                    start = reader.GetAttribute("uriStartString", "");
                    prefix = reader.GetAttribute("rewritePrefix", "");
                    entry = _catalog.AddRewriteUri(baseUri, id, start, prefix);
                    break;
                case "delegateURI":
                    start = reader.GetAttribute("uriStartString", "");
                    entry = _catalog.AddDelegateUri(baseUri, id, start, caturi);
                    break;
                case "nextCatalog":
                    entry = _catalog.AddNextCatalog(baseUri, id, caturi);
                    break;
                case "doctype":
                    entry = _catalog.AddDoctype(baseUri, id, name, uri);
                    break;
                case "document":
                    entry = _catalog.AddDocument(baseUri, id, uri);
                    break;
                case "dtddecl":
                    publicId = reader.GetAttribute("publicId", "");
                    entry = _catalog.AddDtdDecl(baseUri, id, publicId, uri);
                    break;
                case "entity":
                    entry = _catalog.AddEntity(baseUri, id, name, uri);
                    break;
                case "linktype":
                    entry = _catalog.AddLinktype(baseUri, id, name, uri);
                    break;
                case "notation":
                    entry = _catalog.AddNotation(baseUri, id, name, uri);
                    break;
                case "sgmldecl":
                    entry = _catalog.AddSgmlDecl(baseUri, id, uri);
                    break;
                default:
                    // This shouldn't happen!
                    break;
            }

            if (reader.HasAttributes) {
                for (var pos = 0; pos < reader.AttributeCount; pos++) {
                    reader.MoveToAttribute(pos);
                    if (ResolverConstants.XMLRESOURCE_EXT_NS.Equals(reader.NamespaceURI)) {
                        entry.SetProperty(reader.LocalName, reader.Value);
                    }
                }
            }

            if (entry == null) {
                entry = new EntryNull();
            }
            
            _parserStack.Push(entry);
            _baseUriStack.Push(baseUri);
            _preferPublicStack.Push(preferPublic);
        }
        
        private void PushNull() {
            _parserStack.Push(new EntryNull());
            _baseUriStack.Push(_baseUriStack.Peek());
            _preferPublicStack.Push(_preferPublicStack.Peek());
        }
        
        public void SetPreferPublic(bool prefer) {
            _preferPublic = prefer;
        }

        public bool GetPreferPublic() {
            return _preferPublic;
        }

        public void SetArchivedCatalogs(bool archived) {
            _archivedCatalogs = archived;
        }

        public bool GetArchivedCatalogs() {
            return _archivedCatalogs;
        }

        private EntryCatalog ArchiveCatalog(Uri caturi) {
            if (caturi.Scheme != "file") {
                // Only support file: URIs at the moment
                return new EntryCatalog(caturi, null, true);
            }

            string path = caturi.AbsolutePath;
            HashSet<string> catalogSet = new HashSet<string>();
            bool firstEntry = true;
            string leadingDir = null;

            try {
                ZipArchive zipRead = ZipFile.OpenRead(path);
                logger.Debug("Searching ZIP file for catalog: {0}", caturi.ToString());
                foreach (ZipArchiveEntry entry in zipRead.Entries) {
                    int pos = entry.FullName.IndexOf("/");
                    if (firstEntry) {
                        if (pos >= 0) {
                            leadingDir = entry.FullName.Substring(0, pos);
                        }

                        firstEntry = false;
                    }
                    else {
                        if (leadingDir != null) {
                            if (pos < 0 || !leadingDir.Equals(entry.FullName.Substring(0, pos))) {
                                leadingDir = null;
                            }
                        }
                    }

                    if (entry.FullName.EndsWith("catalog.xml")) {
                        catalogSet.Add(entry.FullName);
                    }
                }
            }
            catch (Exception ex) {
                logger.Debug("Failed to load catalog as a ZIP file: {0}: {1}", 
                    caturi.ToString(), ex.Message);
                return new EntryCatalog(caturi, null, true);
            }

            string catpath = null;
            if (leadingDir != null) {
                if (catalogSet.Contains(leadingDir + "/catalog.xml")) {
                    catpath = "/" + leadingDir + "/catalog.xml";
                }
                if (catalogSet.Contains(leadingDir + "/org/xmlresolver/catalog.xml")) {
                    catpath = "/" + leadingDir + "/org/xmlresolver/catalog.xml";
                }
            }
            else {
                if (catalogSet.Contains("catalog.xml")) {
                    catpath = "/catalog.xml";
                }
                if (catalogSet.Contains("org/xmlresolver/catalog.xml")) {
                    catpath = "/org/xmlresolver/catalog.xml";
                }
            }

            if (catpath != null) {
                string packuri = caturi.ToString();
                packuri = packuri.Replace(":", "%3A");
                packuri = packuri.Replace("/", ",");
                packuri = "pack://" + packuri + catpath;
                Stream s = ResourceAccess.GetStream(packuri);
                if (s != null) {
                    return _LoadCatalog(caturi, UriUtils.NewUri(packuri), s);
                }
            }
            else {
                logger.Debug("Did not find catalog in ZIP file");
            }
            
            return new EntryCatalog(caturi, null, true);
        }
    }
}