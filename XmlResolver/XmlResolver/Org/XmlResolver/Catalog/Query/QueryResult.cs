using System;
using System.Collections.Generic;
using NLog;
using Org.XmlResolver.Catalog.Entry;

namespace Org.XmlResolver.Catalog.Query {
    public class QueryResult {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        public static readonly QueryResult EMPTY_RESULT = new QueryResult();
        public static readonly QueryResult FINAL_RESULT = new QueryResult(null);
        private Uri result = null;
        private readonly bool _resolved;

        protected QueryResult() {
            _resolved = false;
        }

        public QueryResult(Uri uri) {
            _resolved = true;
            result = uri;
        }

        public virtual bool Query() {
            return false;
        }

        public virtual bool Resolved() {
            return _resolved;
        }

        public Uri ResultUri() {
            return result;
        }

        public virtual List<Uri> UpdateCatalogSearchList(EntryCatalog catalog, List<Uri> catalogs) {
            // <nextCatalog>
            List<Uri> next = new();
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.NEXT_CATALOG)) {
                next.Add(((EntryNextCatalog) raw).Catalog);
            }

            foreach (var cat in catalogs) {
                next.Add(cat);
            }

            return next;
        }
    }
}