using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class ResolverTest : XmlResolverTest
{
    public static readonly string catalog1 = "/Tests/resources/rescat.xml";
    private XmlResolverConfiguration? config = null;
    private XmlResolver.XmlResolver? resolver = null;

    [SetUp]
    public void BaseSetup()
    {
        List<string> catalogs = new List<string>();
        catalogs.Add(TestRootPath + catalog1);
        config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
        config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
        resolver = new XmlResolver.XmlResolver(config);
    }

    [Test]
    public void LookupSystemFail()
    {
        if (resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                var req = resolver.GetRequest("https://example.com/not/in/catalog");
                var resp = resolver.Resolve(req);
                Assert.That(false, Is.EqualTo(resp.IsResolved));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }

    [Test]
    public void LookupLocalSystemFail()
    {
        if (resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                var req = resolver.GetRequest("file:///path/to/thing/that/isnt/likely/to/exist");
                var resp = resolver.Resolve(req);
                Assert.That(false, Is.EqualTo(resp.IsResolved));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }

    [Test]
    public void LookupSystem()
    {
        if (resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                Uri result = UriUtils.Resolve(TestRootDirectory, "Tests/resources/sample10/sample.dtd");
                var req = resolver.GetRequest("https://example.com/sample/1.0/sample.dtd", null,
                    ResolverConstants.EXTERNAL_ENTITY_NATURE, ResolverConstants.ANY_PURPOSE);
                var resp = resolver.Resolve(req);

                Assert.That(true, Is.EqualTo(resp.IsResolved));
                Assert.That(resp.Stream, Is.Not.Null);
                Assert.That(result, Is.EqualTo(resp.ResolvedUri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }

    [Test]
    public void LookupSystemAsUri()
    {
        if (resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                Uri result = UriUtils.Resolve(TestRootDirectory, "Tests/resources/sample10/sample.dtd");
                var req = resolver.GetRequest("https://example.com/sample/1.0/uri.dtd", null,
                    ResolverConstants.EXTERNAL_ENTITY_NATURE, ResolverConstants.ANY_PURPOSE);
                var resp = resolver.Resolve(req);

                Assert.That(true, Is.EqualTo(resp.IsResolved));
                Assert.That(resp.Stream, Is.Not.Null);
                Assert.That(result, Is.EqualTo(resp.ResolvedUri));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }

    [Test]
    public void SequenceTest()
    {
        if (resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                List<string> catalogs = new List<string>();
                catalogs.Add(TestRootPath + "/Tests/resources/seqtest1.xml");
                catalogs.Add(TestRootPath + "/Tests/resources/seqtest2.xml");

                XmlResolverConfiguration lconfig = new XmlResolverConfiguration(new List<Uri>(), catalogs);
                lconfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);

                XmlResolver.XmlResolver lresolver = new XmlResolver.XmlResolver(lconfig);

                var req = lresolver.GetRequest("https://xmlresolver.org/ns/sample-as-uri/sample.dtd");
                var resp = lresolver.Resolve(req);
            
                Assert.That(true, Is.EqualTo(resp.IsResolved));
                Assert.That(resp.Stream, Is.Not.Null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }

}