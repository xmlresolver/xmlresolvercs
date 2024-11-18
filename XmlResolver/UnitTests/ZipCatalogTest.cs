using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class ZipCatalogTest {
        private static string root = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVER_ROOT");

        private static string zip1 = root + "/XmlResolver/UnitTests/resources/sample.zip";
        private static string zip2 = root + "/XmlResolver/UnitTests/resources/dir-sample.zip";
        private static string zip3 = root + "/XmlResolver/UnitTests/resources/sample-org.zip";

        [SetUp]
        public void Setup() {
        }
        
        [Test]
        public void lookupZip1System() {
            var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.AddCatalog(zip1);
            var manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);

            Uri result = manager.LookupSystem("https://xmlresolver.org/ns/zipped/blocks.dtd");
            Assert.That(result.ToString().StartsWith("pack://file%3"), Is.EqualTo(true));
            Assert.That(result.ToString().EndsWith(",sample.zip/sample.dtd"), Is.EqualTo(true));
        }

        [Test]
        public void lookupZip2Uri() {
            var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.AddCatalog(zip2);
            var manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            
            Uri result = manager.LookupUri("https://xmlresolver.org/ns/zipped/sample.rnc");
            Assert.That(result.ToString().StartsWith("pack://file%3"), Is.EqualTo(true));
            Assert.That(result.ToString().EndsWith(",dir-sample.zip/directory-x.y/sample.rnc"), Is.EqualTo(true));
        }
        [Test]
        public void lookupZip3Uri() {
            var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.AddCatalog(zip3);
            var manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);

            Uri result = manager.LookupUri("https://xmlresolver.org/ns/zipped/blocks.rnc");
            Assert.That(result.ToString().StartsWith("pack://file%3"), Is.EqualTo(true));
            Assert.That(result.ToString().EndsWith(",sample-org.zip/schemas/blocks.rnc"), Is.EqualTo(true));
        }
    }
}
