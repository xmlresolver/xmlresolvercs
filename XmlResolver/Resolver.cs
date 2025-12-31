#nullable enable

using System;
using NLog;
using XmlResolver.Adapters;

namespace XmlResolver;

/// <summary>
/// The catalog resolver implementation of the main URI-related interfaces.
/// </summary>
[Obsolete]
public class Resolver : System.Xml.XmlResolver, INamespaceResolver {
    private static readonly ResolverLogger Logger = new(LogManager.GetCurrentClassLogger());
    private readonly XmlResolver _xmlResolver;
    private SystemResolver? systemResolver = null;

    /// <summary>
    /// Creates a new resolver with a default configuration.
    /// </summary>
    public Resolver()
    {
        _xmlResolver = new XmlResolver(new XmlResolverConfiguration());
    }

    /// <summary>
    /// Creates a new resolver with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    public Resolver(XmlResolverConfiguration config)
    {
        _xmlResolver = new XmlResolver(config);
    }

    /// <summary>
    /// Creates a new resolver using the specified underlying resolver.
    /// </summary>
    /// <para>The underlying resolver is used directly, it is not copied. If the underlying resolver is used in
    /// several resolvers, any changes made will simultaneously effect all the resolvers.
    /// </para>
    /// <param name="resolver">The underlying resolver.</param>
    public Resolver(XmlResolver resolver)
    {
        _xmlResolver = resolver;
    }

    public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn)
    {
        systemResolver ??= new SystemResolver(_xmlResolver);
        return systemResolver.GetEntity(absoluteUri, role, ofObjectToReturn);
    }

    public override Uri ResolveUri(Uri? baseUri, string? relativeUri)
    {
        systemResolver ??= new SystemResolver(_xmlResolver);
        return systemResolver.ResolveUri(baseUri, relativeUri);
    }

    public object? GetEntity(Uri absoluteUri, string? nature, string? purpose)
    {
        var req = _xmlResolver.GetRequest(absoluteUri.ToString(), nature, purpose);
        var resp = _xmlResolver.Lookup(req);
        return ResourceAccess.GetResource(resp).Stream;
    }

    public object? GetEntity(string href, Uri? baseUri, string? nature, string? purpose)
    {
        var req = _xmlResolver.GetRequest(href, baseUri?.ToString(), nature, purpose);
        var resp = _xmlResolver.Lookup(req);
        return ResourceAccess.GetResource(resp).Stream;
    }
}