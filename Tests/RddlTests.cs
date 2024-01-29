using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class RddlTests
{
    public const string catalog = "src/test/resources/docker.xml";
    private XmlResolverConfiguration config = null;
    private XmlResolver.XmlResolver resolver = null;

    [SetUp]
    public void setup()
    {
        config = new XmlResolverConfiguration(catalog);
        resolver = new XmlResolver.XmlResolver(config);

        var req = resolver.GetRequest("http://localhost:8222/docs/sample/sample.dtd");
        var resp = resolver.Resolve(req);
        Assert.That(200, Is.EqualTo(resp.StatusCode));
    }
    
    [Test]
    public void XsdTest()
    {
        resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, true);
        var req = resolver.GetRequest("http://localhost:8222/docs/sample",
            null,
            "http://www.w3.org/2001/XMLSchema",
            "http://www.rddl.org/purposes#schema-validation");
        var resp = resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That("application/xml", Is.EqualTo(resp.ContentType));
    }
    
    [Test]
    public void XslTest()
    {
        resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, true);
        var req = resolver.GetRequest("http://localhost:8222/docs/sample",
            null,
            "http://www.w3.org/1999/XSL/Transform",
            "http://www.rddl.org/purposes#transformation");
        var resp = resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That("application/xml", Is.EqualTo(resp.ContentType));
    }

    [Test]
    public void XslTestBaseUri()
    {
        resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, true);
        var req = resolver.GetRequest("sample",
            "http://localhost:8222/docs/",
            "http://www.w3.org/1999/XSL/Transform",
            "http://www.rddl.org/purposes#transformation");
        var resp = resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That("application/xml", Is.EqualTo(resp.ContentType));
    }

    [Test]
    public void XslTestNoRddl()
    {
        resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, false);
        var req = resolver.GetRequest("http://localhost:8222/docs/sample",
            null,
            "http://www.w3.org/2001/XMLSchema",
            "http://www.rddl.org/purposes#schema-validation");
        var resp = resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        // Extra "/" because Apache redirects to the directory listing.
        Assert.That("http://localhost:8222/docs/sample/", Is.EqualTo(resp.ResolvedUri));
    }

    [Test]
    public void XmlTestResolved()
    {
        // This test gets the xml.xsd file from the catalog, so it runs quickly and proves
        // that we parse the resolved resource, not the original URI.
        XmlResolverConfiguration lconfig = new XmlResolverConfiguration("Tests/resources/catalog.xml");
        lconfig.SetFeature(ResolverFeature.PARSE_RDDL, true);
        XmlResolver.XmlResolver lresolver = new XmlResolver.XmlResolver(lconfig);

        var req = lresolver.GetRequest("http://www.w3.org/XML/1998/namespace",
            null,
            "http://www.w3.org/2001/XMLSchema",
            "http://www.rddl.org/purposes#validation");
        var resp = lresolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That(true, Is.EqualTo(resp.ResolvedUri.ToString().EndsWith("xml.xsd")));


    }
}