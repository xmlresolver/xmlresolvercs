using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class NamespaceTest : XmlResolverTest
{
    private XmlResolverConfiguration? _nsConfig = null;
    private CatalogManager? _manager = null;
    private XmlResolver.XmlResolver? _resolver = null;

    private XmlResolverConfiguration NsConfig
    {
        get
        {
            if (_nsConfig == null)
            {
                string catalog1 = "/Tests/resources/rescat.xml";
                List<string> catalogs = new List<string>();
                catalogs.Add(TestRootPath + catalog1);
                _nsConfig = new XmlResolverConfiguration(new List<Uri>(), catalogs);
                _nsConfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            }

            return _nsConfig;
        }
    }
    
    private CatalogManager Manager
    {
        get
        {
            if (_manager == null)
            {
                _manager = (CatalogManager)NsConfig.GetFeature(ResolverFeature.CATALOG_MANAGER)!;
            }

            return _manager;
        }
    }

    private XmlResolver.XmlResolver Resolver
    {
        get
        {
            if (_resolver == null)
            {
                _resolver = new XmlResolver.XmlResolver(NsConfig);
            }

            return _resolver;
        }
    }
    
    [Test]
    public void LookupOneForValidation()
    {
        var result = Manager.LookupNamespaceUri("http://example.com/one", "the-one-nature", "validation");
        if (result == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(result.ToString().EndsWith("/resources/one-validate.xml"));
        }
    }

    [Test]
    public void LookupOneForSomethingElse()
    {
        var result = Manager.LookupNamespaceUri("http://example.com/one", "the-one-nature", "somethingelse");
        if (result == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(result.ToString().EndsWith("/resources/one-else.xml"));
        }
    }

    [Test]
    public void LookupTwoForValidation()
    {
        var result = Manager.LookupNamespaceUri("http://example.com/two", "the-two-nature", "validation");
        if (result == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(result.ToString().EndsWith("/resources/two-validate.xml"));
        }
    }

    [Test]
    public void LookupTwoForAnythingElse()
    {
        var result = Manager.LookupNamespaceUri("http://example.com/two", "the-two-nature", "anything-else");
        if (result == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(result.ToString().EndsWith("/resources/two-anything-else.xml"));
        }
    }

    [Test]
    public void ResolveWithHref()
    {
        var req = Resolver.GetRequest("one", "http://example.com/", "the-one-nature", "validation");
        var resp = Resolver.Resolve(req);
        Assert.That(resp.Stream, Is.Not.Null);
    }

    [Test]
    public void ResolveWithUri()
    {
        var req = Resolver.GetRequest("http://example.com/one", null, "the-one-nature", "validation");
        var resp = Resolver.Resolve(req);
        Assert.That(resp.Stream, Is.Not.Null);
    }
}