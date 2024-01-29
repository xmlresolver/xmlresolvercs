using System.Reflection;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class MergeHttpsTests
{
    private Uri? _catUri = null;
    private CatalogManager? _mergeManager = null;
    private CatalogManager? _noMergeManager = null;

    // For convenience, this set of tests uses the data catalog because it's known to contain
    // both http: and https: Uris.
    private Uri CatUri
    {
        get
        {
            if (_catUri == null)
            {
                try
                {
                    Assembly asm = Assembly.Load("XmlResolverData");
                    _catUri = UriUtils.GetLocationUri("Org.XmlResolver.catalog.xml", asm);
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }

            return _catUri!;
        }
    }
    
    private CatalogManager MergeManager
    {
        get
        {
            if (_mergeManager == null)
            {
                string catalog = CatUri.ToString();
                var catList = new List<string>();
                catList.Add(catalog);

                XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
                config.SetFeature(ResolverFeature.CATALOG_FILES, catList);
                config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
                config.SetFeature(ResolverFeature.MERGE_HTTPS, true);
                _mergeManager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER)!;
            }

            return _mergeManager;
        }
    }

    private CatalogManager NoMergeManager
    {
        get
        {
            if (_noMergeManager == null)
            {
                string catalog = CatUri.ToString();
                var catList = new List<string>();
                catList.Add(catalog);

                XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
                config.SetFeature(ResolverFeature.CATALOG_FILES, catList);
                config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
                config.SetFeature(ResolverFeature.MERGE_HTTPS, false);
                _noMergeManager = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER)!;
            }

            return _noMergeManager;
        }
    }

    [Test]
    public void Equivalentcp1()
    {
        Assert.That("classpath:path/to/thing",
            Is.EqualTo(NoMergeManager.NormalizedForComparison("classpath:/path/to/thing")));
    }

    [Test]
    public void Equivalentcp2()
    {
        Assert.That("classpath:path/to/thing",
            Is.EqualTo(NoMergeManager.NormalizedForComparison("classpath:path/to/thing")));
    }

    [Test]
    public void Equivalentcp1m()
    {
        Assert.That("classpath:path/to/thing",
            Is.EqualTo(MergeManager.NormalizedForComparison("classpath:/path/to/thing")));
    }

    [Test]
    public void Equivalentcp2m()
    {
        Assert.That("classpath:path/to/thing", Is.EqualTo(MergeManager.NormalizedForComparison("classpath:path/to/thing")));
    }

    [Test]
    public void Equivalenthttp1()
    {
        Assert.That("https://localhost/path/to/thing",
            Is.EqualTo(NoMergeManager.NormalizedForComparison("https://localhost/path/to/thing")));
    }

    [Test]
    public void Equivalenthttp2()
    {
        Assert.That("http://localhost/path/to/thing",
            Is.EqualTo(NoMergeManager.NormalizedForComparison("http://localhost/path/to/thing")));
    }

    [Test]
    public void Equivalenthttp3()
    {
        Assert.That("ftp://localhost/path/to/thing",
            Is.EqualTo(NoMergeManager.NormalizedForComparison("ftp://localhost/path/to/thing")));
    }

    [Test]
    public void Equivalenthttp1m()
    {
        Assert.That("https://localhost/path/to/thing",
            Is.EqualTo(MergeManager.NormalizedForComparison("https://localhost/path/to/thing")));
    }

    [Test]
    public void Equivalenthttp2m()
    {
        Assert.That("https://localhost/path/to/thing",
            Is.EqualTo(MergeManager.NormalizedForComparison("http://localhost/path/to/thing")));
    }

    [Test]
    public void Equivalenthttp3m()
    {
        Assert.That("ftp://localhost/path/to/thing",
            Is.EqualTo(MergeManager.NormalizedForComparison("ftp://localhost/path/to/thing")));
    }


    [Test]
    public void LookupHttpUri()
    {
        var result = MergeManager.LookupUri("http://www.w3.org/2001/xml.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void LookupHttpSystem()
    {
        var result = MergeManager.LookupSystem("http://www.w3.org/MarkUp/DTD/xhtml-basic11.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void LookupHttpsSystem()
    {
        var result = MergeManager.LookupSystem("https://www.rddl.org/rddl-xhtml.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void LookupHttpUriNoMerge()
    {
        var result = NoMergeManager.LookupUri("http://www.w3.org/2001/xml.xsd");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void LookupHttpSystemNoMerge()
    {
        var result = NoMergeManager.LookupSystem("http://www.w3.org/MarkUp/DTD/xhtml-basic11.dtd");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void LookupHttpsSystemNoMerge()
    {
        var result = NoMergeManager.LookupSystem("https://www.rddl.org/rddl-xhtml.dtd");
        Assert.That(result, Is.Null);
    }
}