using XmlResolver;
using XmlResolver.Utils;

namespace Tests;

public class CMNextTest : XmlResolverTest
{
    private Uri baseUri = new Uri("file:///tmp/");
    private CatalogManager manager = null;

    [SetUp]
    public void Setup()
    {
        if (TestRootPath[1] == ':')
        {
            // Fix the path for Windows
            baseUri = new Uri("file:///" + TestRootPath[0] + ":/tmp/");
        }

        XmlResolverConfiguration config = new XmlResolverConfiguration();
        config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/nextroot.xml")
            .ToString());
        config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/following.xml")
            .ToString());
        manager = new CatalogManager(config);
    }

    [Test]
    public void NextTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
        Uri result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void nextTest2()
    {
        // no next required
        Uri expected = UriUtils.Resolve(baseUri, "system.dtd");
        Uri result = manager.LookupPublic("http://example.com/system.dtd", "-//EXAMPLE//DTD Example//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void nextTest3()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system-next.dtd");
        Uri result = manager.LookupSystem("http://example.com/system-next.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void nextTest4()
    {
        Uri expected = UriUtils.Resolve(baseUri, "system-next.dtd");
        Uri result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example Next//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void nextTest5()
    {
        Uri expected = UriUtils.Resolve(baseUri, "public-next.dtd");
        Uri result = manager.LookupPublic("http://example.com/miss.dtd", "-//EXAMPLE//DTD Example Next//EN");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void nextTest6()
    {
        Uri expected = UriUtils.Resolve(baseUri, "found-in-one.xml");
        Uri result = manager.LookupUri("http://example.com/document.xml");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void nextTest7()
    {
        // After looking in the next catalogs, continue in the following catalogs
        Uri result = manager.LookupSystem("http://example.com/found-in-following.dtd");
        Assert.That(result, Is.Not.Null);
        Assert.That(true, Is.EqualTo(result.ToString().EndsWith("cm/following.dtd")));
    }

    [Test]
    public void nextTest8()
    {
        // After looking in the delegated catalogs, do not return to the following catalogs
        Uri result = manager.LookupSystem("http://example.com/delegated/but/not/found/in/delegated/catalogs.dtd");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void delegateSystemTest1()
    {
        Uri expected = UriUtils.Resolve(baseUri, "delegated-to-one.dtd");
        Uri result = manager.LookupSystem("http://example.com/delegated/one/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void delegateSystemTest2()
    {
        Uri expected = UriUtils.Resolve(baseUri, "delegated-to-two.dtd");
        Uri result = manager.LookupSystem("http://example.com/delegated/two/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void delegateSystemTest3()
    {
        Uri expected = UriUtils.Resolve(baseUri, "three-from-two.dtd");
        Uri result = manager.LookupSystem("http://example.com/delegated/three/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void delegateSystemTest4()
    {
        Uri expected = UriUtils.Resolve(baseUri, "test-from-two.dtd");
        Uri result = manager.LookupSystem("http://example.com/delegated/one/test/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void delegateSystemTest5()
    {
        // This Uri is in nextone.xml, but because nextroot.xml delegates to different catalogs,
        // it's never seen by the resolver.
        Uri result = manager.LookupSystem("http://example.com/delegated/four/system.dtd");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void delegateSystemTest6()
    {
        Uri expected = UriUtils.Resolve(baseUri, "five-from-two.dtd");
        Uri result = manager.LookupSystem("http://example.com/delegated/five/system.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }
}