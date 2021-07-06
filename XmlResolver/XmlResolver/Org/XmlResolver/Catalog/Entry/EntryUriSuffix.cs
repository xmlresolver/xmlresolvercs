using System;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryUriSuffix : Entry {
        public readonly string UriSuffix;
        public readonly Uri ResourceUri;

        public EntryUriSuffix(Uri baseUri, string id, string suffix, string uri) : base(baseUri, id) {
            UriSuffix = suffix;
            ResourceUri = UriUtils.Resolve(baseUri, uri);
        }

        public override EntryType GetEntryType() {
            return EntryType.URI_SUFFIX;
        }
    }
}