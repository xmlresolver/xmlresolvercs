namespace XmlResolver.Features {
    public class StringResolverFeature : ResolverFeature {
        private readonly string _defaultValue;
        
        public StringResolverFeature(string name, string defaultValue) : base(name, typeof(string)) {
            _defaultValue = defaultValue;
        }

        public string GetDefaultValue() {
            return _defaultValue;
        }
    }
}