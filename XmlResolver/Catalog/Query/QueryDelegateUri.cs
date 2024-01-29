using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public class QueryDelegateUri : QueryUri
{
    protected readonly List<Uri> catalogs;

    public QueryDelegateUri(string uri, string? nature, string? purpose, List<Uri> catalogs) :
        base(uri, nature, purpose)
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