using System;
using System.Xml;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;

namespace UnitTests {
    public class Issue48 {
        [Test]
        public void testResolver() {
            var resolverConfig = new XmlResolverConfiguration();
            resolverConfig.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "XmlResolverData.dll");
            //resolverConfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);

            var resolver = new XmlResolver.XmlResolver(resolverConfig);

            string uri = "https://www.w3.org/TR/xslt-30/schema-for-xslt30.xsd";

            Console.WriteLine(resolver.ResolveUri(null, uri));

            using (XmlReader xmlReader = XmlReader.Create(uri, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse, XmlResolver = resolver.GetXmlResolver(), Async = false}))
            {
                while (xmlReader.Read())
                {
                }
            }
        }
    }
}