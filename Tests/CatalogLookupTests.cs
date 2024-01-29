using System.Reflection;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class CatalogLookupTests : XmlResolverTest
{
    private Uri? _catalog1;
    private Uri? _catalog2;
    private CatalogManager? _manager;

    private Uri Catalog1
    {
        get
        {
            _catalog1 ??= UriUtils.GetLocationUri("/resources/lookup1.xml", Asm);
            if (_catalog1 == null)
            {
                throw new NullReferenceException("Failed to load catalog /resources/lookup1.xml");
            }

            return _catalog1;
        }
    }

    private Uri Catalog2
    {
        get
        {
            _catalog2 ??= UriUtils.GetLocationUri("/resources/lookup2.xml", Asm);
            if (_catalog2 == null)
            {
                throw new NullReferenceException("Failed to load catalog /resources/lookup2.xml");
            }

            return _catalog2;
        }
    }

    private CatalogManager Manager
    {
        get
        {
            _manager ??= (CatalogManager?)Config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            if (_manager == null)
            {
                throw new NullReferenceException("Failed to get CatalogManager");
            }

            return _manager;
        }
    }

    [SetUp]
    public void Setup()
    {
        Stream? data = ResourceAccess.GetStream(Config, Catalog1);
        if (data == null)
        {
            throw new NullReferenceException("Failed to load catalog1: " + Catalog1);
        }

        Config.AddCatalog(Catalog1, data);

        data = ResourceAccess.GetStream(Config, Catalog2);
        if (data == null)
        {
            throw new NullReferenceException("Failed to load catalog2: " + Catalog2);
        }

        Config.AddCatalog(Catalog2, data);

        Config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
    }

    [Test]
    public void LookupSystem()
    {
        Uri? result = Manager.LookupSystem("https://example.com/sample/1.0/sample.dtd");
        Console.WriteLine(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"));
        Console.WriteLine(result);
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupSystemMiss()
    {
        Uri? result = Manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.rng");
        Assert.That(result, Is.Null);
    }

    // ============================================================
    // See https://www.oasis-open.org/committees/download.php/14809/xml-catalogs.html#attrib.prefer
    // Note that the N/A entries in column three are a bit misleading.

    [Test]
    public void LookupPublic_prefer_public_nosystem_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-public.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_public_nosystem_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void lookupPublic_prefer_public_nosystem_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void lookupPublic_prefer_public_system_nopublic1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void lookupPublic_prefer_public_system_nopublic2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }


    [Test]
    public void lookupPublic_prefer_public_system_nopublic3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_public_system_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd",
            "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-public.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_public_system_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Not Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_public_system_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_system_nosystem_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-public.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_system_nosystem_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void lookupPublic_prefer_system_nosystem_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void lookupPublic_prefer_system_system_nopublic1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void lookupPublic_prefer_system_system_nopublic2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_system_system_nopublic3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_system_system_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd",
            "-//Sample//DTD Prefer Sample 1.0//EN");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void lookupPublic_prefer_system_system_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Not Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void lookupPublic_prefer_system_system_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Prefer Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    // ============================================================

    [Test]
    public void rewriteSystem()
    {
        Uri? result = Manager.LookupSystem("https://example.com/path1/sample/3.0/sample.dtd");
        Uri expected = new Uri("https://example.com/path2/sample/3.0/sample.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void systemSuffix()
    {
        Uri? result = Manager.LookupSystem("https://example.com/whatever/you/want/suffix.dtd");
        Assert.That(UriUtils.Resolve(Catalog1, "sample20/sample-suffix.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void delegatePublicLong()
    {
        Uri? result = Manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-delegated.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void delegatePublicShort()
    {
        Uri? result = Manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 2.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample20/sample-shorter.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void delegatePublicFail()
    {
        Uri? result = Manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void delegateSystemLong()
    {
        Uri? result = Manager.LookupPublic("https://example.com/delegated/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-delegated.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void delegateSystemShort()
    {
        Uri? result = Manager.LookupPublic("https://example.com/delegated/sample/2.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample20/sample-shorter.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void delegateSystemFail()
    {
        Uri? result = Manager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
        Assert.That(result, Is.Null);
    }

    /*
    [Test]
    public void undelegated() {
        // If there aren't any delegate entries, the entries in lookup2.xml really do match.
        XMLResolverConfiguration uconfig = new XMLResolverConfiguration(Collections.emptyList(), Collections.emptyList());
        uconfig.setFeature(ResolverFeature.CATALOG_FILES, Collections.singletonList(catalog2));
        CatalogManager umanager = new CatalogManager(uconfig);

        Uri? result = uManager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
        Assert.That(UriUtils.Resolve(Catalog1, "sample30/fail.dtd"), result);

        result = uManager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(Catalog1, "sample30/fail.dtd"), result);
    }
    */

    // ============================================================

    [Test]
    public void lookupUri()
    {
        Uri? result = Manager.LookupUri("https://xmlresolver.org/ns/sample/sample.rng");
        Assert.That(UriUtils.Resolve(UriUtils.Resolve(UriUtils.Cwd(), Catalog1), "sample/sample.rng"),
            Is.EqualTo(result));
    }

    [Test]
    public void rewriteUri()
    {
        Uri? result = Manager.LookupUri("https://example.com/path1/sample/sample.rng");
        Uri expected = new Uri("https://example.com/path2/sample/sample.rng");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void uriSuffix()
    {
        Uri? result = Manager.LookupUri("https://example.com/whatever/you/want/suffix.rnc");
        Assert.That(UriUtils.Resolve(Catalog1, "sample20/sample-suffix.rnc"), Is.EqualTo(result));
    }

    [Test]
    public void delegateUriLong()
    {
        Uri? result = Manager.LookupUri("https://example.com/delegated/sample/1.0/sample.rng");
        Assert.That(UriUtils.Resolve(Catalog1, "sample10/sample-delegated.rng"), Is.EqualTo(result));
    }

    [Test]
    public void delegateUriShort()
    {
        Uri? result = Manager.LookupUri("https://example.com/delegated/sample/2.0/sample.rng");
        Assert.That(UriUtils.Resolve(Catalog1, "sample20/sample-shorter.rng"), Is.EqualTo(result));
    }

    [Test]
    public void delegateUriFail()
    {
        Uri? result = Manager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void undelegatedUri()
    {
        // If there aren't any delegate entries, the entries in lookup2.xml really do match.
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new(), new());
        List<string> cats = new();
        cats.Add(Catalog2.ToString());
        uconfig.SetFeature(ResolverFeature.CATALOG_FILES, cats);

        CatalogManager? uManager = (CatalogManager?)uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (uManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = uManager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
            Assert.That(UriUtils.Resolve(Catalog1, "sample30/fail.rng"), Is.EqualTo(result));
        }

    }

    [Test]
    public void baseUriRootTest()
    {
        // Make sure an xml:base attribute on the root element works
        List<String> catalog = new();
        catalog.Add(UriUtils.Resolve(Catalog1, "lookup-test.xml").ToString());
        XmlResolverConfiguration config = new XmlResolverConfiguration(new(), catalog);
        CatalogManager? localManager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (localManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = localManager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd",
                "-//W3C//DTD SVG 1.1//EN");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That(result.AbsolutePath, Is.EqualTo("/usr/local/DTDs/svg11/system-svg11.dtd"));
            }
        }
    }

    [Test]
    public void baseUriGroupTest()
    {
        // Make sure an xml:base attribute on a group element works
        List<String> catalog = new();
        catalog.Add(UriUtils.Resolve(Catalog1, "lookup-test.xml").ToString());
        XmlResolverConfiguration config = new XmlResolverConfiguration(new(), catalog);
        CatalogManager? localManager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (localManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = localManager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd",
                "-//W3C//DTD SVG 1.1 Basic//EN");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That(result.AbsolutePath, Is.EqualTo("/usr/local/nested/DTDs/svg11/system-svg11-basic.dtd"));
            }
        }
    }

    [Test]
    public void baseUriOnElementTest()
    {
        // Make sure an xml:base attribute on the actual element works
        List<String> catalog = new();
        catalog.Add(UriUtils.Resolve(Catalog1, "lookup-test.xml").ToString());
        XmlResolverConfiguration config = new XmlResolverConfiguration(new(), catalog);
        CatalogManager? localManager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (localManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = localManager.LookupSystem("https://example.com/test.dtd");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That(result.AbsolutePath, Is.EqualTo("/usr/local/on/DTDs/test.dtd"));
            }
        }
    }
}
