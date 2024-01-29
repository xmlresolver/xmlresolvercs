using System.Reflection;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class CatalogQuerierTests : XmlResolverTest
{
    private Uri? _catalog1;
    private Uri? _catalog2;
    private Uri? _catLoc;
    private CatalogManager? _manager;

    private Uri Catalog1
    {
        get
        {
            _catalog1 ??= UriUtils.Resolve(TestRootDirectory, "Tests/resources/lookup1.xml");
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
            _catalog2 ??= UriUtils.Resolve(TestRootDirectory, "Tests/resources/lookup2.xml");
            if (_catalog2 == null)
            {
                throw new NullReferenceException("Failed to load catalog /resources/lookup2.xml");
            }

            return _catalog2;
        }
    }

    private Uri CatLoc
    {
        get
        {
            _catLoc ??= UriUtils.Resolve(UriUtils.Cwd(), Catalog1);
            if (_catLoc == null)
            {
                throw new NullReferenceException("Failed to load catLoc from Catalog1");
            }

            return _catLoc;
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
        Uri expected = UriUtils.Resolve(Catalog1, "sample10/sample-system.dtd");
        Assert.That(expected, Is.EqualTo(result));
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
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-public.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_nopublic1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_nopublic2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_public_system_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd",
            "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-public.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_public_system_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Not Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_public_system_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_system_nosystem_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-public.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_system_nosystem_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void LookupPublic_prefer_system_nosystem_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void LookupPublic_prefer_system_system_nopublic1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Assert.Pass(); // N/A
    }

    [Test]
    public void LookupPublic_prefer_system_system_nopublic2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_system_system_nopublic3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_system_system_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri? result = Manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd",
            "-//Sample//DTD Prefer Sample 1.0//EN");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void LookupPublic_prefer_system_system_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Not Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void LookupPublic_prefer_system_system_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri? result = Manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
            "-//Sample//DTD Prefer Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-system.dtd"), Is.EqualTo(result));
    }

    // ============================================================

    [Test]
    public void RewriteSystem()
    {
        Uri? result = Manager.LookupSystem("https://example.com/path1/sample/3.0/sample.dtd");
        Uri expected = new Uri("https://example.com/path2/sample/3.0/sample.dtd");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void SystemSuffix()
    {
        Uri? result = Manager.LookupSystem("https://example.com/whatever/you/want/suffix.dtd");
        Assert.That(UriUtils.Resolve(CatLoc, "sample20/sample-suffix.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void DelegatePublicLong()
    {
        Uri? result = Manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 1.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-delegated.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void DelegatePublicShort()
    {
        Uri? result = Manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 2.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample20/sample-shorter.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void DelegatePublicFail()
    {
        Uri? result = Manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void DelegateSystemLong()
    {
        Uri? result = Manager.LookupPublic("https://example.com/delegated/sample/1.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-delegated.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void DelegateSystemShort()
    {
        Uri? result = Manager.LookupPublic("https://example.com/delegated/sample/2.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(CatLoc, "sample20/sample-shorter.dtd"), Is.EqualTo(result));
    }

    [Test]
    public void DelegateSystemFail()
    {
        Uri? result = Manager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Undelegated()
    {
        // If there aren't any delegate entries, the entries in lookup2.xml really do match.
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        Stream? data = ResourceAccess.GetStream(Config, Catalog2);
        if (data == null)
        {
            throw new NullReferenceException("Failed to load catalog2: " + Catalog2);
        }
        uconfig.AddCatalog(Catalog2, data);

        CatalogManager uManager = new CatalogManager(uconfig);

        Uri? result = uManager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
        Assert.That(UriUtils.Resolve(CatLoc, "sample30/fail.dtd"), Is.EqualTo(result));

        result = uManager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
        Assert.That(UriUtils.Resolve(CatLoc, "sample30/fail.dtd"), Is.EqualTo(result));
    }

    // ============================================================

    [Test]
    public void LookupUri()
    {
        Uri? result = Manager.LookupUri("https://xmlresolver.org/ns/sample/sample.rng");
        Assert.That(UriUtils.Resolve(CatLoc, "sample/sample.rng"), Is.EqualTo(result));
    }

    [Test]
    public void RewriteUri()
    {
        Uri? result = Manager.LookupUri("https://example.com/path1/sample/sample.rng");
        Uri expected = new Uri("https://example.com/path2/sample/sample.rng");
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public void UriSuffix()
    {
        Uri? result = Manager.LookupUri("https://example.com/whatever/you/want/suffix.rnc");
        Assert.That(UriUtils.Resolve(CatLoc, "sample20/sample-suffix.rnc"), Is.EqualTo(result));
    }

    [Test]
    public void DelegateLongUri()
    {
        Uri? result = Manager.LookupUri("https://example.com/delegated/sample/1.0/sample.rng");
        Assert.That(UriUtils.Resolve(CatLoc, "sample10/sample-delegated.rng"), Is.EqualTo(result));
    }

    [Test]
    public void DelegateShortUri()
    {
        Uri? result = Manager.LookupUri("https://example.com/delegated/sample/2.0/sample.rng");
        Assert.That(UriUtils.Resolve(CatLoc, "sample20/sample-shorter.rng"), Is.EqualTo(result));
    }

    [Test]
    public void DelegateUriFail()
    {
        Uri? result = Manager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void UndelegatedUri()
    {
        // If there aren't any delegate entries, the entries in lookup2.xml really do match.
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        Stream? data = ResourceAccess.GetStream(Config, Catalog2);
        if (data == null)
        {
            throw new NullReferenceException("Failed to load catalog2: " + Catalog2);
        }
        uconfig.AddCatalog(Catalog2, data);

        CatalogManager uManager = new CatalogManager(uconfig);

        Uri? result = uManager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
        Assert.That(UriUtils.Resolve(CatLoc, "sample30/fail.rng"), Is.EqualTo(result));
    }

    [Test]
    public void BaseUriRootTest()
    {
        // Make sure an xml:base attribute on the root element works
        List<string> catalog = new List<string>();
        catalog.Add(UriUtils.Resolve(CatLoc, "lookup-test.xml").ToString());

        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), catalog);
        CatalogManager? uManager = (CatalogManager?)uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (uManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = uManager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd",
                "-//W3C//DTD SVG 1.1//EN");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That("/usr/local/DTDs/svg11/system-svg11.dtd", Is.EqualTo(result.AbsolutePath));
            }
        }

    }

    [Test]
    public void BaseUriGroupTest()
    {
        // Make sure an xml:base attribute on a group element works
        List<string> catalog = new List<string>();
        catalog.Add(UriUtils.Resolve(CatLoc, "lookup-test.xml").ToString());

        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), catalog);
        CatalogManager? uManager = (CatalogManager?)uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (uManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = uManager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd",
                "-//W3C//DTD SVG 1.1 Basic//EN");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That("/usr/local/nested/DTDs/svg11/system-svg11-basic.dtd", Is.EqualTo(result.AbsolutePath));
            }
        }
    }


    [Test]
    public void BaseUriOnElementTest()
    {
        // Make sure an xml:base attribute on the actual element works
        List<string> catalog = new List<string>();
        catalog.Add(UriUtils.Resolve(CatLoc, "lookup-test.xml").ToString());

        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), catalog);
        CatalogManager? uManager = (CatalogManager?)uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (uManager == null)
        {
            Assert.Fail();
        }
        else
        {
            Uri? result = uManager.LookupSystem("https://example.com/test.dtd");
            if (result == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That("/usr/local/on/DTDs/test.dtd", Is.EqualTo(result.AbsolutePath));
            }
        }

    }
}