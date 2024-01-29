using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class FeatureTest
{
    private XmlResolverConfiguration config;

    [SetUp]
    public void Setup()
    {
        config = new XmlResolverConfiguration();
        config.SetFeature(ResolverFeature.USE_DATA_ASSEMBLY, false);
    }

    private void BooleanFeature(BoolResolverFeature feature)
    {
        bool orig = (bool)config.GetFeature(feature);
        config.SetFeature(feature, false);
        Assert.That(false, Is.EqualTo(config.GetFeature(feature)));
        config.SetFeature(feature, true);
        Assert.That(true, Is.EqualTo(config.GetFeature(feature)));
        config.SetFeature(feature, orig);
    }

    private void StringFeature(StringResolverFeature feature)
    {
        string orig = (string)config.GetFeature(feature);
        config.SetFeature(feature, "apple pie");
        Assert.That("apple pie", Is.EqualTo(config.GetFeature(feature)));
        config.SetFeature(feature, orig);
    }

    [Test]
    public void TestAllowCatalogPi()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.ALLOW_CATALOG_PI)));
        BooleanFeature(ResolverFeature.ALLOW_CATALOG_PI);
    }

    [Test]
    public void TestArchivedCatalogs()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.ARCHIVED_CATALOGS)));
        BooleanFeature(ResolverFeature.ARCHIVED_CATALOGS);
    }

    [Test]
    public void TestMaskPackUris()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.MASK_PACK_URIS)));
        BooleanFeature(ResolverFeature.MASK_PACK_URIS);
    }

    [Test]
    public void TestMergeHttps()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.MERGE_HTTPS)));
        BooleanFeature(ResolverFeature.MERGE_HTTPS);
    }

    [Test]
    public void TestParseRddl()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.PARSE_RDDL)));
        BooleanFeature(ResolverFeature.PARSE_RDDL);
    }

    [Test]
    public void TestPreferPropertyFile()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.PREFER_PROPERTY_FILE)));
        BooleanFeature(ResolverFeature.PREFER_PROPERTY_FILE);
    }

    [Test]
    public void TestPreferPublic()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.PREFER_PUBLIC)));
        BooleanFeature(ResolverFeature.PREFER_PUBLIC);
    }

    [Test]
    public void TestUriForSystem()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.URI_FOR_SYSTEM)));
        BooleanFeature(ResolverFeature.URI_FOR_SYSTEM);
    }

    [Test]
    public void TestAssemblyCatalog()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.ASSEMBLY_CATALOGS)));

        List<string> orig = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);

        List<string> myCatalogs = new List<string> { "One", "Two", "Three" };
        config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, myCatalogs);
        List<string> current = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
        sameLists(current, myCatalogs);

        myCatalogs = new List<string> { "Alpha", "Beta", "Gamma" };
        config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, myCatalogs);
        current = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
        sameLists(current, myCatalogs);

        // For backwards compatibility, this feature accepts either a string or a list of strings
        // If you provide a single string, it adds that string to the list.

        config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "Spoon");
        myCatalogs = new List<string> { "Alpha", "Beta", "Gamma", "Spoon" };
        current = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
        sameLists(current, myCatalogs);

        config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, orig);
    }

    [Test]
    public void TestCatalogLoaderClass()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.CATALOG_LOADER_CLASS)));
        StringFeature(ResolverFeature.CATALOG_LOADER_CLASS);
    }

    private void sameLists(List<string> list1, List<string> list2)
    {
        // Hack
        foreach (var cur in list1)
        {
            Assert.That(true, Is.EqualTo(list2.Contains(cur)));
        }

        foreach (var cur in list2)
        {
            Assert.That(true, Is.EqualTo(list1.Contains(cur)));
        }
    }

    [Test]
    public void TestFeatureCatalogFiles()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.CATALOG_FILES)));

        List<string> orig = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
        List<string> myList = new List<string> { "One", "Two", "Three" };
        config.SetFeature(ResolverFeature.CATALOG_FILES, myList);
        List<string> current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
        sameLists(current, myList);

        // Setting CATALOG_FILES replaces the list
        List<string> newList = new List<string> { "Alpha", "Beta", "Gamma" };
        config.SetFeature(ResolverFeature.CATALOG_FILES, newList);
        current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);

        // None of "mine" survived
        foreach (var cur in myList)
        {
            Assert.That(false, Is.EqualTo(current.Contains(cur)));
        }

        sameLists(current, newList);

        config.SetFeature(ResolverFeature.CATALOG_FILES, orig);
    }

    [Test]
    public void TestFeatureCatalogAdditions()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.CATALOG_ADDITIONS)));

        List<string> origCatalogs = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
        List<string> origAdditions = (List<string>)config.GetFeature(ResolverFeature.CATALOG_ADDITIONS);

        List<string> myCatalogs = new List<string> { "One", "Two", "Three" };
        config.SetFeature(ResolverFeature.CATALOG_FILES, myCatalogs);
        List<string> current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
        sameLists(current, myCatalogs);

        // Setting CATALOG_ADDITIONS augments the list of catalogs
        List<string> myList = new List<string> { "Alpha", "Beta", "Gamma" };
        config.SetFeature(ResolverFeature.CATALOG_ADDITIONS, myList);
        current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);

        foreach (var mine in myCatalogs)
        {
            Assert.That(true, Is.EqualTo(current.Contains(mine)));
        }

        foreach (var mine in myList)
        {
            Assert.That(true, Is.EqualTo(current.Contains(mine)));
        }

        config.SetFeature(ResolverFeature.CATALOG_FILES, origCatalogs);
        config.SetFeature(ResolverFeature.CATALOG_ADDITIONS, origAdditions);
    }

    [Test]
    public void TestFeatureCatalogManager()
    {
        Assert.That(true, Is.EqualTo(config.GetFeatures().Contains(ResolverFeature.CATALOG_MANAGER)));
        CatalogManager manager = new CatalogManager(config);
        CatalogManager orig = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        config.SetFeature(ResolverFeature.CATALOG_MANAGER, manager);
        Assert.That(true, Is.EqualTo(manager == config.GetFeature(ResolverFeature.CATALOG_MANAGER)));
        config.SetFeature(ResolverFeature.CATALOG_MANAGER, orig);
    }
}