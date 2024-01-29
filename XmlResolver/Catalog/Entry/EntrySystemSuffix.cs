using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntrySystemSuffix : Entry
{
    public readonly string SystemIdSuffix;
    public readonly Uri ResourceUri;

    public EntrySystemSuffix(Uri baseUri, string? id, string suffix, string uri) : base(baseUri, id) 
    {
        SystemIdSuffix = suffix;
        ResourceUri = UriUtils.Resolve(baseUri, uri);
    }

    public override EntryType GetEntryType()
    {
        return EntryType.SystemSuffix;
    }
        
    public override string ToString()
    {
        return $"systemSuffix {SystemIdSuffix} {Rarr} {ResourceUri}";
    }
}
