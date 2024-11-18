using System;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CMNextTest : BaseTestRoot {
        private Uri baseUri = new Uri("file:///tmp/");
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            if (TEST_ROOT_PATH[1] == ':')
            {
                // Fix the path for Windows
                baseUri = new Uri("file:///" + TEST_ROOT_PATH[0] + ":/tmp/");
            }
            
            XmlResolverConfiguration config = new XmlResolverConfiguration();
            config.AddCatalog(UriUtils.Resolve(TEST_ROOT_DIRECTORY, "XmlResolver/UnitTests/resources/cm/nextroot.xml").ToString());
            config.AddCatalog(UriUtils.Resolve(TEST_ROOT_DIRECTORY, "XmlResolver/UnitTests/resources/cm/following.xml").ToString());
            manager = new CatalogManager(config);
        }

        [Test]
        public void NextTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
            Uri result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NextTest2() {
            // no next required
            Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
            Uri result = manager.LookupPublic("http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NextTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "system-next.dtd");
            Uri result = manager.LookupSystem("http://example.com/system-next.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NextTest4() {
            Uri expected = UriUtils.Resolve(baseUri, "system-next.dtd");
            Uri result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example Next//EN");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NextTest5() {
            Uri expected = UriUtils.Resolve(baseUri, "public-next.dtd");
            Uri result = manager.LookupPublic("http://example.com/miss.dtd", "-//EXAMPLE//DTD Example Next//EN");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NextTest6() {
            Uri expected = UriUtils.Resolve(baseUri, "found-in-one.xml");
            Uri result = manager.LookupUri("http://example.com/document.xml");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NextTest7() {
            // After looking in the next catalogs, continue in the following catalogs
            Uri result = manager.LookupSystem("http://example.com/found-in-following.dtd");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToString().EndsWith("cm/following.dtd"), Is.EqualTo(true));
        }

        [Test]
        public void NextTest8() {
            // After looking in the delegated catalogs, do not return to the following catalogs
            Uri result = manager.LookupSystem("http://example.com/delegated/but/not/found/in/delegated/catalogs.dtd");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DelegateSystemTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "delegated-to-one.dtd");
            Uri result = manager.LookupSystem("http://example.com/delegated/one/system.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DelegateSystemTest2() {
            Uri expected = UriUtils.Resolve(baseUri, "delegated-to-two.dtd");
            Uri result = manager.LookupSystem("http://example.com/delegated/two/system.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DelegateSystemTest3() {
            Uri expected = UriUtils.Resolve(baseUri, "three-from-two.dtd");
            Uri result = manager.LookupSystem("http://example.com/delegated/three/system.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DelegateSystemTest4() {
            Uri expected = UriUtils.Resolve(baseUri, "test-from-two.dtd");
            Uri result = manager.LookupSystem("http://example.com/delegated/one/test/system.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DelegateSystemTest5() {
            // This Uri is in nextone.xml, but because nextroot.xml delegates to different catalogs,
            // it's never seen by the resolver.
            Uri result = manager.LookupSystem("http://example.com/delegated/four/system.dtd");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DelegateSystemTest6() {
            Uri expected = UriUtils.Resolve(baseUri, "five-from-two.dtd");
            Uri result = manager.LookupSystem("http://example.com/delegated/five/system.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
