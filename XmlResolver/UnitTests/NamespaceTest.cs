using System;
using System.Collections.Generic;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;

namespace UnitTests {
    public class NamespaceTest : BaseTestRoot {
        public static readonly string catalog1 = "/XmlResolver/UnitTests/resources/rescat.xml";
        private XmlResolverConfiguration config = null;
        private CatalogManager manager = null;
        private Resolver resolver = null;
        
        [SetUp]
        public void BaseSetup() {
            List<string> catalogs = new List<string>();
            catalogs.Add(TEST_ROOT_PATH + catalog1);
            config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            resolver = new Resolver(config);
        }

        [Test]
        public void lookupOneForValidation() {
            Uri result = manager.LookupNamespaceUri("http://example.com/one", "the-one-nature", "validation");
            Assert.That(result.ToString().EndsWith("/resources/one-validate.xml"));
        }

        [Test]
        public void lookupOneForSomethingElse() {
            Uri result = manager.LookupNamespaceUri("http://example.com/one", "the-one-nature", "somethingelse");
            Assert.That(result.ToString().EndsWith("/resources/one-else.xml"));
        }

        [Test]
        public void lookupTwoForValidation() {
            Uri result = manager.LookupNamespaceUri("http://example.com/two", "the-two-nature", "validation");
            Assert.That(result.ToString().EndsWith("/resources/two-validate.xml"));
        }

        [Test]
        public void lookupTwoForAnythingElse() {
            Uri result = manager.LookupNamespaceUri("http://example.com/two", "the-two-nature", "anything-else");
            Assert.That(result.ToString().EndsWith("/resources/two-anything-else.xml"));
        }
        
        [Test]
        public void resolveWithHref() {
            object stream = resolver.GetEntity("one", new Uri("http://example.com/"), "the-one-nature", "validation");
            Assert.NotNull(stream);
        }

        [Test]
        public void resolveWithURI() {
            object stream = resolver.GetEntity(new Uri("http://example.com/one"), "the-one-nature", "validation");
            Assert.NotNull(stream);
        }
    }
}