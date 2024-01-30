using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntryDelegateSystem : Entry 
{
    public readonly string SystemIdStart;
    public readonly Uri Catalog;
        
    public EntryDelegateSystem(Uri baseUri, string? id, string start, string catalog) : base(baseUri, id) 
    {
        if (start.StartsWith("classpath:/")) {
            // classpath:/path/to/thing is the same as classpath:path/to/thing
            // normalize without the leading slash.
            SystemIdStart = "classpath:" + start[11..];
        } else {
            SystemIdStart = start;
        }
        Catalog = UriUtils.Resolve(baseUri, catalog);
    }

    public override EntryType GetEntryType() 
    {
        return EntryType.DelegateSystem;
    }
        
    public override string ToString() 
    {
        return $"delegateSystem {SystemIdStart} {Entry.Rarr} {Catalog}";
    }
}
