using System;
using System.Collections.Generic;
using System.IO;

namespace XmlResolver;

#nullable enable

public interface IResourceResponse
{
    IResourceRequest Request { get; }
    string? ContentType { get; }
    Stream? Stream { get; }
    bool Rejected { get; }
    bool Resolved { get; }
    Uri? Uri { get; }
    Uri? ResolvedUri { get; }
    string? Encoding { get; }
    int StatusCode { get; }
    IDictionary<string, List<string>> GetHeaders();
    List<string> GetHeader(string name);
}