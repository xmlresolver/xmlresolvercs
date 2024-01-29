using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class NamespaceTest : XmlResolverTest
{
    public static readonly string catalog1 = "/Tests/resources/rescat.xml";
    private XmlResolverConfiguration config = null;
    private CatalogManager manager = null;
    private XmlResolver.XmlResolver resolver = null;

    [SetUp]
    public void BaseSetup()
    {
        List<string> catalogs = new List<string>();
        catalogs.Add(TestRootPath + catalog1);
        config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
        config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
        manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        resolver = new XmlResolver.XmlResolver(config);
    }

    [Test]
    public void LookupOneForValidation()
    {
        Uri result = manager.LookupNamespaceUri("http://example.com/one", "the-one-nature", "validation");
        Assert.That(result.ToString().EndsWith("/resources/one-validate.xml"));
    }

    [Test]
    public void LookupOneForSomethingElse()
    {
        Uri result = manager.LookupNamespaceUri("http://example.com/one", "the-one-nature", "somethingelse");
        Assert.That(result.ToString().EndsWith("/resources/one-else.xml"));
    }

    [Test]
    public void LookupTwoForValidation()
    {
        Uri result = manager.LookupNamespaceUri("http://example.com/two", "the-two-nature", "validation");
        Assert.That(result.ToString().EndsWith("/resources/two-validate.xml"));
    }

    [Test]
    public void LookupTwoForAnythingElse()
    {
        Uri result = manager.LookupNamespaceUri("http://example.com/two", "the-two-nature", "anything-else");
        Assert.That(result.ToString().EndsWith("/resources/two-anything-else.xml"));
    }

    [Test]
    public void ResolveWithHref()
    {
        var req = resolver.GetRequest("one", "http://example.com/", "the-one-nature", "validation");
        var resp = resolver.Resolve(req);
        Assert.That(resp.Stream, Is.Not.Null);
    }

    [Test]
    public void ResolveWithUri()
    {
        var req = resolver.GetRequest("http://example.com/one", null, "the-one-nature", "validation");
        var resp = resolver.Resolve(req);
        Assert.That(resp.Stream, Is.Not.Null);
    }
}