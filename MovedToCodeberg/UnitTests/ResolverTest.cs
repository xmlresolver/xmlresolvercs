using System;
using System.Collections.Generic;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace UnitTests {
    public class ResolverTest : BaseTestRoot {
        public static readonly string catalog1 = "/UnitTests/resources/rescat.xml";
        private XmlResolverConfiguration config = null;
        private XmlResolver.XmlResolver resolver = null;
        
        [SetUp]
        public void BaseSetup() {
            List<string> catalogs = new List<string>();
            catalogs.Add(TEST_ROOT_PATH + catalog1);
            config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            resolver = new XmlResolver.XmlResolver(config);
        }
        
        [Test]
        public void LookupSystemFail() {
            try {
                IResourceResponse rsrc = resolver.ResolveEntity(null, null, "https://example.com/not/in/catalog", null);
                Assert.NotNull(rsrc);
                Assert.Null(rsrc.Stream);
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void LookupLocalSystemFail() {
            try {
                IResourceResponse rsrc = resolver.ResolveEntity(null, null, "file:///path/to/thing/that/isnt/likely/to/exist", null);
                Assert.NotNull(rsrc);
                Assert.Null(rsrc.Stream);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Test]
        public void LookupSystem() {
            try {
                Uri result = UriUtils.Resolve(TEST_ROOT_DIRECTORY, "UnitTests/resources/sample10/sample.dtd");
                IResourceResponse rsrc = resolver.ResolveEntity(null, null, "https://example.com/sample/1.0/sample.dtd", null);

                Assert.NotNull(rsrc.Stream);
                Assert.AreEqual(result, rsrc.ResolvedUri);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Test]
        public void LookupSystemAsUri() {
            try {
                Uri result = UriUtils.Resolve(TEST_ROOT_DIRECTORY, "UnitTests/resources/sample10/sample.dtd");
                IResourceResponse rsrc = resolver.ResolveEntity(null, null, "https://example.com/sample/1.0/uri.dtd", null);
                Assert.NotNull(rsrc.Stream);
                Assert.AreEqual(result, rsrc.ResolvedUri);
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void LookupUri() {
            try
            {
                IResourceResponse rsrc = resolver.ResolveUri("XMLSchema.xsd", "http://www.w3.org/TR/xmlschema11-1/");
                Assert.NotNull(rsrc.Uri);
                Assert.NotNull(rsrc.ResolvedUri);
                Assert.NotNull(rsrc.Stream);
                
                Assert.AreEqual(new Uri("http://www.w3.org/TR/xmlschema11-1/XMLSchema.xsd"), rsrc.Uri);
                Assert.AreEqual("pack", rsrc.ResolvedUri.Scheme);
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void LookupRedirectedUri() {
            try
            {
                IResourceResponse rsrc = resolver.ResolveUri("https://www.w3.org/TR/1999/WD-font-19990902");
                Assert.NotNull(rsrc.Uri);
                Assert.NotNull(rsrc.ResolvedUri);
                Assert.NotNull(rsrc.Stream);
                Assert.NotNull(rsrc.Encoding);
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void SequenceTest() {
            try {
                List<string> catalogs = new List<string>();
                catalogs.Add(TEST_ROOT_PATH + "/UnitTests/resources/seqtest1.xml");
                catalogs.Add(TEST_ROOT_PATH + "/UnitTests/resources/seqtest2.xml");

                var lconfig = new XmlResolverConfiguration(new List<Uri>(), catalogs);
                lconfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);

                var lresolver = new XmlResolver.XmlResolver(lconfig);

                var rsrc = lresolver.ResolveEntity(null, null, "https://xmlresolver.org/ns/sample-as-uri/sample.dtd", null);

                Assert.NotNull(rsrc.Stream);
            } catch (Exception) 
            {
                Assert.Fail();
            }
        }
        
    }
}
