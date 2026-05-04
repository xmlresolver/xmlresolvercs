using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace UnitTests {
    public class DataAssemblyTest : BaseTestRoot {
        public static readonly string catalog1 = "/UnitTests/resources/rescat.xml";
        private XmlResolverConfiguration config = null;
        private XmlResolver.XmlResolver resolver = null;
        private HashSet<String> assemblies = new HashSet<String> ();
        private List<string> catalogs = new List<string>();

        [SetUp]
        public void BaseSetup() {
            catalogs.Add(TEST_ROOT_PATH + catalog1);
            config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
            // This is enabled by default now
            // config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "XmlResolverData.dll");
            resolver = new XmlResolver.XmlResolver(config);
        }

        [Test]
        public void LookupRddl()
        {
            var req = resolver.GetRequest("http://www.rddl.org/rddl-resource-1.mod",
                ResolverConstants.EXTERNAL_ENTITY_NATURE, ResolverConstants.ANY_PURPOSE);
            var res = resolver.Resolve(req);
            /*
            var res = resolver.CatalogResolver.ResolveEntity(null, null,
                "http://www.rddl.org/rddl-resource-1.mod", null);
                */
            Assert.NotNull(res);
            Assert.NotNull(res.Stream);
            Assert.NotNull(res.ResolvedUri);
            Assert.That(res.ResolvedUri.Scheme == "pack");
        }
        
        /*
        [Test]
        public void LookupRddlWithoutData() {
            config.SetFeature(ResolverFeature.USE_DATA_ASSEMBLY, false);
            var res = resolver.CatalogResolver.ResolveEntity(null, null,
                "http://www.rddl.org/rddl-resource-1.mod", null);
            Assert.IsNull(res);
        }

        [Test]
        public void LookupRddlWithoutDataByDefault() {
            List<Uri> propertyFiles = new List<Uri>()
                { new Uri("file://" + TEST_ROOT_PATH + "/UnitTests/resources/xmlresolver-minimal.json") };
            var localConfig = new XmlResolverConfiguration(propertyFiles, catalogs);
            var localResolver = new Resolver(localConfig);

            var res = localResolver.CatalogResolver.ResolveEntity(null, null,
                "http://www.rddl.org/rddl-resource-1.mod", null);
            
            // With the cache and the data assembly disabled, we won't return anything
            Assert.Null(res);
        }

        [Test]
        public void ParseSvg() {
            Uri docuri = new Uri(TEST_ROOT_PATH + "/UnitTests/resources/test.svg");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            
            ResolvingXmlReader reader = new ResolvingXmlReader(docuri, settings, resolver);
            try {
                while (reader.Read()) {
                    // nop;
                }
            }
            catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void lookupCheckUri() {
            var res = resolver.CatalogResolver.ResolveUri("https://xmlresolver.org/data/resolver/succeeded/test/check.xml", null);
            Assert.NotNull(res);
            Assert.NotNull(res.GetInputStream());
            Assert.NotNull(res.GetLocalUri());
            Assert.That(res.GetLocalUri().Scheme == "pack");
        }
    */
    }
}
