using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public class QueryNotation : QueryCatalog
{
    public readonly string? NotationName;
    public readonly string? SystemId;
    public readonly string? PublicId;

    public QueryNotation(string? notationName, string? systemId, string? publicId) : base()
    {
        NotationName = notationName;
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

        // <notation>
        foreach (var raw in catalog.Entries(Entry.Entry.EntryType.Notation))
        {
            EntryNotation entry = (EntryNotation)raw;
            if (entry.Name.Equals(NotationName))
            {
                return new QueryResult(entry.ResourceUri);
            }
        }

        return EmptyResult;
    }
}