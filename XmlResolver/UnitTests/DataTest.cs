using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Version = Org.XmlResolverData.Version;

namespace UnitTests {
    public class DataTest : ResolverTest {
        private string dataVersion = Version.DataVersion;
        private XmlResolverConfiguration config = null;
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());

            try {
                Assembly asm = Assembly.Load("XmlResolverData");
                config.AddAssemblyCatalog("Org.XmlResolver.catalog.xml", asm);
            }
            catch (Exception) {
                Assert.Fail();
            }

            manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        }

        [Test]
        public void Test1() {
            Uri result = manager.LookupUri("https://www.w3.org/1999/xlink.xsd");
            Assert.NotNull(result);
        }
        
    }
}