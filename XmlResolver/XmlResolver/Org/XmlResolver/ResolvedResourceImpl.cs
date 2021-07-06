using System;
using System.IO;

namespace Org.XmlResolver {
    public class ResolvedResourceImpl : ResolvedResource {
        private readonly Uri ResolvedUri;
        private readonly Uri LocalUri;
        private readonly Stream InputStream;
        private readonly string ContentType;

        public ResolvedResourceImpl(Uri resolvedUri, Uri localUri, Stream stream, string contentType) {
            ResolvedUri = resolvedUri;
            LocalUri = localUri;
            InputStream = stream;
            ContentType = contentType;
        }
        
        public override Uri GetResolvedUri() {
            return ResolvedUri;
        }

        public override Uri GetLocalUri() {
            return LocalUri;
        }

        public override Stream GetInputStream() {
            return InputStream;
        }

        public override string GetContentType() {
            return ContentType;
        }
    }
}