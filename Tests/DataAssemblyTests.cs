using System.Xml;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Tools;

namespace Tests;

public class DataAssemblyTests : XmlResolverTest
{
    private Uri? _catalog1;
    private XmlResolver.XmlResolver? _resolver;

    private Uri Catalog1
    {
        get
        {
            _catalog1 ??= new Uri(TestRootDirectory, "resources/rescat.xml");
            if (_catalog1 == null)
            {
                throw new NullReferenceException("Failed to load catalog /resources/catalog.xml");
            }
            return _catalog1;
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
    public void BaseSetup()
    {
        CatalogFiles.Add(Catalog1.ToString());
        // This is enabled by default now
        // config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "XmlResolverData.dll");
    }

    [Test]
    public void LookupRddl()
    {
        var req = Resolver.GetRequest("http://www.rddl.org/rddl-resource-1.mod", null,
            ResolverConstants.EXTERNAL_ENTITY_NATURE, ResolverConstants.ANY_PURPOSE);
        var res = Resolver.Resolve(req);

        Assert.That(res, Is.Not.Null);
        Assert.That(res.Stream, Is.Not.Null);
        if (res.Uri == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(res.Uri.Scheme == "pack");
        }
    }

    [Test]
    public void LookupRddlWithoutData()
    {
        Config.SetFeature(ResolverFeature.USE_DATA_ASSEMBLY, false);
        var res = Resolver.LookupEntity(null, "http://www.rddl.org/rddl-resource-1.mod");
        Assert.That(res.IsResolved, Is.False);
    }

    [Test]
    public void LookupRddlWithoutDataByDefault()
    {
        List<Uri> propertyFiles = new List<Uri>()
            { new Uri("file://" + TestRootPath + "/Tests/resources/xmlresolver-minimal.json") };
        var localConfig = new XmlResolverConfiguration(propertyFiles, CatalogFiles);
        var localResolver = new XmlResolver.XmlResolver(localConfig);

        var res = localResolver.LookupEntity(null, "http://www.rddl.org/rddl-resource-1.mod");

        // With the the data assembly disabled, we won't return anything
        Assert.That(res.IsResolved, Is.False);
    }

    [Test]
    public void ParseSvg()
    {
        Uri docuri = new Uri(TestRootPath + "/Tests/resources/test.svg");
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
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    [Test]
    public void LookupCheckUri()
    {
        var req = Resolver.GetRequest("https://xmlresolver.org/data/resolver/succeeded/test/check.xml", null);
        var res = Resolver.Resolve(req);
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Stream, Is.Not.Null);
        if (res.Uri == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(res.Uri.Scheme, Is.EqualTo("pack"));
        }
    }
}