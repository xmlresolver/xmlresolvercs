using System.Reflection;
using System.Xml;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace Tests;

public class PiTest
{
    private XmlResolverConfiguration? _config = null;
    private XmlResolver.XmlResolver? _resolver = null;

    private XmlResolverConfiguration Config
    {
        get
        {
            if (_config == null)
            {
                _config = new XmlResolverConfiguration();
                _config.SetFeature(ResolverFeature.USE_DATA_ASSEMBLY, true);
            }

            return _config;
        }
    }

    private XmlResolver.XmlResolver Resolver
    {
        get
        {
            if (_resolver == null)
            {
                _resolver = new XmlResolver.XmlResolver(Config);
            }

            return _resolver;
        }
    }

    [Test]
    public void PiTestPass()
    {
        Uri docuri = UriUtils.GetLocationUri("resources/xml/pi.xml", Assembly.GetExecutingAssembly());
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;

        ResolvingXmlReader reader = new ResolvingXmlReader(docuri, settings, Resolver);
        try
        {
            while (reader.Read())
            {
                // nop;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Assert.Fail();
        }
    }

    [Test]
    public void piTestTemporary()
    {
        Uri docuri = UriUtils.GetLocationUri("resources/xml/pi.xml", Assembly.GetExecutingAssembly());
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;

        ResolvingXmlReader reader = new ResolvingXmlReader(docuri, settings, Resolver);
        try
        {
            while (reader.Read())
            {
                // nop;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Assert.Fail();
        }

        // The catalog added by the PI in the previous parse should not
        // still be in the catalog list now.
        Config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, false);
        reader = new ResolvingXmlReader(docuri, settings, Resolver);
        try
        {
            while (reader.Read())
            {
                // nop;
            }

            Assert.Fail();
        }
        catch (Exception)
        {
            Assert.Pass();
        }

        // But it should still be possible to do it again (make sure we haven't broken the parser)
        Config.SetFeature(ResolverFeature.ALLOW_CATALOG_PI, true);
        reader = new ResolvingXmlReader(docuri, settings, Resolver);
        try
        {
            while (reader.Read())
            {
                // nop;
            }
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }
}