using System;
using System.IO;
using NLog;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver {
    /// <summary>
    /// The CatalogResolver extends the functionality of the CatalogManager. It looks up
    /// resources in the catalog(s) and returns a description of what was found, if the resource
    /// was successfully resolved.
    /// </summary>
    /// <para>The CatalogResolver also handles the caching layer, if caching is enabled.</para>
    ///
    /// <para>This class is a utility class used by the primary public API, the Resolver.</para>

    public class CatalogResolver : IResourceResolver {
        protected static readonly ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        private XmlResolverConfiguration config;

        /// <summary>
        /// Create a new resolver using a default configuration.
        /// </summary>
        public CatalogResolver() : this(new XmlResolverConfiguration()) {
            // nop   
        }

        /// <summary>
        /// Create a new resolver using the specified configuration.
        /// </summary>
        /// <param name="config">The resolver configuration.</param>
        public CatalogResolver(XmlResolverConfiguration config) {
            this.config = config;
        }

        private ResolvedResourceImpl Resource(string requestUri, Uri responseUri, CacheEntry cached) {
            try {
                if (cached == null) {
                    return UncachedResource(new Uri(requestUri), responseUri);
                }

                var fs = File.Open(cached.CacheFile.ToString(), FileMode.Open, FileAccess.Read);
                return new ResolvedResourceImpl(responseUri, new Uri(cached.CacheFile.ToString()), fs,
                    cached.ContentType());
            }
            catch (Exception ex) {
                logger.Log(ResolverLogger.TRACE, "Failed to resolve {0}: {1}", requestUri, ex.Message);
                return null;
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
        
        /// <summary>
        /// Resolve a URI against the catalog(s).
        /// </summary>
        /// <para>This method attempts to locate <c>href</c> with <see cref="CatalogManager.LookupUri"/>.
        /// If that fails, and <c>baseUri</c> is provided, the <c>href</c> value will be made
        /// absolute and another attmept will be made with the absolute URI.</para>
        /// <param name="href">The URI.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <returns>A description of the resolved resource, or null if no resource was found.</returns>
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

            ResourceCache cache = (ResourceCache) config.GetFeature(ResolverFeature.CACHE);

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

        /// <summary>
        /// Resolve a URI against the catalog(s), optionally employing nature and purpose to limit the scope.
        /// </summary>
        /// <para>This method attempts to locate <c>href</c> with <see cref="CatalogManager.LookupNamespace"/>.
        /// If that fails, and <c>baseUri</c> is provided, the <c>href</c> value will be made
        /// absolute and another attmept will be made with the absolute URI.</para>
        /// <para>If nature and purpose are both null, this method has the same effect as calling
        /// <see cref="ResolveUri"/>.</para>
        /// <param name="href">The URI.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <param name="nature">The nature, which may be null.</param>
        /// <param name="purpose">The purpose, which may be null.</param>
        /// <returns></returns>
        public ResolvedResource ResolveNamespace(string href, string baseUri, string nature, string purpose) {
            logger.Log(ResolverLogger.REQUEST, "ResolveNamespace: {0} (base URI: {1}) nature={2}, purpose={3}", href, baseUri, nature, purpose);

            if (href == null || "".Equals(href.Trim())) {
                href = baseUri;
                baseUri = null;
                if (href == null || "".Equals(href.Trim())) {
                    logger.Log(ResolverLogger.RESPONSE, "ResolveNamespace: null");
                    return null;
                }
            }

            ResourceCache cache = (ResourceCache) config.GetFeature(ResolverFeature.CACHE);

            CatalogManager catalog = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            Uri resolved = catalog.LookupNamespaceUri(href, nature, purpose);
            if (resolved != null) {
                logger.Log(ResolverLogger.RESPONSE, "ResolveNamespace: {0}", resolved.ToString());
                return Resource(href, resolved, cache.CachedUri(resolved));
            }

            string absolute = href;
            if (baseUri != null) {
                absolute = UriUtils.Resolve(new Uri(baseUri), href).ToString();
                if (!href.Equals(absolute)) {
                    resolved = catalog.LookupNamespaceUri(absolute, nature, purpose);
                    if (resolved != null) {
                        logger.Log(ResolverLogger.RESPONSE, "ResolveNamespace: {0}", resolved.ToString());
                        return Resource(absolute, resolved, cache.CachedUri(resolved));
                    }
                }
            }

            if (cache.CacheUri(absolute)) {
                Uri absuri = new Uri(absolute);
                logger.Log(ResolverLogger.RESPONSE, "ResolveNamespace: cached: {0}", absolute);
                return Resource(absolute, absuri, cache.CachedUri(absuri));
            }
            else {
                logger.Log(ResolverLogger.RESPONSE, "ResolvedNamespace: null");
                return null;
            }
        }

        /// <summary>
        /// Resolve an entity against the catalog(s).
        /// </summary>
        /// <para>If a <c>name</c> is provided, but both the <c>systemId</c> and
        /// <c>publicId</c> are null, an attempt is made to locate a document type with the
        /// specified name (calling <see cref="CatalogManager.LookupDoctype"/>.</para>
        /// <para>If at least one of the identifiers is provided, they are used to lookup the
        /// resource, with the <c>name</c> used with <see cref="CatalogManager.LookupEntity"/>
        /// if nothing is located with the identifiers.</para>
        /// <para>If the <c>baseUri</c> and system identifiers are not null, and resolution fails,
        /// the system identifier is made absolute against the base URI and a second attempt is made.</para>
        /// <para>If all of the parameters are null, null is returned.</para>
        /// <param name="name">The entity name, which may be null.</param>
        /// <param name="publicId">The public identifier, which may be null.</param>
        /// <param name="systemId">The system identifier, which may be null.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <returns></returns>
        public ResolvedResource ResolveEntity(string name, string publicId, string systemId, string baseUri) {
            if (name == null && publicId == null && systemId == null && baseUri == null) {
                logger.Log(ResolverLogger.REQUEST, "ResolveEntity: null");
                return null;
            }

            if (UriUtils.isWindows() && (bool) config.GetFeature(ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS))
            {
                systemId = systemId.Replace("\\", "/");
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

            ResourceCache cache = (ResourceCache) config.GetFeature(ResolverFeature.CACHE);

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
                ResourceCache cache = (ResourceCache) config.GetFeature(ResolverFeature.CACHE);

                logger.Log(ResolverLogger.RESPONSE, "resolveDoctype: {0}", resolved.ToString());
                ResolvedResourceImpl result = Resource(null, resolved, cache.CachedSystem(resolved, null));
                return result;
            }
        }
        
        /// <summary>
        /// Returns the underlying configuration used by this resolver.
        /// </summary>
        /// <para>The resolver configuration can be interrogated or changed by calling
        /// <see cref="XmlResolverConfiguration.GetFeature"/> or
        /// <see cref="XmlResolverConfiguration.SetFeature"/> on the configuration.</para>
        /// <returns></returns>
        public IResolverConfiguration GetConfiguration() {
            return config;
        }
    }
}