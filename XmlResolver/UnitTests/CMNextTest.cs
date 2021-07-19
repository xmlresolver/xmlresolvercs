using System;
using System.Reflection;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CMNextTest {
        private static readonly Assembly asm = Assembly.GetExecutingAssembly();
        private readonly Uri baseUri = new Uri("file:///tmp/");
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            XmlResolverConfiguration config = new XmlResolverConfiguration();
            config.SetFeature(ResolverFeature.PREFER_PUBLIC, true);
            config.AddAssemblyCatalog("/resources/cm/nextroot.xml", asm);
            config.AddAssemblyCatalog("/resources/cm/following.xml", asm);
            manager = new CatalogManager(config);
        }

        [Test]
        public void NextTest1() {
            Uri expected = UriUtils.Resolve(baseUri, "public.dtd");
            Uri result = manager.LookupPublic("http://example.com/system-next.dtd", "-//EXAMPLE//DTD Example//EN");
            Assert.AreEqual(expected, result);
        }
    }
}