using System;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryRewriteUri : Entry {
        public readonly string UriStart;
        public readonly Uri RewritePrefix;

        public EntryRewriteUri(Uri baseUri, string id, string start, string rewrite) : base(baseUri, id) {
            if (start.StartsWith("classpath:/")) {
                // classpath:/path/to/thing is the same as classpath:path/to/thing
                // normalize without the leading slash.
                UriStart = "classpath:" + start.Substring(11);
            } else {
                UriStart = start;
            }

            RewritePrefix = UriUtils.Resolve(baseUri, rewrite);
        }

        public override EntryType GetEntryType() {
            return EntryType.REWRITE_URI;
        }
        
        public override string ToString() {
            return $"rewriteURI {UriStart} {Entry.Rarr} {RewritePrefix}";
        }
    }
}