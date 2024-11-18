using System;
using System.IO;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CacheTest: CacheManager {
        // FIXME:
        private readonly string cacheDir = "/tmp/test-cache";

        private XmlResolverConfiguration config = null;
        private ResourceCache cache = null;
        private CatalogResolver cresolver = null;

        [SetUp]
        public void Setup() {
            ClearCache(cacheDir);
            config = new XmlResolverConfiguration();
            config.SetFeature(ResolverFeature.CACHE_DIRECTORY, Path.Combine(Directory.GetCurrentDirectory(), cacheDir));
            config.SetFeature(ResolverFeature.CACHE_ENABLED, true);
            cache = new ResourceCache(config);
            cresolver = new CatalogResolver(config);
        }
        
        [TearDown]
        public void Teardown()
        {
            ClearCache(cacheDir);
        }

        [Test]
        public void DefaultInfo() {
            Assert.That(cache.GetCacheInfo("^file:"), Is.Not.Null);
            Assert.That(cache.GetCacheInfo("^jar:file:"), Is.Not.Null);
            Assert.That(cache.GetCacheInfo("^classpath:"), Is.Not.Null);
            Assert.That(cache.GetCacheInfo("^pack:"), Is.Not.Null);
            Assert.That(cache.GetCacheInfo("^fribble:"), Is.Null);
            Assert.That(cache.GetCacheInfoList().Count, Is.EqualTo(4));
         }

        [Test]
        public void AddCacheInfoDefault() {
            cache.AddCacheInfo("^fribble:", true);
            Assert.That(cache.GetCacheInfoList().Count, Is.EqualTo(5));
            CacheInfo info = cache.GetCacheInfo("^fribble:");
            Assert.That(info.UriPattern.ToString(), Is.EqualTo("^fribble:"));
            Assert.That(info.CacheSize, Is.EqualTo(ResourceCache.CacheSize));
            Assert.That(info.CacheSpace, Is.EqualTo(ResourceCache.CacheSpace));
            Assert.That(info.DeleteWait, Is.EqualTo(ResourceCache.DeleteWait));
            Assert.That(info.MaxAge, Is.EqualTo(ResourceCache.MaxAge));
            cache.RemoveCacheInfo("^fribble:");
            Assert.That(cache.GetCacheInfoList().Count, Is.EqualTo(4));
            Assert.That(cache.GetCacheInfo("^fribble:"), Is.Null);
        }

        [Test]
        public void AddCacheInfoExplicit() {
            cache.AddCacheInfo("^fribble:", true);
            cache.AddCacheInfo("^frabble:", false, 123, 456, 789, 10);
            Assert.That(cache.GetCacheInfoList().Count, Is.EqualTo(6));
            CacheInfo info = cache.GetCacheInfo("^frabble:");
            Assert.That(info.UriPattern.ToString(), Is.EqualTo("^frabble:"));
            Assert.That(info.CacheSize, Is.EqualTo(456));
            Assert.That(info.CacheSpace, Is.EqualTo(789));
            Assert.That(info.DeleteWait, Is.EqualTo(123));
            Assert.That(info.MaxAge, Is.EqualTo(10));
            cache.RemoveCacheInfo("^frabble:");
            Assert.That(cache.GetCacheInfoList().Count, Is.EqualTo(5));
            Assert.That(cache.GetCacheInfo("^frabble:"), Is.Null);
            Assert.That(cache.GetCacheInfo("^fribble:"), Is.Not.Null);
            cache.RemoveCacheInfo("^fribble:");
            Assert.That(cache.GetCacheInfoList().Count, Is.EqualTo(4));
            Assert.That(cache.GetCacheInfo("^fribble:"), Is.Null);
        }

        [Test]
        public void AddCacheSave() {
            cache.AddCacheInfo("^fribble:", true);
            cache.AddCacheInfo("^frabble:", true, 123, 456, 789, 10);

            XmlResolverConfiguration secondConfig = new();
            secondConfig.SetFeature(ResolverFeature.CACHE_DIRECTORY,
                UriUtils.Resolve(UriUtils.Cwd(), cacheDir).AbsolutePath);
            secondConfig.SetFeature(ResolverFeature.CACHE_ENABLED, true);
            ResourceCache secondCache = new ResourceCache(secondConfig);

            Assert.That(secondCache.GetCacheInfoList().Count, Is.EqualTo(6));
            CacheInfo info = secondCache.GetCacheInfo("^frabble:");
            Assert.That(info.UriPattern.ToString(), Is.EqualTo("^frabble:"));
            Assert.That(info.CacheSize, Is.EqualTo(456));
            Assert.That(info.CacheSpace, Is.EqualTo(789));
            Assert.That(info.DeleteWait, Is.EqualTo(123));
            Assert.That(info.MaxAge, Is.EqualTo(10));
            secondCache.RemoveCacheInfo("^frabble:");
            Assert.That(secondCache.GetCacheInfoList().Count, Is.EqualTo(5));
            Assert.That(secondCache.GetCacheInfo("^frabble:"), Is.Null);
            Assert.That(secondCache.GetCacheInfo("^fribble:"), Is.Not.Null);
            secondCache.RemoveCacheInfo("^fribble:");
            Assert.That(secondCache.GetCacheInfoList().Count, Is.EqualTo(4));
            Assert.That(secondCache.GetCacheInfo("^fribble:"), Is.Null);
        }

        [Test]
        public void TestCacheUri() {
            cache.AddCacheInfo("^fribble:", true);
            cache.AddCacheInfo("^frabble:", false, 123, 456, 789, 10);
            cache.AddCacheInfo("\\.dtd$", false);

            Assert.That(cache.CacheUri("http://example.com"), Is.EqualTo(true));
            Assert.That(cache.CacheUri("fribble://example.com"), Is.EqualTo(true));
            Assert.That(cache.CacheUri("frabble://example.com"), Is.EqualTo(false));
            Assert.That(cache.CacheUri("file:///foo"), Is.EqualTo(false));
            Assert.That(cache.CacheUri("jar:file:///foo"), Is.EqualTo(false));
            Assert.That(cache.CacheUri("classpath:whatever"), Is.EqualTo(false));

            Assert.That(cache.CacheUri("jar:http://example.com"), Is.EqualTo(true));
            Assert.That(cache.CacheUri("http://examle.com/path/to/some.dtd"), Is.EqualTo(false));

            cache.RemoveCacheInfo("^fribble:");
            cache.RemoveCacheInfo("^frabble:");
            cache.RemoveCacheInfo("\\.dtd$");
        }

        [Test]
        public void TestCacheData() {
            try {
                ResolvedResource rsrc = cresolver.ResolveUri("http://localhost:8222/docs/sample/xlink.xsd", null);
                Assert.That(rsrc.GetInputStream(), Is.Not.Null);
                rsrc.GetInputStream().Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCacheDisabled() {
            XmlResolverConfiguration localConfig = new XmlResolverConfiguration();
            localConfig.SetFeature(ResolverFeature.CACHE_DIRECTORY, "/tmp/cache");
            localConfig.SetFeature(ResolverFeature.CACHE_ENABLED, false);
            ResourceCache localCache = new ResourceCache(localConfig);
            Assert.That(localCache.GetDirectory(), Is.Null);
            Assert.That(localCache.CacheUri("http://example.com/test.dtd"), Is.EqualTo(false));
        }

        [Test]
        public void TestCacheDisabledAfterInitialization()
        {
            XmlResolverConfiguration localConfig = new XmlResolverConfiguration();
            Assert.That((bool) localConfig.GetFeature(ResolverFeature.CACHE_ENABLED), Is.EqualTo(false));

            CatalogResolver resolver = new CatalogResolver(localConfig);
            resolver.GetConfiguration().SetFeature(ResolverFeature.CACHE_ENABLED, false);

            // With the cache disabled, we get back null
            ResolvedResource result = resolver.ResolveUri("https://jats.nlm.nih.gov/publishing/1.3/JATS-journalpublishing1-3.dtd", null);
            Assert.That(result, Is.Null);
        }
        
    }
}
