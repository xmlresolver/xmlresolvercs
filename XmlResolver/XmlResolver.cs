using System.Xml;
using NLog;
using XmlResolver.Features;
using XmlResolver.Tools;
using XmlResolver.Utils;

namespace XmlResolver;

public class XmlResolver
{
    public readonly IResolverConfiguration Config;
    protected ResolverLogger Logger;

    public XmlResolver(): this(new XmlResolverConfiguration())
    {
        // nop
    }
    
    public XmlResolver(IResolverConfiguration config)
    {
        Config = config;
        Logger = new(LogManager.GetCurrentClassLogger());
    }

    /// <summary>
    /// Implements <see cref="System.Xml.XmlResolver.GetEntity"/>.
    /// </summary>
    /// <para>The role and return type are ignored because they have no useful purpose in the system
    /// version of the API.</para>
    /// <para>There's an awful failing in the way that the system parser uses this API.
    /// Presented with an entity that has both system and public identifiers, it calls
    /// <c>GetEntity</c> initially passing the public identifier as the absolute URI.
    /// This is complete madness, but if attempting to use the URI throws an exception and
    /// the URI looks like it might be a public identifier, we try to resolve with it.</para>
    /// <para>In principle, an XML document can't have only a public identifier, but this API forces
    /// us to pretend that it might because the public and system identifiers are passed independently
    /// in two separate API calls.</para>
    /// <para>Note that this method will attempt to open the specified URI, for example, over the web,
    /// if resolution fails to find something local in the catalog.</para>
    /// <param name="absoluteUri">The URI.</param>
    /// <param name="role">The role, which is ignored.</param>
    /// <param name="ofObjectToReturn">The type of object to return, which is ignored.</param>
    /// <returns>A stream if the resource was located or null otherwise.</returns>

    public System.Xml.XmlResolver GetXmlResolver()
    {
        return new SystemXmlResolver(this);
    }
    
    public ResourceRequest GetRequest(string? uri)
    {
        return GetRequest(uri, null, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE);
    }

    public ResourceRequest GetRequest(string? uri, string? baseUri)
    {
        return GetRequest(uri, baseUri, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE);
    }

    public ResourceRequest GetRequest(string? uri, string? baseUri, string? nature, string? purpose)
    {
        ResourceRequest request = new ResourceRequest(Config, nature, purpose)
        {
            Uri = uri,
            BaseUri = baseUri,
        };
        
        return request;
    }

    public ResourceResponse LookupEntity(string? publicId, string? systemId)
    {
        return LookupEntity(publicId, systemId, null);
    }

    public ResourceResponse LookupEntity(string? publicId, string? systemId, string? baseUri)
    {
        ResourceRequest request =
            new ResourceRequest(Config, ResolverConstants.EXTERNAL_ENTITY_NATURE, ResolverConstants.ANY_PURPOSE)
            {
                Uri = systemId,
                BaseUri = baseUri,
                PublicId = publicId
            };
        
        return Lookup(request);
    }

    public ResourceResponse LookupDoctype(String name)
    {
        return LookupDoctype(name, null, null, null);
    }

    public ResourceResponse LookupDoctype(String name, string? publicId, string? systemId)
    {
        return LookupDoctype(name, publicId, systemId, null);
    }

    public ResourceResponse LookupDoctype(String name, string? publicId, string? systemId, string? baseUri)
    {
        ResourceRequest request =
            new ResourceRequest(Config, ResolverConstants.DTD_NATURE, ResolverConstants.ANY_PURPOSE)
            {
                EntityName = name,
                Uri = systemId,
                BaseUri = baseUri,
                PublicId = publicId
            };
        
        return Lookup(request);
    }

    public ResourceResponse LookupUri(string href)
    {
        return LookupUri(href, null);
    }

    public ResourceResponse LookupUri(string? href, string? baseUri)
    {
        ResourceRequest request =
            new ResourceRequest(Config, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE)
            {
                Uri = href,
                BaseUri = baseUri
            };

        return Lookup(request);
    }

    public ResourceResponse LookupNamespace(string href, string? nature, string? purpose)
    {
        ResourceRequest request = new ResourceRequest(Config, nature, purpose)
        {
            Uri = href
        };

        return Lookup(request);
    }

    public ResourceResponse Lookup(ResourceRequest request)
    {
        CatalogManager manager = (CatalogManager)Config.GetFeature(ResolverFeature.CATALOG_MANAGER)!;

        if (ResolverConstants.DTD_NATURE.Equals(request.Nature))
        {
            return _lookupDtd(request, manager);
        }

        if (ResolverConstants.EXTERNAL_ENTITY_NATURE.Equals(request.Nature))
        {
            return _lookupEntity(request, manager);
        }

        ResourceResponse response = _lookupUri(request, manager);

        if (!response.IsResolved && ResolverConstants.ANY_NATURE == request.Nature)
        {
            // What about an entity?
            response = _lookupEntity(request, manager);
            if (!response.IsResolved && request.EntityName != null)
            {
                // What about a DTD then?
                response = _lookupDtd(request, manager);
            }
        }

        return response;
    }

    private ResourceResponse _lookupDtd(ResourceRequest request, CatalogManager manager)
    {
        if (request.EntityName == null)
        {
            throw new NullReferenceException("Name must not be null for DTD lookup");
        }

        Uri? found = manager.LookupDoctype(request.EntityName, request.SystemId, request.PublicId);
        if (found == null && request.BaseUri != null)
        {
            Uri? absuri = request.GetAbsoluteUri();
            if (absuri != null)
            {
                found = manager.LookupDoctype(request.EntityName, absuri.ToString(), request.PublicId);
            }
        }

        return new ResourceResponse(request, found);
    }

    private ResourceResponse _lookupEntity(ResourceRequest request, CatalogManager manager)
    {
        if (request.EntityName == null && request.PublicId == null && request.SystemId == null && request.BaseUri == null)
        {
            Logger.Log(ResolverLogger.Request, "lookupEntity: null");
            return new ResourceResponse(request);
        }

        string allowed = (string) Config.GetFeature(ResolverFeature.ACCESS_EXTERNAL_ENTITY)!;
        bool mergeHttps = (bool) (Config.GetFeature(ResolverFeature.MERGE_HTTPS)??false);

        Uri? systemIdUri = _makeUri(request.SystemId);
        if (systemIdUri != null)
        {
            if (systemIdUri.IsAbsoluteUri)
            {
                if (UriUtils.ForbidAccess(allowed, systemIdUri.ToString(), mergeHttps))
                {
                    Logger.Log(ResolverLogger.Request, "lookupEntity (access denied): {0}", systemIdUri.ToString());
                    return new ResourceResponse(request, true);
                }
            }
        }

        Logger.Log(ResolverLogger.Request, "lookupEntity: {0}{1} (baseUri: {2}, publicId: {3})",
            request.EntityName == null ? "" : request.EntityName + " ", request.SystemId??"null",
            request.BaseUri??"null", request.PublicId??"null");

        bool uriForSystem = (bool)(Config.GetFeature(ResolverFeature.URI_FOR_SYSTEM) ?? false);
        Uri? resolved = manager.LookupEntity(request.EntityName, request.SystemId, request.PublicId);
        if (resolved == null && request.SystemId != null && uriForSystem)
        {
            resolved = manager.LookupUri(request.SystemId);
        }

        if (resolved != null)
        {
            return new ResourceResponse(request, resolved);
        }

        Uri? absSystem = _makeAbsolute(request);
        if (absSystem != null)
        {
            if (UriUtils.ForbidAccess(allowed, absSystem.ToString(), mergeHttps))
            {
                Logger.Log(ResolverLogger.Request, "lookupEntity (access denied): {0}", absSystem.ToString());
                return new ResourceResponse(request, true);
            }

            resolved = manager.LookupEntity(request.EntityName, absSystem.ToString(), request.PublicId);
            if (resolved == null && (bool)(Config.GetFeature(ResolverFeature.URI_FOR_SYSTEM) ?? false))
            {
                resolved = manager.LookupUri(absSystem.ToString());
            }
        }

        // On .NET, ResolverFeature.ALWAYS_RESOLVE is always true
        if (resolved == null)
        {
            if (absSystem == null)
            {
                return new ResourceResponse(request);
            }

            return new ResourceResponse(request, absSystem);
        }

        return new ResourceResponse(request, resolved);
    }

    private ResourceResponse _lookupUri(ResourceRequest request, CatalogManager manager)
    {
        if (request.SystemId == null && request.BaseUri == null)
        {
            Logger.Log(ResolverLogger.Request, "lookupUri: null");
            return new ResourceResponse(request);
        }

        string? systemId = request.SystemId;
        string? baseUri = request.BaseUri;

        if (systemId == null)
        {
            systemId = baseUri;
        }
        
        string allowed = (string) Config.GetFeature(ResolverFeature.ACCESS_EXTERNAL_DOCUMENT)!;
        bool mergeHttps = (bool) (Config.GetFeature(ResolverFeature.MERGE_HTTPS)??false);

        Uri? systemIdUri = _makeUri(systemId);
        if (systemIdUri != null)
        {
            if (systemIdUri.IsAbsoluteUri)
            {
                if (UriUtils.ForbidAccess(allowed, systemIdUri.ToString(), mergeHttps))
                {
                    Logger.Log(ResolverLogger.Request, "lookupUri (access denied): {0}", systemIdUri.ToString());
                    return new ResourceResponse(request, true);
                }
            }
        }

        Logger.Log(ResolverLogger.Request, "lookupUri: {0} (baseUri: {1})",
            request.SystemId??"null", request.BaseUri??"null");

        Uri? resolved = manager.LookupNamespaceUri(systemId, request.Nature, request.Purpose);
        if (resolved != null)
        {
            return new ResourceResponse(request, resolved);
        }

        Uri? absSystem = _makeAbsolute(request);
        if (absSystem != null)
        {
            if (UriUtils.ForbidAccess(allowed, absSystem.ToString(), mergeHttps))
            {
                Logger.Log(ResolverLogger.Request, "lookupUri (access denied): {0}", absSystem.ToString());
                return new ResourceResponse(request, true);
            }

            resolved = manager.LookupNamespaceUri(absSystem.ToString(), request.Nature, request.Purpose);
        }

        // On .NET, ResolverFeature.ALWAYS_RESOLVE is always true
        if (resolved == null)
        {
            if (absSystem == null)
            {
                return new ResourceResponse(request);
            }

            return new ResourceResponse(request, absSystem);
        }

        return new ResourceResponse(request, resolved);
    }

    public ResourceResponse Resolve(ResourceRequest request)
    {
        ResourceResponse lookup = Lookup(request);
        if (lookup.IsRejected)
        {
            return lookup;
        }

        bool tryRddl = (bool)(Config.GetFeature(ResolverFeature.PARSE_RDDL) ?? false) &&
                       lookup.Request is { Nature: not null, Purpose: not null };
        if (tryRddl)
        {
            if (lookup.IsResolved)
            {
                lookup = _rddlLookup(lookup, lookup.ResolvedUri);
            }
            else
            {
                var absUri = lookup.Request.GetAbsoluteUri();
                if (absUri != null)
                {
                    lookup = _rddlLookup(lookup, absUri);
                }
            }
        }

        return ResourceAccess.GetResource(lookup);
    }

    private Uri? _makeUri(string? uri)
    {
        if (uri != null)
        {
            try
            {
                return new Uri(uri);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }

        return null;
    }

    private Uri? _makeAbsolute(ResourceRequest request)
    {
        Uri? absUri = request.GetAbsoluteUri();
        if (absUri != null && absUri.ToString() != request.Uri)
        {
            return absUri;
        }

        return null;
    }

    private ResourceResponse _rddlLookup(ResourceResponse lookup)
    {
        if (lookup.Uri == null)
        {
            return lookup;
        }
        return _rddlLookup(lookup, lookup.Uri);
    }

    private ResourceResponse _rddlLookup(ResourceResponse lookup, Uri resolved)
    {
        Uri? rddl = checkRddl(resolved, lookup.Request.Nature!, lookup.Request.Purpose!);
        if (rddl == null)
        {
            return lookup;
        }

        ResourceRequest rddlRequest =
            new ResourceRequest(Config, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE);
        rddlRequest.Uri = rddl.ToString();
        
        ResourceResponse resp = Lookup(rddlRequest);
        if (!resp.IsResolved)
        {
            Logger.Log(ResolverLogger.Response, "RDDL {0}: {1}", resolved.ToString(), rddl.ToString());
            return ResourceAccess.GetResource(resp);
        }
        
        Logger.Log(ResolverLogger.Response, "RDDL {0}: {1}", resolved.ToString(), resp.Uri!.ToString());
        return resp;
    }

    private Uri? checkRddl(Uri resolved, string nature, string purpose)
    {
        ResourceRequest req = new ResourceRequest(Config, nature, purpose);
        req.Uri = resolved.ToString();

        ResourceResponse resp = ResourceAccess.GetResource(req);
        string contentType = resp.ContentType ?? "application/octet-stream";

        Uri? found = null;

        if (contentType is "text/html" or "application/html+xml" or "application/xhtml+xml")
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            ResolvingXmlReader reader = new ResolvingXmlReader(resp.ResolvedUri!, settings, this);
            try
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (found == null && reader.NamespaceURI == ResolverConstants.RDDL_NS &&
                            reader.LocalName == "resource")
                        {
                            string? rnature = reader.GetAttribute("role", ResolverConstants.XLINK_NS);
                            string? rpurpose = reader.GetAttribute("arcrole", ResolverConstants.XLINK_NS);
                            string? href = reader.GetAttribute("href", ResolverConstants.XLINK_NS);
                            if (nature == rnature && purpose == rpurpose && href != null)
                            {
                                Uri baseUri = new Uri(reader.BaseURI);
                                found = new Uri(baseUri, href);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ResolverLogger.Response, "Error parsing RDDL: {0} ({1})", 
                    resolved.ToString(), ex.Message);
            }
        }

        return found;
    }
    
    private class SystemXmlResolver : System.Xml.XmlResolver
    {
        private readonly XmlResolver _resolver;

        public SystemXmlResolver(XmlResolver resolver)
        {
            _resolver = resolver;
        }
        
        public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn)
        {
            ResourceRequest request = _resolver.GetRequest(absoluteUri.ToString());
            ResourceResponse resp = _resolver.Resolve(request);

            if (resp.IsResolved)
            {
                return resp.Stream;
            }

            return null;
        }
    }
    
}