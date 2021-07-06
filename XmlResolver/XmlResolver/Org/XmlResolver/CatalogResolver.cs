using System;
using System.IO;
using System.Net;
using System.Text;
using NLog;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver {
    public class CatalogResolver : ResourceResolver {
        protected static readonly ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        private XmlResolverConfiguration config;
        private ResourceCache cache;

        public CatalogResolver() : this(new XmlResolverConfiguration()) {
            // nop   
        }

        public CatalogResolver(XmlResolverConfiguration config) {
            this.config = config;
            cache = (ResourceCache) config.GetFeature(ResolverFeature.CACHE);
        }

        private ResolvedResourceImpl Resource(string requestUri, Uri responseUri, CacheEntry cached) {
            if (cached == null) {
                return UncachedResource(new Uri(requestUri), responseUri);
            }
            else {
                var fs = File.Open(cached.CacheFile, FileMode.Open, FileAccess.Read);
                return new ResolvedResourceImpl(responseUri, new Uri(cached.CacheFile), fs, cached.ContentType());
            }
        }

        private ResolvedResourceImpl UncachedResource(Uri req, Uri res) {
            bool mask = (bool) config.GetFeature(ResolverFeature.MASK_PACK_URIS);
            Uri showResolvedUri = res;
            if (mask && "pack".Equals(showResolvedUri.Scheme)) {
                showResolvedUri = req;
            }

            if ("data".Equals(res.Scheme)) {
                // This is a little bit crude; see RFC 2397

                String contentType = null;
                // Can't use URI accessors because they percent decode the string incorrectly.
                String path = res.ToString().Substring(5);
                int pos = path.IndexOf(",");
                if (pos >= 0) {
                    String mediatype = path.Substring(0, pos);
                    if (mediatype.EndsWith(";base64")) {
                        contentType = mediatype.Substring(0, mediatype.Length - 7);
                    }
                    else {
                        String charset = "UTF-8";
                        pos = mediatype.IndexOf(";charset=");
                        if (pos > 0) {
                            charset = mediatype.Substring(pos + 9);
                            pos = charset.IndexOf(";");
                            if (pos >= 0) {
                                charset = charset.Substring(0, pos);
                            }
                        }

                        contentType = "".Equals(mediatype) ? null : mediatype;
                    }
                }

                return new ResolvedResourceImpl(showResolvedUri, res, UriUtils.GetStream(res), contentType);
            }

            return new ResolvedResourceImpl(showResolvedUri, res, UriUtils.GetStream(res), null);
        }
        
        public ResolvedResource ResolveUri(string href, string baseUri) {
            logger.Log(ResolverLogger.REQUEST, "ResolveUri: {0} (base URI: {1})", href, baseUri);

            if (href == null || "".Equals(href.Trim())) {
                href = baseUri;
                baseUri = null;
                if (href == null || "".Equals(href.Trim())) {
                    logger.Log(ResolverLogger.RESPONSE, "ResolveUri: null");
                    return null;
                }
            }

            CatalogManager catalog = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            Uri resolved = catalog.LookupUri(href);
            if (resolved != null) {
                logger.Log(ResolverLogger.RESPONSE, "ResolveUri: {0}", resolved.ToString());
                return Resource(href, resolved, cache.CachedUri(resolved));
            }

            string absolute = href;
            if (baseUri != null) {
                absolute = UriUtils.Resolve(new Uri(baseUri), href).ToString();
                if (!href.Equals(absolute)) {
                    resolved = catalog.LookupUri(absolute);
                    if (resolved != null) {
                        logger.Log(ResolverLogger.RESPONSE, "ResolveUri: {0}", resolved.ToString());
                        return Resource(absolute, resolved, cache.CachedUri(resolved));
                    }
                }
            }

            if (cache.CacheUri(absolute)) {
                Uri absuri = new Uri(absolute);
                logger.Log(ResolverLogger.RESPONSE, "ResolveUri: cached: {0}", absolute);
                return Resource(absolute, absuri, cache.CachedUri(absuri));
            }
            else {
                logger.Log(ResolverLogger.RESPONSE, "ResolvedUri: null");
                return null;
            }
        }

        public ResolvedResource ResolveNamespace(string href, string baseUri, string nature, string purpose) {
            throw new NotImplementedException();
        }

        public ResolvedResource ResolveEntity(string name, string publicId, string systemId, string baseUri) {
            if (name == null && publicId == null && systemId == null && baseUri == null) {
                logger.Log(ResolverLogger.REQUEST, "ResolveEntity: null");
                return null;
            }

            if (name != null && publicId == null && systemId == null) {
                logger.Log(ResolverLogger.REQUEST, "ResolveEntity: name: {0} ({1})", name, baseUri);
                return ResolveDoctype(name, baseUri);
            }

            if (name != null) {
                logger.Log(ResolverLogger.REQUEST, "resolveEntity: {0} {1} (baseURI: {2}, publicId: {3})",
                    name, systemId, baseUri, publicId);
            } else {
                logger.Log(ResolverLogger.REQUEST, "resolveEntity: {0} (baseURI: {1}, publicId: {2})",
                    systemId, baseUri, publicId);
            }
            
            Uri absSystem = null;

            CatalogManager catalog = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            ResolvedResourceImpl result = null;
            Uri resolved = catalog.LookupEntity(name, systemId, publicId);
            if (resolved == null && systemId != null && (bool) config.GetFeature(ResolverFeature.URI_FOR_SYSTEM)) {
                resolved = catalog.LookupUri(systemId);
            }
            if (resolved != null) {
                result = Resource(systemId, resolved, cache.CachedSystem(resolved, publicId));
            } else {
                if (systemId != null) {
                    absSystem = new Uri(systemId);
                }
                if (baseUri != null) {
                    absSystem = new Uri(baseUri);
                    if (systemId != null) {
                        absSystem = UriUtils.Resolve(absSystem, systemId);
                    }
                    resolved = catalog.LookupEntity(name, absSystem.ToString(), publicId);
                    if (resolved == null && (bool) config.GetFeature(ResolverFeature.URI_FOR_SYSTEM)) {
                        resolved = catalog.LookupUri(absSystem.ToString());
                    }
                    if (resolved != null) {
                        result = Resource(absSystem.ToString(), resolved, cache.CachedSystem(resolved, publicId));
                    }
                }
            }

            if (result != null) {
                logger.Log(ResolverLogger.RESPONSE, "resolveEntity: {0}", resolved.ToString());
                return result;
            }

            if (absSystem == null) {
                logger.Log(ResolverLogger.RESPONSE, "resolveEntity: null");
                return null;
            }

            if (cache.CacheUri(absSystem.ToString())) {
                logger.Log(ResolverLogger.RESPONSE, "resolveEntity: cached {0}", absSystem.ToString());
                return Resource(absSystem.ToString(), absSystem, cache.CachedSystem(absSystem, publicId));
            }

            logger.Log(ResolverLogger.RESPONSE, "resolveEntity: null");
            return null;
            
        }

        private ResolvedResource ResolveDoctype(string name, string baseUri) {
            logger.Log(ResolverLogger.REQUEST, "ResolveDoctype: {0}", name);
            CatalogManager catalog = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            Uri resolved = catalog.LookupDoctype(name, null, null);
            if (resolved == null) {
                logger.Log(ResolverLogger.RESPONSE, "resolveDoctype: null");
                return null;
            } else {
                logger.Log(ResolverLogger.RESPONSE, "resolveDoctype: {0}", resolved.ToString());
                ResolvedResourceImpl result = Resource(null, resolved, cache.CachedSystem(resolved, null));
                return result;
            }
        }
        
        public ResolverConfiguration GetConfiguration() {
            return config;
        }
    }
}