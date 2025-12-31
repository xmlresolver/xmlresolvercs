using System;
using System.Collections.Generic;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;

namespace UnitTests {
    public class NamespaceTest : BaseTestRoot {
        public static readonly string catalog1 = "/UnitTests/resources/rescat.xml";
        private XmlResolverConfiguration config = null;
        private CatalogManager manager = null;
        private XmlResolver.XmlResolver resolver = null;
        
        [SetUp]
        public void BaseSetup() {
            List<string> catalogs = new List<string>();
            catalogs.Add(TEST_ROOT_PATH + catalog1);
            config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            resolver = new XmlResolver.XmlResolver(config);
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
        public void resolveWithHref()
        {
            var req = resolver.GetRequest("one", "http://example.com/", "the-one-nature", "validation");
            var resp = resolver.Lookup(req);
            var stream =  ResourceAccess.GetResource(resp).Stream;
            Assert.NotNull(stream);
        }

        [Test]
        public void resolveWithURI() {
            var req = resolver.GetRequest("http://example.com/", "the-one-nature", "validation");
            var resp = resolver.Lookup(req);
            var stream =  ResourceAccess.GetResource(resp).Stream;
            Assert.NotNull(stream);
        }
    }
}
