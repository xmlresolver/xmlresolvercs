using XmlResolver;
using XmlResolver.Features;

namespace Tests;

public class FeatureTest
{
    private XmlResolverConfiguration? _config;

    private XmlResolverConfiguration Config
    {
        get
        {
            if (_config == null)
            {
                _config = new XmlResolverConfiguration();
                _config.SetFeature(ResolverFeature.USE_DATA_ASSEMBLY, false);
            }

            return _config;
        }
    }

    private void BooleanFeature(BoolResolverFeature feature)
    {
        var orig = (bool?)Config.GetFeature(feature)!;
        Config.SetFeature(feature, false);
        Assert.That(false, Is.EqualTo(Config.GetFeature(feature)));
        Config.SetFeature(feature, true);
        Assert.That(true, Is.EqualTo(Config.GetFeature(feature)));
        Config.SetFeature(feature, orig);
    }

    private void StringFeature(StringResolverFeature feature)
    {
        var orig = (string?)Config.GetFeature(feature);
        Config.SetFeature(feature, "apple pie");
        Assert.That("apple pie", Is.EqualTo(Config.GetFeature(feature)));
        Config.SetFeature(feature, orig);
    }

    [Test]
    public void TestAllowCatalogPi()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.ALLOW_CATALOG_PI)));
        BooleanFeature(ResolverFeature.ALLOW_CATALOG_PI);
    }

    [Test]
    public void TestArchivedCatalogs()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.ARCHIVED_CATALOGS)));
        BooleanFeature(ResolverFeature.ARCHIVED_CATALOGS);
    }

    [Test]
    public void TestMaskPackUris()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.MASK_PACK_URIS)));
        BooleanFeature(ResolverFeature.MASK_PACK_URIS);
    }

    [Test]
    public void TestMergeHttps()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.MERGE_HTTPS)));
        BooleanFeature(ResolverFeature.MERGE_HTTPS);
    }

    [Test]
    public void TestParseRddl()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.PARSE_RDDL)));
        BooleanFeature(ResolverFeature.PARSE_RDDL);
    }

    [Test]
    public void TestPreferPropertyFile()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.PREFER_PROPERTY_FILE)));
        BooleanFeature(ResolverFeature.PREFER_PROPERTY_FILE);
    }

    [Test]
    public void TestPreferPublic()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.PREFER_PUBLIC)));
        BooleanFeature(ResolverFeature.PREFER_PUBLIC);
    }

    [Test]
    public void TestUriForSystem()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.URI_FOR_SYSTEM)));
        BooleanFeature(ResolverFeature.URI_FOR_SYSTEM);
    }

    [Test]
    public void TestAssemblyCatalog()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.ASSEMBLY_CATALOGS)));

        var orig = (List<string>)Config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS)!;

        var myCatalogs = new List<string> { "One", "Two", "Three" };
        Config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, myCatalogs);
        var current = (List<string>)Config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS)!;
        SameLists(current, myCatalogs);

        myCatalogs = new List<string> { "Alpha", "Beta", "Gamma" };
        Config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, myCatalogs);
        current = (List<string>)Config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS)!;
        SameLists(current, myCatalogs);

        // For backwards compatibility, this feature accepts either a string or a list of strings
        // If you provide a single string, it adds that string to the list.

        Config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "Spoon");
        myCatalogs = new List<string> { "Alpha", "Beta", "Gamma", "Spoon" };
        current = (List<string>)Config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS)!;
        SameLists(current, myCatalogs);

        Config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, orig);
    }

    [Test]
    public void TestCatalogLoaderClass()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.CATALOG_LOADER_CLASS)));
        StringFeature(ResolverFeature.CATALOG_LOADER_CLASS);
    }

    private void SameLists(List<string> list1, List<string> list2)
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
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.CATALOG_FILES)));

        var orig = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_FILES)!;
        var myList = new List<string> { "One", "Two", "Three" };
        Config.SetFeature(ResolverFeature.CATALOG_FILES, myList);
        var current = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_FILES)!;
        SameLists(current, myList);

        // Setting CATALOG_FILES replaces the list
        var newList = new List<string> { "Alpha", "Beta", "Gamma" };
        Config.SetFeature(ResolverFeature.CATALOG_FILES, newList);
        current = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_FILES)!;

        // None of "mine" survived
        foreach (var cur in myList)
        {
            Assert.That(false, Is.EqualTo(current.Contains(cur)));
        }

        SameLists(current, newList);

        Config.SetFeature(ResolverFeature.CATALOG_FILES, orig);
    }

    [Test]
    public void TestFeatureCatalogAdditions()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.CATALOG_ADDITIONS)));

        var origCatalogs = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_FILES)!;
        var origAdditions = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_ADDITIONS)!;

        var myCatalogs = new List<string> { "One", "Two", "Three" };
        Config.SetFeature(ResolverFeature.CATALOG_FILES, myCatalogs);
        var current = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_FILES)!;
        SameLists(current, myCatalogs);

        // Setting CATALOG_ADDITIONS augments the list of catalogs
        var myList = new List<string> { "Alpha", "Beta", "Gamma" };
        Config.SetFeature(ResolverFeature.CATALOG_ADDITIONS, myList);
        current = (List<string>)Config.GetFeature(ResolverFeature.CATALOG_FILES)!;

        foreach (var mine in myCatalogs)
        {
            Assert.That(true, Is.EqualTo(current.Contains(mine)));
        }

        foreach (var mine in myList)
        {
            Assert.That(true, Is.EqualTo(current.Contains(mine)));
        }

        Config.SetFeature(ResolverFeature.CATALOG_FILES, origCatalogs);
        Config.SetFeature(ResolverFeature.CATALOG_ADDITIONS, origAdditions);
    }

    [Test]
    public void TestFeatureCatalogManager()
    {
        Assert.That(true, Is.EqualTo(Config.GetFeatures().Contains(ResolverFeature.CATALOG_MANAGER)));
        CatalogManager manager = new CatalogManager(Config);
        CatalogManager orig = (CatalogManager)Config.GetFeature(ResolverFeature.CATALOG_MANAGER)!;
        Config.SetFeature(ResolverFeature.CATALOG_MANAGER, manager);
        Assert.That(true, Is.EqualTo(manager == Config.GetFeature(ResolverFeature.CATALOG_MANAGER)));
        Config.SetFeature(ResolverFeature.CATALOG_MANAGER, orig);
    }
}