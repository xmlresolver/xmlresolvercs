using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public class QueryDoctype : QueryCatalog
{
    public readonly string? EntityName;
    public readonly string? SystemId;
    public readonly string? PublicId;

    public QueryDoctype(string? entityName, string? systemId, string? publicId) : base()
    {
        EntityName = entityName;
        SystemId = systemId;
        PublicId = publicId;
    }

    internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog)
    {
        QueryPublic queryPublic = new QueryPublic(SystemId, PublicId);
        QueryResult result = queryPublic.lookup(manager, catalog);
        if (result.Resolved())
        {
            return result;
        }

        // <doctype>
        foreach (var raw in catalog.Entries(Entry.Entry.EntryType.Doctype))
        {
            var entry = (EntryDoctype)raw;
            if (entry.Name.Equals(EntityName))
            {
                return new QueryResult(entry.ResourceUri);
            }
        }

        return EmptyResult;
    }
}