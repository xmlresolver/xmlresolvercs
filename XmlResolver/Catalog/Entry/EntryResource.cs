using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public abstract class EntryResource : Entry 
{
    public readonly Uri ResourceUri;

    public EntryResource(Uri baseUri, string? id, string uri) : base(baseUri, id)
    {
        ResourceUri = UriUtils.Resolve(baseUri, uri);
    }
}
