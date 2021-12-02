using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Org.XmlResolver.Catalog.Entry;
using Org.XmlResolver.Catalog.Query;
using Org.XmlResolver.Features;
using Org.XmlResolver.Loaders;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver {
    public class CatalogManager : IXmlCatalogResolver {
        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        protected readonly ResolverConfiguration _resolverConfiguration;
        protected CatalogLoader _catalogLoader;

        public CatalogManager(ResolverConfiguration config) {
            _resolverConfiguration = config;
            String loaderClassName = (String) config.GetFeature(ResolverFeature.CATALOG_LOADER_CLASS);
            if (loaderClassName == null || "".Equals(loaderClassName.Trim())) {
                loaderClassName = ResolverFeature.CATALOG_LOADER_CLASS.GetDefaultValue();
            }

            // FIXME: what if this doesn't work?
            _catalogLoader = (CatalogLoader) Activator.CreateInstance(Type.GetType(loaderClassName));
        }

        public CatalogManager(CatalogManager current, ResolverConfiguration config) {
            _catalogLoader = current._catalogLoader;
            _resolverConfiguration = config;
        }

        public ResolverConfiguration GetResolverConfiguration() => _resolverConfiguration;

        public List<Uri> Catalogs() {
            List<Uri> catlist = new();
            foreach (var cat in (List<string>) _resolverConfiguration.GetFeature(ResolverFeature.CATALOG_FILES)) {
                catlist.Add(new Uri(UriUtils.Cwd(), cat));
            }
            return catlist;
        }

        public EntryCatalog LoadCatalog(Uri catalog) {
            EntryCatalog cat = _catalogLoader.LoadCatalog(catalog);
            return cat;
        }
        
        public EntryCatalog LoadCatalog(Uri catalog, Stream data) {
            return _catalogLoader.LoadCatalog(catalog, data);
        }
        
        public virtual Uri LookupUri(string uri) {
            return LookupNamespaceUri(uri, null, null);
        }

        public virtual Uri LookupNamespaceUri(string uri, string nature, string purpose) {
            return new QueryUri(uri, nature, purpose).Search(this).ResultUri();
        }
        
        public virtual Uri LookupPublic(string systemId, string publicId) {
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryPublic(external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        public virtual Uri LookupSystem(string systemId) {
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, null);
            if (external.SystemId == null) {
                return null;
            }

            return new QuerySystem(systemId).Search(this).ResultUri();
        }

        public virtual Uri LookupDoctype(string entityName, string systemId, string publicId) {
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryDoctype(entityName, external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        public virtual Uri LookupEntity(string entityName, string systemId, string publicId) {
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryEntity(entityName, external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        public Uri LookupNotation(string notationName, string systemId, string publicId) {
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryNotation(notationName, external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        public virtual Uri LookupDocument() {
            return new QueryDocument().Search(this).ResultUri();
        }

        public string NormalizedForComparison(string uri) {
            if (uri != null) {
                if (uri.StartsWith("classpath:/")) {
                    return "classpath:" + uri.Substring(11);
                }

                if (((bool) _resolverConfiguration.GetFeature(ResolverFeature.MERGE_HTTPS)) && uri.StartsWith("http:")) {
                    return "https:" + uri.Substring(5);
                }
            }

            return uri;
        }
        
        private class ExternalIdentifiers {
            public readonly String SystemId;
            public readonly String PublicId;
            public ExternalIdentifiers(String systemId, String publicId) {
                SystemId = systemId;
                PublicId = publicId;
            }
        }

        private static ExternalIdentifiers NormalizeExternalIdentifiers(String systemId, String publicId) {
            if (systemId != null) {
                systemId = UriUtils.NormalizeUri(systemId);
            }

            if (publicId != null && publicId.StartsWith("urn:publicid:")) {
                publicId = PublicId.DecodeUrn(publicId);
            }

            if (systemId != null && systemId.StartsWith("urn:publicid:")) {
                String decodedSysId = PublicId.DecodeUrn(systemId);
                if (publicId != null && !publicId.Equals(decodedSysId)) {
                    logger.Log(ResolverLogger.ERROR, "urn:publicid: system identifier differs from public identifier; using public identifier");
                } else {
                    publicId = decodedSysId;
                }
                systemId = null;
            }

            return new ExternalIdentifiers(systemId, publicId);
        }
    }
}