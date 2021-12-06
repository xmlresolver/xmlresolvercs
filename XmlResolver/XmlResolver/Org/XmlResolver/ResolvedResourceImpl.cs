using System;
using System.IO;

namespace Org.XmlResolver {
    /// <summary>
    /// A concrete implementation of the <see cref="ResolvedResource"/> class.
    /// </summary>
    public class ResolvedResourceImpl : ResolvedResource {
        private readonly Uri ResolvedUri;
        private readonly Uri LocalUri;
        private readonly Stream InputStream;
        private readonly string ContentType;

        /// <summary>
        /// Construct a resolved resource.
        /// </summary>
        /// <param name="resolvedUri">The resolved URI.</param>
        /// <param name="localUri">The local URI.</param>
        /// <param name="stream">A stream from which the resource can be read.</param>
        /// <param name="contentType">The content type of the resource, if known.</param>
        public ResolvedResourceImpl(Uri resolvedUri, Uri localUri, Stream stream, string contentType) {
            ResolvedUri = resolvedUri;
            LocalUri = localUri;
            InputStream = stream;
            ContentType = contentType;
        }
        
        /// <summary>
        /// Identifies the URI that was found in the catalog. This may be the absolute form if,
        /// for example, the API called accepts both a relative URI and a base URI.
        /// </summary>
        /// <returns>The URI of the entry that matched in the catalog.</returns>
        public override Uri GetResolvedUri() {
            return ResolvedUri;
        }

        /// <summary>
        /// Identifies the local URI of the resolved resource. If caching is enabled, this may be
        /// a URI in the cache.
        /// </summary>
        /// <returns>The URI of the local resource returned.</returns>
        public override Uri GetLocalUri() {
            return LocalUri;
        }

        /// <summary>
        /// Returns an open input stream from which the resource can be read.
        /// </summary>
        /// <returns>The resource stream.</returns>
        public override Stream GetInputStream() {
            return InputStream;
        }

        /// <summary>
        /// Identifies the content type of the resource located. This is not always available.
        /// </summary>
        /// <returns>The resource content type.</returns>
        public override string GetContentType() {
            return ContentType;
        }
    }
}