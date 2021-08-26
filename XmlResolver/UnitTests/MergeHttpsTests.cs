using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class MergeHttpsTests {
        public static CatalogManager mergeManager = null;
        public static CatalogManager noMergeManager = null;

        [SetUp]
        public void Setup() {
            // For convenience, this set of tests uses the data catalog because it's known to contain
            // both http: and https: Uris.

            Uri caturi = null;
            try {
                Assembly asm = Assembly.Load("XmlResolverData");
                caturi = UriUtils.GetLocationUri("Org.XmlResolver.catalog.xml", asm);
            }
            catch (Exception) {
                Assert.Fail();
            }

            string catalog = caturi.ToString();
            List<string> catlist = new List<string>();
            catlist.Add(catalog);

            XmlResolverConfiguration config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.SetFeature(ResolverFeature.CATALOG_FILES, catlist);
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            config.SetFeature(ResolverFeature.CACHE, null);
            config.SetFeature(ResolverFeature.CACHE_UNDER_HOME, false);
            config.SetFeature(ResolverFeature.MERGE_HTTPS, true);
            mergeManager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);

            config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.SetFeature(ResolverFeature.CATALOG_FILES, catlist);
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            config.SetFeature(ResolverFeature.CACHE, null);
            config.SetFeature(ResolverFeature.CACHE_UNDER_HOME, false);
            config.SetFeature(ResolverFeature.MERGE_HTTPS, false);
            noMergeManager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        }

        [Test]
        public void Equivalentcp1() {
            Assert.AreEqual("classpath:path/to/thing",
                noMergeManager.NormalizedForComparison("classpath:/path/to/thing"));
        }

        [Test]
        public void Equivalentcp2() {
            Assert.AreEqual("classpath:path/to/thing",
                noMergeManager.NormalizedForComparison("classpath:path/to/thing"));
        }

        [Test]
        public void Equivalentcp1m() {
            Assert.AreEqual("classpath:path/to/thing",
                mergeManager.NormalizedForComparison("classpath:/path/to/thing"));
        }

        [Test]
        public void Equivalentcp2m() {
            Assert.AreEqual("classpath:path/to/thing", mergeManager.NormalizedForComparison("classpath:path/to/thing"));
        }

        [Test]
        public void Equivalenthttp1() {
            Assert.AreEqual("https://localhost/path/to/thing",
                noMergeManager.NormalizedForComparison("https://localhost/path/to/thing"));
        }

        [Test]
        public void Equivalenthttp2() {
            Assert.AreEqual("http://localhost/path/to/thing",
                noMergeManager.NormalizedForComparison("http://localhost/path/to/thing"));
        }

        [Test]
        public void Equivalenthttp3() {
            Assert.AreEqual("ftp://localhost/path/to/thing",
                noMergeManager.NormalizedForComparison("ftp://localhost/path/to/thing"));
        }

        [Test]
        public void Equivalenthttp1m() {
            Assert.AreEqual("https://localhost/path/to/thing",
                mergeManager.NormalizedForComparison("https://localhost/path/to/thing"));
        }

        [Test]
        public void Equivalenthttp2m() {
            Assert.AreEqual("https://localhost/path/to/thing",
                mergeManager.NormalizedForComparison("http://localhost/path/to/thing"));
        }

        [Test]
        public void Equivalenthttp3m() {
            Assert.AreEqual("ftp://localhost/path/to/thing",
                mergeManager.NormalizedForComparison("ftp://localhost/path/to/thing"));
        }


        [Test]
        public void LookupHttpUri() {
            Uri result = mergeManager.LookupUri("http://www.w3.org/2001/xml.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void LookupHttpSystem() {
            Uri result = mergeManager.LookupSystem("http://www.w3.org/MarkUp/DTD/xhtml-basic11.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void LookupHttpsSystem() {
            Uri result = mergeManager.LookupSystem("https://www.rddl.org/rddl-xhtml.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void LookupHttpUriNoMerge() {
            Uri result = noMergeManager.LookupUri("http://www.w3.org/2001/xml.xsd");
            Assert.Null(result);
        }

        [Test]
        public void LookupHttpSystemNoMerge() {
            Uri result = noMergeManager.LookupSystem("http://www.w3.org/MarkUp/DTD/xhtml-basic11.dtd");
            Assert.Null(result);
        }

        [Test]
        public void LookupHttpsSystemNoMerge() {
            Uri result = noMergeManager.LookupSystem("https://www.rddl.org/rddl-xhtml.dtd");
            Assert.Null(result);
        }
    }
}