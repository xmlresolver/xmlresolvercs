using Org.XmlResolver.Cache;

namespace Org.XmlResolver.Features {
    public class ResourceCacheResolverFeature : ResolverFeature {
        private readonly ResourceCache _defaultValue;
        
        public ResourceCacheResolverFeature(string name, ResourceCache defaultValue) : base(name, typeof(ResourceCache)) {
            _defaultValue = defaultValue;
        }

        public ResourceCache GetDefaultValue() {
            return _defaultValue;
        }
    }
}