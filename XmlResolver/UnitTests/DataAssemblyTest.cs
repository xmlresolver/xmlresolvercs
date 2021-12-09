using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Tools;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class DataAssemblyTest : BaseTestRoot {
        public static readonly string catalog1 = "/XmlResolver/UnitTests/resources/rescat.xml";
        private XmlResolverConfiguration config = null;
        private Resolver resolver = null;
        private HashSet<String> assemblies = new HashSet<String> ();

        [SetUp]
        public void BaseSetup() {
            List<string> catalogs = new List<string>();
            catalogs.Add(TEST_ROOT_PATH + catalog1);
            config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "XmlResolverData.dll");
            resolver = new Resolver(config);
        }

        [Test]
        public void LookupSvg() {
            var res = resolver.CatalogResolver.ResolveEntity(null, null,
                "http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd", null);
            Assert.NotNull(res);
            Assert.NotNull(res.GetInputStream());
            Assert.NotNull(res.GetLocalUri());
            Assert.That(res.GetLocalUri().Scheme == "pack");
        }
        
        [Test]
        public void ParseSvg() {
            Uri docuri = new Uri(TEST_ROOT_PATH + "/XmlResolver/UnitTests/resources/test.svg");
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

    }
}