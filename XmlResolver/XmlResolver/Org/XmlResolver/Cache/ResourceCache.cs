using System;

namespace Org.XmlResolver.Cache {
    public class ResourceCache {
        private readonly XmlResolverConfiguration _config;

        public ResourceCache(XmlResolverConfiguration config) {
            _config = config;
        }

        public CacheEntry CachedUri(Uri uri) {
            return CachedNamespaceUri(uri, null, null);
        }

        public CacheEntry CachedNamespaceUri(Uri uri, Uri nature, Uri purpose) {
            return null;
        }

        public CacheEntry CachedSystem(Uri systemId, string publicId) {
            return null;
        }
        
        public bool CacheUri(string uri) {
            return false;
        }
        
    }
}