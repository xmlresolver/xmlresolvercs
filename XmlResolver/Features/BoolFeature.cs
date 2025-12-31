using System;

namespace XmlResolver.Features {
    public class BoolResolverFeature : ResolverFeature {
        private readonly bool _defaultValue;

        public BoolResolverFeature(string name, bool defaultValue) : base(name, typeof(bool)) {
            _defaultValue = defaultValue;
        }
 
        public bool GetDefaultValue() {
            return _defaultValue;
        }
    }
}