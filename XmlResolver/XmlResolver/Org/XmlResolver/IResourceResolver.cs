namespace Org.XmlResolver {
    /// <summary>
    /// The resource resolver interface.
    /// </summary>
    public interface IResourceResolver {
        /// <summary>
        /// Resolve a URI.
        /// </summary>
        /// <para>If the <c>baseUri</c> is not null and resolution fails with the initial
        /// value of <c>href</c>, the <c>href</c> will be made absolute against the
        /// base URI and another attempt will be made.</para>
        /// <param name="href">The URI.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <returns>A description of the resolved resource, or null if no resolution was possible.</returns>
        ResolvedResource ResolveUri(string href, string baseUri);

        /// <summary>
        /// Resolve a (namespace) URI.
        /// </summary>
        /// <para>If the <c>baseUri</c> is not null and resolution fails with the initial
        /// value of <c>href</c>, the <c>href</c> will be made absolute against the
        /// base URI and another attempt will be made.</para>
        /// <param name="href">The URI.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <param name="nature">The nature, which may be null.</param>
        /// <param name="purpose">The purpose, which may be null.</param>
        /// <returns>A description of the resolved resource, or null if no resolution was possible.</returns>
        ResolvedResource ResolveNamespace(string href, string baseUri, string nature, string purpose);

        /// <summary>
        /// Resolve an entity.
        /// </summary>
        /// <para>If a <c>name</c> is provided, but both the <c>systemId</c> and
        /// <c>publicId</c> are null, an attempt is made to locate a document type with the
        /// specified name.</para>
        /// <para>If at least one of the identifiers is provided, they are used to lookup the
        /// resource, with the <c>name</c> used if nothing is located with the identifiers.</para>
        /// <para>If the <c>baseUri</c> and system identifiers are not null, and resolution fails,
        /// the system identifier is made absolute against the base URI and a second attempt is made.</para>
        /// <para>If all of the parameters are null, null is returned.</para>
        /// <param name="name">The entity name, which may be null.</param>
        /// <param name="publicId">The public identifier, which may be null.</param>
        /// <param name="systemId">The system identifier, which may be null.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <returns>A description of the resolved resource, or null if no resolution was possible.</returns>
        ResolvedResource ResolveEntity(string name, string publicId, string systemId, string baseUri);
        
        /// <summary>
        /// Obtain the resolver configuration.
        /// </summary>
        /// <returns>The underlying resolver configuration.</returns>
        IResolverConfiguration GetConfiguration();
    }
}