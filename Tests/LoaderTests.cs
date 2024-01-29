using System.Reflection;
using XmlResolver;
using XmlResolver.Features;

namespace Tests;


public class LoaderTest
{
    [Test]
    public void NonValidatingValidCatalog()
    {
        XmlResolverConfiguration config = new(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/catalog.xml", Assembly.GetExecutingAssembly());
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
        Assert.That(rsrc, Is.Not.Null);
    }

    [Test]
    public void NonValidatingInvalidCatalog()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/invalid-catalog.xml", Assembly.GetExecutingAssembly());
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
        Assert.That(rsrc, Is.Not.Null);
    }

    [Test]
    public void validatingValidCatalog()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/catalog.xml", Assembly.GetExecutingAssembly());
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
        Assert.That(rsrc, Is.Not.Null);
    }

    [Test]
    public void validatingDtd10ValidCatalog()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/dtd10catalog.xml", Assembly.GetExecutingAssembly());
        config.SetFeature(ResolverFeature.CATALOG_LOADER_CLASS, "XmlResolver.Loaders.ValidatingXmlLoader");
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
        Assert.That(rsrc, Is.Not.Null);
    }

    [Test]
    public void validatingDtd11ValidCatalog()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/dtd11catalog.xml", Assembly.GetExecutingAssembly());
        config.SetFeature(ResolverFeature.CATALOG_LOADER_CLASS, "XmlResolver.Loaders.ValidatingXmlLoader");
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
        Assert.That(rsrc, Is.Not.Null);
    }

    //FIXME: [Test]
    // This test isn't being run because the C# implementation doesn't do catalog validation
    public void validatingInvalidCatalog()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/invalid-catalog.xml", Assembly.GetExecutingAssembly());
        config.SetFeature(ResolverFeature.CATALOG_LOADER_CLASS, "Org.XmlResolver.Loaders.ValidatingXmlLoader");
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        try
        {
            Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
            Assert.Fail();
        }
        catch (Exception)
        {
            // pass
        }
    }

    [Test]
    public void validatingMissingCatalog()
    {
        // File not found isn't a validation error
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddCatalog("./not-a-catalog-in-sight.xml");
        config.AddAssemblyCatalog("resources/catalog.xml", Assembly.GetExecutingAssembly());
        CatalogManager manager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        try
        {
            Uri rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }
}