using System;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace XmlResolver;

#nullable enable

public class ResourceRequest: IResourceRequest
{
    private string? _uri = null;
    private string? _baseUri = null;

    internal ResourceRequest(IResolverConfiguration config,
        string? nature = null /*ResolverConstants.ANY_NATURE*/,
        string? purpose = null /*ResolverConstants.ANY_PURPOSE*/)
    {
        Configuration = config;
        Nature = nature ?? ResolverConstants.ANY_NATURE;
        Purpose = purpose ?? ResolverConstants.ANY_PURPOSE;
        ResolvingAsEntity = nature == ResolverConstants.DTD_NATURE || nature == ResolverConstants.EXTERNAL_ENTITY_NATURE;
    }

    public IResolverConfiguration Configuration
    {
        get;
    }
    
    public string? Nature
    {
        get;
    }

    public string? Purpose
    {
        get;
    }

    public string? Uri
    {
        get => _uri;
        set => SetUri(value);
    }

    private void SetUri(string? newUri)
    {
        _uri = newUri;
        if (_uri != null && (bool)Configuration.GetFeature(ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS))
        {
            _uri = UriUtils.WindowsPathUri(_uri);
        }
    }

    public void SetUri(Uri newUri)
    {
        _uri = newUri.ToString();
    }

    public string? BaseUri
    {
        get => _baseUri;
        set => SetBaseUri(value);
    }

    private void SetBaseUri(string? newBaseUri)
    {
        _baseUri = newBaseUri;
        if (_baseUri != null && (bool)Configuration.GetFeature(ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS))
        {
            _baseUri = UriUtils.WindowsPathUri(_baseUri);
        }
    }

    public void SetBaseUri(Uri newBaseUri)
    {
        _baseUri = newBaseUri.ToString();
    }

    public Uri? GetAbsoluteUri()
    {
        Uri? abs = null;
        
        if (_baseUri != null)
        {
            abs = new Uri(_baseUri);
            if (abs.IsAbsoluteUri)
            {
                return string.IsNullOrEmpty(_uri) ? abs : UriUtils.Resolve(abs, _uri);
            }
        }

        if (_uri == null)
        {
            return null;
        }

        abs = new Uri(_uri);
        return abs.IsAbsoluteUri ? abs : null;
    }

    public string? EntityName
    {
        get;
        set;
    }

    public string? SystemId => _uri;

    public string? PublicId
    {
        get;
        set;
    }

    public string? Encoding
    {
        get;
        set;
    }

    public bool ResolvingAsEntity
    {
        get;
    }

    public bool OpenStream
    {
        get;
        set;
    }
}