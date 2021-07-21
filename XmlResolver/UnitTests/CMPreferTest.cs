using System;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CMPreferTest : ResolverTest {
        private readonly Uri baseUri = new Uri("file:///tmp/");
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            XmlResolverConfiguration config = new XmlResolverConfiguration();
            config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
            config.AddCatalog(UriUtils.Resolve(TEST_ROOT_DIRECTORY, "src/test/resources/cm/pref-public.xml").ToString());
            manager = new CatalogManager(config);
        }
        
        [Test]
        public void publicTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "prefer-public.dtd");
            Uri result = manager.LookupPublic("http://example.com/miss", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void publicTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
            Uri result = manager.LookupPublic("http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void publicTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "prefer-system.dtd");
            Uri result = manager.LookupNotation("irrelevant", null, "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void publicTest4() {
            Uri expected = UriUtils.Resolve(baseUri, "prefer-system.dtd");
            Uri result = manager.LookupPublic(PublicId.EncodeUrn("-//EXAMPLE//DTD Example//EN").ToString(), null);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void publicTest5() {
            Uri expected = UriUtils.Resolve(baseUri, "prefer-system.dtd");
            Uri result = manager.LookupPublic(PublicId.EncodeUrn("-//EXAMPLE//DTD Different//EN").ToString(), "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }
    }
}