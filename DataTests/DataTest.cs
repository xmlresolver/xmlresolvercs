using System.Reflection;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;

namespace UnitTests;

public class DataTest : ResolverTest
{
    private XmlResolverConfiguration? _config = null;
    private CatalogManager? _manager = null;

    private XmlResolverConfiguration Config
    {
        get
        {
            if (_config == null)
            {
                _config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
                try
                {
                    Assembly asm = Assembly.Load("XmlResolverData");
                    Config.AddAssemblyCatalog("XmlResolver.catalog.xml", asm);
                }
                catch (Exception)
                {
                    throw new Exception("Failed to load data assembly");
                }

            }

            return _config;
        }
    }

    private CatalogManager Manager
    {
        get
        {
            if (_manager == null)
            {
                _manager = (CatalogManager)Config.GetFeature(ResolverFeature.CATALOG_MANAGER)!;
            }

            return _manager;
        }
    }

    [Test]
    public void Gen_lookupPublicd2e3()
    {
        var result = Manager.LookupPublic(null, "-//XML-DEV//ENTITIES RDDL QName Module 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e4()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/rddl-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e6()
    {
        var result = Manager.LookupPublic(null, "-//XML-DEV//ELEMENTS RDDL Resource 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e7()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/rddl-resource-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e9()
    {
        var result = Manager.LookupPublic(null, "-//XML-DEV//DTD XHTML RDDL 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e10()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/rddl-xhtml.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e12()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-arch-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e14()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-attribs-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e16()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-base-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e18()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-basic-form-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e20()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-basic-table-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e22()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-blkphras-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e24()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-blkstruct-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e26()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-charent-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e29()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-datatypes-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e31()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-events-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e33()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-framework-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e35()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-hypertext-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e37()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-image-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e39()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-inlphras-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e41()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-inlstruct-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e43()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-link-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e45()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-list-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e47()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-meta-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e49()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-notations-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e52()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-object-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e54()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-param-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e56()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e58()
    {
        var result = Manager.LookupPublic(null, "-//XML-DEV//ENTITIES RDDL Document Model 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e59()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-rddl-model-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e61()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-struct-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e63()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-text-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e65()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml11.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e67()
    {
        var result = Manager.LookupPublic(null, "-//XML-DEV//ENTITIES XLink Module 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e68()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xlink-module-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e70()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-lat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e72()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-special.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e74()
    {
        var result = Manager.LookupSystem("http://www.rddl.org/xhtml-symbol.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e77()
    {
        var result = Manager.LookupUri("https://www.w3.org/1999/xlink.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e82()
    {
        var result = Manager.LookupUri("https://www.w3.org/XML/2008/06/xlink.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e84()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XMLSCHEMA 200102//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e85()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2001/XMLSchema.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e87()
    {
        var result = Manager.LookupUri("https://www.w3.org/2001/XMLSchema.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e92()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XSD 1.0 Datatypes//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e93()
    {
        var result = Manager.LookupPublic(null, "datatypes");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e94()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2001/datatypes.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e96()
    {
        var result = Manager.LookupUri("https://www.w3.org/2001/xml.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e101()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD Specification V2.10//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e102()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2002/xmlspec/dtd/2.10/xmlspec.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e104()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES uppercase aliases for HTML//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e105()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/html5-uppercase.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e107()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES HTML MathML Set//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e108()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/htmlmathml-f.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e110()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES HTML MathML Set//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e111()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/htmlmathml.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e113()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Arrow Relations//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e114()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e117()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Binary Operators//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e118()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e120()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Delimiters//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e121()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e123()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Negated Relations//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e124()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e126()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Ordinary//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e127()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e129()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Relations//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e130()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e132()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Box and Line Drawing//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e133()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isobox.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e135()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Russian Cyrillic//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e136()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isocyr1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e138()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Non-Russian Cyrillic//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e139()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isocyr2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e141()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Diacritical Marks//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e142()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isodia.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e144()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Greek Letters//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e145()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e147()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Monotoniko Greek//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e148()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e151()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Greek Symbols//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e152()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e154()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Alternative Greek Symbols//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e155()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isogrk4.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e157()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 1//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e158()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isolat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e160()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 2//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e161()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isolat2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e163()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Fraktur//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e164()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isomfrk.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e166()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Open Face//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e167()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isomopf.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e169()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Script//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e170()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isomscr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e172()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Numeric and Special Graphic//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e173()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isonum.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e175()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Publishing//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e176()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isopub.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e178()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES General Technical//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e179()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e181()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES MathML Aliases//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e182()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/mmlalias.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e185()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Additional MathML Symbols//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e186()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/mmlextra.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e188()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Predefined XML//EN///XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e189()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/predefined.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e191()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Combined Set//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e192()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/w3centities-f.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e194()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Combined Set//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e195()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/w3centities.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e197()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Latin for HTML//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e198()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/xhtml1-lat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e200()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Special for HTML//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e201()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/xhtml1-special.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e203()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Symbol for HTML//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e204()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/2007/xhtml1-symbol.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e206()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Arrow Relations//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e207()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e209()
    {
        var result =
            Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Binary Operators//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e210()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e212()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Delimiters//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e213()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e215()
    {
        var result =
            Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Negated Relations//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e216()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e219()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Ordinary//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e220()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e222()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Math Symbols: Relations//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e223()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e225()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Box and Line Drawing//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e226()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isobox.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e228()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Russian Cyrillic//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e229()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isocyr1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e231()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Non-Russian Cyrillic//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e232()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isocyr2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e234()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Diacritical Marks//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e235()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isodia.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e237()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Greek Letters//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e238()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e240()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Monotoniko Greek//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e241()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e243()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Greek Symbols//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e244()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e246()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Alternative Greek Symbols//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e247()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isogrk4.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e249()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Latin 1//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e250()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isolat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e253()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Added Latin 2//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e254()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isolat2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e256()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Numeric and Special Graphic//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e257()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isonum.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e259()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES Publishing//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e260()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isopub.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e262()
    {
        var result = Manager.LookupPublic(null, "ISO 8879:1986//ENTITIES General Technical//EN//XML");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e263()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2003/entities/iso8879/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e265()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XSD 1.1//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e266()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2009/XMLSchema/XMLSchema.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e267()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xmlschema11-1/XMLSchema.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e268()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xmlschema11-2/XMLSchema.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e270()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XSD 1.1 Datatypes//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e271()
    {
        var result = Manager.LookupSystem("https://www.w3.org/2009/XMLSchema/datatypes.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e272()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xmlschema11-1/datatypes.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e273()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xmlschema11-2/datatypes.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e275()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Animation//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e276()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-animation.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e278()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Animation Events Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e279()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-animevents-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e281()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Clip//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e282()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-clip.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e284()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Filter//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e285()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-filter.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e287()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Font//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e288()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-font.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e291()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Graphics Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e292()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-graphics-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e294()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Paint Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e295()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-paint-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e297()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Structure//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e298()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-structure.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e300()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Basic Text//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e301()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-basic-text.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e303()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Clip//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e304()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-clip.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e306()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Conditional Processing//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e307()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-conditional.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e309()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Container Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e310()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-container-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e312()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Core Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e313()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-core-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e315()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Cursor//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e316()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-cursor.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e318()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Datatypes//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e319()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-datatypes.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e321()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Document Events Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e322()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-docevents-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e325()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Extensibility//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e326()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-extensibility.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e328()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 External Resources Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e329()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-extresources-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e331()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Filter//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e332()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-filter.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e334()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Font//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e335()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-font.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e337()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Modular Framework//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e338()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-framework.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e340()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Gradient//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e341()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-gradient.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e343()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Graphical Element Events Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e344()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-graphevents-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e346()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Graphics Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e347()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-graphics-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e349()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Hyperlinking//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e350()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-hyperlink.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e352()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Image//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e353()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-image.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e355()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Marker//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e356()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-marker.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e359()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Mask//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e360()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-mask.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e362()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Paint Opacity Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e363()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-opacity-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e365()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Paint Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e366()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-paint-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e368()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Pattern//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e369()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-pattern.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e371()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Color Profile//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e372()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-profile.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e374()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Qualified Name//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e375()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-qname.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e377()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Scripting//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e378()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-script.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e380()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Shape//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e381()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-shape.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e383()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Structure//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e384()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-structure.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e386()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Style//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e387()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-style.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e389()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG Template Qualified Name//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e390()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-template-qname.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e393()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-template.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e395()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 Text//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e396()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-text.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e398()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS SVG 1.1 View//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e399()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-view.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e401()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Viewport Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e402()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-viewport-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e404()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 XLink Attribute//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e405()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg-xlink-attrib.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e407()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Attribute Collection//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e408()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-attribs.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e410()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Attribute Collection//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e411()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic-attribs.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e413()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic-flat.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e415()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Basic Document Model//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e416()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic-model.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e418()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD SVG 1.1 Basic//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e419()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e421()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-flat-20030114.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e424()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-flat.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e426()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Document Model//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e427()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-model.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e429()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD SVG 1.1 Template//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e430()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-template.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e432()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Tiny Attribute Collection//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e433()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny-attribs.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e435()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny-flat.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e437()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES SVG 1.1 Tiny Document Model//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e438()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny-model.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e440()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD SVG 1.1 Tiny//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e441()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e443()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD SVG 1.1//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e444()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e446()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Access Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e447()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-access-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e449()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Access Attribute Qnames 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e450()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-access-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e452()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Java Applets 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e453()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-applet-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e456()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Base Architecture 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e457()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-arch-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e459()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Common Attributes 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e460()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-attribs-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e462()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Base Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e463()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-base-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e465()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Basic Forms 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e466()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic-form-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e468()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Basic Tables 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e469()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic-table-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e471()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Basic 1.0 Document Model 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e472()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic10-model-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e474()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Basic 1.1 Document Model 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e475()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic11-model-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e477()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML Basic 1.1//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e478()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-basic11.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e480()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML BIDI Override Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e481()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-bdo-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e483()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Block Phrasal 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e484()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-blkphras-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e486()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Block Presentation 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e487()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-blkpres-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e490()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Block Structural 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e491()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-blkstruct-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e493()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Character Entities 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e494()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-charent-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e496()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Client-side Image Maps 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e497()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-csismap-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e499()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Datatypes 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e500()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-datatypes-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e502()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Editing Elements 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e503()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-edit-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e505()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Intrinsic Events 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e506()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-events-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e508()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Forms 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e509()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-form-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e511()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Frames 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e512()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-frames-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e514()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Modular Framework 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e515()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-framework-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e517()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML HyperAttributes 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e518()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-hyperAttributes-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e520()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Hypertext 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e521()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-hypertext-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e524()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Frame Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e525()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-iframe-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e527()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Images 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e528()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-image-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e530()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Phrasal 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e531()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlphras-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e533()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Presentation 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e534()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlpres-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e536()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Structural 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e537()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlstruct-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e539()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inline Style 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e540()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inlstyle-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e542()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Inputmode 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e543()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-inputmode-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e545()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-lat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e547()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Legacy MarkUp 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e548()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-legacy-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e550()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Legacy Redeclarations 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e551()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-legacy-redecl-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e553()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Link Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e554()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-link-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e557()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Lists 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e558()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-list-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e560()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Metainformation 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e561()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-meta-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e563()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Metainformation 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e564()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-meta-2.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e566()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML MetaAttributes 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e567()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-metaAttributes-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e569()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Name Identifier 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e570()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-nameident-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e572()
    {
        var result = Manager.LookupPublic(null, "-//W3C//NOTATIONS XHTML Notations 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e573()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-notations-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e575()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Embedded Object 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e576()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-object-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e578()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Param Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e579()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-param-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e581()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Presentation 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e582()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-pres-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e584()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML-Print 1.0 Document Model 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e585()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-print10-model-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e587()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Qualified Names 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e588()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e591()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML+RDFa Document Model 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e592()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-rdfa-model-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e594()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML RDFa Attribute Qnames 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e595()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-rdfa-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e597()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Role Attribute 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e598()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-role-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e600()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML Role Attribute Qnames 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e601()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-role-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e603()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Ruby 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e604()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-ruby-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e606()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Scripting 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e607()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-script-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e609()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-special.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e611()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Server-side Image Maps 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e612()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-ssismap-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e614()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Document Structure 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e615()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-struct-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e617()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Style Sheets 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e618()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-style-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e620()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-symbol.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e623()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Tables 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e624()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-table-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e626()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Target 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e627()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-target-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e629()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Text 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e630()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml-text-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e632()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Frameset//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e633()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml1-frameset.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e635()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Strict//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e636()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml1-strict.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e638()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Transitional//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e639()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml1-transitional.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e641()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES XHTML 1.1 Document Model 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e642()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml11-model-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e644()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.1//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e645()
    {
        var result = Manager.LookupSystem("https://www.w3.org/MarkUp/DTD/xhtml11.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e647()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e649()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e651()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e654()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e656()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e658()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e660()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isobox.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e662()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isocyr1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e664()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isocyr2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e666()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isodia.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e668()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e670()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e672()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e674()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isogrk4.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e677()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isolat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e679()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isolat2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e681()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isomfrk.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e683()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isomopf.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e685()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isomscr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e687()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isonum.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e689()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isopub.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e691()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e693()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/mathml.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e695()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/mmlalias.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e697()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml1/mmlextra.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e700()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/html/lat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e702()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/html/special.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e704()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/html/symbol.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e706()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e708()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e710()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e712()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e714()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e716()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e718()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Box and Line Drawing for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e719()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isobox.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e721()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Russian Cyrillic for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e722()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isocyr1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e725()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Non-Russian Cyrillic for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e726()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isocyr2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e728()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Diacritical Marks for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e729()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isodia.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e731()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e733()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e735()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e737()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isogrk4.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e739()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 1 for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e740()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isolat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e742()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Latin 2 for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e743()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isolat2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e745()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Numeric and Special Graphic for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e746()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isonum.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e748()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Publishing for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e749()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isopub.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e751()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso8879/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e754()
    {
        var result = Manager.LookupPublic(null,
            "-//W3C//ENTITIES Added Math Symbols: Arrow Relations for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e755()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e757()
    {
        var result = Manager.LookupPublic(null,
            "-//W3C//ENTITIES Added Math Symbols: Binary Operators for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e758()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e760()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Delimiters for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e761()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e763()
    {
        var result = Manager.LookupPublic(null,
            "-//W3C//ENTITIES Added Math Symbols: Negated Relations for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e764()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e766()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Ordinary for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e767()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e769()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Added Math Symbols: Relations for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e770()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e772()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Greek Symbols for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e773()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e775()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isogrk4.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e777()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Fraktur for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e778()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isomfrk.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e780()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Open Face for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e781()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isomopf.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e783()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Math Alphabets: Script for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e784()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isomscr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e787()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES General Technical for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e788()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/iso9573-13/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e790()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e792()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e794()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e796()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e798()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e800()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e802()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isobox.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e804()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isocyr1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e806()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isocyr2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e808()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isodia.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e811()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e813()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isolat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e815()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isolat2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e817()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isomfrk.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e819()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isomopf.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e821()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isomscr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e823()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isonum.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e825()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isopub.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e827()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e829()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Aliases for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e830()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml/mmlalias.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e832()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Extra for MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e833()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml/mmlextra.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e836()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml2-a.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e838()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES MathML 2.0 Qualified Names 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e839()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml2-qname-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e841()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD MathML 2.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e842()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mathml2.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e844()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mmlalias.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e846()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/mmlextra.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e848()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/xhtml-math11-f-a.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e850()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/xhtml-math11-f.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e852()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml2/xhtml-math11.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e854()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsa.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e856()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsb.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e858()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsc.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e861()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsn.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e863()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamso.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e865()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isoamsr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e867()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isobox.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e869()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isocyr1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e871()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isocyr2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e873()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isodia.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e875()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isogrk3.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e877()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isolat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e879()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isolat2.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e881()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isomfrk.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e884()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isomopf.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e886()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isomscr.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e888()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isonum.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e890()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isopub.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e892()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/isotech.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e894()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES MathML 3.0 Qualified Names 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e895()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mathml3-qname.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e897()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mathml3.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e899()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mmlalias.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e901()
    {
        var result = Manager.LookupSystem("https://www.w3.org/Math/DTD/mathml3/mmlextra.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e903()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD SVG 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e904()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e906()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Ruby 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e907()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/ruby/xhtml-ruby-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e910()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML Basic 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e911()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-basic/xhtml-basic10.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e913()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML BIDI Override Element 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e914()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-bdo-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e916()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Client-side Image Maps 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e917()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-csismap-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e919()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Editing Elements 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e920()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-edit-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e922()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Legacy MarkUp 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e923()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-legacy-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e925()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Server-side Image Maps 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e926()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-ssismap-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e928()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ELEMENTS XHTML Tables 1.0//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e929()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-table-1.mod");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e931()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Latin 1 for XHTML//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e932()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-lat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e934()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Symbols for XHTML//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e935()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-special.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e937()
    {
        var result = Manager.LookupPublic(null, "-//W3C//ENTITIES Special for XHTML//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e938()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml-modularization/DTD/xhtml-symbol.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e940()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml-lat1.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e943()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml-special.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e945()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml-symbol.ent");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e947()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Frameset//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e948()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e950()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Strict//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e951()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e953()
    {
        var result = Manager.LookupPublic(null, "-//W3C//DTD XHTML 1.0 Transitional//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupSystemd2e954()
    {
        var result = Manager.LookupSystem("https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e956()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xmlschema-1/XMLSchema.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e958()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xmlschema-2/datatypes.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e960()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xmlschema11-1/XMLSchema.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e962()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xmlschema11-2/datatypes.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e964()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-json.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e966()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-xslt30.rnc");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e969()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-xslt30.rng");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e971()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xslt-30/schema-for-xslt30.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e973()
    {
        var result = Manager.LookupUri("https://www.w3.org/TR/xslt-30/xml-to-json.xsl");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e975()
    {
        var result = Manager.LookupUri("https://www.w3.org/2007/schema-for-xslt20.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e977()
    {
        var result = Manager.LookupUri("https://xmlcatalogs.org/schema/1.1/catalog.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e978()
    {
        var result = Manager.LookupUri("http://www.oasis-open.org/committees/entity/release/1.1/catalog.xsd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e983()
    {
        var result = Manager.LookupUri("https://xmlcatalogs.org/schema/1.1/catalog.rnc");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e985()
    {
        var result = Manager.LookupUri("https://xmlcatalogs.org/schema/1.1/catalog.rng");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e986()
    {
        var result = Manager.LookupUri("http://www.oasis-open.org/committees/entity/release/1.1/catalog.rng");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupPublicd2e991()
    {
        var result = Manager.LookupPublic(null, "-//OASIS//DTD XML Catalogs V1.1//EN");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e992()
    {
        var result = Manager.LookupUri("https://xmlcatalogs.org/schema/1.1/catalog.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e993()
    {
        var result = Manager.LookupUri("http://www.oasis-open.org/committees/entity/release/1.1/catalog.dtd");
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Gen_lookupUrid2e999()
    {
        var result = Manager.LookupUri("https://xmlresolver.org/data/resolver/succeeded/test/check.xml");
        Assert.That(result, Is.Not.Null);
    }
}
