using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntryNextCatalog : Entry
{
    public readonly Uri Catalog;

    public EntryNextCatalog(Uri baseUri, string? id, string catalog) : base(baseUri, id)
    {
        Catalog = UriUtils.Resolve(baseUri, catalog);
    }

    public override EntryType GetEntryType()
    {
        return EntryType.NextCatalog;
    }

    public override string ToString()
    {
        return $"nextCatalog {Entry.Rarr} {Catalog}";
    }
}
