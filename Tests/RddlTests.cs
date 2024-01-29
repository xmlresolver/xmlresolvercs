using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class RddlTests
{
    private XmlResolverConfiguration? _config = null;
    private XmlResolver.XmlResolver? _resolver = null;

    private XmlResolverConfiguration Config
    {
        get
        {
            if (_config == null)
            {
                _config = new XmlResolverConfiguration("src/test/resources/docker.xml");
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

    [SetUp]
    public void Setup()
    {
        var req = Resolver.GetRequest("http://localhost:8222/docs/sample/sample.dtd");
        var resp = Resolver.Resolve(req);
        Assert.That(200, Is.EqualTo(resp.StatusCode));
    }
    
    [Test]
    public void XsdTest()
    {
        Resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, true);
        var req = Resolver.GetRequest("http://localhost:8222/docs/sample",
            null,
            "http://www.w3.org/2001/XMLSchema",
            "http://www.rddl.org/purposes#schema-validation");
        var resp = Resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That("application/xml", Is.EqualTo(resp.ContentType));
    }
    
    [Test]
    public void XslTest()
    {
        Resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, true);
        var req = Resolver.GetRequest("http://localhost:8222/docs/sample",
            null,
            "http://www.w3.org/1999/XSL/Transform",
            "http://www.rddl.org/purposes#transformation");
        var resp = Resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That("application/xml", Is.EqualTo(resp.ContentType));
    }

    [Test]
    public void XslTestBaseUri()
    {
        Resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, true);
        var req = Resolver.GetRequest("sample",
            "http://localhost:8222/docs/",
            "http://www.w3.org/1999/XSL/Transform",
            "http://www.rddl.org/purposes#transformation");
        var resp = Resolver.Resolve(req);
        Assert.That(true, Is.EqualTo(resp.IsResolved));
        Assert.That("application/xml", Is.EqualTo(resp.ContentType));
    }

    [Test]
    public void XslTestNoRddl()
    {
        Resolver.Config.SetFeature(ResolverFeature.PARSE_RDDL, false);
        var req = Resolver.GetRequest("http://localhost:8222/docs/sample",
            null,
            "http://www.w3.org/2001/XMLSchema",
            "http://www.rddl.org/purposes#schema-validation");
        var resp = Resolver.Resolve(req);
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

        if (resp.ResolvedUri == null)
        {
            Assert.Fail();
        }
        else
        {
            Assert.That(true, Is.EqualTo(resp.ResolvedUri.ToString().EndsWith("xml.xsd")));
        }
    }
}