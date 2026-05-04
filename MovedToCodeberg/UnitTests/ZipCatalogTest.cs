using System;
using System.Collections.Generic;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;

namespace UnitTests {
    public class ZipCatalogTest: BaseTestRoot
    {
        private string zip1 = "";
        private string zip2 = "";
        private string zip3 = "";
        
        [SetUp]
        public void Setup()
        {
            zip1 = TEST_ROOT_PATH + "/UnitTests/resources/sample.zip";
            zip2 = TEST_ROOT_PATH + "/UnitTests/resources/dir-sample.zip";
            zip3 = TEST_ROOT_PATH + "/UnitTests/resources/sample-org.zip";
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
