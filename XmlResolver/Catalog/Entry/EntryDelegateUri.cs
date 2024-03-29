using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntryDelegateUri : Entry
{
    public readonly string UriStart;
    public readonly Uri Catalog;
        
    public EntryDelegateUri(Uri baseUri, string? id, string start, string catalog) : base(baseUri, id) {
        if (start.StartsWith("classpath:/"))
        {
            // classpath:/path/to/thing is the same as classpath:path/to/thing
            // normalize without the leading slash.
            UriStart = "classpath:" + start[11..];
        } else
        {
            UriStart = start;
        }
        Catalog = UriUtils.Resolve(baseUri, catalog);
    }

    public override EntryType GetEntryType()
    {
        return EntryType.DelegateUri;
    }
        
    public override string ToString()
    {
        return $"delegateURI {UriStart} {Entry.Rarr} {Catalog}";
    }
}
