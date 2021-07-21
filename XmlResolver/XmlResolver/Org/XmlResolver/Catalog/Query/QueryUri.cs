using System;
using System.Collections.Generic;
using Org.XmlResolver.Catalog.Entry;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Catalog.Query {
    public class QueryUri : QueryCatalog {
        public readonly string Uri;
        public readonly string Nature;
        public readonly string Purpose;

        public QueryUri(string uri, string nature, string purpose) : base() {
            Uri = uri;
            Nature = nature;
            Purpose = purpose;
        }

        public QueryUri(string uri) : base() {
            Uri = uri;
            Nature = null;
            Purpose = null;
        }

        internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog) {
            string compareUri = manager.NormalizedForComparison(Uri);
            string compareNature = manager.NormalizedForComparison(Nature);
            string comparePurpose = manager.NormalizedForComparison(Purpose);

            // <uri>
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.URI)) {
                EntryUri entry = (EntryUri) raw;

                bool sameNature = false;
                bool samePurpose = false;

                if (Nature == null || entry.Nature == null) {
                    sameNature = true;
                }
                else {
                    sameNature = compareNature.Equals(manager.NormalizedForComparison(entry.Nature));
                }

                if (Purpose == null || entry.Purpose == null) {
                    samePurpose = true;
                }
                else {
                    samePurpose = comparePurpose.Equals(manager.NormalizedForComparison(entry.Purpose));
                }
                
                if (compareUri.Equals(manager.NormalizedForComparison(entry.Name)) && sameNature && samePurpose) {
                    return new QueryResult(entry.ResourceUri);
                }
            }
            
            // <rewriteUri>
            EntryRewriteUri rewrite = null;
            string rewriteStart = null;
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.REWRITE_URI)) {
                EntryRewriteUri entry = (EntryRewriteUri) raw;
                string compareStart = manager.NormalizedForComparison(entry.UriStart);
                if (compareUri.StartsWith(compareStart)) {
                    if (rewrite == null || compareStart.Length > rewriteStart.Length) {
                        rewrite = entry;
                        rewriteStart = compareStart;
                    }
                }
            }
            if (rewrite != null) {
                Uri resolved = UriUtils.Resolve(rewrite.RewritePrefix, compareUri.Substring(rewriteStart.Length));
                return new QueryResult(resolved);
            }

            // <uriSuffix>
            EntryUriSuffix suffix = null;
            string systemSuffix = null;
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.URI_SUFFIX)) {
                EntryUriSuffix entry = (EntryUriSuffix) raw;
                string compareSuffix = manager.NormalizedForComparison(entry.UriSuffix);
                if (compareUri.EndsWith(compareSuffix)) {
                    if (suffix == null || compareSuffix.Length > systemSuffix.Length) {
                        suffix = entry;
                        systemSuffix = compareSuffix;
                    }
                }
            }
            if (suffix != null) {
                return new QueryResult(suffix.ResourceUri);
            }

            // <delegateSystem>
            List<EntryDelegateUri> delegated = new();
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.DELEGATE_URI)) {
                EntryDelegateUri entry = (EntryDelegateUri) raw;
                string delegateStart = manager.NormalizedForComparison(entry.UriStart);
                if (compareUri.StartsWith(delegateStart)) {
                    var pos = 0;
                    while (pos < delegated.Count
                           && delegateStart.Length <=
                           manager.NormalizedForComparison(delegated[pos].UriStart).Length) {
                        pos += 1;
                    }

                    delegated.Insert(pos, entry);
                }
            }
            if (delegated.Count > 0) {
                List<Uri> catalogs = new();
                foreach (var entry in delegated) {
                    catalogs.Add(entry.Catalog);
                }

                return new QueryDelegateUri(Uri, Nature, Purpose, catalogs);
            }

            return QueryResult.EMPTY_RESULT;
        }
    }
}