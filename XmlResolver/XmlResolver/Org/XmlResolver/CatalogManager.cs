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

    /// <summary>
    /// The CatalogManager manages a list of OASIS XML Catalogs and performs lookup
    /// operations based on those catalogs.
    /// </summary>
    ///
    /// <para>The managers job is to implement the semantics of XML Catalog, see
    /// https://xmlcatalogs.org/catalogs-1.1.html</para>
    ///
    /// <para>This class loads OASIS XML Catalog files and provides methods for
    /// searching the catalog. All of the XML Catalog entry types defined in
    /// §6 (catalog, group, public, system, rewriteSystem, systemSuffix,
    /// delegatePublic, delegateSystem, uri, rewriteURI, uriSuffix,
    /// delegateURI, and nextCatalog) are supported. In addition, the
    /// following TR9401 Catalog entry types from §D are supported: doctype,
    /// document, entity, and notation. (The other types do not apply to
    /// XML.)</para>
    ///
    /// <para>This class is a utility class used by the primary public API, the Resolver.</para>
    ///
    public class CatalogManager : IXmlCatalogResolver {
        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        protected readonly IResolverConfiguration _resolverConfiguration;
        protected ICatalogLoader _catalogLoader;

        /// <summary>
        /// Creates a new CatalogManager using the specified configuration.
        /// </summary>
        /// <param name="config">The resolver configuration.</param>
        /// <exception cref="NullReferenceException">If no catalog loader can be instantiated.</exception>
        public CatalogManager(IResolverConfiguration config) {
            _resolverConfiguration = config;
            String loaderClassName = (String) config.GetFeature(ResolverFeature.CATALOG_LOADER_CLASS);
            if (loaderClassName == null || "".Equals(loaderClassName.Trim())) {
                loaderClassName = ResolverFeature.CATALOG_LOADER_CLASS.GetDefaultValue();
            }

            // FIXME: what if this doesn't work?
            _catalogLoader = (ICatalogLoader) Activator.CreateInstance(Type.GetType(loaderClassName));
            if (_catalogLoader == null) {
                throw new NullReferenceException("Failed to create catalog loader from " + loaderClassName);
            }
            _catalogLoader.SetPreferPublic((bool) config.GetFeature(ResolverFeature.PREFER_PUBLIC));
            _catalogLoader.SetArchivedCatalogs((bool) config.GetFeature(ResolverFeature.ARCHIVED_CATALOGS));
        }

        /// <summary>
        /// Creates a CatalogManager from an existing manager with the specified configuration.
        /// </summary>
        /// <para>Creating a new manager in this way preserves the catalog loader class associated with the
        /// initial manager, but uses the specified configuration otherwise.</para>
        /// <param name="current">A current CatalogManager to use as a base.</param>
        /// <param name="config">The resolver configuration.</param>
        public CatalogManager(CatalogManager current, IResolverConfiguration config) {
            _catalogLoader = current._catalogLoader;
            _resolverConfiguration = config;
        }

        /// <summary>
        /// Get the underlying configuration associated with this manager.
        /// </summary>
        /// <returns>The underlying resolver configuration.</returns>
        public IResolverConfiguration GetResolverConfiguration() => _resolverConfiguration;

        /// <summary>
        /// Returns the list of catalog files currently being used by this manager.
        /// </summary>
        /// <para>This is a convenience method that returns absolute URIs for all the catalogs.</para>
        /// <returns>The list of catalog files, as absolute URIs.</returns>
        public List<Uri> Catalogs() {
            List<Uri> catlist = new();
            foreach (var cat in (List<string>) _resolverConfiguration.GetFeature(ResolverFeature.CATALOG_FILES)) {
                catlist.Add(new Uri(UriUtils.Cwd(), cat));
            }
            return catlist;
        }

        /// <summary>
        /// Loads an XML Catalog.
        /// </summary>
        /// <param name="catalog">The absolute URI of the catalog to load.</param>
        /// <returns>The EntryCatalog that represents the entries from the catalog file.</returns>
        public EntryCatalog LoadCatalog(Uri catalog) {
            EntryCatalog cat = _catalogLoader.LoadCatalog(catalog);
            return cat;
        }
        
        /// <summary>
        /// Loads an XML catalog from an open Stream.
        /// </summary>
        /// <para>This method assumes that you've already opened the catalog file and reads data from the
        /// stream. The specified <c>catalog</c> URI will be used to resolve relative URIs.</para>
        /// <param name="catalog">The absolute URI of the catalog to load.</param>
        /// <param name="data">An open data from which to read the catalog.</param>
        /// <returns>The EntryCatalog that represents the entries from the catalog file.</returns>
        public EntryCatalog LoadCatalog(Uri catalog, Stream data) {
            return _catalogLoader.LoadCatalog(catalog, data);
        }

        private String fixWindowsSystemIdentifier(String systemId)
        {
            if (UriUtils.isWindows() && (bool) _resolverConfiguration.GetFeature(ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS))
            {
                return systemId.Replace("\\", "/");
            }

            return systemId;
        }
        
        /// <summary>
        /// Lookup a URI.
        /// </summary>
        /// <para>This method matches <c>uri</c> entries in the catalog.</para>
        /// <param name="uri">The URI to find</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public virtual Uri LookupUri(string uri) {
            return LookupNamespaceUri(uri, null, null);
        }

        /// <summary>
        /// Lookup a URI with a given nature and purpose.
        /// </summary>
        /// <para>This method matches <c>uri</c> entries, preferentially entries marked with matching
        /// <c>rddl:nature</c> and <c>rddl:purpose</c> attributes. It will not match <c>uri</c>
        /// entries that have different natures or purposes specified.</para>
        /// <para>An entry is considered to have a matching purpose if the purpose parameter is null, or
        /// if the entry has no <c>rddl:purpose</c> attribute, or if the parameter and the attribute
        /// value are the same. Similarly for the nature.</para>
        /// <para>Calling this method with both the nature and purpose set to null is equivalent to
        /// calling <c>lookupUri</c> with just the URI.
        /// <param name="uri">The URI.</param>
        /// <param name="nature">The desired nature URI, may be null.</param>
        /// <param name="purpose">The desired purpose URI, may be null</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public virtual Uri LookupNamespaceUri(string uri, string nature, string purpose) {
            return new QueryUri(uri, nature, purpose).Search(this).ResultUri();
        }
        
        /// <summary>
        /// Lookup an entity by its public and system identifiers.
        /// </summary>
        /// <para>This method matches <c>system</c> and <c>public</c> entries in the catalog.
        /// If the <c>ResolverFeature.URI_FOR_SYSTEM</c> is true, then <c>uri</c> entries in
        /// the catalog will also match the system identifier.</para>
        /// <para>Note that public identifiers can be encoded in the system identifier with a URN,
        /// so it is possible for <c>public</c> entries to match in the catalog, even though
        /// the system identifier is required in XML.</para>
        /// <param name="systemId">The system identifier for the entity.</param>
        /// <param name="publicId">The public identifier for the entity.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public virtual Uri LookupPublic(string systemId, string publicId)
        {
            systemId = fixWindowsSystemIdentifier(systemId);
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryPublic(external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        /// <summary>
        /// Lookup an entity by its system identifier.
        /// </summary>
        /// <para>This method matches <c>system</c> entries in the catalog.
        /// If the <c>ResolverFeature.URI_FOR_SYSTEM</c> is true, then <c>uri</c> entries in
        /// the catalog will also match the system identifier.</para>
        /// <para>Note that public identifiers can be encoded in the system identifier with a URN.
        /// If such a system identifier is provided, the matching will be performed against the
        /// corresponding public identifier and against <c>public</c> entries.</para>
        /// <param name="systemId">The system identifier for the entity.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public virtual Uri LookupSystem(string systemId) 
        {
            systemId = fixWindowsSystemIdentifier(systemId);
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, null);
            if (external.SystemId == null) {
                return null;
            }

            return new QuerySystem(systemId).Search(this).ResultUri();
        }

        /// <summary>
        /// Lookup a doctype.
        /// </summary>
        /// <para>This method matches <c>system</c> and <c>public</c> entries in the catalog.
        /// If no match is found, it attempts to match the entity name, if one is provided, against
        /// <c>doctype</c> entries in the catalog.</para>
        /// <param name="entityName">The entity name, may be null.</param>
        /// <param name="systemId">The system identifier, may be null.</param>
        /// <param name="publicId">The public identifier, may be null.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public virtual Uri LookupDoctype(string entityName, string systemId, string publicId) 
        {
            systemId = fixWindowsSystemIdentifier(systemId);
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryDoctype(entityName, external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        /// <summary>
        /// Lookup an external entity.
        /// </summary>
        /// <para>This method matches <c>system</c> and <c>public</c> entries in the catalog.
        /// If no match is found, it attempts to match the entity name, if one is provided, against
        /// <c>entity</c> entries in the catalog.</para>
        /// <param name="entityName">The entity name, may be null.</param>
        /// <param name="systemId">The system identifier, may be null.</param>
        /// <param name="publicId">The public identifier, may be null.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public virtual Uri LookupEntity(string entityName, string systemId, string publicId)
        {
            systemId = fixWindowsSystemIdentifier(systemId);
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryEntity(entityName, external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        /// <summary>
        /// Lookup a notation.
        /// </summary>
        /// <para>This method matches <c>system</c> and <c>public</c> entries in the catalog.
        /// If no match is found, it attempts to match the notation name, if one is provided, against
        /// <c>notation</c> entries in the catalog.</para>
        /// <param name="notationName">The notation name, may be null.</param>
        /// <param name="systemId">The system identifier, may be null.</param>
        /// <param name="publicId">The public identifier, may be null.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupNotation(string notationName, string systemId, string publicId) 
        {
            systemId = fixWindowsSystemIdentifier(systemId);
            ExternalIdentifiers external = NormalizeExternalIdentifiers(systemId, publicId);
            return new QueryNotation(notationName, external.SystemId, external.PublicId).Search(this).ResultUri();
        }

        /// <summary>
        /// Looks up the default document.
        /// </summary>
        /// <para>This method matches <c>document</c> entries in the catalog.
        /// <returns>The default document URI or null if no default document was specified.</returns>
        public virtual Uri LookupDocument() {
            return new QueryDocument().Search(this).ResultUri();
        }

        /// <summary>
        /// Normalize a URI string for comparison.
        /// </summary>
        /// <para>This method attempts to generate a normalized or canonical version of a URI for comparison.
        /// If the URI begins with <c>classpath:/</c>, the initial slash is removed. If the
        /// <c>ResolverFeature.MERGE_HTTPS</c> feature is enabled and the URI starts with 
        /// <param name="uri"></param>
        /// <returns></returns>
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