using System;
using System.IO;

namespace Org.XmlResolver {
    public abstract class ResolvedResource {
        public abstract Uri GetResolvedUri();
        public abstract Uri GetLocalUri();
        public abstract Stream GetInputStream();
        public abstract string GetContentType();
    }
}