using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntryUriSuffix : Entry
{
    public readonly string UriSuffix;
    public readonly Uri ResourceUri;

    public EntryUriSuffix(Uri baseUri, string? id, string suffix, string uri) : base(baseUri, id)
    {
        UriSuffix = suffix;
        ResourceUri = UriUtils.Resolve(baseUri, uri);
    }

    public override EntryType GetEntryType()
    {
        return EntryType.UriSuffix;
    }
        
    public override string ToString()
    {
        return $"uriSuffix {UriSuffix} {Entry.Rarr} {ResourceUri}";
    }
}
