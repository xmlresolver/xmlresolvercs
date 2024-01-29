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
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
        }
    }

    [Test]
    public void NonValidatingInvalidCatalog()
    {
        XmlResolverConfiguration config = new(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/invalid-catalog.xml", Assembly.GetExecutingAssembly());
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
        }
    }

    [Test]
    public void ValidatingValidCatalog()
    {
        var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/catalog.xml", Assembly.GetExecutingAssembly());
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
        }
    }

    [Test]
    public void ValidatingDtd10ValidCatalog()
    {
        var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/dtd10catalog.xml", Assembly.GetExecutingAssembly());
        config.SetFeature(ResolverFeature.CATALOG_LOADER_CLASS, "XmlResolver.Loaders.ValidatingXmlLoader");
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
        }
    }

    [Test]
    public void ValidatingDtd11ValidCatalog()
    {
        var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/dtd11catalog.xml", Assembly.GetExecutingAssembly());
        config.SetFeature(ResolverFeature.CATALOG_LOADER_CLASS, "XmlResolver.Loaders.ValidatingXmlLoader");
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
            Assert.That(rsrc, Is.Not.Null);
        }
    }

    //FIXME: [Test]
    // This test isn't being run because the C# implementation doesn't do catalog validation
    public void ValidatingInvalidCatalog()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddAssemblyCatalog("resources/invalid-catalog.xml", Assembly.GetExecutingAssembly());
        config.SetFeature(ResolverFeature.CATALOG_LOADER_CLASS, "Org.XmlResolver.Loaders.ValidatingXmlLoader");
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
                Assert.That(rsrc, Is.Not.Null);
                Assert.Fail();
            }
            catch (Exception)
            {
                // pass
            }
        }
    }

    [Test]
    public void ValidatingMissingCatalog()
    {
        // File not found isn't a validation error
        var config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.AddCatalog("./not-a-catalog-in-sight.xml");
        config.AddAssemblyCatalog("resources/catalog.xml", Assembly.GetExecutingAssembly());
        var manager = (CatalogManager?)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        if (manager == null)
        {
            Assert.Fail();
        }
        else
        {
            try
            {
                var rsrc = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.dtd");
                Assert.That(rsrc, Is.Not.Null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}