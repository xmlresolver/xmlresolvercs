using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class CmPreferTest : ResolverTest
{
    private readonly Uri baseUri = new Uri("file:///tmp/");
    private CatalogManager? manager = null;

    [SetUp]
    public void Setup()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration();
        config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
        config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/pref-public.xml")
            .ToString());
        manager = new CatalogManager(config);
    }

    [Test]
    public void PublicTest1()
    {
        var expected = UriUtils.Resolve(baseUri, "prefer-public.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic("http://example.com/miss", "-//EXAMPLE//DTD Example//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void PublicTest2()
    {
        var expected = UriUtils.Resolve(baseUri, "system.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic("http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void PublicTest3()
    {
        var expected = UriUtils.Resolve(baseUri, "prefer-system.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupNotation("irrelevant", null, "-//EXAMPLE//DTD Example//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void PublicTest4()
    {
        var expected = UriUtils.Resolve(baseUri, "prefer-system.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic(PublicId.EncodeUrn("-//EXAMPLE//DTD Example//EN")!.ToString(), null);
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void PublicTest5()
    {
        var expected = UriUtils.Resolve(baseUri, "prefer-system.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic(PublicId.EncodeUrn("-//EXAMPLE//DTD Different//EN")!.ToString(),
                "-//EXAMPLE//DTD Example//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }
}
