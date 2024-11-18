using System.IO;
using System.Text;
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
        public void TestDataUritext() {
            string href = "example.txt";
            string baseuri = "http://example.com/";

            string line = null;
            ResolvedResource result = resolver.ResolveUri(href, baseuri);
            using (StreamReader reader = new StreamReader(result.GetInputStream())) {
                line = reader.ReadLine();
            }
            Assert.That(line, Is.EqualTo("A short note."));
        }
        
        [Test]
        public void TestDataUricharset() {
            string href = "greek.txt";
            string baseuri = "http://example.com/";

            // We don't do this in the resolver, but we do it here to demonstrate that
            // it will work in applications that use the resolver.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            string line = null;
            ResolvedResource result = resolver.ResolveUri(href, baseuri);
            using (StreamReader reader = new StreamReader(result.GetInputStream())) {
                line = reader.ReadLine();
            }
            Assert.That(line, Is.EqualTo("ΎχΎ"));
        }
        
        [Test]
        public void TestDataUriencoded() {
            string href = "example.xml";
            string baseuri = "http://example.com/";

            string line = null;
            ResolvedResource result = resolver.ResolveUri(href, baseuri);
            using (StreamReader reader = new StreamReader(result.GetInputStream())) {
                line = reader.ReadLine();
            }
            Assert.That(line, Is.EqualTo("<doc>I was a data URI</doc>"));
        }
    }
}
