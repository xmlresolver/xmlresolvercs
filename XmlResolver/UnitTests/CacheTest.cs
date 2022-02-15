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
            Assert.NotNull(cache.GetCacheInfo("^file:"));
            Assert.NotNull(cache.GetCacheInfo("^jar:file:"));
            Assert.NotNull(cache.GetCacheInfo("^classpath:"));
            Assert.NotNull(cache.GetCacheInfo("^pack:"));
            Assert.Null(cache.GetCacheInfo("^fribble:"));
            Assert.AreEqual(4, cache.GetCacheInfoList().Count);
         }

        [Test]
        public void AddCacheInfoDefault() {
            cache.AddCacheInfo("^fribble:", true);
            Assert.AreEqual(5, cache.GetCacheInfoList().Count);
            CacheInfo info = cache.GetCacheInfo("^fribble:");
            Assert.AreEqual("^fribble:", info.UriPattern.ToString());
            Assert.AreEqual(ResourceCache.CacheSize, info.CacheSize);
            Assert.AreEqual(ResourceCache.CacheSpace, info.CacheSpace);
            Assert.AreEqual(ResourceCache.DeleteWait, info.DeleteWait);
            Assert.AreEqual(ResourceCache.MaxAge, info.MaxAge);
            cache.RemoveCacheInfo("^fribble:");
            Assert.AreEqual(4, cache.GetCacheInfoList().Count);
            Assert.Null(cache.GetCacheInfo("^fribble:"));
        }

        [Test]
        public void AddCacheInfoExplicit() {
            cache.AddCacheInfo("^fribble:", true);
            cache.AddCacheInfo("^frabble:", false, 123, 456, 789, 10);
            Assert.AreEqual(6, cache.GetCacheInfoList().Count);
            CacheInfo info = cache.GetCacheInfo("^frabble:");
            Assert.AreEqual("^frabble:", info.UriPattern.ToString());
            Assert.AreEqual(456, info.CacheSize);
            Assert.AreEqual(789, info.CacheSpace);
            Assert.AreEqual(123, info.DeleteWait);
            Assert.AreEqual(10, info.MaxAge);
            cache.RemoveCacheInfo("^frabble:");
            Assert.AreEqual(5, cache.GetCacheInfoList().Count);
            Assert.Null(cache.GetCacheInfo("^frabble:"));
            Assert.NotNull(cache.GetCacheInfo("^fribble:"));
            cache.RemoveCacheInfo("^fribble:");
            Assert.AreEqual(4, cache.GetCacheInfoList().Count);
            Assert.Null(cache.GetCacheInfo("^fribble:"));
        }

        [Test]
        public void AddCacheSave() {
            cache.AddCacheInfo("^fribble:", true);
            cache.AddCacheInfo("^frabble:", true, 123, 456, 789, 10);

            XmlResolverConfiguration secondConfig = new();
            secondConfig.SetFeature(ResolverFeature.CACHE_DIRECTORY,
                UriUtils.Resolve(UriUtils.Cwd(), cacheDir).AbsolutePath);
            ResourceCache secondCache = new ResourceCache(secondConfig);

            Assert.AreEqual(6, secondCache.GetCacheInfoList().Count);
            CacheInfo info = secondCache.GetCacheInfo("^frabble:");
            Assert.AreEqual("^frabble:", info.UriPattern.ToString());
            Assert.AreEqual(456, info.CacheSize);
            Assert.AreEqual(789, info.CacheSpace);
            Assert.AreEqual(123, info.DeleteWait);
            Assert.AreEqual(10, info.MaxAge);
            secondCache.RemoveCacheInfo("^frabble:");
            Assert.AreEqual(5, secondCache.GetCacheInfoList().Count);
            Assert.Null(secondCache.GetCacheInfo("^frabble:"));
            Assert.NotNull(secondCache.GetCacheInfo("^fribble:"));
            secondCache.RemoveCacheInfo("^fribble:");
            Assert.AreEqual(4, secondCache.GetCacheInfoList().Count);
            Assert.Null(secondCache.GetCacheInfo("^fribble:"));
        }

        [Test]
        public void TestCacheUri() {
            cache.AddCacheInfo("^fribble:", true);
            cache.AddCacheInfo("^frabble:", false, 123, 456, 789, 10);
            cache.AddCacheInfo("\\.dtd$", false);

            Assert.True(cache.CacheUri("http://example.com"));
            Assert.True(cache.CacheUri("fribble://example.com"));
            Assert.False(cache.CacheUri("frabble://example.com"));
            Assert.False(cache.CacheUri("file:///foo"));
            Assert.False(cache.CacheUri("jar:file:///foo"));
            Assert.False(cache.CacheUri("classpath:whatever"));

            Assert.True(cache.CacheUri("jar:http://example.com"));
            Assert.False(cache.CacheUri("http://examle.com/path/to/some.dtd"));

            cache.RemoveCacheInfo("^fribble:");
            cache.RemoveCacheInfo("^frabble:");
            cache.RemoveCacheInfo("\\.dtd$");
        }

        [Test]
        public void TestCacheData() {
            try {
                ResolvedResource rsrc = cresolver.ResolveUri("http://localhost:8222/docs/sample/xlink.xsd", null);
                Assert.NotNull(rsrc.GetInputStream());
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
            Assert.Null(localCache.GetDirectory());
            Assert.False(localCache.CacheUri("http://example.com/test.dtd"));
        }
    }
}