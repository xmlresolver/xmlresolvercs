using System.Reflection;
using System.Xml;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace Tests;

public class ParserTest
{
    private XmlResolverConfiguration? _config = null;
    private XmlResolver.XmlResolver? _resolver = null;

    [SetUp]
    public void Setup()
    {
        _config = new XmlResolverConfiguration();
        _resolver = new XmlResolver.XmlResolver(_config);
        _config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, true);
    }

    [Test]
    public void ParseWithRedirect()
    {
        var docUri = UriUtils.GetLocationUri("resources/xml/sample-dtd-redirect.xml", Assembly.GetExecutingAssembly());
        var settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;

        if (_resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            ResolvingXmlReader reader = new ResolvingXmlReader(docUri, settings, _resolver);
            try
            {
                while (reader.Read())
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }

    [Test]
    public void ParseWithRedirect2()
    {
        // This will redirect to https: 
        var docUri = new Uri("http://www.w3.org/TR/xmlschema11-1/XMLSchema.xsd");
        var settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;

        if (_resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            var reader = new ResolvingXmlReader(docUri, settings, _resolver);
            try
            {
                while (reader.Read())
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

    }
}

