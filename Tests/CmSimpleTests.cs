using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class CmSimpleTest : XmlResolverTest
{
    private readonly Uri _baseUri = new("file:///tmp/");
    private CatalogManager? _manager = null;

    private CatalogManager Manager
    {
        get
        {
            if (_manager == null)
            {
                XmlResolverConfiguration config = new XmlResolverConfiguration();
                config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
                config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/simple.xml")
                    .ToString());
                _manager = new CatalogManager(config);
            }

            return _manager;
        }
    }

    [Test]
    public void PublicTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "public.dtd");
        var result = Manager.LookupPublic("http://example.com/miss", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void PublicTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "system.dtd");
        var result = Manager.LookupPublic("http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemTest()
    {
        var expected = UriUtils.Resolve(_baseUri, "system.dtd");
        var result = Manager.LookupSystem("http://example.com/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void RewriteSystemTest()
    {
        var expected = UriUtils.Resolve(_baseUri, "local/path/system.dtd");
        var result = Manager.LookupSystem("http://example.com/rewrite/path/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemSuffixTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "suffix/base-long.dtd");
        var result = Manager.LookupSystem("http://example.com/path/base.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemSuffixTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "suffix/base-short.dtd");
        var result = Manager.LookupSystem("http://example.com/alternate/base.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "/path/document.xml");
        var result = Manager.LookupUri("http://example.com/document.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "/path/rddl.xml");
        var result = Manager.LookupUri("http://example.com/rddl.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest3()
    {
        var expected = UriUtils.Resolve(_baseUri, "/path/rddl.xml");
        var result = Manager.LookupNamespaceUri("http://example.com/rddl.xml",
            "nature", "purpose");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriTest4()
    {
        var result = Manager.LookupNamespaceUri("http://example.com/rddl.xml",
            "not-nature", "not-purpose");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void RewriteUriTest()
    {
        var expected = UriUtils.Resolve(_baseUri, "/path/local/docs/document.xml");
        var result = Manager.LookupUri("http://example.com/rewrite/docs/document.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriSuffixTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "suffix/base-long.xml");
        var result = Manager.LookupUri("http://example.com/path/base.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriSuffixTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "suffix/base-short.xml");
        var result = Manager.LookupUri("http://example.com/alternate/base.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void BookTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "path/docbook.dtd");
        var result = Manager.LookupDoctype("book", null, null);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void BookTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "system.dtd");
        var result = Manager.LookupDoctype("book",
            "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void BookTest3()
    {
        var expected = UriUtils.Resolve(_baseUri, "public.dtd");
        var result = Manager.LookupDoctype("book",
            "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void DocumentTest()
    {
        var expected = UriUtils.Resolve(_baseUri, "path/default.xml");
        var result = Manager.LookupDocument();
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void EntityTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "chap01.xml");
        var result = Manager.LookupEntity("chap01", null, null);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void EntityTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "system.dtd");
        var result = Manager.LookupEntity("chap01",
            "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void EntityTest3()
    {
        var expected = UriUtils.Resolve(_baseUri, "public.dtd");
        var result = Manager.LookupEntity("chap01",
            "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void NotationTest1()
    {
        var expected = UriUtils.Resolve(_baseUri, "notation.xml");
        var result = Manager.LookupNotation("notename", null, null);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void NotationTest2()
    {
        var expected = UriUtils.Resolve(_baseUri, "system.dtd");
        var result = Manager.LookupNotation("notename",
            "http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void NotationTest3()
    {
        var expected = UriUtils.Resolve(_baseUri, "public.dtd");
        var result = Manager.LookupNotation("notename",
            "http://example.com/miss.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }
}