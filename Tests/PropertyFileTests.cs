using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class PropertyFileTest : XmlResolverTest
{
    [Test]
    public void TestRelativeCatalogs()
    {
        var pfiles = new List<Uri>();
        pfiles.Add(UriUtils.Resolve(TestRootDirectory, "Tests/resources/pfile-rel.xml"));
        var config = new XmlResolverConfiguration(pfiles, null);
        var catalogs = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES)!;
        string s1 = UriUtils.Resolve(TestRootDirectory, "Tests/resources/catalog.xml").ToString();
        bool f1 = false;
        string s2 = UriUtils.Resolve(TestRootDirectory, "Tests/resources/a/catalog.xml").ToString();
        bool f2 = false;
        foreach (string cat in catalogs)
        {
            f1 = f1 || cat.EndsWith(s1);
            f2 = f2 || cat.EndsWith(s2);
        }

        Assert.That(f1, Is.EqualTo(true));
        Assert.That(f2, Is.EqualTo(true));
    }

    [Test]
    public void TestNotRelativeCatalogs()
    {
        var pfiles = new List<Uri>();
        pfiles.Add(UriUtils.Resolve(TestRootDirectory, "Tests/resources/pfile-abs.xml"));
        var config = new XmlResolverConfiguration(pfiles, null);
        var catalogs = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES)!;
        string s1 = "./catalog.xml";
        bool f1 = false;
        string s2 = "a/catalog.xml";
        bool f2 = false;
        foreach (string cat in catalogs)
        {
            f1 = f1 || cat.Equals(s1);
            f2 = f2 || cat.Equals(s2);
        }

        Assert.That(f1, Is.EqualTo(true));
        Assert.That(f2, Is.EqualTo(true));
    }
}