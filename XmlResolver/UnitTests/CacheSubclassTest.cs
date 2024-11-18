using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Features;

namespace UnitTests {
    public class CacheSubclassTest {
        private XmlResolverConfiguration config;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration();
        }
        
        [Test]
        public void TestFeatureCache() {
            // Test that a subclass of ResourceCache can be used as a ResourceCache.
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CACHE), Is.EqualTo(true));
            CacheSubclass myCache = new CacheSubclass(config);
            ResourceCache orig = (ResourceCache)config.GetFeature(ResolverFeature.CACHE);
            config.SetFeature(ResolverFeature.CACHE, myCache);
            Assert.That(myCache, Is.SameAs(config.GetFeature(ResolverFeature.CACHE)));
            config.SetFeature(ResolverFeature.CACHE, orig);
        }

    }
}
