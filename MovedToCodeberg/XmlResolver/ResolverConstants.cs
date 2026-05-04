namespace XmlResolver;

public static class ResolverConstants {
    /** The XML Namespace name of OASIS XML Catalog files, "urn:oasis:names:tc:entity:xmlns:xml:catalog". */
    public static readonly string CATALOG_NS = "urn:oasis:names:tc:entity:xmlns:xml:catalog";
    /** The XML Namespace name of OASIS XML Catalog files, "urn:oasis:names:tc:entity:xmlns:xml:catalog". */
    public static readonly string TR9401_NS = "urn:oasis:names:tc:entity:xmlns:tr9401:catalog";
    /** The XML Namespace name of RDDL, "http://www.rddl.org/". */
    public static readonly string RDDL_NS ="http://www.rddl.org/";
    /** The XML Namespace name of XLink, "http://www.w3.org/1999/xlink". */
    public static readonly string XLINK_NS = "http://www.w3.org/1999/xlink";
    /** The (X)HTML Namespace name, "http://www.w3.org/1999/xhtml". */
    public static readonly string HTML_NS = "http://www.w3.org/1999/xhtml";
    /** The XML Namespace name, "http://www.w3.org/XML/1998/namespace". */
    public static readonly string XML_NS = "http://www.w3.org/XML/1998/namespace";
    /** The XML Namespace name of XML Resolver Catalog extensions, "http://xmlresolver.org/ns/catalog". */
    public static readonly string XMLRESOURCE_EXT_NS = "http://xmlresolver.org/ns/catalog";
    
    /** The schema validation purpose. */
    public static readonly string PURPOSE_SCHEMA_VALIDATION = "http://www.rddl.org/purposes#schema-validation";
    /** The XML Schema nature. */
    public static readonly string NATURE_XML_SCHEMA = "http://www.w3.org/2001/XMLSchema";
    /** The XML Schema 1.1 nature. */
    public static readonly string NATURE_XML_SCHEMA_1_1 = "http://www.w3.org/2001/XMLSchema/v1.1";
    /** The RELAX NG nature. */
    public static readonly string NATURE_RELAX_NG = "http://relaxng.org/ns/structure/1.0";
       
    /** The text nature. */
    public static readonly string TEXT_NATURE = "https://www.iana.org/assignments/media-types/text/plain";
    /** The text nature. */
    public static readonly string BINARY_NATURE = "https://www.iana.org/assignments/media-types/application/octet-stream";
    /** The XML nature. */
    public static readonly string XML_NATURE = "https://www.iana.org/assignments/media-types/application/xml";
    /** The DTD nature. */
    public static readonly string DTD_NATURE = "https://www.iana.org/assignments/media-types/application/xml-dtd";
    /** The XML Schema nature. */
    public static readonly string SCHEMA_NATURE = "http://www.w3.org/2001/XMLSchema";
    /** The RELAX NG nature. */
    public static readonly string RELAXNG_NATURE = "http://relaxng.org/ns/structure/1.0";
    /** The XML external parsed entity nature. */
    public static readonly string EXTERNAL_ENTITY_NATURE = "https://www.iana.org/assignments/media-types/application/xml-external-parsed-entity";
    /** The symbolic name for "any" nature, <code>null</code>. */
    public static readonly string ANY_NATURE = null;
       
    /** The validation purpose. */
    public static readonly string VALIDATION_PURPOSE = "http://www.rddl.org/purposes#validation";
    /** The symbolic name for "any" purpose, <code>null</code>. */
    public static readonly string ANY_PURPOSE = null;
}
