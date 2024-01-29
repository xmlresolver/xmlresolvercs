using System.Data;
using System.Xml;
using NLog;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace XmlResolver.Tools;

public class ResolvingXmlReader : XmlReader
{
    protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

    // We can't have a static resolver here because we can't change the resolver
    // after the underlying XmlReader is created.
    private readonly XmlReader _reader;
    private XmlResolver resolver;
    private bool seenRoot;
    private bool parseStarted;
    private bool allowXmlCatalogPI;

    public ResolvingXmlReader(Uri uri) : this(null, uri, null, null)
    {
        // nop
    }

    public ResolvingXmlReader(Stream data) : this(data, null, null, null)
    {
        // nop
    }

    public ResolvingXmlReader(Uri uri, Stream data) : this(data, uri, null, null)
    {
        // nop
    }

    public ResolvingXmlReader(Uri? uri, Stream? data, XmlReaderSettings settings) : this(data, uri, settings, null)
    {
        // nop
    }

    public ResolvingXmlReader(Uri uri, XmlReaderSettings settings, XmlResolver resolver) : this(null, uri, settings,
        resolver)
    {
        // nop
    }

    public ResolvingXmlReader(Stream data, XmlReaderSettings settings, XmlResolver resolver) : this(data, null,
        settings, resolver)
    {
        // nop
    }

    public ResolvingXmlReader(Stream? data, Uri? baseUri, XmlReaderSettings? parseSettings, XmlResolver? xmlResolver)
    {
        if (data == null && baseUri == null)
        {
            throw new NoNullAllowedException("One of data or baseUri must be provided");
        }

        resolver = xmlResolver ?? new XmlResolver();
        Stream? stream = data ?? ResourceAccess.GetStream(resolver.Config, baseUri!);
        XmlReaderSettings settings = parseSettings ?? new XmlReaderSettings();
        string baseUriString = baseUri == null ? "" : baseUri.ToString();

        if (stream == null)
        {
            throw new NoNullAllowedException("Attempt to open stream returned null");
        }
        
        settings.XmlResolver = resolver.GetXmlResolver();
        _reader = Create(stream, settings, baseUriString);
        seenRoot = false;
        parseStarted = false;
        allowXmlCatalogPI = false;
    }

    public XmlResolver Resolver => resolver;

    public override string GetAttribute(int i)
    {
        return _reader.GetAttribute(i);
    }

    public override string? GetAttribute(string name)
    {
        return _reader.GetAttribute(name);
    }

    public override string? GetAttribute(string name, string? namespaceURI)
    {
        return _reader.GetAttribute(name, namespaceURI);
    }

    public override string? LookupNamespace(string prefix)
    {
        return _reader.LookupNamespace(prefix);
    }

    public override bool MoveToAttribute(string name)
    {
        return _reader.MoveToAttribute(name);
    }

    public override bool MoveToAttribute(string name, string? ns)
    {
        return _reader.MoveToAttribute(name, ns);
    }

    public override bool MoveToElement()
    {
        return _reader.MoveToElement();
    }

    public override bool MoveToFirstAttribute()
    {
        return _reader.MoveToFirstAttribute();
    }

    public override bool MoveToNextAttribute()
    {
        return _reader.MoveToNextAttribute();
    }

    public override bool Read()
    {
        if (!parseStarted)
        {
            allowXmlCatalogPI = (bool)resolver.Config.GetFeature(ResolverFeature.ALLOW_CATALOG_PI)!;
        }

        bool read = _reader.Read();

        if (read)
        {
            if (!seenRoot)
            {
                if (allowXmlCatalogPI && _reader.NodeType == XmlNodeType.ProcessingInstruction)
                {
                    string? baseUri = _reader.BaseURI;
                    if ("oasis-xml-catalog".Equals(_reader.Name))
                    {
                        string data = _reader.Value;
                        int pos = data.IndexOf("catalog=", StringComparison.Ordinal);
                        if (pos >= 0)
                        {
                            data = data[(pos + 8)..];
                            string quote = data[..1];
                            data = data[1..];
                            pos = data.IndexOf(quote, StringComparison.Ordinal);
                            if (pos >= 0)
                            {
                                data = data[..pos];
                                Uri? catalog = null;
                                catalog = string.IsNullOrEmpty(baseUri) ? UriUtils.NewUri(data) : UriUtils.Resolve(UriUtils.NewUri(baseUri), data);
                                ((XmlResolverConfiguration)resolver.Config).AddCatalog(catalog.ToString());
                            }
                        }

                        if (pos < 0)
                        {
                            logger.Log(ResolverLogger.Warning,
                                "Invalid oasis-xml-catalog processing instruction {0}", _reader.Value);
                        }
                    }
                }

                seenRoot = _reader.NodeType == XmlNodeType.Element;
            }
        }

        parseStarted = true;
        return read;
    }

    public override bool ReadAttributeValue()
    {
        return _reader.ReadAttributeValue();
    }

    public override void ResolveEntity()
    {
        _reader.ResolveEntity();
    }

    public override int AttributeCount => _reader.AttributeCount;
    public override string BaseURI => _reader.BaseURI;
    public override int Depth => _reader.Depth;
    public override bool EOF => _reader.EOF;
    public override bool IsEmptyElement => _reader.IsEmptyElement;
    public override string LocalName => _reader.LocalName;
    public override string NamespaceURI => _reader.NamespaceURI;
    public override XmlNameTable NameTable => _reader.NameTable;
    public override XmlNodeType NodeType => _reader.NodeType;
    public override string Prefix => _reader.Prefix;
    public override ReadState ReadState => _reader.ReadState;
    public override string Value => _reader.Value;
}
