using System;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CMSimpleTest : ResolverTest {
        private readonly Uri baseUri = new Uri("file:///tmp/");
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            XmlResolverConfiguration config = new XmlResolverConfiguration();
            config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
            config.AddCatalog(UriUtils.Resolve(TEST_ROOT_DIRECTORY, "src/test/resources/cm/simple.xml").ToString());
            manager = new CatalogManager(config);
        }

        [Test]
        public void publicTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
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
        public void systemTest() {
            Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
            Uri result = manager.LookupSystem("http://example.com/system.dtd");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void rewriteSystemTest() {
            Uri expected = UriUtils.Resolve(baseUri, "local/path/system.dtd");
            Uri result = manager.LookupSystem("http://example.com/rewrite/path/system.dtd");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void systemSuffixTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "suffix/base-long.dtd");
            Uri result = manager.LookupSystem("http://example.com/path/base.dtd");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void systemSuffixTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "suffix/base-short.dtd");
            Uri result = manager.LookupSystem("http://example.com/alternate/base.dtd");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void uriTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "/path/document.xml");
            Uri result = manager.LookupUri("http://example.com/document.xml");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void uriTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "/path/rddl.xml");
            Uri result = manager.LookupUri("http://example.com/rddl.xml");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void uriTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "/path/rddl.xml");
            Uri result = manager.LookupNamespaceUri("http://example.com/rddl.xml",
                "nature", "purpose");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void uriTest4() {
            Uri result = manager.LookupNamespaceUri("http://example.com/rddl.xml",
                "not-nature", "not-purpose");
            Assert.Null(result);
        }

        [Test]
        public void rewriteUriTest() {
            Uri expected = UriUtils.Resolve(baseUri, "/path/local/docs/document.xml");
            Uri result = manager.LookupUri("http://example.com/rewrite/docs/document.xml");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void uriSuffixTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "suffix/base-long.xml");
            Uri result = manager.LookupUri("http://example.com/path/base.xml");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void uriSuffixTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "suffix/base-short.xml");
            Uri result = manager.LookupUri("http://example.com/alternate/base.xml");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void bookTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "path/docbook.dtd");
            Uri result = manager.LookupDoctype("book", null, null);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void bookTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
            Uri result = manager.LookupDoctype("book",
                "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void bookTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
            Uri result = manager.LookupDoctype("book",
                "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void documentTest() {
            Uri expected = UriUtils.Resolve(baseUri, "path/default.xml");
            Uri result = manager.LookupDocument();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void entityTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "chap01.xml");
            Uri result = manager.LookupEntity("chap01", null, null);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void entityTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
            Uri result = manager.LookupEntity("chap01",
                "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void entityTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
            Uri result = manager.LookupEntity("chap01",
                "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void notationTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "notation.xml");
            Uri result = manager.LookupNotation("notename", null, null);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void notationTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
            Uri result = manager.LookupNotation("notename",
                "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void notationTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
            Uri result = manager.LookupNotation("notename",
                "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }
    }
}