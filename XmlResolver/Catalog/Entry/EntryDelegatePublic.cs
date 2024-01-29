using XmlResolver.Utils;

namespace XmlResolver.Catalog.Entry;

public class EntryDelegatePublic : Entry
{
    public readonly bool PreferPublic;
    public readonly string PublicIdStart;
    public readonly Uri Catalog;
        
    public EntryDelegatePublic(Uri baseUri, string? id, string start, string catalog, bool prefer) : base(baseUri, id)
    {
        PreferPublic = prefer;
        PublicIdStart = start;
        Catalog = UriUtils.Resolve(baseUri, catalog);
    }

    public override EntryType GetEntryType()
    {
        return EntryType.DelegatePublic;
    }
                    
    public override string ToString() {
        return $"delegatePublic {PublicIdStart} {Rarr} {Catalog}";
    }
}
