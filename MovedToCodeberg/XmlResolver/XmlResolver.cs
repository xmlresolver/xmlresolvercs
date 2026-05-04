using System;
using System.Collections.Generic;
using System.Xml;
using NLog;
using XmlResolver.Adapters;
using XmlResolver.Features;
using XmlResolver.Utils;

#nullable enable

namespace XmlResolver;

public class XmlResolver
{
    private static readonly ResolverLogger Logger = new(LogManager.GetCurrentClassLogger());

    /// <summary>
    /// Creates a new resolver with a default configuration.
    /// </summary>
    public XmlResolver()
    {
        Configuration = new XmlResolverConfiguration();
    }

    /// <summary>
    /// Creates a new resolver with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    public XmlResolver(XmlResolverConfiguration config)
    {
        Configuration = config;
    }

    /// <summary>
    /// Returns the configuration of the underlying resolver.
    /// </summary>
    /// <returns>The configuration object.</returns>
    public IResolverConfiguration Configuration
    {
        get;
    }

    public IResourceRequest GetRequest(string? uri)
    {
        var req = new ResourceRequest(Configuration, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE)
        {
            Uri = uri
        };
        return req;
    }

    public IResourceRequest GetRequest(string? uri, string? baseUri)
    {
        var req = new ResourceRequest(Configuration, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE)
        {
            Uri = uri,
            BaseUri = baseUri
        };
        return req;
    }

    public IResourceRequest GetRequest(string? uri, string? nature, string? purpose)
    {
        var req = new ResourceRequest(Configuration, nature, purpose)
        {
            Uri = uri
        };
        return req;
    }

    public IResourceRequest GetRequest(string? uri, string? baseUri, string? nature, string? purpose)
    {
        var req = new ResourceRequest(Configuration, nature, purpose)
        {
            Uri = uri,
            BaseUri = baseUri
        };
        return req;
    }

    public IResourceResponse LookupEntity(string? name, string? publicId, string? systemId, string? baseUri = null)
    {
        var req = GetRequest(systemId, ResolverConstants.EXTERNAL_ENTITY_NATURE, ResolverConstants.ANY_PURPOSE);
        req.EntityName = name;
        req.BaseUri = baseUri;
        req.PublicId = publicId;
        return Lookup(req);
    }

    public IResourceResponse ResolveEntity(string? name, string? publicId, string? systemId, string? baseUri = null)
    {
        var resp = LookupEntity(name, publicId, systemId, baseUri);
        
        if (resp.Resolved)
        {
            return ResourceAccess.GetResource(resp);
        }

        return resp;
    }

    public IResourceResponse LookupDoctype(string name)
    {
        return LookupDoctype(name, null, null, null);
    }

    public IResourceResponse LookupDoctype(string name, string? publicId, string? systemId)
    {
        return LookupDoctype(name, publicId, systemId, null);
    }

    public IResourceResponse LookupDoctype(string name, string? publicId, string? systemId, string? baseUri)
    {
        var req = GetRequest(systemId, ResolverConstants.DTD_NATURE, ResolverConstants.ANY_PURPOSE);
        req.EntityName = name;
        req.BaseUri = baseUri;
        req.PublicId = publicId;
        return Lookup(req);
    }

    public IResourceResponse LookupUri(string href, string? baseUri = null)
    {
        var req = GetRequest(href, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE);
        req.BaseUri = baseUri;
        return Lookup(req);
    }

    public IResourceResponse ResolveUri(string uri, string? baseUri = null)
    {
        var resp = LookupUri(uri, baseUri);
        return ResourceAccess.GetResource(resp);
    }    
    
    public IResourceResponse LookupNamespace(string href, string? nature, string? purpose)
    {
        var req = GetRequest(href, nature, purpose);
        return Lookup(req);
    }

    public IResourceResponse Lookup(IResourceRequest request)
    {
        CatalogManager manager = (CatalogManager) Configuration.GetFeature(ResolverFeature.CATALOG_MANAGER);

        if (ResolverConstants.DTD_NATURE == request.Nature)
        {
            return LookupDtd(request, manager);
        }

        if (ResolverConstants.EXTERNAL_ENTITY_NATURE == request.Nature)
        {
            return LookupEntity(request, manager);
        }

        var response = LookupUri(request, manager);

        if (response.Resolved)
        {
            return response;
        }

        if (request.Nature == ResolverConstants.ANY_NATURE)
        {
            // What about an entity?
            response = LookupEntity(request, manager);
            if (!response.Resolved && request.EntityName != null)
            {
                // What about a DTD?
                response = LookupDtd(request, manager);
            }
        }

        return response;
    }

    private IResourceResponse LookupDtd(IResourceRequest request, CatalogManager manager)
    {
        var name = request.EntityName;
        var publicId = request.PublicId;
        var systemId = request.SystemId;
        var baseUri = request.BaseUri;

        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        
        Logger.Debug("lookupDoctype: {0} {1} (baseUri: {2}, publicId: {3})", name, systemId,baseUri, publicId);

        var found = manager.LookupDoctype(name, systemId, publicId);
        if (found == null && baseUri != null)
        {
            var absuri = MakeAbsolute(request);
            if (absuri != null)
            {
                found = manager.LookupDoctype(name, absuri.ToString(), publicId);
            }
        }

        if (found == null)
        {
            Logger.Debug("lookupDoctype: null");
        }
        else
        {
            Logger.Debug("lookupDoctype: {0}", found);
        }

        return new ResourceResponse(request, found);
    }

    private IResourceResponse LookupEntity(IResourceRequest request, CatalogManager manager)
    {
        var name = request.EntityName;
        var publicId = request.PublicId;
        var systemId = request.SystemId;
        var baseUri = request.BaseUri;
        
        var allowed = (List<string>) Configuration.GetFeature(ResolverFeature.ACCESS_EXTERNAL_ENTITY);

        if (name == null && publicId == null && systemId == null && baseUri == null)
        {
            Logger.Debug("lookupEntity: null");
            return new ResourceResponse(request);
        }

        var systemIdUri = MakeUri(systemId);
        if (systemIdUri != null && systemIdUri.IsAbsoluteUri)
        {
            if (UriUtils.ForbidAccess(allowed, systemId, (bool)Configuration.GetFeature(ResolverFeature.MERGE_HTTPS)))
            {
                Logger.Debug("lookupEntity (access denied): {0}", systemId);
                throw new ArgumentException("LookupEntity (access denied): " + systemId);
            }
        }
        
        Logger.Debug("lookupEntity: {0}{1} (baseUri: {2}, publicId: {3})", (name == null ? "" : name + " "), systemId, baseUri, publicId);

        Uri? resolved = manager.LookupEntity(name, systemId, publicId);
        if (resolved == null && systemId != null && (bool) Configuration.GetFeature(ResolverFeature.URI_FOR_SYSTEM))
        {
            resolved = manager.LookupUri(systemId);
        }

        if (resolved != null)
        {
            Logger.Debug("lookupEntity: {0}", resolved);
            return new ResourceResponse(request, resolved);
        }
        
        var absSystem = MakeAbsolute(request);
        if (absSystem != null)
        {
            if (UriUtils.ForbidAccess(allowed, absSystem.ToString(), (bool) Configuration.GetFeature(ResolverFeature.MERGE_HTTPS)))
            {
                Logger.Debug("lookupEntity (access denied): {0}", absSystem);
                throw new ArgumentException("LookupEntity (access denied): " + absSystem);
            }
            
            resolved = manager.LookupEntity(name, absSystem.ToString(), publicId);
            if (resolved == null && (bool) Configuration.GetFeature(ResolverFeature.URI_FOR_SYSTEM))
            {
                resolved = manager.LookupUri(absSystem.ToString());
            }
        }

        if (resolved == null)
        {
            if (absSystem == null)
            {
                Logger.Debug("lookupEntity: null");
                return new ResourceResponse(request);
            }
            Logger.Debug("lookupEntity: {0}", absSystem);
            return new ResourceResponse(request, absSystem);
        }
        
        Logger.Debug("lookupEntity: {0}", resolved);
        return new ResourceResponse(request, resolved);
    }

    private IResourceResponse LookupUri(IResourceRequest request, CatalogManager manager)
    {
        var systemId = request.SystemId;
        var baseUri = request.BaseUri;
   
        var allowed = (List<string>) Configuration.GetFeature(ResolverFeature.ACCESS_EXTERNAL_ENTITY);

        if (systemId == null && baseUri == null)
        {
            Logger.Debug("lookupUri: null");
            return new ResourceResponse(request);
        }

        systemId ??= baseUri;

        var systemIdUri = MakeUri(systemId);
        if (systemIdUri != null && systemIdUri.IsAbsoluteUri)
        {
            if (UriUtils.ForbidAccess(allowed, systemId, (bool)Configuration.GetFeature(ResolverFeature.MERGE_HTTPS)))
            {
                Logger.Debug("LookupUri (access denied): {0}", systemId);
                throw new ArgumentException("LookupUri (access denied): " + systemId);
            }
        }
        
        Logger.Debug("LookupUri: {0} (baseUri: {1})", systemId, baseUri);

        Uri? resolved = manager.LookupNamespaceUri(systemId, request.Nature, request.Purpose);
        
        if (resolved != null)
        {
            Logger.Debug("LookupUri: {0}", resolved);
            return new ResourceResponse(request, resolved);
        }
        
        var absSystem = MakeAbsolute(request);
        if (absSystem != null)
        {
            if (UriUtils.ForbidAccess(allowed, absSystem.ToString(), (bool) Configuration.GetFeature(ResolverFeature.MERGE_HTTPS)))
            {
                Logger.Debug("LookupUri (access denied): {0}", absSystem);
                throw new ArgumentException("LookupUri (access denied): " + absSystem);
            }
            
            resolved = manager.LookupNamespaceUri(absSystem.ToString(), request.Nature, request.Purpose);
        }

        if (resolved == null)
        {
            if (absSystem == null)
            {
                Logger.Debug("LookupUri: null");
                return new ResourceResponse(request);
            }
            Logger.Debug("LookupUri: {0}", absSystem);
            return new ResourceResponse(request, absSystem);
        }
        
        Logger.Debug("LookupUri: {0}", resolved);
        return new ResourceResponse(request, resolved);
    }

    public IResourceResponse Resolve(IResourceRequest request)
    {
        var lookup = Lookup(request);
        if (lookup.Rejected)
        {
            return lookup;
        }

        var tryRddl = (bool)Configuration.GetFeature(ResolverFeature.PARSE_RDDL)
                      && lookup.Request.Nature != ResolverConstants.ANY_NATURE 
                      && lookup.Request.Purpose != ResolverConstants.ANY_PURPOSE;

        if (tryRddl)
        {
            if (lookup.Resolved)
            {
                lookup = RddlLookup(lookup);
            }
            else
            {
                var absuri = lookup.Request.GetAbsoluteUri();
                if (absuri != null)
                {
                    lookup = RddlLookup(lookup, absuri);
                }
            }
        }

        return Configuration.GetResource(lookup);
    }

    private IResourceResponse RddlLookup(IResourceResponse lookup)
    {
        var resolved = lookup.ResolvedUri;
        if (resolved != null)
        {
            return RddlLookup(lookup, resolved);
        }

        return lookup;
    }

    private IResourceResponse RddlLookup(IResourceResponse lookup, Uri resolved)
    {
        var rddl = CheckRddl(resolved, lookup.Request.Nature!, lookup.Request.Purpose!);
        if (rddl == null)
        {
            return lookup;
        }

        var rddlRequest = new ResourceRequest(Configuration, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_NATURE);
        rddlRequest.Uri = rddl.ToString();
        rddlRequest.BaseUri = resolved.ToString();
        
        var resp = Lookup(rddlRequest);
        if (!resp.Resolved)
        {
            Logger.Debug("RDDL {0}: {1}", resolved, rddl);
            return new ResourceResponse(lookup.Request, rddl);
        }
        
        // FIXME: In Java, that's resp.getURI()...
        Logger.Debug("RDDL {0}: {1}", resolved, resp.ResolvedUri);
        return resp;
    }

    private Uri? CheckRddl(Uri uri, string nature, string purpose)
    {
        ResourceRequest req = new ResourceRequest(Configuration, nature, purpose)
        {
            Uri = uri.ToString()
        };
        IResourceResponse rsrc = ResourceAccess.GetResource(req);
        string? contentType = rsrc.ContentType;

        if (contentType != null &&
            (contentType.StartsWith("text/html") || contentType.StartsWith("application/html+xml")))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(rsrc.ResolvedUri!.ToString());
            if (doc.DocumentElement == null)
            {
                return null;
            }
                
            NameTable nt = new NameTable();
            XmlNamespaceManager nsmanage = new XmlNamespaceManager(nt);
            nsmanage.AddNamespace("rddl", "http://www.rddl.org/");
            XmlNodeList? resources = doc.DocumentElement.SelectNodes("//rddl:resource", nsmanage);
            if (resources != null)
            {
                foreach (XmlNode node in resources)
                {
                    if (node.Attributes is not null)
                    {
                        XmlNode? anode = node.Attributes.GetNamedItem("nature", ResolverConstants.XLINK_NS);
                        string? rnature = anode?.InnerText;
                        anode = node.Attributes.GetNamedItem("purpose", ResolverConstants.XLINK_NS);
                        string? rpurpose = anode?.InnerText;
                        anode = node.Attributes.GetNamedItem("href");
                        string? href = anode?.InnerText;
                        if (nature == rnature && purpose == rpurpose && href != null)
                        {
                            var baseUri = new Uri(node.BaseURI);
                            return new Uri(baseUri, href);
                        }
                    }
                }
            }
        }

        return null;
    }

    public System.Xml.XmlResolver GetXmlResolver()
    {
        return new SystemResolver(this);
    }
    
    private static Uri? MakeUri(string? uri)
    {
        if (uri == null)
        {
            return null;
        }

        try
        {
            return new Uri(uri);
        }
        catch (UriFormatException)
        {
            return null;
        }
        
    }
    
    private static Uri? MakeAbsolute(IResourceRequest request)
    {
        var absuri = request.GetAbsoluteUri();
        if (absuri != null && absuri.ToString() != request.Uri)
        {
            return absuri;
        }

        return null;
    }
}