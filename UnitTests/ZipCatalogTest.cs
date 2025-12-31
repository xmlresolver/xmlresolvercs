using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace UnitTests {
    public class ZipCatalogTest {
        private static string root = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVER_ROOT");

        private static string zip1 = root + "/UnitTests/resources/sample.zip";
        private static string zip2 = root + "/UnitTests/resources/dir-sample.zip";
        private static string zip3 = root + "/UnitTests/resources/sample-org.zip";

        [SetUp]
        public void Setup() {
        }
        
        [Test]
        public void lookupZip1System() {
            var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.AddCatalog(zip1);
            var manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);

            Uri result = manager.LookupSystem("https://xmlresolver.org/ns/zipped/blocks.dtd");
            Assert.True(result.ToString().StartsWith("pack://file%3"));
            Assert.True(result.ToString().EndsWith(",sample.zip/sample.dtd"));
        }

        [Test]
        public void lookupZip2Uri() {
            var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.AddCatalog(zip2);
            var manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            
            Uri result = manager.LookupUri("https://xmlresolver.org/ns/zipped/sample.rnc");
            Assert.True(result.ToString().StartsWith("pack://file%3"));
            Assert.True(result.ToString().EndsWith(",dir-sample.zip/directory-x.y/sample.rnc"));
        }
        [Test]
        public void lookupZip3Uri() {
            var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.AddCatalog(zip3);
            var manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);

            Uri result = manager.LookupUri("https://xmlresolver.org/ns/zipped/blocks.rnc");
            Assert.True(result.ToString().StartsWith("pack://file%3"));
            Assert.True(result.ToString().EndsWith(",sample-org.zip/schemas/blocks.rnc"));
        }
    }
}
