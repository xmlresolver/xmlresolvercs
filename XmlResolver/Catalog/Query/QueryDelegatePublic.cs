using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public class QueryDelegatePublic : QueryPublic
{
    protected readonly List<Uri> catalogs;

    public QueryDelegatePublic(string? systemId, string? publicId, List<Uri> catalogs) : base(systemId, publicId)
    {
        this.catalogs = new List<Uri>(catalogs);
    }

    public override bool Resolved()
    {
        return true;
    }

    public override List<Uri> UpdateCatalogSearchList(EntryCatalog catalog, List<Uri> oldCatalogs)
    {
        // Delegation replaces the catalog list
        return new(catalogs);
    }
}
