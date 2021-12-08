using Org.XmlResolver;
using Org.XmlResolver.Cache;

namespace UnitTests {
    public class CacheSubclass: ResourceCache {
        public CacheSubclass(XmlResolverConfiguration config) : base(config) {
            // nop
        }

        public new string GetDirectory() {
            return "";
        }
        
    }
}