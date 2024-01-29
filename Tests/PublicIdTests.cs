using System.Reflection;
using System.Xml;
using XmlResolver;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace Tests;

public class PublicIdTest {
    private XmlResolverConfiguration config = null;
    private XmlResolver.XmlResolver resolver = null;

    [SetUp]
    public void Setup() {
        config = new XmlResolverConfiguration();
        config.AddCatalog(UriUtils.GetLocationUri("resources/parse/catalog.xml", Assembly.GetExecutingAssembly()).ToString());
        resolver = new XmlResolver.XmlResolver(config);
    }
        
    [Test]
    public void PublicTest() {
        Uri docuri = UriUtils.GetLocationUri("resources/parse/doc.xml", Assembly.GetExecutingAssembly());
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
