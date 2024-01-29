using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntryDoctype : Entry
{
    public readonly string Name;
    public readonly Uri ResourceUri;
        
    public EntryDoctype(Uri baseUri, string? id, string name, string uri) : base(baseUri, id)
    {
        Name = name;
        ResourceUri = UriUtils.Resolve(baseUri, uri);
    }

    public override Catalog.Entry.Entry.EntryType GetEntryType()
    {
        return Catalog.Entry.Entry.EntryType.Doctype;
    }
        
    public override string ToString()
    {
        return $"doctype {Name} {Entry.Rarr} {ResourceUri}";
    }
}
