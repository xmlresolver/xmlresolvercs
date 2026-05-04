using System;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace UnitTests
{
    public class ParserTest
    {
        private XmlResolverConfiguration config = null;
        private XmlResolver.XmlResolver resolver = null;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration();
            resolver = new XmlResolver.XmlResolver(config);
            config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, true);
        }

        [Test]
        public void ParseWithRedirect()
        {
            Uri docuri = UriUtils.GetLocationUri("resources/xml/sample-dtd-redirect.xml", Assembly.GetExecutingAssembly());
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            
            ResolvingXmlReader reader = new ResolvingXmlReader(docuri, settings, resolver);
            try {
                while (reader.Read())
                {
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    
        [Test]
        public void ParseWithRedirect2()
        {
            // This will redirect to https: 
            Uri docuri = new Uri("http://www.w3.org/TR/xmlschema11-1/XMLSchema.xsd");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            
            ResolvingXmlReader reader = new ResolvingXmlReader(docuri, settings, resolver);
            try {
                while (reader.Read())
                {
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
        
    }
}