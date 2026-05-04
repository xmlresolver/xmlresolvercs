using System;
using System.IO;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace UnitTests {
    public class CatalogResolverTest {
        private static readonly Assembly asm = Assembly.GetExecutingAssembly();
        private static readonly Uri catalog = UriUtils.GetLocationUri("/resources/catalog.xml", asm);
        private XmlResolverConfiguration config = new XmlResolverConfiguration();
        private XmlResolver.XmlResolver _resolver = null;

        [SetUp]
        public void Setup() {
            config.AddCatalog(catalog, ResourceAccess.GetStream(catalog));
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
            _resolver = new XmlResolver.XmlResolver(config);
        }

        [Test]
        public void UriForSystemFail() {
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
            try {
                // We don't use GetStream here because the semantics of GetStream are that it has to
                // attempt to get the resource, so it'll try the HTTP URI if it's not in the catalog.
                // That's not an interesting result here.
                CatalogManager manager =
                    (CatalogManager) _resolver.Configuration.GetFeature(ResolverFeature.CATALOG_MANAGER);
                Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample-as-uri/sample.dtd");
                Assert.Null(rsrc);
            }
            catch (Exception) {
                Assert.Fail();
            }
        }
        
        [Test]
        public void UriForSystemSuccess() {
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            try {
                object stream = _resolver.GetXmlResolver().GetEntity(new Uri("https://xmlresolver.org/ns/sample-as-uri/sample.dtd"),
                    null, null);
                Assert.NotNull(stream);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Test]
        public void ParseSample() {
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            Uri document = UriUtils.GetLocationUri("/resources/sample10/sample.xml", asm);
            Assert.That(document.ToString().StartsWith("pack:"));
            Console.WriteLine("ParseSample: " + document.ToString());
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;
            settings.DtdProcessing = DtdProcessing.Parse;
            Type t = typeof(Stream);
            settings.XmlResolver = _resolver.GetXmlResolver();
            using (XmlReader reader = XmlReader.Create(ResourceAccess.GetStream(document), settings)) {
                while (reader.Read()) {
                    // nop
                }
            }
            Assert.Pass();
        }
    }
}