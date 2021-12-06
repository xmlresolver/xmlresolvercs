using System;
using System.IO;

namespace Org.XmlResolver {
    /// <summary>
    /// Describes a resolved resource.
    /// </summary>
    public abstract class ResolvedResource {
        /// <summary>
        /// Identifies the URI that was found in the catalog. This may be the absolute form if,
        /// for example, the API called accepts both a relative URI and a base URI.
        /// </summary>
        /// <returns>The URI of the entry that matched in the catalog.</returns>
        public abstract Uri GetResolvedUri();
        
        /// <summary>
        /// Identifies the local URI of the resolved resource. If caching is enabled, this may be
        /// a URI in the cache.
        /// </summary>
        /// <returns>The URI of the local resource returned.</returns>
        public abstract Uri GetLocalUri();
        
        /// <summary>
        /// Returns an open input stream from which the resource can be read.
        /// </summary>
        /// <returns>The resource stream.</returns>
        public abstract Stream GetInputStream();
        
        /// <summary>
        /// Identifies the content type of the resource located. This is not always available.
        /// </summary>
        /// <returns>The resource content type.</returns>
        public abstract string GetContentType();
    }
}