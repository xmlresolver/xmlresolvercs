using System;

namespace Org.XmlResolver {
    /// <summary>
    /// The catalog resolver interface.
    /// </summary>
    public interface IXmlCatalogResolver {
        /// <summary>
        /// Lookup a URI.
        /// </summary>
        /// <para>This method matches <code>uri</code> entries in the catalog.</para>
        /// <param name="uri">The URI to find</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupUri(string uri);
        
        /// <summary>
        /// Lookup an entity by its public and system identifiers.
        /// </summary>
        /// <para>This method matches <code>system</code> and <code>public</code> entries in the catalog.
        /// If the <code>ResolverFeature.URI_FOR_SYSTEM</code> is true, then <code>uri</code> entries in
        /// the catalog will also match the system identifier.</para>
        /// <para>Note that public identifiers can be encoded in the system identifier with a URN,
        /// so it is possible for <code>public</code> entries to match in the catalog, even though
        /// the system identifier is required in XML.</para>
        /// <param name="systemId">The system identifier for the entity.</param>
        /// <param name="publicId">The public identifier for the entity.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupPublic(string systemId, string publicId);
        
        /// <summary>
        /// Lookup an entity by its system identifier.
        /// </summary>
        /// <para>This method matches <code>system</code> entries in the catalog.
        /// If the <code>ResolverFeature.URI_FOR_SYSTEM</code> is true, then <code>uri</code> entries in
        /// the catalog will also match the system identifier.</para>
        /// <para>Note that public identifiers can be encoded in the system identifier with a URN.
        /// If such a system identifier is provided, the matching will be performed against the
        /// corresponding public identifier and against <code>public</code> entries.</para>
        /// <param name="systemId">The system identifier for the entity.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupSystem(string systemId);

        /// <summary>
        /// Lookup a doctype.
        /// </summary>
        /// <para>This method matches <code>system</code> and <code>public</code> entries in the catalog.
        /// If no match is found, it attempts to match the entity name, if one is provided, against
        /// <code>doctype</code> entries in the catalog.</para>
        /// <param name="entityName">The entity name, may be null.</param>
        /// <param name="systemId">The system identifier, may be null.</param>
        /// <param name="publicId">The public identifier, may be null.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupDoctype(string entityName, string systemId, string publicId);
        
        /// <summary>
        /// Lookup an external entity.
        /// </summary>
        /// <para>This method matches <code>system</code> and <code>public</code> entries in the catalog.
        /// If no match is found, it attempts to match the entity name, if one is provided, against
        /// <code>entity</code> entries in the catalog.</para>
        /// <param name="entityName">The entity name, may be null.</param>
        /// <param name="systemId">The system identifier, may be null.</param>
        /// <param name="publicId">The public identifier, may be null.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupEntity(string entityName, string systemId, string publicId);
        
        /// <summary>
        /// Lookup a notation.
        /// </summary>
        /// <para>This method matches <code>system</code> and <code>public</code> entries in the catalog.
        /// If no match is found, it attempts to match the notation name, if one is provided, against
        /// <code>notation</code> entries in the catalog.</para>
        /// <param name="notationName">The notation name, may be null.</param>
        /// <param name="systemId">The system identifier, may be null.</param>
        /// <param name="publicId">The public identifier, may be null.</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupNotation(string notationName, string systemId, string publicId);
        
        /// <summary>
        /// Looks up the default document.
        /// </summary>
        /// <para>This method matches <code>document</code> entries in the catalog.
        /// <returns>The default document URI or null if no default document was specified.</returns>
        public Uri LookupDocument();
        
        /// <summary>
        /// Lookup a URI with a given nature and purpose.
        /// </summary>
        /// <para>This method matches <code>uri</code> entries, preferentially entries marked with matching
        /// <code>rddl:nature</code> and <code>rddl:purpose</code> attributes. It will not match <code>uri</code>
        /// entries that have different natures or purposes specified.</para>
        /// <para>An entry is considered to have a matching purpose if the purpose parameter is null, or
        /// if the entry has no <code>rddl:purpose</code> attribute, or if the parameter and the attribute
        /// value are the same. Similarly for the nature.</para>
        /// <para>Calling this method with both the nature and purpose set to null is equivalent to
        /// calling <code>lookupUri</code> with just the URI.
        /// <param name="uri">The URI.</param>
        /// <param name="nature">The desired nature URI, may be null.</param>
        /// <param name="purpose">The desired purpose URI, may be null</param>
        /// <returns>The resolved URI or null if no matching entry could be found.</returns>
        public Uri LookupNamespaceUri(string uri, string nature, string purpose);
    }
}