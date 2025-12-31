using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace UnitTests;

public class CatalogQuerierTest : BaseTestRoot
{
    private Uri catalog1 = null; 
    private Uri catalog2 = null;
    private Uri catloc = null;
    private XmlResolverConfiguration config = null;
    private CatalogManager manager = null;

    [SetUp]
    public void Setup() {
        catalog1 = UriUtils.Resolve(TEST_ROOT_DIRECTORY, "UnitTests/resources/lookup1.xml");
        catalog2 = UriUtils.Resolve(TEST_ROOT_DIRECTORY, "UnitTests/resources/lookup2.xml");
        catloc = UriUtils.Resolve(UriUtils.Cwd(), catalog1);
        
        config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddCatalog(catalog1, ResourceAccess.GetStream(catalog1));
        config.AddCatalog(catalog2, ResourceAccess.GetStream(catalog2));
        config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
        manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
    }

    [Test]
    public void LookupSystem()
    {
        Uri result = manager.LookupSystem("https://example.com/sample/1.0/sample.dtd");
        Uri expected = UriUtils.Resolve(catalog1, "sample10/sample-system.dtd");
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void LookupSystemMiss()
    {
        Uri result = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.rng");
        Assert.Null(result);
    }

    // ============================================================
    // See https://www.oasis-open.org/committees/download.php/14809/xml-catalogs.html#attrib.prefer
    // Note that the N/A entries in column three are a bit misleading.

    [Test]
    public void LookupPublic_prefer_public_nosystem_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri result = manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-public.dtd"), result);
    }
    
    [Test]
    public void LookupPublic_prefer_public_nosystem_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Assert.True(true); // N/A
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Assert.True(true); // N/A
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_nopublic1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Assert.True(true); // N/A
    }

    [Test]
    public void LookupPublic_prefer_public_nosystem_nopublic2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_public_system_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri result = manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd", "-//Sample//DTD Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-public.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_public_system_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", "-//Sample//DTD Not Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_public_system_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", "-//Sample//DTD Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_system_nosystem_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri result = manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-public.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_system_nosystem_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Assert.True(true); // N/A
    }

    [Test]
    public void LookupPublic_prefer_system_nosystem_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Assert.True(true); // N/A
    }

    [Test]
    public void LookupPublic_prefer_system_system_nopublic1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Assert.True(true); // N/A
    }

    [Test]
    public void LookupPublic_prefer_system_system_nopublic2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_system_system_nopublic3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_system_system_public1()
    {
        // Catalog contains a matching public entry, but not a matching system entry
        Uri result = manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd", "-//Sample//DTD Prefer Sample 1.0//EN");
        Assert.Null(result);
    }

    [Test]
    public void LookupPublic_prefer_system_system_public2()
    {
        // Catalog contains a matching system entry, but not a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", "-//Sample//DTD Not Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    [Test]
    public void LookupPublic_prefer_system_system_public3()
    {
        // Catalog contains both a matching system entry and a matching public entry
        Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", "-//Sample//DTD Prefer Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-system.dtd"), result);
    }

    // ============================================================

    [Test]
    public void RewriteSystem()
    {
        Uri result = manager.LookupSystem("https://example.com/path1/sample/3.0/sample.dtd");
        Uri expected = new Uri("https://example.com/path2/sample/3.0/sample.dtd");
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void SystemSuffix()
    {
        Uri result = manager.LookupSystem("https://example.com/whatever/you/want/suffix.dtd");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample20/sample-suffix.dtd"), result);
    }

    [Test]
    public void DelegatePublicLong()
    {
        Uri result = manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 1.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-delegated.dtd"), result);
    }

    [Test]
    public void DelegatePublicShort()
    {
        Uri result = manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 2.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample20/sample-shorter.dtd"), result);
    }

    [Test]
    public void DelegatePublicFail()
    {
        Uri result = manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
        Assert.Null(result);
    }
    
    [Test]
    public void DelegateSystemLong()
    {
        Uri result = manager.LookupPublic("https://example.com/delegated/sample/1.0/sample.dtd", null);
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-delegated.dtd"), result);
    }

    [Test]
    public void DelegateSystemShort()
    {
        Uri result = manager.LookupPublic("https://example.com/delegated/sample/2.0/sample.dtd", null);
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample20/sample-shorter.dtd"), result);
    }

    [Test]
    public void DelegateSystemFail()
    {
        Uri result = manager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
        Assert.Null(result);
    }
    
    [Test]
    public void Undelegated()
    {
        // If there aren't any delegate entries, the entries in lookup2.xml really do match.
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        uconfig.AddCatalog(catalog2, ResourceAccess.GetStream(catalog2));
        CatalogManager umanager = new CatalogManager(uconfig);

        Uri result = umanager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample30/fail.dtd"), result);

        result = umanager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample30/fail.dtd"), result);
    }

    // ============================================================

    [Test]
    public void LookupUri()
    {
        Uri result = manager.LookupUri("https://xmlresolver.org/ns/sample/sample.rng");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample/sample.rng"), result);
    }
    
    [Test]
    public void RewriteUri()
    {
        Uri result = manager.LookupUri("https://example.com/path1/sample/sample.rng");
        Uri expected = new Uri("https://example.com/path2/sample/sample.rng");
        Assert.AreEqual(expected, result);
    }
    
    [Test]
    public void UriSuffix()
    {
        Uri result = manager.LookupUri("https://example.com/whatever/you/want/suffix.rnc");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample20/sample-suffix.rnc"), result);
    }
    
    [Test]
    public void DelegateLongUri()
    {
        Uri result = manager.LookupUri("https://example.com/delegated/sample/1.0/sample.rng");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample10/sample-delegated.rng"), result);
    }
    
    [Test]
    public void DelegateShortUri()
    {
        Uri result = manager.LookupUri("https://example.com/delegated/sample/2.0/sample.rng");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample20/sample-shorter.rng"), result);
    }

    [Test]
    public void DelegateUriFail()
    {
        Uri result = manager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
        Assert.Null(result);
    }

    [Test]
    public void UndelegatedUri()
    {
        // If there aren't any delegate entries, the entries in lookup2.xml really do match.
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        uconfig.AddCatalog(catalog2, ResourceAccess.GetStream(catalog2));
        CatalogManager umanager = new CatalogManager(uconfig);

        Uri result = umanager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
        Assert.AreEqual(UriUtils.Resolve(catloc, "sample30/fail.rng"), result);
    }

    [Test]
    public void BaseUriRootTest()
    {
        // Make sure an xml:base attribute on the root element works
        List<string> catalog = new List<string>();
        catalog.Add(UriUtils.Resolve(catloc, "lookup-test.xml").ToString());
        
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), catalog);
        CatalogManager umanager = (CatalogManager) uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        
        Uri result = umanager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", "-//W3C//DTD SVG 1.1//EN");
        Assert.AreEqual("/usr/local/DTDs/svg11/system-svg11.dtd", result.AbsolutePath);
    }

    [Test]
    public void BaseUriGroupTest()
    {
        // Make sure an xml:base attribute on a group element works
        List<string> catalog = new List<string>();
        catalog.Add(UriUtils.Resolve(catloc, "lookup-test.xml").ToString());
        
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), catalog);
        CatalogManager umanager = (CatalogManager) uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        
        Uri result = umanager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd", "-//W3C//DTD SVG 1.1 Basic//EN");
        Assert.AreEqual("/usr/local/nested/DTDs/svg11/system-svg11-basic.dtd", result.AbsolutePath);
    }


    [Test]
    public void BaseUriOnElementTest()
    {
        // Make sure an xml:base attribute on the actual element works
        List<string> catalog = new List<string>();
        catalog.Add(UriUtils.Resolve(catloc, "lookup-test.xml").ToString());
        
        XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new List<Uri>(), catalog);
        CatalogManager umanager = (CatalogManager) uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
        
        Uri result = umanager.LookupSystem("https://example.com/test.dtd");
        Assert.AreEqual("/usr/local/on/DTDs/test.dtd", result.AbsolutePath);
    }
}
