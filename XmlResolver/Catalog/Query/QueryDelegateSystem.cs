using System;
using System.Collections.Generic;
using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query {
    public class QueryDelegateSystem : QuerySystem {
        protected readonly List<Uri> catalogs;

        public QueryDelegateSystem(string systemId, List<Uri> catalogs): base(systemId) {
            this.catalogs = new List<Uri>(catalogs);
        }

        public override bool Resolved() {
            return true;
        }
        
        public override List<Uri> UpdateCatalogSearchList(EntryCatalog catalog, List<Uri> oldCatalogs) {
            // Delegation replaces the catalog list
            return new(catalogs);
        }
    }
}