using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using NLog;
using Org.XmlResolver.Catalog.Entry;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Cache {
    public class ResourceCache : CatalogManager {
        public static readonly long DeleteWait = 60 * 60 * 24 * 7; // 1 week
        public static readonly long CacheSize = 1000;
        public static readonly long CacheSpace = 1024 * 1000 * 10; // 10mb
        public static readonly long MaxAge = -1;
        public static readonly string DefaultPattern = ".*";

        // The ^jar:file: and ^classpath: patterns are part of the Java implementation;
        // they're included here in case the cache is shared across different applications
        public static readonly string[] excludedPatterns = new string[]
            {"^file:", "^pack:", "^jar:file:", "^classpath:"};

        new protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

        private bool loaded = false;
        private string cacheDir = null;
        private string dataDir = null;
        private string entryDir = null;
        private string expiredDir = null;

        private readonly List<CacheInfo> cacheInfo;
        private CacheEntryCatalog catalog = null;
        private CacheInfo defaultCacheInfo;
        private string cacheVersion = null;

        private int xmlDepth = 0;
        private bool xmlControlFile = false;
        private long xmlDeleteWait = DeleteWait;
        private long xmlCacheSize = CacheSize;
        private long xmlCacheSpace = CacheSpace;
        private long xmlMaxAge = MaxAge;
        
        public ResourceCache(XmlResolverConfiguration config): base(config) {
            cacheInfo = new();
            defaultCacheInfo = new CacheInfo(DefaultPattern, true, DeleteWait, CacheSize, CacheSpace, MaxAge);

            string dir = (string) config.GetFeature(ResolverFeature.CACHE_DIRECTORY);
            if (dir == null) {
                if ((bool) config.GetFeature(ResolverFeature.CACHE_UNDER_HOME)) {
                    dir = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".xmlresolver.org/cache");
                }
            }

            if (dir == null) {
                return;
            }

            cacheDir = Path.GetFullPath(dir);
            try {
                logger.Log(ResolverLogger.CACHE, "Cache dir: {0}", cacheDir);
                if (!Directory.Exists(cacheDir)) {
                    Directory.CreateDirectory(cacheDir);
                }

                if (!Directory.Exists(cacheDir)) {
                    logger.Log(ResolverLogger.ERROR, "Cannot create cache directory: {0}", cacheDir);
                    cacheDir = null;
                }
            }
            catch (Exception) {
                logger.Log(ResolverLogger.ERROR, "Exception getting cache directory: {0}", cacheDir);
                cacheDir = null;
            }

            if (cacheDir == null) {
                return;
            }

            bool update = false;
            string control = Path.Combine(cacheDir, "control.xml");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;
            settings.DtdProcessing = DtdProcessing.Ignore;

            try {
                xmlDepth = 0;
                xmlControlFile = false;
                xmlDeleteWait = DeleteWait;
                xmlCacheSize = CacheSize;
                xmlCacheSpace = CacheSpace;
                xmlMaxAge = MaxAge;

                FileStream data = new FileStream(control, FileMode.Open);
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

                foreach (string pattern in excludedPatterns) {
                    CacheInfo info = GetCacheInfo(pattern);
                    if (info == null) {
                        update = true;
                        cacheInfo.Add(new CacheInfo(pattern, false));
                    }
                }
            }
            catch (FileNotFoundException) {
                update = true;
                foreach (string pattern in excludedPatterns) {
                    cacheInfo.Add(new CacheInfo(pattern, false));
                }
            }
            catch (Exception) {
                // nevermind
            }

            if (update) {
                UpdateCacheControlFile();
            }
        }
        
        private void StartElement(XmlReader reader) {
            if (xmlDepth == 0) {
                xmlControlFile = ResolverConstants.XMLRESOURCE_EXT_NS.Equals(reader.NamespaceURI) &&
                                 "cache-control".Equals(reader.LocalName);
                if (xmlControlFile) {
                    cacheVersion = reader.GetAttribute("version");
                    xmlDeleteWait = CacheParser.ParseTimeLong(reader.GetAttribute("delete-wait"), DeleteWait);
                    xmlCacheSize = CacheParser.ParseLong(reader.GetAttribute("size"), CacheSize);
                    xmlCacheSpace = CacheParser.ParseSizeLong(reader.GetAttribute("space"), CacheSpace);
                    xmlMaxAge = CacheParser.ParseTimeLong(reader.GetAttribute("max-age"), MaxAge);
                    defaultCacheInfo = new CacheInfo(DefaultPattern, true, xmlDeleteWait, xmlCacheSize, xmlCacheSpace, xmlMaxAge);
                }
            }

            if (xmlDepth == 1 && xmlControlFile && ResolverConstants.XMLRESOURCE_EXT_NS.Equals(reader.NamespaceURI)) {
                long deleteWait = CacheParser.ParseTimeLong(reader.GetAttribute("delete-wait"), xmlDeleteWait);
                long cacheSize = CacheParser.ParseLong(reader.GetAttribute("size"), xmlCacheSize);
                long cacheSpace = CacheParser.ParseSizeLong(reader.GetAttribute("space"), xmlCacheSpace);
                long maxAge = CacheParser.ParseTimeLong(reader.GetAttribute("max-age"), xmlMaxAge);
                string cacheRegex = reader.GetAttribute("uri");
                if (cacheRegex != null || !"".Equals(cacheRegex.Trim())) {
                    switch (reader.LocalName) {
                        case "cache":
                        case "no-cache":
                            CacheInfo info = new CacheInfo(cacheRegex, "cache".Equals(reader.LocalName), deleteWait, cacheSize, cacheSpace, maxAge);
                            cacheInfo.Add(info);
                            break;
                        default:
                            logger.Log(ResolverLogger.ERROR, "Unexpected element in cache control file: {0}", reader.LocalName);
                            break;
                    }
                }
            }
            
            xmlDepth++;
        }

        private void EndElement(XmlReader reader) {
            xmlDepth--;
        }

        private void UpdateCacheControlFile() {
            if (cacheDir == null) {
                return;
            }

            String control = Path.Combine(cacheDir, "control.xml");
            using (StreamWriter xml = new StreamWriter(control)) {
                xml.WriteLine("<cache-control version='2' xmlns='" + ResolverConstants.XMLRESOURCE_EXT_NS + "'>");
                foreach (CacheInfo info in cacheInfo) {
                    if (info.Cache) {
                        xml.Write("<cache ");
                    }
                    else {
                        xml.Write("<no-cache ");
                    }

                    xml.Write("uri='" + CacheEntryCatalog.XmlEscape(info.Pattern) + "'");
                    xml.Write(" delete-wait='" + info.DeleteWait + "'");
                    xml.Write(" size='" + info.CacheSize + "'");
                    xml.Write(" space='" + info.CacheSpace + "'");
                    xml.WriteLine(" max-age='" + info.MaxAge + "'/>");
                }

                xml.WriteLine("</cache-control>\n");
            }

        }

        public string GetDirectory() {
            return cacheDir;
        }

        public List<CacheInfo> GetCacheInfoList() {
            List<CacheInfo> lst = new();
            lst.AddRange(cacheInfo);
            return lst;
        }

        public CacheInfo GetCacheInfo(string pattern) {
            if (pattern == null) {
                return null;
            }

            foreach (CacheInfo info in cacheInfo) {
                if (pattern.Equals(info.Pattern)) {
                    return info;
                }
            }

            return null;
        }

        public CacheInfo GetDefaultCacheInfo() {
            return defaultCacheInfo;
        }

        public CacheInfo AddCacheInfo(string pattern, bool cache) {
            return AddCacheInfo(pattern, cache, xmlDeleteWait, xmlCacheSize, xmlCacheSpace, xmlMaxAge);
        }

        public CacheInfo AddCacheInfo(string pattern, bool cache, long deleteWait, long cacheSize, long cacheSpace,
            long maxAge) {
            CacheInfo info = new CacheInfo(pattern, cache, deleteWait, cacheSize, cacheSpace, maxAge);
            RemoveCacheInfo(pattern, false);
            cacheInfo.Add(info);
            UpdateCacheControlFile();
            return info;
        }

        public void RemoveCacheInfo(string pattern) {
            RemoveCacheInfo(pattern, true);
        }
        
        public void RemoveCacheInfo(string pattern, bool writeUpdate) {
            CacheInfo info = GetCacheInfo(pattern);
            while (info != null) {
                cacheInfo.Remove(info);
                info = GetCacheInfo(pattern);
            }

            if (writeUpdate) {
                UpdateCacheControlFile();
            }
        }

        public List<CacheEntry> GetEntries() {
            loadCache();
            return catalog.cached;
        }

        public bool Expired(Uri local) {
            if (local == null) {
                return false;
            }

            string offline = Environment.GetEnvironmentVariable("XMLRESOLVER_OFFLINE");
            if (offline != null && !"false".Equals(offline) && !"0".Equals(offline) && !"no".Equals(offline)) {
                return false;
            }

            loadCache();
            if (cacheDir == null) {
                return true;
            }

            CacheEntry entry = null;
            foreach (CacheEntry search in catalog.cached) {
                if (local.Equals(search.LocalFileUri)) {
                    entry = search;
                    break;
                }
            }

            if (entry == null) {
                // The URI isn't in the cache? This should never happen, but if it does,
                // the entry is definitely expired!
                return true;
            }

            CacheInfo info = null;
            for (int count = 0; info == null && count < cacheInfo.Count; count++) {
                CacheInfo chk = cacheInfo[count];
                if (chk.UriPattern.IsMatch(entry.CacheUri.ToString())) {
                    info = chk;
                }
            }

            if (info == null) {
                info = defaultCacheInfo;
            }

            // If this entry isn't supposed to be cached, then it's definitely expired.
            if (!info.Cache) {
                // Cleanup the cache if someone changed a pattern
                CacheInfo cleanup = new CacheInfo(info.Pattern, false, DeleteWait, 0, 0, 0);
                flushCache(cleanup);
                return true;
            }

            long cacheCount = 0;
            long cacheSize = 0;
            foreach (CacheEntry search in catalog.cached) {
                if (search.CacheCatalogEntry.GetEntryType() == Entry.EntryType.PUBLIC || search.Expired) {
                    // If it's public, it must also have a system entry, don't count it twice.
                    // If we already expired it last time around, don't bother expiring it again.
                    continue;
                }

                if (info.UriPattern.IsMatch(search.CacheUri.ToString())) {
                    cacheCount++;
                    cacheSize += search.CacheFile.Length;
                }
            }

            if (cacheCount > info.CacheSize || cacheSize > info.CacheSpace) {
                logger.Log(ResolverLogger.CACHE,
                    "Too many cache entries, or cache size too large: expiring oldest entries");
                flushCache(info);
            }

            long cacheTime = entry.Time;
            String cachedEtag = entry.ETag();

            ResourceConnection conn = new ResourceConnection(entry.CacheUri.ToString(), true);
            conn.Close();
            string etag = conn.ETag;
            long lastModified = conn.LastModified;
            if ("".Equals(etag)) {
                etag = null;
            }

            if (lastModified < 0 && (etag == null || cachedEtag == null)) {
                // Hmm. We're not sure when it was last modified?
                long maxAge = info.MaxAge;
                if (maxAge > 0) {
                    long oldest = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeMilliseconds() - (maxAge * 1000);
                    if (maxAge == 0 || cacheTime < oldest) {
                        return true;
                    }
                }

                lastModified = conn.Date;
            }

            if (conn.StatusCode != 200) {
                logger.Log(ResolverLogger.CACHE, "Not expired: {0} (HTTP {1})",
                    entry.CacheUri.ToString(), conn.StatusCode.ToString());
                return false;
            }

            bool etagsDiffer = (etag != null && cachedEtag != null && !etag.Equals(cachedEtag));

            if (lastModified < 0) {
                if (etagsDiffer) {
                    logger.Log(ResolverLogger.CACHE, "Expired: {0} (no last-modified header, etags differ)",
                        entry.CacheUri.ToString());
                    return true;
                }
                logger.Log(ResolverLogger.CACHE, "Not expired: {0}", entry.CacheUri.ToString());
                return false;
            }
            
            if (lastModified > cacheTime || etagsDiffer) {
                logger.Log(ResolverLogger.CACHE, "Expired: {0}",
                    entry.CacheUri.ToString());
                return true;
            }
            
            logger.Log(ResolverLogger.CACHE, "Not expired: {0}", entry.CacheUri.ToString());
            return false;
        }

        public bool CacheUri(string uri) {
            loadCache();
            if (cacheDir == null) {
                return false;
            }

            uri = UriUtils.Resolve(UriUtils.Cwd(), uri).ToString();
            
            CacheInfo info = null;
            for (int count = 0; info == null && count < cacheInfo.Count; count++) {
                CacheInfo chk = cacheInfo[count];
                if (chk.UriPattern.IsMatch(uri)) {
                    info = chk;
                }
            }

            if (info == null) {
                info = defaultCacheInfo;
            }
            
            logger.Log(ResolverLogger.CACHE, "Cache cacheURI: {0} ({1})", info.Cache.ToString(), uri);

            return info.Cache;
        }

        public override Uri LookupUri(string uri) {
            loadCache();
            if (cacheDir == null) {
                return null;
            }

            Uri local = base.LookupUri(uri);
            return Expired(local) ? null : local;
        }

        public override Uri LookupNamespaceUri(string uri, string nature, string purpose) {
            loadCache();
            if (cacheDir == null) {
                return null;
            }

            Uri local = base.LookupNamespaceUri(uri, nature, purpose);
            return Expired(local) ? null : local;
        }
        
        public override Uri LookupPublic(string systemId, string publicId) {
            loadCache();
            if (cacheDir == null) {
                return null;
            }

            Uri local = base.LookupPublic(systemId, publicId);
            return Expired(local) ? null : local;
        }

        public override Uri LookupSystem(string systemId) {
            loadCache();
            if (cacheDir == null) {
                return null;
            }

            Uri local = base.LookupSystem(systemId);
            return Expired(local) ? null : local;
        }

        public CacheEntry CachedUri(Uri uri) {
            return CachedNamespaceUri(uri, null, null);
        }

        public CacheEntry CachedNamespaceUri(Uri uri, string nature, string purpose) {
            CacheEntry entry = FindNamespaceCacheEntry(uri.ToString(), nature, purpose);
            if (entry == null || entry.Expired) {
                if (CacheUri(uri.ToString())) {
                    ResourceConnection conn = new ResourceConnection(uri.ToString());
                    if (conn.Stream != null && conn.StatusCode == 200) {
                        entry = AddNamespaceUri(conn, nature, purpose);
                    }
                }
                return null;
            }

            return entry;
        }

        public CacheEntry CachedSystem(Uri systemId, string publicId) {
            if (systemId == null) {
                return null;
            }

            CacheEntry entry = FindSystemCacheEntry(systemId.ToString());
            if (entry == null || entry.Expired) {
                if (CacheUri(systemId.ToString())) {
                    ResourceConnection conn = new ResourceConnection(systemId.ToString());
                    if (conn.Stream != null && conn.StatusCode == 200) {
                        entry = AddSystem(conn);
                    }
                }
                return null;
            }

            return entry;
        }

        private void loadCache() {
            if (loaded) {
                return;
            }

            loaded = true;

            if (cacheDir == null) {
                return;
            }

            catalog = new CacheEntryCatalog(UriUtils.Resolve(UriUtils.Cwd(), cacheDir), null, true);
            dataDir = Path.Combine(cacheDir, "data");
            entryDir = Path.Combine(cacheDir, "entry");
            expiredDir = Path.Combine(cacheDir, "expired");

            if (!Directory.Exists(dataDir)) {
                Directory.CreateDirectory(dataDir);
            }
            if (!Directory.Exists(entryDir)) {
                Directory.CreateDirectory(entryDir);
            }
            if (!Directory.Exists(expiredDir)) {
                Directory.CreateDirectory(expiredDir);
            }

            if (!Directory.Exists(dataDir) || !Directory.Exists(entryDir) || !Directory.Exists(expiredDir)) {
                logger.Log(ResolverLogger.CACHE, "Failed to setup data, entry, and expired directories in cache");
                entryDir = null;
                return;
            }

            DirectoryLock clock = new (cacheDir);
            cleanupCache();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;
            settings.DtdProcessing = DtdProcessing.Ignore;

            string[] entryfiles = Directory.GetFiles(entryDir, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (string entryfn in entryfiles) {
                FileStream data = new FileStream(Path.Combine(entryDir, entryfn), FileMode.Open);
                bool root = true;
                try {
                    using (XmlReader reader = XmlReader.Create(data, settings)) {
                        while (reader.Read()) {
                            switch (reader.NodeType) {
                                case XmlNodeType.Element:
                                    root = root && ResolverConstants.CATALOG_NS.Equals(reader.NamespaceURI);
                                    if (root) {
                                        long timestamp = -1;
                                        Dictionary<string, string> props = new();
                                        string name = reader.GetAttribute("name");
                                        string uri = reader.GetAttribute("uri");
                                        Uri baseUri = UriUtils.Resolve(UriUtils.Cwd(), entryfn);
                                        string xmlbase = reader.GetAttribute("xml:base");
                                        if (xmlbase != null) {
                                            baseUri = UriUtils.Resolve(baseUri, xmlbase);
                                        }

                                        Entry entry = null;
                                        string localName = reader.LocalName;
                                        switch (localName) {
                                            case "uri":
                                                string nature = reader.GetAttribute("nature");
                                                string purpose = reader.GetAttribute("purpose");
                                                timestamp = CacheTimestamp(reader, out props);
                                                entry = catalog.AddUri(baseUri, name, uri, nature, purpose, timestamp);
                                                break;
                                            case "system":
                                                string systemId = reader.GetAttribute("systemId");
                                                timestamp = CacheTimestamp(reader, out props);
                                                entry = catalog.AddSystem(baseUri, systemId, uri, timestamp);
                                                break;
                                            case "public":
                                                String publicId = reader.GetAttribute("publicId");
                                                timestamp = CacheTimestamp(reader, out props);
                                                entry = catalog.AddPublic(baseUri, publicId, uri, timestamp);
                                                break;
                                            default:
                                                logger.Log(ResolverLogger.CACHE, "Unexpected cache entry: {0}",
                                                    localName);
                                                break;
                                        }

                                        foreach (string pkey in props.Keys) {
                                            entry.SetProperty(pkey, props[pkey]);
                                        }

                                        entry.SetProperty("filesize", "FIXME:");
                                        entry.SetProperty("filemodified", "FIXME:");
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                catch (Exception) {
                    logger.Log(ResolverLogger.CONFIG, "Failed to read cache: {0}", Path.Combine(entryDir, entryfn));
                }
            }
            
            clock.Release();
        }

        private long CacheTimestamp(XmlReader reader, out Dictionary<string,string> props) {
            props = new Dictionary<string, string>();
            long ts = -1;

            if (reader.HasAttributes) {
                for (var pos = 0; pos < reader.AttributeCount; pos++) {
                    reader.MoveToAttribute(pos);
                    if (ResolverConstants.XMLRESOURCE_EXT_NS.Equals(reader.NamespaceURI)) {
                        if ("time".Equals(reader.LocalName)) {
                            try {
                                ts = long.Parse(reader.Value);
                            }
                            catch (Exception) {
                                logger.Log(ResolverLogger.ERROR, "Bad numeric value in cache file: {0}", reader.Value);
                                ts = -1;
                            }
                        }
                        else {
                            props.Add(reader.LocalName, reader.Value);
                        }
                    }
                }
            }

            return ts;
        }
        
        private void cleanupCache() {
            long now = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeMilliseconds();
            long threshold = 24 * 3600 * 1000; // Cleanup once a day
            long age = 0;

            string cleanupFn = Path.Combine(cacheDir, "cleanup");
            FileInfo cleanup = new FileInfo(cleanupFn);
            if (cleanup.Exists) {
                age = now - ((DateTimeOffset) cleanup.LastWriteTimeUtc).ToUnixTimeMilliseconds();
            }
            else {
                using (StreamWriter text = new StreamWriter(cleanup.FullName)) {
                    text.WriteLine("The timestamp on this file indicates when the cache was last pruned.");
                }

                age = threshold + 1;
            }

            if (age > threshold) {
                logger.Log(ResolverLogger.CACHE, "Cleaning up expired cache entries");
                
                // If there are any expired entries that are too old, remove them
                string[] entryfiles = Directory.GetFiles(entryDir, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string fn in entryfiles) {
                    FileInfo info = new(fn);
                    age = now - ((DateTimeOffset) info.LastWriteTimeUtc).ToUnixTimeMilliseconds();
                    if (age > DeleteWait * 1000) {
                        logger.Log(ResolverLogger.CACHE, "Deleting expired entry: {0}", fn);
                        File.Delete(fn);
                    }
                }
                
                // If there are any files in the data dir that don't have an entry, remove them
                entryfiles = Directory.GetFiles(entryDir, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string fn in entryfiles) {
                    string basename = fn.Substring(0, fn.LastIndexOf("."));
                    bool found = false;
                    string[] dfiles = Directory.GetFiles(dataDir, "*.*", SearchOption.TopDirectoryOnly);
                    foreach (string dfn in dfiles) {
                        string dataname = dfn.Substring(0, dfn.LastIndexOf("."));
                        found = dataname.Equals(basename);
                    }

                    if (!found) {
                        logger.Log(ResolverLogger.CACHE, "Deleting dangling entry: {0}", fn);
                        File.Delete(fn);
                    }
                }
            }
            
            File.SetLastWriteTime(cleanupFn, DateTime.Now);
        }
        
        private CacheEntry AddNamespaceUri(ResourceConnection connection, string nature, string purpose) {
            loadCache();
            if (cacheDir == null) {
                logger.Log(ResolverLogger.CACHE, "Attempting to cache URI, but no cache is available");
                return null;
            }

            DirectoryLock dlock = new DirectoryLock(cacheDir);
            try {
                if (nature == null && purpose == null) {
                    logger.Log(ResolverLogger.CACHE, "Caching resource for uri: {0}", connection.Uri.ToString());
                }
                else {
                    logger.Log(ResolverLogger.CACHE, "Caching resource for uri: {0} (nature: {1}, purpose: {2})",
                        connection.Uri.ToString(), nature, purpose);
                }

                string basefn = cacheBaseName(connection.Uri);
                string entryFile = Path.Combine(entryDir, basefn + ".xml");
                string localFile = Path.Combine(dataDir, basefn + pickSuffix(connection.Uri, connection.ContentType));
                StoreStream(connection.Stream, localFile);
                connection.Close();

                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Entry entry = catalog.AddUri(UriUtils.NewUri(entryFile), connection.Uri.ToString(), localFile, nature, purpose,
                    now);
                entry.SetProperty("contentType", connection.ContentType);
                entry.SetProperty("time", now.ToString());
                // FIXME: what about redirections?
                if (connection.ETag != null) {
                    entry.SetProperty("etag", connection.ETag);
                }

                FileInfo info = new FileInfo(localFile);
                entry.SetProperty("filesize", info.Length.ToString());
                entry.SetProperty("filemodified",
                    ((DateTimeOffset) info.LastWriteTime).ToUnixTimeMilliseconds().ToString());
                catalog.WriteCacheEntry(entry, entryFile);
            }
            finally {
                dlock.Release();
            }

            return FindNamespaceCacheEntry(connection.Uri.ToString(), nature, purpose);
        }

        private CacheEntry AddSystem(ResourceConnection connection) {
            loadCache();
            if (cacheDir == null) {
                logger.Log(ResolverLogger.CACHE, "Attempting to cache URI, but no cache is available");
                return null;
            }

            DirectoryLock dlock = new DirectoryLock(cacheDir);
            try {
                logger.Log(ResolverLogger.CACHE, "Caching systemId: %s", connection.Uri.ToString());
                
                string basefn = cacheBaseName(connection.Uri);
                string entryFile = Path.Combine(entryDir, basefn + ".xml");
                string localFile = Path.Combine(dataDir, basefn + pickSuffix(connection.Uri, connection.ContentType));
                StoreStream(connection.Stream, localFile);
                connection.Close();

                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Entry entry = catalog.AddSystem(UriUtils.NewUri(entryFile), connection.Uri.ToString(), localFile, now);
                entry.SetProperty("contentType", connection.ContentType);
                entry.SetProperty("time", now.ToString());
                // FIXME: what about redirections?
                if (connection.ETag != null) {
                    entry.SetProperty("etag", connection.ETag);
                }

                FileInfo info = new FileInfo(localFile);
                entry.SetProperty("filesize", info.Length.ToString());
                entry.SetProperty("filemodified",
                    ((DateTimeOffset) info.LastWriteTime).ToUnixTimeMilliseconds().ToString());
                catalog.WriteCacheEntry(entry, entryFile);
            }
            finally {
                dlock.Release();
            }

            return FindSystemCacheEntry(connection.Uri.ToString());
        }

        private string cacheBaseName(Uri name) {
            using (SHA256 sha256Hash = SHA256.Create()) {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(name.ToString()));  
                StringBuilder builder = new StringBuilder();  
                for (int pos = 0; pos < bytes.Length; pos++) {  
                    builder.Append(bytes[pos].ToString("x2"));  
                }  
                return builder.ToString(); 
            }
        }

        private string pickSuffix(Uri uri, string contentType) {
            string suffix = uri.ToString();
            int pos = suffix.LastIndexOf(".");
            if (pos > 0) {
                suffix = suffix.Substring(pos);
                if (suffix.Length <= 5) {
                    return suffix;
                }
            }

            if (contentType == null) {
                return ".bin";
            }
            
            if ("application/xml-dtd".Equals(contentType)) {
                return ".dtd";
            }

            if (contentType.Contains("application/xml")) {
                return ".xml";
            }

            if (contentType.Contains("text/html") || contentType.Contains("application/html+xml")) {
                return ".html";
            }

            if (contentType.Contains("text/plain")) {
                if (uri.ToString().EndsWith(".dtd")) {
                    return ".dtd";
                }
                return ".txt";
            }

            return ".bin";
        }

        private void StoreStream(Stream stream, string filename) {
            using (Stream output = File.OpenWrite(filename)) {
                stream.CopyTo(output);
            }
        }
        
        private CacheEntry FindNamespaceCacheEntry(string uri, string nature, string purpose) {
            if (uri == null) {
                return null;
            }
            
            loadCache();
            if (cacheDir == null) {
                return null;
            }
            
            // Find the entry for this uri
            foreach (CacheEntry search in catalog.cached) {
                if (search.CacheCatalogEntry.GetEntryType() == Entry.EntryType.URI &&
                    uri.Equals(search.CacheUri.ToString())) {
                    EntryUri entry = (EntryUri) search.CacheCatalogEntry;
                    if ((nature == null || entry.Nature == null || nature.Equals(entry.Nature))
                        && (purpose == null || entry.Purpose == null || purpose.Equals(entry.Purpose))) {
                        return search;
                    }
                }
                
            }

            return null;
        }

        private CacheEntry FindSystemCacheEntry(string systemId) {
            if (systemId == null) {
                return null;
            }
            
            loadCache();
            if (cacheDir == null) {
                return null;
            }
            
            // Find the entry for this uri
            foreach (CacheEntry search in catalog.cached) {
                if (search.CacheCatalogEntry.GetEntryType() == Entry.EntryType.SYSTEM &&
                    systemId.Equals(search.CacheUri.ToString())) {
                    return search;
                }
                
            }

            return null;
        }

        private void flushCache(CacheInfo info) {
            DirectoryLock dlock = new DirectoryLock(cacheDir);
            catalog.FlushCache(info.UriPattern, info.CacheSize, info.CacheSpace, expiredDir);
            dlock.Release();
        }
    }
}