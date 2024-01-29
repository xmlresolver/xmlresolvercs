using System.Reflection;

namespace XmlResolver;

public class ResourceRequest
{
    public readonly IResolverConfiguration Config;
    public readonly string? Nature;
    public readonly string? Purpose;

    public ResourceRequest(IResolverConfiguration config): this(config, ResolverConstants.ANY_NATURE, ResolverConstants.ANY_PURPOSE)
    {
    }

    public ResourceRequest(IResolverConfiguration config, string? nature, string? purpose)
    {
        Config = config;
        Nature = nature;
        Purpose = purpose;
        IsResolvingAsEntity = ResolverConstants.EXTERNAL_ENTITY_NATURE.Equals(nature) ||
                              ResolverConstants.DTD_NATURE.Equals(nature);
    }

    public string? Uri { get; set; } = null;

    // SystemId is an alias for Uri
    public string? SystemId
    {
        get => Uri;
        set => Uri = value;
    }
    public string? BaseUri { get; set; } = null;
    public string? EntityName { get; set; } = null;
    public string? PublicId { get; set; } = null;
    public string? Encoding { get; set; } = null;
    public bool IsResolvingAsEntity { get; set; } = false;
    public bool IsOpenStream { get; set; } = true;
    public Assembly Assembly { get; internal set; } = Assembly.GetExecutingAssembly();

    public Uri? GetAbsoluteUri()
    {
        if (BaseUri != null)
        {
            Uri abs = new Uri(BaseUri);
            if (abs.IsAbsoluteUri)
            {
                return Uri is null or "" ? abs : new Uri(abs, Uri);
            }
        }

        if (Uri != null)
        {
            Uri abs = new Uri(Uri);
            if (abs.IsAbsoluteUri)
            {
                return abs;
            }
        }

        return null;
    }

    public override string ToString()
    {
        string str = EntityName == null ? "" : EntityName + ": ";
        str += Uri;
        if (BaseUri != null)
        {
            str += " (" + BaseUri + ")";
        }

        return str;
    }
}
 