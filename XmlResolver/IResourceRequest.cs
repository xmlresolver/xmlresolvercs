using System;

namespace XmlResolver;

#nullable enable

public interface IResourceRequest
{
    IResolverConfiguration Configuration
    {
        get;
    }

    string? Nature
    {
        get;
    }

    string? Purpose
    {
        get;
    }

    string? Uri
    {
        get;
        set;
    }

    string? BaseUri
    {
        get;
        set;
    }
    Uri? GetAbsoluteUri();

    string? EntityName
    {
        get;
        set;
    }

    string? SystemId
    {
        get;
    }

    string? PublicId
    {
        get;
        set;
    }

    string? Encoding
    {
        get;
        set;
    }

    bool ResolvingAsEntity
    {
        get;
    }

    bool OpenStream
    {
        get;
        set;
    }
}