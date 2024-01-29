using System.Xml;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class CatalogResolverTests : XmlResolverTest
{
    private Uri? _catalog;
    private XmlResolver.XmlResolver? _resolver;

    private Uri Catalog
    {
        get
        {
            _catalog ??= UriUtils.GetLocationUri("/resources/catalog.xml", Asm);
            if (_catalog == null)
            {
                throw new NullReferenceException("Failed to load catalog /resources/catalog.xml");
            }
            return _catalog;
        }
    }

    private XmlResolver.XmlResolver Resolver
    {
        get
        {
            _resolver ??= new XmlResolver.XmlResolver(Config);
            return _resolver!;
        }
    }

    [SetUp]
    public void Setup()
    {
        Config.AddCatalog(Catalog, GetStream(Catalog));
        Config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
    }

    [Test]
    public void UriForSystemFail()
    {
        Config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
        try
        {
            // We don't use GetStream here because the semantics of GetStream are that it has to
            // attempt to get the resource, so it'll try the HTTP URI if it's not in the catalog.
            // That's not an interesting result here.
            CatalogManager? manager =
                (CatalogManager?)Config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            if (manager == null)
            {
                Assert.Fail();
            }
            else
            {
                Uri? rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample-as-uri/sample.dtd");
                Assert.That(rsrc, Is.Null);
            }
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    [Test]
    public void UriForSystemSuccess()
    {
        Config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
        try
        {
            System.Xml.XmlResolver resolver = Resolver.GetXmlResolver();
            object? stream = resolver.GetEntity(new Uri("https://xmlresolver.org/ns/sample-as-uri/sample.dtd"),
                null, null);
            Assert.That(stream, Is.Not.Null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Assert.Fail();
        }
    }

    [Test]
    public void ParseSample()
    {
        Config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
        Uri document = UriUtils.GetLocationUri("/resources/sample10/sample.xml", Asm);
        Assert.That(document.ToString().StartsWith("pack:"));
        //Console.WriteLine("ParseSample: " + document.ToString());
        XmlReaderSettings settings = new XmlReaderSettings
        {
            Async = false,
            DtdProcessing = DtdProcessing.Parse,
            XmlResolver = Resolver.GetXmlResolver()
        };

        using (XmlReader reader = XmlReader.Create(GetStream(document), settings))
        {
            while (reader.Read())
            {
                // nop
            }
        }

        Assert.Pass();
    }
}
