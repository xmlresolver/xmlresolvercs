using System.Collections.Generic;

namespace XmlResolver.Features {
    public class ListOfStringResolverFeature : ResolverFeature {
        private readonly List<string> _defaultValue;

        public ListOfStringResolverFeature(string name, List<string> defaultValue) : base(name, typeof(List<string>)) {
            _defaultValue = defaultValue;
        }

        public List<string> GetDefaultValue() {
            return _defaultValue;
        }
   }
}