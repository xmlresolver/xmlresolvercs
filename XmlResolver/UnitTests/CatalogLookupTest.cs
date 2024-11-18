using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class CatalogLookupTest {
        private static readonly Assembly asm = Assembly.GetExecutingAssembly();
        private static readonly Uri catalog1 = UriUtils.GetLocationUri("/resources/lookup1.xml", asm);
        private static readonly Uri catalog2 = UriUtils.GetLocationUri("/resources/lookup2.xml", asm);

        //private static string resourcePath = null;
        private static Assembly assembly = null;
        XmlResolverConfiguration config = null;
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            assembly = Assembly.GetExecutingAssembly();
            StringBuilder sb = new StringBuilder();
            sb.Append("application:,,,");
            sb.Append(assembly.GetName().Name);
            if (assembly.GetName().Version != null) {
                sb.Append(";");
                sb.Append(assembly.GetName().Version);
            }

            // FIXME: public key
            sb.Append(";component");

            config = new(new List<Uri>(), new List<string>());
            config.AddCatalog(catalog1, UriUtils.GetStream(catalog1));
            config.AddCatalog(catalog2, UriUtils.GetStream(catalog2));
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, false);
            manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        }

        [Test]
        public void LookupSystem() {
            Uri result = manager.LookupSystem("https://example.com/sample/1.0/sample.dtd");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupSystemMiss() {
            Uri result = manager.LookupSystem("https://xmlresolver.org/ns/sample/sample.rng");
            Assert.That(result, Is.Null);
        }

        // ============================================================
        // See https://www.oasis-open.org/committees/download.php/14809/xml-catalogs.html#attrib.prefer
        // Note that the N/A entries in column three are a bit misleading.

        [Test]
        public void LookupPublic_prefer_public_nosystem_public1() {
            // Catalog contains a matching public entry, but not a matching system entry
            Uri result = manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-public.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_public_nosystem_public2() {
            // Catalog contains a matching system entry, but not a matching public entry
        }

        [Test]
        public void LookupPublic_prefer_public_nosystem_public3() {
            // Catalog contains both a matching system entry and a matching public entry
        }

        [Test]
        public void LookupPublic_prefer_public_system_nopublic1() {
            // Catalog contains a matching public entry, but not a matching system entry
        }

        [Test]
        public void LookupPublic_prefer_public_system_nopublic2() {
            // Catalog contains a matching system entry, but not a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }


        [Test]
        public void LookupPublic_prefer_public_system_nopublic3() {
            // Catalog contains both a matching system entry and a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_public_system_public1() {
            // Catalog contains a matching public entry, but not a matching system entry
            Uri result = manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd",
                "-//Sample//DTD Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-public.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_public_system_public2() {
            // Catalog contains a matching system entry, but not a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
                "-//Sample//DTD Not Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_public_system_public3() {
            // Catalog contains both a matching system entry and a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
                "-//Sample//DTD Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_system_nosystem_public1() {
            // Catalog contains a matching public entry, but not a matching system entry
            Uri result = manager.LookupPublic(null, "-//Sample//DTD Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-public.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_system_nosystem_public2() {
            // Catalog contains a matching system entry, but not a matching public entry
        }

        [Test]
        public void LookupPublic_prefer_system_nosystem_public3() {
            // Catalog contains both a matching system entry and a matching public entry
        }

        [Test]
        public void LookupPublic_prefer_system_system_nopublic1() {
            // Catalog contains a matching public entry, but not a matching system entry
        }

        [Test]
        public void LookupPublic_prefer_system_system_nopublic2() {
            // Catalog contains a matching system entry, but not a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_system_system_nopublic3() {
            // Catalog contains both a matching system entry and a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_system_system_public1() {
            // Catalog contains a matching public entry, but not a matching system entry
            Uri result = manager.LookupPublic("https://example.com/not-sample/1.0/sample.dtd",
                "-//Sample//DTD Prefer Sample 1.0//EN");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void LookupPublic_prefer_system_system_public2() {
            // Catalog contains a matching system entry, but not a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
                "-//Sample//DTD Not Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        [Test]
        public void LookupPublic_prefer_system_system_public3() {
            // Catalog contains both a matching system entry and a matching public entry
            Uri result = manager.LookupPublic("https://example.com/sample/1.0/sample.dtd",
                "-//Sample//DTD Prefer Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-system.dtd")));
        }

        // ============================================================

        [Test]
        public void RewriteSystem() {
            Uri result = manager.LookupSystem("https://example.com/path1/sample/3.0/sample.dtd");
            Uri expected = new Uri("https://example.com/path2/sample/3.0/sample.dtd");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SystemSuffix() {
            Uri result = manager.LookupSystem("https://example.com/whatever/you/want/suffix.dtd");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample20/sample-suffix.dtd")));
        }

        [Test]
        public void DelegatePublicLong() {
            Uri result = manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 1.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-delegated.dtd")));
        }

        [Test]
        public void DelegatePublicShort() {
            Uri result = manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 2.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample20/sample-shorter.dtd")));
        }

        [Test]
        public void DelegatePublicFail() {
            Uri result = manager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DelegateSystemLong() {
            Uri result = manager.LookupPublic("https://example.com/delegated/sample/1.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-delegated.dtd")));
        }

        [Test]
        public void DelegateSystemShort() {
            Uri result = manager.LookupPublic("https://example.com/delegated/sample/2.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample20/sample-shorter.dtd")));
        }

        [Test]
        public void DelegateSystemFail() {
            Uri result = manager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
            Assert.That(result, Is.Null);
        }

        /*
        [Test]
        public void Undelegated() {
            // If there aren't any delegate entries, the entries in lookup2.xml really do match.
            XMLResolverConfiguration uconfig = new XMLResolverConfiguration(Collections.emptyList(), Collections.emptyList());
            uconfig.setFeature(ResolverFeature.CATALOG_FILES, Collections.singletonList(catalog2));
            CatalogManager umanager = new CatalogManager(uconfig);
    
            Uri result = umanager.LookupPublic(null, "-//Sample Delegated//DTD Sample 3.0//EN");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample30/fail.dtd")));
    
            result = umanager.LookupPublic("https://example.com/delegated/sample/3.0/sample.dtd", null);
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample30/fail.dtd")));
        }
        */

        // ============================================================

        [Test]
        public void LookupUri() {
            Uri result = manager.LookupUri("https://xmlresolver.org/ns/sample/sample.rng");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(UriUtils.Resolve(UriUtils.Cwd(), catalog1), "sample/sample.rng")));
        }

        [Test]
        public void RewriteUri() {
            Uri result = manager.LookupUri("https://example.com/path1/sample/sample.rng");
            Uri expected = new Uri("https://example.com/path2/sample/sample.rng");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UriSuffix() {
            Uri result = manager.LookupUri("https://example.com/whatever/you/want/suffix.rnc");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample20/sample-suffix.rnc")));
        }

        [Test]
        public void DelegateUriLong() {
            Uri result = manager.LookupUri("https://example.com/delegated/sample/1.0/sample.rng");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample10/sample-delegated.rng")));
        }

        [Test]
        public void DelegateUriShort() {
            Uri result = manager.LookupUri("https://example.com/delegated/sample/2.0/sample.rng");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample20/sample-shorter.rng")));
        }

        [Test]
        public void DelegateUriFail() {
            Uri result = manager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UndelegatedUri() {
            // If there aren't any delegate entries, the entries in lookup2.xml really do match.
            XmlResolverConfiguration uconfig = new XmlResolverConfiguration(new(), new());
            List<string> cats = new();
            cats.Add(catalog2.ToString());
            uconfig.SetFeature(ResolverFeature.CATALOG_FILES, cats);
            CatalogManager umanager = (CatalogManager) uconfig.GetFeature(ResolverFeature.CATALOG_MANAGER);
    
            Uri result = umanager.LookupUri("https://example.com/delegated/sample/3.0/sample.rng");
            Assert.That(result, Is.EqualTo(UriUtils.Resolve(catalog1, "sample30/fail.rng")));
        }
    
        [Test]
        public void BaseUriRootTest() {
            // Make sure an xml:base attribute on the root element works
            List<String> catalog = new();
            catalog.Add(UriUtils.Resolve(catalog1, "lookup-test.xml").ToString());
            XmlResolverConfiguration config = new XmlResolverConfiguration(new(), catalog);
            CatalogManager manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            Uri result = manager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", "-//W3C//DTD SVG 1.1//EN");
            Assert.That(result.AbsolutePath, Is.EqualTo("/usr/local/DTDs/svg11/system-svg11.dtd"));
        }
    
        [Test]
        public void BaseUriGroupTest() {
            // Make sure an xml:base attribute on a group element works
            List<String> catalog = new();
            catalog.Add(UriUtils.Resolve(catalog1, "lookup-test.xml").ToString());
            XmlResolverConfiguration config = new XmlResolverConfiguration(new(), catalog);
            CatalogManager manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            Uri result = manager.LookupPublic("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd", "-//W3C//DTD SVG 1.1 Basic//EN");
            Assert.That(result.AbsolutePath, Is.EqualTo("/usr/local/nested/DTDs/svg11/system-svg11-basic.dtd"));
        }
    
        [Test]
        public void BaseUriOnElementTest() {
            // Make sure an xml:base attribute on the actual element works
            List<String> catalog = new();
            catalog.Add(UriUtils.Resolve(catalog1, "lookup-test.xml").ToString());
            XmlResolverConfiguration config = new XmlResolverConfiguration(new(), catalog);
            CatalogManager manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
            Uri result = manager.LookupSystem("https://example.com/test.dtd");
            Assert.That(result.AbsolutePath, Is.EqualTo("/usr/local/on/DTDs/test.dtd"));
        }
    }
}
