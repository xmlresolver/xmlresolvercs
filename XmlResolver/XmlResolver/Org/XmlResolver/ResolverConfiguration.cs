using System.Collections;
using System.Collections.Generic;
using Org.XmlResolver.Features;

namespace Org.XmlResolver {
    public interface ResolverConfiguration {
        public void SetFeature(ResolverFeature feature, object value);
        public object GetFeature(ResolverFeature resolverFeature);
        public List<ResolverFeature> GetFeatures();
    }
}