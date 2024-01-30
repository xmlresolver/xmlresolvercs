using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace Tests;

public class ResolverTestJar : XmlResolverTest {
    private System.Xml.XmlResolver? _resolver = null;

    [SetUp]
    public void Setup() {
        XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
        config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
        config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        _resolver = new XmlResolver.XmlResolver(config).GetXmlResolver();
    }
        
    [Test]
    public void LookupSystem() {
        string systemId = "https://xmlresolver.org/ns/assembly-sample/sample.dtd";

        if (_resolver == null)
        {
            Assert.Fail();
        }
        else
        {
            try {
                var stream = _resolver.GetEntity(UriUtils.NewUri(systemId), null, typeof(Stream));
                Assert.That(stream, Is.Not.Null);
            } catch (Exception) {
                Assert.Fail();
            }
        }
    }
}
