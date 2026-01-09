using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace DataTests;

public class ResolverTest : BaseTestRoot
{
    private XmlResolverConfiguration? _config = null;
    private XmlResolver.XmlResolver? _resolver = null;

    private XmlResolverConfiguration Config
    {
        get
        {
            if (_config == null)
            {
                string catalog1 = "/DataTests/resources/rescat.xml";
                List<string> catalogs = new List<string>();
                catalogs.Add(TestRootPath + catalog1);
                _config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
                _config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
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
    public void LookupSystem()
    {
        Uri result = UriUtils.Resolve(TestRootDirectory, "DataTests/resources/sample10/sample.dtd");
        var req = Resolver.GetRequest("https://example.com/sample/1.0/sample.dtd");
        var resp = Resolver.Resolve(req);
            
        Assert.That(resp.Stream, Is.Not.Null);
        Assert.That(resp.Uri, Is.EqualTo(result));
    }

    [Test]
    public void LookupSystemAsUri()
    {
        Uri result = UriUtils.Resolve(TestRootDirectory, "DataTests/resources/sample10/sample.dtd");
        var req = Resolver.GetRequest("https://example.com/sample/1.0/uri.dtd");
        var resp = Resolver.Resolve(req);

        Assert.That(resp.Stream, Is.Not.Null);
        Assert.That(resp.Uri, Is.EqualTo(result));
    }

    [Test]
    public void SequenceTest()
    {
        var catalogs = new List<string>
            {
                TestRootPath + "/DataTests/resources/seqtest1.xml",
                TestRootPath + "/DataTests/resources/seqtest2.xml"
            };

        XmlResolverConfiguration localConfig = new XmlResolverConfiguration(new List<Uri>(), catalogs);
        localConfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);

        XmlResolver.XmlResolver localResolver = new XmlResolver.XmlResolver(localConfig);
        var req = localResolver.GetRequest("https://xmlresolver.org/ns/sample-as-uri/sample.dtd");
        var resp = localResolver.Resolve(req);

        Assert.That(resp.Stream, Is.Not.Null);
    }
}