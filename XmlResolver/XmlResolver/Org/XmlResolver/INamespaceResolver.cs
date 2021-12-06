using System;

#nullable enable
namespace Org.XmlResolver {
    /// <summary>
    /// The namespace resolver interface.
    /// </summary>
    /// <para>This interface has been made to resemble the <see cref="System.Xml.UriResolver"/> interface
    /// as a convenience to users.</para>
    public interface INamespaceResolver {
        /// <summary>
        /// Resolve a URI.
        /// </summary>
        /// <param name="absoluteUri">The absolute (namespace) URI.</param>
        /// <param name="nature">The nature, which may be null.</param>
        /// <param name="purpose">The purpose, which may be null.</param>
        /// <returns>The resolved entity as a stream, or null.</returns>
        public object? GetEntity(Uri absoluteUri, string nature, string purpose);
        
        /// <summary>
        /// Resolve a URI.
        /// </summary>
        /// <para>If the <code>baseUri</code> is not null and resolution fails with the initial
        /// value of <code>href</code>, the <code>href</code> will be made absolute against the
        /// base URI and another attempt will be made.</para>
        /// <param name="href">The (namespace) URI.</param>
        /// <param name="baseUri">The base URI, which may be null.</param>
        /// <param name="nature">The nature, which may be null.</param>
        /// <param name="purpose">The purpose, which may be null.</param>
        /// <returns>The resolved entity as a stream, or null.</returns>
        public object? GetEntity(String href, Uri baseUri, string nature, string purpose);
    }
}