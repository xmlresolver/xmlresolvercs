using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Version = Org.XmlResolverData.Version;

namespace UnitTests {
    public class DataTest : ResolverTest {
        private string dataVersion = Version.DataVersion;
        private XmlResolverConfiguration config = null;
        private CatalogManager manager = null;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());

            try {
                Assembly asm = Assembly.Load("XmlResolverData");
                config.AddAssemblyCatalog("Org.XmlResolver.catalog.xml", asm);
            }
            catch (Exception) {
                Assert.Fail();
            }

            manager = (CatalogManager) config.GetFeature(ResolverFeature.CATALOG_MANAGER);
        }

        [Test]
        public void Gen_lookupPublicd1e3() {
            Uri result = manager.LookupPublic(null, "-//XML-DEV//ENTITIES RDDL QName Module 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e4() {
            Uri result = manager.LookupSystem("http://www.rddl.org/rddl-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e6() {
            Uri result = manager.LookupPublic(null, "-//XML-DEV//ELEMENTS RDDL Resource 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e7() {
            Uri result = manager.LookupSystem("http://www.rddl.org/rddl-resource-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e9() {
            Uri result = manager.LookupPublic(null, "-//XML-DEV//DTD XHTML RDDL 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e10() {
            Uri result = manager.LookupSystem("http://www.rddl.org/rddl-xhtml.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e12() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-arch-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e14() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-attribs-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e16() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-base-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e18() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-basic-form-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e20() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-basic-table-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e22() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-blkphras-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e24() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-blkstruct-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e26() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-charent-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e29() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-datatypes-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e31() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-events-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e33() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-framework-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e35() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-hypertext-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e37() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-image-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e39() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-inlphras-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e41() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-inlstruct-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e43() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-link-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e45() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-list-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e47() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-meta-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e49() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-notations-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e52() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-object-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e54() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-param-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e56() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e58() {
            Uri result = manager.LookupPublic(null, "-//XML-DEV//ENTITIES RDDL Document Model 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e59() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-rddl-model-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e61() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-struct-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e63() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-text-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e65() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml11.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e67() {
            Uri result = manager.LookupPublic(null, "-//XML-DEV//ENTITIES XLink Module 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e68() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xlink-module-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e70() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-lat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e72() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-special.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e74() {
            Uri result = manager.LookupSystem("http://www.rddl.org/xhtml-symbol.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e78() {
            Uri result = manager.LookupUri("https://www.w3.org/1999/xlink.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e81() {
            Uri result = manager.LookupUri("https://www.w3.org/XML/2008/06/xlink.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e83() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XMLSCHEMA 200102//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e84() {
            Uri result = manager.LookupSystem("https://www.w3.org/2001/XMLSchema.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e86() {
            Uri result = manager.LookupUri("https://www.w3.org/2001/XMLSchema.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e88() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XSD 1.0 Datatypes//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e89() {
            Uri result = manager.LookupPublic(null, "datatypes");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e90() {
            Uri result = manager.LookupSystem("https://www.w3.org/2001/datatypes.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e93() {
            Uri result = manager.LookupUri("https://www.w3.org/2001/xml.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e95() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD Specification V2.10//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e96() {
            Uri result = manager.LookupSystem("https://www.w3.org/2002/xmlspec/dtd/2.10/xmlspec.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e98() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES uppercase aliases for HTML//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e99() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/html5-uppercase.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e101() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES HTML MathML Set//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e102() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/htmlmathml-f.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e104() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES HTML MathML Set//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e105() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/htmlmathml.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e107() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Arrow Relations//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e108() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e111() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Binary Operators//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e112() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e114() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Delimiters//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e115() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e117() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Negated Relations//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e118() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e120() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Ordinary//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e121() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e123() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Relations//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e124() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e126() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Box and Line Drawing//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e127() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isobox.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e129() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Russian Cyrillic//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e130() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isocyr1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e132() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Non-Russian Cyrillic//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e133() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isocyr2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e135() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Diacritical Marks//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e136() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isodia.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e138() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Greek Letters//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e139() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e141() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Monotoniko Greek//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e142() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e145() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Greek Symbols//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e146() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e148() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Alternative Greek Symbols//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e149() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk4.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e151() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 1//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e152() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isolat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e154() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 2//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e155() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isolat2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e157() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Fraktur//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e158() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isomfrk.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e160() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Open Face//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e161() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isomopf.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e163() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Script//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e164() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isomscr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e166() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Numeric and Special Graphic//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e167() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isonum.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e169() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Publishing//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e170() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isopub.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e172() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES General Technical//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e173() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e175() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES MathML Aliases//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e176() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/mmlalias.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e179() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Additional MathML Symbols//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e180() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/mmlextra.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e182() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Predefined XML//EN///XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e183() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/predefined.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e185() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Combined Set//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e186() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/w3centities-f.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e188() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Combined Set//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e189() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/w3centities.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e191() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Latin for HTML//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e192() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/xhtml1-lat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e194() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Special for HTML//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e195() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/xhtml1-special.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e197() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Symbol for HTML//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e198() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/2007/xhtml1-symbol.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e200() {
            Uri result = manager.LookupPublic(null,
                "ISO 8879:1986//ENTITIES Added Math Symbols: Arrow Relations//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e201() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e203() {
            Uri result = manager.LookupPublic(null,
                "ISO 8879:1986//ENTITIES Added Math Symbols: Binary Operators//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e204() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e206() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Delimiters//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e207() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e209() {
            Uri result = manager.LookupPublic(null,
                "ISO 8879:1986//ENTITIES Added Math Symbols: Negated Relations//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e210() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e213() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Ordinary//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e214() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e216() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Relations//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e217() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e219() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Box and Line Drawing//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e220() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isobox.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e222() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Russian Cyrillic//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e223() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isocyr1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e225() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Non-Russian Cyrillic//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e226() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isocyr2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e228() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Diacritical Marks//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e229() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isodia.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e231() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Greek Letters//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e232() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e234() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Monotoniko Greek//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e235() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e237() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Greek Symbols//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e238() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e240() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Alternative Greek Symbols//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e241() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk4.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e243() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Latin 1//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e244() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isolat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e247() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Latin 2//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e248() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isolat2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e250() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Numeric and Special Graphic//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e251() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isonum.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e253() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Publishing//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e254() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isopub.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e256() {
            Uri result = manager.LookupPublic(null, "ISO 8879:1986//ENTITIES General Technical//EN//XML");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e257() {
            Uri result = manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e259() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XSD 1.1//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e260() {
            Uri result = manager.LookupSystem("https://www.w3.org/2009/XMLSchema/XMLSchema.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e262() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XSD 1.1 Datatypes//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e263() {
            Uri result = manager.LookupSystem("https://www.w3.org/2009/XMLSchema/datatypes.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e265() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Animation//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e266() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-animation.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e268() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Animation Events Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e269() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-animevents-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e271() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Clip//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e272() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-clip.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e274() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Filter//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e275() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-filter.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e277() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Font//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e278() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-font.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e281() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Graphics Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e282() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-graphics-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e284() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Paint Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e285() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-paint-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e287() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Structure//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e288() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-structure.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e290() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Text//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e291() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-text.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e293() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Clip//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e294() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-clip.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e296() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Conditional Processing//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e297() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-conditional.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e299() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Container Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e300() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-container-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e302() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Core Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e303() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-core-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e305() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Cursor//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e306() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-cursor.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e308() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Datatypes//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e309() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-datatypes.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e311() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Document Events Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e312() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-docevents-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e315() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Extensibility//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e316() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-extensibility.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e318() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 External Resources Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e319() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-extresources-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e321() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Filter//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e322() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-filter.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e324() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Font//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e325() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-font.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e327() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Modular Framework//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e328() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-framework.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e330() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Gradient//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e331() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-gradient.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e333() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Graphical Element Events Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e334() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-graphevents-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e336() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Graphics Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e337() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-graphics-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e339() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Hyperlinking//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e340() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-hyperlink.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e342() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Image//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e343() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-image.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e345() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Marker//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e346() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-marker.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e349() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Mask//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e350() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-mask.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e352() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Paint Opacity Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e353() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-opacity-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e355() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Paint Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e356() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-paint-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e358() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Pattern//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e359() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-pattern.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e361() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Color Profile//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e362() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-profile.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e364() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Qualified Name//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e365() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-qname.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e367() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Scripting//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e368() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-script.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e370() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Shape//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e371() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-shape.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e373() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Structure//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e374() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-structure.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e376() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Style//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e377() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-style.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e379() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG Template Qualified Name//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e380() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-template-qname.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e383() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-template.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e385() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Text//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e386() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-text.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e388() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 View//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e389() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-view.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e391() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Viewport Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e392() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-viewport-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e394() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 XLink Attribute//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e395() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-xlink-attrib.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e397() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Attribute Collection//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e398() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-attribs.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e400() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Attribute Collection//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e401() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic-attribs.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e403() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic-flat.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e405() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Document Model//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e406() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic-model.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e408() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD SVG 1.1 Basic//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e409() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e411() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-flat-20030114.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e414() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-flat.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e416() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Document Model//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e417() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-model.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e419() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD SVG 1.1 Template//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e420() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-template.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e422() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Tiny Attribute Collection//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e423() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny-attribs.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e425() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny-flat.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e427() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Tiny Document Model//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e428() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny-model.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e430() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD SVG 1.1 Tiny//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e431() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e433() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD SVG 1.1//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e434() {
            Uri result = manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e436() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Access Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e437() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-access-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e439() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Access Attribute Qnames 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e440() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-access-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e442() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Java Applets 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e443() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-applet-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e446() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Base Architecture 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e447() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-arch-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e449() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Common Attributes 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e450() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-attribs-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e452() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Base Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e453() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-base-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e455() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Basic Forms 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e456() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic-form-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e458() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Basic Tables 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e459() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic-table-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e461() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Basic 1.0 Document Model 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e462() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic10-model-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e464() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Basic 1.1 Document Model 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e465() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic11-model-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e467() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML Basic 1.1//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e468() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic11.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e470() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML BIDI Override Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e471() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-bdo-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e473() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Block Phrasal 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e474() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-blkphras-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e476() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Block Presentation 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e477() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-blkpres-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e480() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Block Structural 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e481() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-blkstruct-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e483() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Character Entities 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e484() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-charent-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e486() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Client-side Image Maps 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e487() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-csismap-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e489() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Datatypes 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e490() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-datatypes-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e492() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Editing Elements 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e493() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-edit-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e495() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Intrinsic Events 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e496() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-events-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e498() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Forms 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e499() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-form-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e501() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Frames 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e502() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-frames-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e504() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Modular Framework 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e505() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-framework-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e507() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML HyperAttributes 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e508() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-hyperAttributes-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e510() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Hypertext 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e511() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-hypertext-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e514() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Frame Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e515() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-iframe-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e517() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Images 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e518() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-image-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e520() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Phrasal 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e521() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlphras-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e523() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Presentation 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e524() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlpres-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e526() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Structural 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e527() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlstruct-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e529() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Style 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e530() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlstyle-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e532() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inputmode 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e533() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inputmode-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e535() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-lat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e537() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Legacy MarkUp 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e538() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-legacy-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e540() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Legacy Redeclarations 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e541() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-legacy-redecl-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e543() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Link Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e544() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-link-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e547() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Lists 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e548() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-list-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e550() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Metainformation 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e551() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-meta-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e553() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Metainformation 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e554() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-meta-2.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e556() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML MetaAttributes 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e557() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-metaAttributes-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e559() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Name Identifier 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e560() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-nameident-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e562() {
            Uri result = manager.LookupPublic(null, "-//W3C//NOTATIONS XHTML Notations 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e563() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-notations-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e565() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Embedded Object 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e566() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-object-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e568() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Param Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e569() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-param-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e571() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Presentation 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e572() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-pres-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e574() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML-Print 1.0 Document Model 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e575() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-print10-model-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e577() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Qualified Names 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e578() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e581() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML+RDFa Document Model 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e582() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-rdfa-model-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e584() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML RDFa Attribute Qnames 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e585() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-rdfa-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e587() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Role Attribute 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e588() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-role-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e590() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Role Attribute Qnames 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e591() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-role-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e593() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Ruby 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e594() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-ruby-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e596() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Scripting 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e597() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-script-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e599() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-special.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e601() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Server-side Image Maps 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e602() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-ssismap-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e604() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Document Structure 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e605() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-struct-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e607() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Style Sheets 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e608() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-style-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e610() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-symbol.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e613() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Tables 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e614() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-table-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e616() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Target 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e617() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-target-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e619() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Text 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e620() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-text-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e622() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Frameset//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e623() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml1-frameset.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e625() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Strict//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e626() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml1-strict.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e628() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Transitional//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e629() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml1-transitional.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e631() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES XHTML 1.1 Document Model 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e632() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml11-model-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e634() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.1//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e635() {
            Uri result = manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml11.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e637() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e639() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e641() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e644() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e646() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e648() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e650() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isobox.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e652() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isocyr1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e654() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isocyr2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e656() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isodia.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e658() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e660() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e662() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e664() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk4.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e667() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isolat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e669() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isolat2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e671() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isomfrk.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e673() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isomopf.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e675() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isomscr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e677() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isonum.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e679() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isopub.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e681() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e683() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/mathml.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e685() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/mmlalias.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e687() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/mmlextra.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e690() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/html/lat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e692() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/html/special.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e694() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/html/symbol.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e696() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e698() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e700() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e702() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e704() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e706() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e708() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Box and Line Drawing for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e709() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isobox.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e711() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Russian Cyrillic for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e712() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isocyr1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e715() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Non-Russian Cyrillic for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e716() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isocyr2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e718() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Diacritical Marks for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e719() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isodia.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e721() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e723() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e725() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e727() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk4.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e729() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 1 for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e730() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isolat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e732() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 2 for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e733() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isolat2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e735() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Numeric and Special Graphic for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e736() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isonum.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e738() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Publishing for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e739() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isopub.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e741() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e744() {
            Uri result = manager.LookupPublic(null,
                "-//W3C//ENTITIES Added Math Symbols: Arrow Relations for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e745() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e747() {
            Uri result = manager.LookupPublic(null,
                "-//W3C//ENTITIES Added Math Symbols: Binary Operators for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e748() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e750() {
            Uri result =
                manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Delimiters for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e751() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e753() {
            Uri result = manager.LookupPublic(null,
                "-//W3C//ENTITIES Added Math Symbols: Negated Relations for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e754() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e756() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Ordinary for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e757() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e759() {
            Uri result =
                manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Relations for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e760() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e762() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Greek Symbols for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e763() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e765() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isogrk4.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e767() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Fraktur for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e768() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isomfrk.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e770() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Open Face for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e771() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isomopf.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e773() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Script for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e774() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isomscr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e777() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES General Technical for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e778() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e780() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e782() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e784() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e786() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e788() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e790() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e792() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isobox.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e794() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isocyr1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e796() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isocyr2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e798() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isodia.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e801() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e803() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isolat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e805() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isolat2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e807() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isomfrk.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e809() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isomopf.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e811() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isomscr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e813() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isonum.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e815() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isopub.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e817() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e819() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Aliases for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e820() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml/mmlalias.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e822() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Extra for MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e823() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml/mmlextra.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e826() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml2-a.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e828() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES MathML 2.0 Qualified Names 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e829() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml2-qname-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e831() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD MathML 2.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e832() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml2.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e834() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mmlalias.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e836() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mmlextra.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e838() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/xhtml-math11-f-a.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e840() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/xhtml-math11-f.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e842() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/xhtml-math11.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e844() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsa.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e846() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsb.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e848() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsc.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e851() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsn.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e853() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamso.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e855() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e857() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isobox.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e859() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isocyr1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e861() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isocyr2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e863() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isodia.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e865() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isogrk3.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e867() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isolat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e869() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isolat2.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e871() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isomfrk.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e874() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isomopf.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e876() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isomscr.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e878() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isonum.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e880() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isopub.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e882() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isotech.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e884() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES MathML 3.0 Qualified Names 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e885() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mathml3-qname.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e887() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mathml3.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e889() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mmlalias.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e891() {
            Uri result = manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mmlextra.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e893() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD SVG 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e894() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e896() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Ruby 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e897() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/ruby/xhtml-ruby-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e900() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML Basic 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e901() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-basic/xhtml-basic10.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e903() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML BIDI Override Element 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e904() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-bdo-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e906() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Client-side Image Maps 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e907() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-csismap-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e909() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Editing Elements 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e910() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-edit-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e912() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Legacy MarkUp 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e913() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-legacy-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e915() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Server-side Image Maps 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e916() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-ssismap-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e918() {
            Uri result = manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Tables 1.0//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e919() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-table-1.mod");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e921() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Latin 1 for XHTML//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e922() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-lat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e924() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Symbols for XHTML//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e925() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-special.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e927() {
            Uri result = manager.LookupPublic(null, "-//W3C//ENTITIES Special for XHTML//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e928() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-symbol.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e930() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml-lat1.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e933() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml-special.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e935() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml-symbol.ent");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e937() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Frameset//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e938() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e940() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Strict//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e941() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupPublicd1e943() {
            Uri result = manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Transitional//EN");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupSystemd1e944() {
            Uri result = manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e946() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xmlschema-1/XMLSchema.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e948() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xmlschema-2/datatypes.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e950() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xmlschema11-1/XMLSchema.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e952() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xmlschema11-2/datatypes.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e954() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-json.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e956() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-xslt30.rnc");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e959() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-xslt30.rng");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e961() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-xslt30.xsd");
            Assert.NotNull(result);
        }

        [Test]
        public void Gen_lookupUrid1e963() {
            Uri result = manager.LookupUri("https://www.w3.org/TR/xslt-30/xml-to-json.xsl");
            Assert.NotNull(result);
        }
    }
}