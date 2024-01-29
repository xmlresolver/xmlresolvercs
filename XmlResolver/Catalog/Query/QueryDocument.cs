using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public class QueryDocument : QueryCatalog
{
    internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog)
    {
        // <document>
        foreach (var raw in catalog.Entries(Entry.Entry.EntryType.Document))
        {
            EntryDocument entry = (EntryDocument)raw;
            return new QueryResult(entry.ResourceUri);
        }

        return EmptyResult;
    }
}