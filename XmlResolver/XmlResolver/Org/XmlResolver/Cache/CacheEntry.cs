using System;
using System.IO;
using Org.XmlResolver.Catalog.Entry;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Cache {
    public class CacheEntry {
        public readonly Entry CacheCatalogEntry;
        public readonly Uri CacheUri;
        public readonly string CacheFile;
        public readonly long Time;
        public readonly bool Expired;

        internal CacheEntry(EntryUri entry, long time) {
            CacheCatalogEntry = entry;
            CacheUri = UriUtils.Resolve(entry.BaseUri, entry.Name);
            CacheFile = entry.ResourceUri.AbsolutePath;
            Time = time;
        }

        internal CacheEntry(EntrySystem entry, long time) {
            CacheCatalogEntry = entry;
            CacheUri = UriUtils.Resolve(entry.BaseUri, entry.SystemId);
            CacheFile = entry.ResourceUri.AbsolutePath;
            Time = time;
        }

        internal CacheEntry(EntryPublic entry, long time) {
            CacheCatalogEntry = entry;
            CacheUri = PublicId.EncodeUrn(entry.PublicId);
            CacheFile = entry.ResourceUri.AbsolutePath;
            Time = time;
        }

        public string ETag() {
            return CacheCatalogEntry.GetProperty("etag");
        }

        public string ContentType() {
            return CacheCatalogEntry.GetProperty("contentType");
        }

        public Uri location() {
            if (CacheCatalogEntry.GetProperty("redir") != null) {
                return UriUtils.Resolve(CacheUri, CacheCatalogEntry.GetProperty("redir"));
            }
            else {
                return CacheUri;
            }
        }

        public override string ToString() {
            return CacheFile;
        }
    }
}