using System.Collections.Generic;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Features;

namespace UnitTests {
    public class FeatureTest {
        private XmlResolverConfiguration config;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration();
            config.SetFeature(ResolverFeature.USE_DATA_ASSEMBLY, false);
        }
        
        private void BooleanFeature(BoolResolverFeature feature) {
            bool orig = (bool) config.GetFeature(feature);
            config.SetFeature(feature, false);
            Assert.That(config.GetFeature(feature), Is.EqualTo(false));
            config.SetFeature(feature, true);
            Assert.That(config.GetFeature(feature), Is.EqualTo(true));
            config.SetFeature(feature, orig);
        }

        private void StringFeature(StringResolverFeature feature) {
            string orig = (string) config.GetFeature(feature);
            config.SetFeature(feature, "apple pie");
            Assert.That(config.GetFeature(feature), Is.EqualTo("apple pie"));
            config.SetFeature(feature, orig);
        }

        [Test]
        public void TestAllowCatalogPi() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.ALLOW_CATALOG_PI), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.ALLOW_CATALOG_PI);
        }
        
        [Test]
        public void TestArchivedCatalogs() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.ARCHIVED_CATALOGS), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.ARCHIVED_CATALOGS);
        }

        [Test]
        public void TestCacheUnderHome() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CACHE_UNDER_HOME), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.CACHE_UNDER_HOME);
        }

        [Test]
        public void TestMaskPackUris() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.MASK_PACK_URIS), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.MASK_PACK_URIS);
        }

        [Test]
        public void TestMergeHttps() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.MERGE_HTTPS), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.MERGE_HTTPS);
        }

        [Test]
        public void TestParseRddl() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.PARSE_RDDL), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.PARSE_RDDL);
        }

        [Test]
        public void TestPreferPropertyFile() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.PREFER_PROPERTY_FILE), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.PREFER_PROPERTY_FILE);
        }

        [Test]
        public void TestPreferPublic() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.PREFER_PUBLIC), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.PREFER_PUBLIC);
        }

        [Test]
        public void TestUriForSystem() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.URI_FOR_SYSTEM), Is.EqualTo(true));
            BooleanFeature(ResolverFeature.URI_FOR_SYSTEM);
        }

        [Test]
        public void TestAssemblyCatalog() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.ASSEMBLY_CATALOGS), Is.EqualTo(true));
            
            List<string> orig = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
            
            List<string> myCatalogs = new List<string> { "One", "Two", "Three" };
            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, myCatalogs);
            List<string> current = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
            sameLists(current, myCatalogs);

            myCatalogs = new List<string> { "Alpha", "Beta", "Gamma" };
            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, myCatalogs);
            current = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
            sameLists(current, myCatalogs);

            // For backwards compatibility, this feature accepts either a string or a list of strings
            // If you provide a single string, it adds that string to the list.

            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "Spoon");
            myCatalogs = new List<string> { "Alpha", "Beta", "Gamma", "Spoon" };
            current = (List<string>)config.GetFeature(ResolverFeature.ASSEMBLY_CATALOGS);
            sameLists(current, myCatalogs);

            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, orig);
        }

        [Test]
        public void TestCacheDirectory() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CACHE_DIRECTORY), Is.EqualTo(true));
            StringFeature(ResolverFeature.CACHE_DIRECTORY);
        }

        [Test]
        public void TestCatalogLoaderClass() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CATALOG_LOADER_CLASS), Is.EqualTo(true));
            StringFeature(ResolverFeature.CATALOG_LOADER_CLASS);
        }

        private void sameLists(List<string> list1, List<string> list2) {
            // Hack
            foreach (var cur in list1) {
                Assert.That(list2.Contains(cur), Is.EqualTo(true));
            }
            foreach (var cur in list2) {
                Assert.That(list1.Contains(cur), Is.EqualTo(true));
            }
        }

        [Test]
        public void TestFeatureCatalogFiles() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CATALOG_FILES), Is.EqualTo(true));

            List<string> orig = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
            List<string> myList = new List<string> { "One", "Two", "Three" };
            config.SetFeature(ResolverFeature.CATALOG_FILES, myList);
            List<string> current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
            sameLists(current, myList);

            // Setting CATALOG_FILES replaces the list
            List<string> newList = new List<string> { "Alpha", "Beta", "Gamma" };
            config.SetFeature(ResolverFeature.CATALOG_FILES, newList);
            current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);

            // None of "mine" survived
            foreach (var cur in myList) {
                Assert.That(current.Contains(cur), Is.EqualTo(false));
            }
            
            sameLists(current, newList);
            
            config.SetFeature(ResolverFeature.CATALOG_FILES, orig);
        }
        
        [Test]
        public void TestFeatureCatalogAdditions() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CATALOG_ADDITIONS), Is.EqualTo(true));

            List<string> origCatalogs = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
            List<string> origAdditions = (List<string>)config.GetFeature(ResolverFeature.CATALOG_ADDITIONS);
            
            List<string> myCatalogs = new List<string> { "One", "Two", "Three" };
            config.SetFeature(ResolverFeature.CATALOG_FILES, myCatalogs);
            List<string> current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);
            sameLists(current, myCatalogs);
            
            // Setting CATALOG_ADDITIONS augments the list of catalogs
            List<string> myList = new List<string> { "Alpha", "Beta", "Gamma" };
            config.SetFeature(ResolverFeature.CATALOG_ADDITIONS, myList);
            current = (List<string>)config.GetFeature(ResolverFeature.CATALOG_FILES);

            foreach (var mine in myCatalogs) {
                Assert.That(current.Contains(mine), Is.EqualTo(true));
            }
            foreach (var mine in myList) {
                Assert.That(current.Contains(mine), Is.EqualTo(true));
            }

            config.SetFeature(ResolverFeature.CATALOG_FILES, origCatalogs);
            config.SetFeature(ResolverFeature.CATALOG_ADDITIONS, origAdditions);
        }

        [Test]
        public void TestFeatureCatalogManager() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CATALOG_MANAGER), Is.EqualTo(true));
            CatalogManager manager = new CatalogManager(config);
            CatalogManager orig = (CatalogManager)config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            config.SetFeature(ResolverFeature.CATALOG_MANAGER, manager);
            Assert.That(manager, Is.SameAs(config.GetFeature(ResolverFeature.CATALOG_MANAGER)));
            config.SetFeature(ResolverFeature.CATALOG_MANAGER, orig);
        }

        [Test]
        public void TestFeatureCache() {
            Assert.That(config.GetFeatures().Contains(ResolverFeature.CACHE), Is.EqualTo(true));
            ResourceCache myCache = new ResourceCache(config);
            ResourceCache orig = (ResourceCache)config.GetFeature(ResolverFeature.CACHE);
            config.SetFeature(ResolverFeature.CACHE, myCache);
            Assert.That(myCache, Is.SameAs(config.GetFeature(ResolverFeature.CACHE)));
            config.SetFeature(ResolverFeature.CACHE, orig);
        }
    }
}
