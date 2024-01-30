using System.Xml;
using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class Issue48 {
    [Test]
    public void testResolver() {
        var resolverConfig = new XmlResolverConfiguration();
        resolverConfig.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "XmlResolverData.dll");
        //resolverConfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);

        var resolver = new XmlResolver.XmlResolver(resolverConfig);

        string uri = "https://www.w3.org/TR/xslt-30/schema-for-xslt30.xsd";

        var req = resolver.GetRequest(null, uri);
        var resp = resolver.Resolve(req);

        using (XmlReader xmlReader = XmlReader.Create(uri, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse, XmlResolver = resolver.GetXmlResolver(), Async = false}))
        {
            while (xmlReader.Read())
            {
            }
        }
    }
}
