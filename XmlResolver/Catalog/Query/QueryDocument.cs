using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query {
    public class QueryDocument : QueryCatalog {
        internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog) {
            // <document>
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.DOCUMENT)) {
                EntryDocument entry = (EntryDocument) raw;
                return new QueryResult(entry.ResourceUri);
            }

            return EMPTY_RESULT;
        }
    }
}