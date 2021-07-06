using System;
using System.Collections;
using System.Collections.Generic;
using Org.XmlResolver.Catalog.Entry;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Catalog.Query {
    public class QuerySystem : QueryCatalog {
        public readonly string SystemId;

        public QuerySystem(string systemId) : base() {
            SystemId = systemId;
        }

        internal override QueryResult lookup(CatalogManager manager, EntryCatalog catalog) {
            string compareSystem = manager.NormalizedForComparison(SystemId);

            // There are several flavors of Windows, and there might be more. Of the
            // three likely platforms, only Unix has a case-sensitive filesystem by
            // default.
            bool ignoreFScase = (Environment.OSVersion.Platform == PlatformID.Unix);
            if (ignoreFScase) {
                compareSystem = compareSystem.ToLower();
            }

            // FIXME: the way I'm using ToLower() in this method means that the lowercase
            // version gets returned, not the original version. That's wrong.
            
            // <system>
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.SYSTEM)) {
                EntrySystem entry = (EntrySystem) raw;
                String entrySystem = manager.NormalizedForComparison(entry.SystemId);
                if (ignoreFScase) {
                    entrySystem = entrySystem.ToLower();
                }

                if (entrySystem.Equals(compareSystem)) {
                    return new QueryResult(entry.ResourceUri);
                }
            }

            // <rewriteSystem>
            EntryRewriteSystem rewrite = null;
            string rewriteStart = null;
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.REWRITE_SYSTEM)) {
                EntryRewriteSystem entry = (EntryRewriteSystem) raw;
                string compareStart = manager.NormalizedForComparison(entry.SystemIdStart);
                if (ignoreFScase) {
                    compareStart = compareStart.ToLower();
                }

                if (compareSystem.StartsWith(compareStart)) {
                    if (rewrite == null || compareStart.Length > rewriteStart.Length) {
                        rewrite = entry;
                        rewriteStart = compareStart;
                    }
                }
            }

            if (rewrite != null) {
                Uri resolved = UriUtils.Resolve(rewrite.RewritePrefix, compareSystem.Substring(rewriteStart.Length));
                return new QueryResult(resolved);
            }

            // <systemSuffix>
            EntrySystemSuffix suffix = null;
            string systemSuffix = null;
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.SYSTEM_SUFFIX)) {
                EntrySystemSuffix entry = (EntrySystemSuffix) raw;
                string compareSuffix = manager.NormalizedForComparison(entry.SystemIdSuffix);
                if (ignoreFScase) {
                    compareSuffix = compareSuffix.ToLower();
                }

                if (compareSystem.EndsWith(compareSuffix)) {
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
            List<EntryDelegateSystem> delegated = new();
            foreach (var raw in catalog.Entries(Entry.Entry.EntryType.DELEGATE_SYSTEM)) {
                EntryDelegateSystem entry = (EntryDelegateSystem) raw;
                string delegateStart = manager.NormalizedForComparison(entry.SystemIdStart);
                if (ignoreFScase) {
                    delegateStart = delegateStart.ToLower();
                }

                if (compareSystem.StartsWith(delegateStart)) {
                    var pos = 0;
                    while (pos < delegated.Count
                           && delegateStart.Length <=
                           manager.NormalizedForComparison(delegated[pos].SystemIdStart).Length) {
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

                return new QueryDelegateSystem(SystemId, catalogs);
            }

            if ((bool) manager.GetResolverConfiguration().GetFeature(ResolverFeature.URI_FOR_SYSTEM)) {
                QueryUri query = new QueryUri(SystemId);
                return query.lookup(manager, catalog);
            }

            return QueryResult.EMPTY_RESULT;
        }
    }
}