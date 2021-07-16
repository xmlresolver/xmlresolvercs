using System;
using System.IO;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CatalogResolverTest {
        private static readonly Assembly asm = Assembly.GetExecutingAssembly();
        private static readonly Uri catalog = UriUtils.GetLocationUri("/resources/catalog.xml", asm);
        private XmlResolverConfiguration config = new XmlResolverConfiguration();
        private Resolver resolver = null;

        [SetUp]
        public void Setup() {
            config.AddCatalog(catalog, UriUtils.GetStream(catalog));
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
            resolver = new Resolver(config);
        }

        [Test]
        public void uriForSystemFail() {
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
            try {
                object stream = resolver.GetEntity(new Uri("https://xmlresolver.org/ns/sample-as-uri/sample.dtd"),
                    null, null);
                Assert.Null(stream);
            }
            catch (Exception) {
                Assert.Fail();
            }
        }
        
        [Test]
        public void uriForSystemSuccess() {
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            try {
                object stream = resolver.GetEntity(new Uri("https://xmlresolver.org/ns/sample-as-uri/sample.dtd"),
                    null, null);
                Assert.NotNull(stream);
            }
            catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void parseSample() {
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            Uri document = UriUtils.GetLocationUri("/resources/sample10/sample.xml", asm);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;
            settings.DtdProcessing = DtdProcessing.Parse;
            Type t = typeof(Stream);
            settings.XmlResolver = resolver;
            using (XmlReader reader = XmlReader.Create(UriUtils.GetStream(document), settings)) {
                while (reader.Read()) {
                    // nop
                }
            }
            Assert.Pass();
        }
    }
}