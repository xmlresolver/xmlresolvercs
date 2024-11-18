using System;
using System.Collections.Generic;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class ResolverTest : BaseTestRoot {
        public static readonly string catalog1 = "/XmlResolver/UnitTests/resources/rescat.xml";
        private XmlResolverConfiguration config = null;
        private Resolver resolver = null;
        
        [SetUp]
        public void BaseSetup() {
            List<string> catalogs = new List<string>();
            catalogs.Add(TEST_ROOT_PATH + catalog1);
            config = new XmlResolverConfiguration(new List<Uri>(), catalogs);
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            resolver = new Resolver(config);
        }
        
        [Test]
        public void LookupSystemFail() {
            try {
                CatalogResolver cresolver = resolver.CatalogResolver;

                ResolvedResource rsrc = cresolver.ResolveEntity(null, null, "https://example.com/not/in/catalog", null);

                Assert.That(rsrc, Is.Null);
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void LookupLocalSystemFail() {
            try {
                CatalogResolver cresolver = resolver.CatalogResolver;

                ResolvedResource rsrc = cresolver.ResolveEntity(null, null, "file:///path/to/thing/that/isnt/likely/to/exist", null);

                Assert.That(rsrc, Is.Null);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Test]
        public void LookupSystem() {
            try {
                Uri result = UriUtils.Resolve(TEST_ROOT_DIRECTORY, "XmlResolver/UnitTests/resources/sample10/sample.dtd");
                CatalogResolver cresolver = resolver.CatalogResolver;

                ResolvedResource rsrc = cresolver.ResolveEntity(null, null, "https://example.com/sample/1.0/sample.dtd", null);

                Assert.That(rsrc.GetInputStream(), Is.Not.Null);
                Assert.That(rsrc.GetLocalUri(), Is.EqualTo(result));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Test]
        public void LookupSystemAsUri() {
            try {
                Uri result = UriUtils.Resolve(TEST_ROOT_DIRECTORY, "XmlResolver/UnitTests/resources/sample10/sample.dtd");
                CatalogResolver cresolver = resolver.CatalogResolver;

                ResolvedResource rsrc = cresolver.ResolveEntity(null, null, "https://example.com/sample/1.0/uri.dtd", null);

                Assert.That(rsrc.GetInputStream(), Is.Not.Null);
                Assert.That(rsrc.GetLocalUri(), Is.EqualTo(result));
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [Test]
        public void SequenceTest() {
            try {
                List<string> catalogs = new List<string>();
                catalogs.Add(TEST_ROOT_PATH + "/XmlResolver/UnitTests/resources/seqtest1.xml");
                catalogs.Add(TEST_ROOT_PATH + "/XmlResolver/UnitTests/resources/seqtest2.xml");

                XmlResolverConfiguration lconfig = new XmlResolverConfiguration(new List<Uri>(), catalogs);
                lconfig.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);

                Resolver lresolver = new Resolver(lconfig);
                CatalogResolver cresolver = lresolver.CatalogResolver;

                ResolvedResource rsrc = cresolver.ResolveEntity(null, null, "https://xmlresolver.org/ns/sample-as-uri/sample.dtd", null);

                Assert.That(rsrc.GetInputStream(), Is.Not.Null);
            } catch (Exception) {
                Assert.Fail();
            }
        }
        
    }
}
