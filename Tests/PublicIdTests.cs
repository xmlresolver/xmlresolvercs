using System.Reflection;
using System.Xml;
using XmlResolver;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace Tests;

public class PublicIdTest {
    private XmlResolverConfiguration? _config = null;
    private XmlResolver.XmlResolver? _resolver = null;

    [SetUp]
    public void Setup() {
        _config = new XmlResolverConfiguration();
        _config.AddCatalog(UriUtils.GetLocationUri("resources/parse/catalog.xml", Assembly.GetExecutingAssembly()).ToString());
        _resolver = new XmlResolver.XmlResolver(_config);
    }
        
    [Test]
    public void PublicTest() {
        var docUri = UriUtils.GetLocationUri("resources/parse/doc.xml", Assembly.GetExecutingAssembly());
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse
        };

        if (_resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            var reader = new ResolvingXmlReader(docUri, settings, _resolver);
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
