using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class CmSimpleTest : XmlResolverTest
{
    private readonly Uri baseUri = new Uri("file:///tmp/");
    private CatalogManager manager = null;

    [SetUp]
    public void Setup()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration();
        config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
        config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/simple.xml")
            .ToString());
        manager = new CatalogManager(config);
    }

    [Test]
    public void PublicTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
        Uri result = manager.LookupPublic("http://example.com/miss", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void PublicTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
        Uri result = manager.LookupPublic("http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemTest()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
        Uri result = manager.LookupSystem("http://example.com/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void RewriteSystemTest()
    {
        Uri expected = UriUtils.Resolve(baseUri, "local/path/system.dtd");
        Uri result = manager.LookupSystem("http://example.com/rewrite/path/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemSuffixTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "suffix/base-long.dtd");
        Uri result = manager.LookupSystem("http://example.com/path/base.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemSuffixTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "suffix/base-short.dtd");
        Uri result = manager.LookupSystem("http://example.com/alternate/base.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "/path/document.xml");
        Uri result = manager.LookupUri("http://example.com/document.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "/path/rddl.xml");
        Uri result = manager.LookupUri("http://example.com/rddl.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest3()
    {
        Uri expected = UriUtils.Resolve(baseUri, "/path/rddl.xml");
        Uri result = manager.LookupNamespaceUri("http://example.com/rddl.xml",
            "nature", "purpose");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest4()
    {
        Uri result = manager.LookupNamespaceUri("http://example.com/rddl.xml",
            "not-nature", "not-purpose");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void RewriteUriTest()
    {
        Uri expected = UriUtils.Resolve(baseUri, "/path/local/docs/document.xml");
        Uri result = manager.LookupUri("http://example.com/rewrite/docs/document.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriSuffixTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "suffix/base-long.xml");
        Uri result = manager.LookupUri("http://example.com/path/base.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriSuffixTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "suffix/base-short.xml");
        Uri result = manager.LookupUri("http://example.com/alternate/base.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void BookTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "path/docbook.dtd");
        Uri result = manager.LookupDoctype("book", null, null);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void BookTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
        Uri result = manager.LookupDoctype("book",
            "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void BookTest3()
    {
        Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
        Uri result = manager.LookupDoctype("book",
            "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void DocumentTest()
    {
        Uri expected = UriUtils.Resolve(baseUri, "path/default.xml");
        Uri result = manager.LookupDocument();
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void EntityTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "chap01.xml");
        Uri result = manager.LookupEntity("chap01", null, null);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void EntityTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
        Uri result = manager.LookupEntity("chap01",
            "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void EntityTest3()
    {
        Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
        Uri result = manager.LookupEntity("chap01",
            "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void NotationTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "notation.xml");
        Uri result = manager.LookupNotation("notename", null, null);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void NotationTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
        Uri result = manager.LookupNotation("notename",
            "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void NotationTest3()
    {
        Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
        Uri result = manager.LookupNotation("notename",
            "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }
}