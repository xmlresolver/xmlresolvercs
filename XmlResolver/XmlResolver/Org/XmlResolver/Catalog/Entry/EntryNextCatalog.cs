using System;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryNextCatalog : Entry {
        public readonly Uri Catalog;

        public EntryNextCatalog(Uri baseUri, String id, String catalog) : base(baseUri, id) {
            Catalog = UriUtils.Resolve(baseUri, catalog);
        }

        public override EntryType GetEntryType() {
            return EntryType.NEXT_CATALOG;
        }
    }
}