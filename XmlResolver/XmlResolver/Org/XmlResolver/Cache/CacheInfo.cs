using System.Text.RegularExpressions;

namespace Org.XmlResolver.Cache {
    public class CacheInfo {
        public readonly bool Cache;
        public readonly string Pattern;
        public readonly Regex UriPattern;
        public readonly long DeleteWait;
        public readonly long CacheSize;
        public readonly long CacheSpace;
        public readonly long MaxAge;

        internal CacheInfo(string pattern, bool cache): this(pattern, cache,
            ResourceCache.DeleteWait, ResourceCache.CacheSize, ResourceCache.CacheSpace, ResourceCache.MaxAge) {
            // nop
        }

        internal CacheInfo(string pattern, bool cache, long deleteWait, long cacheSize, long cacheSpace, long maxAge) {
            Pattern = pattern;
            UriPattern = new Regex(pattern);
            Cache = cache;
            DeleteWait = deleteWait;
            CacheSize = cacheSize;
            CacheSpace = cacheSpace;
            MaxAge = maxAge;
        }

        public override string ToString() {
            if (Cache) {
                return "Cache include " + Pattern;
            }

            return "Cache exclude " + Pattern;
        }
    }
}