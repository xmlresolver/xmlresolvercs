using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Org.XmlResolver.Catalog.Entry;

namespace Org.XmlResolver.Cache {
    public class CacheEntryCatalog : EntryCatalog {
        public readonly List<CacheEntry> cached;
        private readonly Uri baseUri;

        public CacheEntryCatalog(Uri baseURI, string id, bool prefer) : base(baseURI, id, prefer) {
            baseUri = baseURI;
            cached = new();
        }

        protected override void Add(Entry entry) {
            throw new NotSupportedException("Attempt to add entry to the cache catalog");
        }

        protected void Add(Entry entry, long time) {
            int pos = 0;
            while (pos < cached.Count) {
                if (cached[pos].Time > time) {
                    break;
                }

                pos++;
            }

            switch (entry.GetEntryType()) {
                case EntryType.URI:
                    cached.Insert(pos, new CacheEntry((EntryUri) entry, time));
                    break;
                case EntryType.SYSTEM:
                    cached.Insert(pos, new CacheEntry((EntrySystem) entry, time));
                    break;
                case EntryType.PUBLIC:
                    cached.Insert(pos, new CacheEntry((EntryPublic) entry, time));
                    break;
                default:
                    throw new ArgumentException("Attempt to cache unexpected entry type");
            }

            base.Add(entry);
        }
        
        protected override void Error(string message, params string[] rest) {
            StringBuilder sb = new();
            sb.Append(BaseUri);
            if (locator != null && locator.LineNumber > 0) {
                sb.Append(":");
                sb.Append(locator.LineNumber);
                if (locator.LinePosition > 0) {
                    sb.Append(":");
                    sb.Append(locator.LinePosition);
                }
            }
            sb.Append(':');
            sb.Append(string.Format(message, rest));
            logger.Log(ResolverLogger.CACHE, sb.ToString());
        }

        public EntryUri AddUri(Uri baseUri, string name, string uri, string nature, string purpose, long time) {
            EntryUri entry = null;
            if (name != null && uri != null) {
                entry = new EntryUri(baseUri, null, name, uri, nature, purpose);
                Add(entry, time);
            } else {
                Error("Invalid uri entry (missing name or uri attribute)");
            }
            return entry;
        }

        public EntryPublic AddPublic(Uri baseUri, string publicId, string uri, long time) {
            EntryPublic entry = null;
            if (publicId != null && uri != null) {
                entry = new EntryPublic(baseUri, null, publicId, uri, true);
                Add(entry, time);
            } else {
                Error("Invalid public entry (missing publicId or uri attribute)");
            }
            return entry;
        }

        public EntrySystem AddSystem(Uri baseUri, string systemId, string uri, long time) {
            EntrySystem entry = null;
            if (systemId != null && uri != null) {
                entry = new EntrySystem(baseUri, null, systemId, uri);
                Add(entry, time);
            } else {
                Error("Invalid system entry (missing systemId or uri attribute)");
            }
            return entry;
        }

        internal void WriteCacheEntry(Entry entry, string cacheFile) {
            // Constructing XML with print statements is kind of grotty, but...
            using (StreamWriter xml = new StreamWriter(cacheFile)) {
                switch (entry.GetEntryType()) {
                    case EntryType.URI:
                        EntryUri uri = (EntryUri) entry;
                        xml.WriteLine("<uri xmlns='" + ResolverConstants.CATALOG_NS + "'");
                        xml.WriteLine("<uri xmlns='" + ResolverConstants.CATALOG_NS + "'");
                        xml.WriteLine("     xmlns:xr='" + ResolverConstants.XMLRESOURCE_EXT_NS + "'");
                        xml.WriteLine("     name='" + XmlEscape(uri.Name) + "'");
                        xml.WriteLine("     uri='" + XmlEscape(uri.ResourceUri.ToString()) + "'");
                        if (uri.Nature != null) {
                            xml.WriteLine("     nature='" + XmlEscape(uri.Nature) + "'");
                        }
                        if (uri.Purpose != null) {
                            xml.WriteLine("     purpose='" + XmlEscape(uri.Purpose) + "'");
                        }

                        break;
                    case EntryType.SYSTEM:
                        EntrySystem system = (EntrySystem) entry;
                        xml.WriteLine("<system xmlns='" + ResolverConstants.CATALOG_NS + "'");
                        xml.WriteLine("        xmlns:xr='" + ResolverConstants.XMLRESOURCE_EXT_NS + "'");
                        xml.WriteLine("        systemId='" + XmlEscape(system.SystemId) + "'");
                        xml.WriteLine("        uri='" + XmlEscape(system.ResourceUri.ToString()) + "'");
                        break;
                    case EntryType.PUBLIC:
                        EntryPublic pub = (EntryPublic) entry;
                        xml.WriteLine("<public xmlns='" + ResolverConstants.CATALOG_NS + "'");
                        xml.WriteLine("        xmlns:xr='" + ResolverConstants.XMLRESOURCE_EXT_NS + "'");
                        xml.WriteLine("        publicId='" + XmlEscape(pub.PublicId) + "'");
                        xml.WriteLine("        uri='" + XmlEscape(pub.ResourceUri.ToString()) + "'");
                        break;
                    default:
                        Error("Attempt to write unexpected entry type");
                        break;
                }
                
                foreach (string name in entry.GetProperties().Keys)
                {
                    if (entry.GetProperty(name) != null) {
                        xml.WriteLine("     xr:" + name + "='" + XmlEscape(entry.GetProperty(name)) + "'");
                    }
                }

                xml.WriteLine("/>");
            }
        }

        public static string XmlEscape(string line) {
            return line.Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("'", "&apos;");
        }

        // N.B. This method must only be called when a directory lock is held!
        internal void FlushCache(Regex uriPattern, long maxCount, long maxSize, string expiredDir) {
            List<Entry> newEntries = new();
            Dictionary<EntryType, List<Entry>> newTypedEntries = new();
            List<CacheEntry> newCache = new();

            long cacheCount = 0;
            long cacheSize = 0;
            
            // N.B. The entries in the cache are sorted in date order, so as soon as we pass
            // a threshold, we can burn everything that follows...
            foreach (CacheEntry entry in cached) {
                bool add = true;
                if (uriPattern.IsMatch(entry.CacheUri.ToString())) {
                    cacheCount++;
                    cacheSize += entry.CacheFile.Length;
                    add = cacheCount < maxCount && cacheSize <= maxSize;
                }

                if (add) {
                    newCache.Add(entry);
                    newEntries.Add(entry.CacheCatalogEntry);
                    if (!typedEntries.ContainsKey(entry.CacheCatalogEntry.GetEntryType())) {
                        newTypedEntries.Add(entry.CacheCatalogEntry.GetEntryType(), new List<Entry>());
                    }

                    newTypedEntries[entry.CacheCatalogEntry.GetEntryType()].Add(entry.CacheCatalogEntry);
                }
                else {
                    logger.Log(ResolverLogger.CACHE, "Expiring {0} ({1})", entry.CacheFile.ToString(), entry.CacheUri.ToString());
                    entry.Expired = true;
                    string source = entry.CacheCatalogEntry.BaseUri.AbsolutePath;
                    string target = Path.Combine(expiredDir, entry.CacheFile.Name);
                    try {
                        File.Move(source, target);
                    }
                    catch (Exception) {
                        logger.Log(ResolverLogger.ERROR, "Attempt to expire cache entry filed: {0}: {1}",
                            source, target);
                    }
                }
            }

            cached.Clear();
            cached.AddRange(newCache);
            
            entries.Clear();
            entries.AddRange(newEntries);
            
            typedEntries.Clear();
            foreach (EntryType etype in newTypedEntries.Keys) {
                typedEntries.Add(etype, newTypedEntries[etype]);
            }
        }
    }
}