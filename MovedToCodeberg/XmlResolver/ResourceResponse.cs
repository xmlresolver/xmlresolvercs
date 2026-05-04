using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace XmlResolver;

#nullable enable

public class ResourceResponse : IResourceResponse
{
    private readonly Dictionary<string, List<string>> _headers = new();

    internal ResourceResponse(IResourceRequest request, Uri? uri = null)
    {
        Request = request;
        Rejected = false;
        Uri = request.GetAbsoluteUri();
        ResolvedUri = uri;
        Resolved = uri != null;
    }

    public IResourceRequest Request
    {
        get;
        internal set;
    }

    public string? ContentType
    {
        get;
        internal set;
    }

    public Stream? Stream
    {
        get;
        internal set;
    }

    public bool Rejected
    {
        get;
        internal set;
    } = false;

    public bool Resolved
    {
        get;
        internal set;
    } = false;

    public Uri? Uri
    {
        get;
        internal set;
    }

    public Uri? ResolvedUri
    {
        get;
        internal set;
    }

    public string? Encoding
    {
        get;
        internal set;
    }

    public IDictionary<string, List<string>> GetHeaders()
    {
        return _headers.ToImmutableDictionary();
    }

    internal void SetHeaders(IDictionary<string, List<string>> newHeaders)
    {
        _headers.Clear();
        foreach (var item in newHeaders)
        {
            _headers.Add(item.Key, item.Value);
        }
    }
    
    public List<string> GetHeader(string name)
    {
        return _headers.TryGetValue(name, out var value) ? value : [];
    }

    public int StatusCode
    {
        get;
        internal set;
    } = -1;
}