using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryUri : EntryResource {
        public readonly string Name;
        public readonly string Nature;
        public readonly string Purpose;
    
        public EntryUri(Uri baseUri, string id, string name, string uri, string nature, string purpose):
            base(baseUri, id, uri) {

            if (name.StartsWith("classpath:/")) {
                // classpath:/path/to/thing is the same as classpath:path/to/thing
                // normalize without the leading slash.
                Name = "classpath:" + name.Substring(11);
            } else {
                Name = name;
            }

            Nature = nature;
            Purpose = purpose;
        }

        public override EntryType GetEntryType() {
            return EntryType.URI;
        }
    }
}