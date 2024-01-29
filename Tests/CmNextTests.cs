using XmlResolver;
using XmlResolver.Utils;

namespace Tests;

public class CMNextTest : XmlResolverTest
{
    private Uri baseUri = new Uri("file:///tmp/");
    private CatalogManager? manager = null;

    [SetUp]
    public void Setup()
    {
        if (TestRootPath[1] == ':')
        {
            // Fix the path for Windows
            baseUri = new Uri("file:///" + TestRootPath[0] + ":/tmp/");
        }

        var config = new XmlResolverConfiguration();
        config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/nextroot.xml")
            .ToString());
        config.AddCatalog(UriUtils.Resolve(TestRootDirectory, "Tests/resources/cm/following.xml")
            .ToString());
        manager = new CatalogManager(config);
    }

    [Test]
    public void NextTest1()
    {
        var expected = UriUtils.Resolve(baseUri, "public.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void NextTest2()
    {
        // no next required
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
    public void NextTest3()
    {
        var expected = UriUtils.Resolve(baseUri, "system-next.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/system-next.dtd");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void NextTest4()
    {
        var expected = UriUtils.Resolve(baseUri, "system-next.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example Next//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void NextTest5()
    {
        var expected = UriUtils.Resolve(baseUri, "public-next.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupPublic("http://example.com/miss.dtd", "-//EXAMPLE//DTD Example Next//EN");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void NextTest6()
    {
        var expected = UriUtils.Resolve(baseUri, "found-in-one.xml");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupUri("http://example.com/document.xml");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void NextTest7()
    {
        // After looking in the next catalogs, continue in the following catalogs
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/found-in-following.dtd");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That(true, Is.EqualTo(result.ToString().EndsWith("cm/following.dtd")));
            }
        }
    }

    [Test]
    public void NextTest8()
    {
        // After looking in the delegated catalogs, do not return to the following catalogs
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/but/not/found/in/delegated/catalogs.dtd");
            Assert.That(result, Is.Null);
        }
    }

    [Test]
    public void DelegateSystemTest1()
    {
        var expected = UriUtils.Resolve(baseUri, "delegated-to-one.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/one/system.dtd");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void DelegateSystemTest2()
    {
        var expected = UriUtils.Resolve(baseUri, "delegated-to-two.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/two/system.dtd");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void DelegateSystemTest3()
    {
        var expected = UriUtils.Resolve(baseUri, "three-from-two.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/three/system.dtd");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void DelegateSystemTest4()
    {
        var expected = UriUtils.Resolve(baseUri, "test-from-two.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/one/test/system.dtd");
            Assert.That(expected, Is.EqualTo(result));
        }
    }

    [Test]
    public void DelegateSystemTest5()
    {
        // This Uri is in nextone.xml, but because nextroot.xml delegates to different catalogs,
        // it's never seen by the resolver.
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/four/system.dtd");
            Assert.That(result, Is.Null);
        }
    }

    [Test]
    public void DelegateSystemTest6()
    {
        var expected = UriUtils.Resolve(baseUri, "five-from-two.dtd");
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var result = manager.LookupSystem("http://example.com/delegated/five/system.dtd");
            Assert.That(expected, Is.EqualTo(result));
            
        }
    }
}