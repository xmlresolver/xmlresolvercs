using System.IO;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class DataUriTest : BaseTestRoot {
        private CatalogResolver resolver = null;

        [SetUp]
        public void Setup() {
            XmlResolverConfiguration config = new XmlResolverConfiguration(UriUtils.Resolve(TEST_ROOT_DIRECTORY, "XmlResolver/UnitTests/resources/datauri.xml").ToString());
            resolver = new CatalogResolver(config);
        }
        
        [Test]
        public void testDataUritext() {
            string href = "example.txt";
            string baseuri = "http://example.com/";

            string line = null;
            ResolvedResource result = resolver.ResolveUri(href, baseuri);
            using (StreamReader reader = new StreamReader(result.GetInputStream())) {
                line = reader.ReadLine();
            }
            Assert.AreEqual("A short note.", line);
        }
        
        // FIXME: .NET doesn't support iso-8859-7?
        /*
        [Test]
        public void testDataUricharset() {
            string href = "greek.txt";
            string baseuri = "http://example.com/";

            string line = null;
            ResolvedResource result = resolver.ResolveUri(href, baseuri);
            using (StreamReader reader = new StreamReader(result.GetInputStream())) {
                line = reader.ReadLine();
            }
            Assert.AreEqual("ΎχΎ", line);
        }
        */
        
        [Test]
        public void testDataUriencoded() {
            string href = "example.xml";
            string baseuri = "http://example.com/";

            string line = null;
            ResolvedResource result = resolver.ResolveUri(href, baseuri);
            using (StreamReader reader = new StreamReader(result.GetInputStream())) {
                line = reader.ReadLine();
            }
            Assert.AreEqual("<doc>I was a data URI</doc>", line);
        }
    }
}