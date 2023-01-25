using Org.XmlResolver.Catalog.Entry;

namespace Org.XmlResolver.Catalog.Query {
    public class QueryEntity : QueryCatalog {
        public readonly string EntityName;
        public readonly string SystemId;
        public readonly string PublicId;

        public QueryEntity(string entityName, string systemId, string publicId) : base()
        {
            EntityName = entityName;
            SystemId = systemId;
            PublicId = publicId;
        }

        internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog) {
            QueryPublic queryPublic = new QueryPublic(SystemId, PublicId);
            QueryResult result = queryPublic.lookup(manager, catalog);
            if (result.Resolved()) {
                return result;
            }
            
            // <entity>
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.ENTITY)) {
                EntryEntity entry = (EntryEntity) raw;
                if (entry.Name.Equals(EntityName)) {
                    return new QueryResult(entry.ResourceUri);
                }
            }

            return EMPTY_RESULT;
        }
    }
}