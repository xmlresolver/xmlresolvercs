using System;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Tools;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class PiTest {
        private XmlResolverConfiguration config = null;
        private Resolver resolver = null;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration();
            resolver = new Resolver(config);
            config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, true);
        }
        
        [Test]
        public void PiTestPass() {
            Uri docuri = UriUtils.GetLocationUri("resources/xml/pi.xml", Assembly.GetExecutingAssembly());
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
        public void piTestTemporary() {
            Uri docuri = UriUtils.GetLocationUri("resources/xml/pi.xml", Assembly.GetExecutingAssembly());
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

            // The catalog added by the PI in the previous parse should not
            // still be in the catalog list now.
            config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, false);
            reader = new ResolvingXmlReader(docuri, settings, resolver);
            try {
                while (reader.Read()) {
                    // nop;
                }
                Assert.Fail();
            } catch (Exception) {
                Assert.Pass();
            }

            // But it should still be possible to do it again (make sure we haven't broken the parser)
            config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, true);
            reader = new ResolvingXmlReader(docuri, settings, resolver);
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