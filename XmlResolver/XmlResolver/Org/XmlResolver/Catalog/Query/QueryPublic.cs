using System;
using System.Collections.Generic;
using Org.XmlResolver.Catalog.Entry;

namespace Org.XmlResolver.Catalog.Query {
    public class QueryPublic : QueryCatalog {
        public readonly string SystemId;
        public readonly string PublicId;

        public QueryPublic(string systemId, string publicId) : base() {
            SystemId = systemId;
            PublicId = publicId;
        }

        internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog) {
            if (SystemId != null) {
                QuerySystem query = new QuerySystem(SystemId);
                QueryResult result = query.lookup(manager, catalog);
                if (result.Resolved()) {
                    return result;
                }
            }

            if (PublicId != null) {
                // <public>
                foreach (var raw in catalog.Entries(Entry.Entry.EntryType.PUBLIC)) {
                    EntryPublic entry = (EntryPublic) raw;
                    if (entry.PreferPublic || SystemId == null) {
                        if (entry.PublicId.Equals(PublicId)) {
                            return new QueryResult(entry.ResourceUri);
                        }
                    }
                }
                
                // <delegatePublic>
                List<EntryDelegatePublic> delegated = new();
                foreach (var raw in catalog.Entries(Entry.Entry.EntryType.DELEGATE_PUBLIC)) {
                    EntryDelegatePublic entry = (EntryDelegatePublic) raw;
                    if (entry.PreferPublic || SystemId == null) {
                        if (PublicId.StartsWith(entry.PublicIdStart)) {
                            var pos = 0;
                            while (pos < delegated.Count
                                   && entry.PublicIdStart.Length <= delegated[pos].PublicIdStart.Length) {
                                pos += 1;
                            }

                            delegated.Insert(pos, entry);
                        }
                    }
                }
                if (delegated.Count > 0) {
                    List<Uri> catalogs = new();
                    foreach (var entry in delegated) {
                        catalogs.Add(entry.Catalog);
                    }

                    return new QueryDelegatePublic(SystemId, PublicId, catalogs);
                }
            }

            return EMPTY_RESULT;
        }
    }
}