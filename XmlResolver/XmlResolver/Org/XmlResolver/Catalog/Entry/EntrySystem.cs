using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntrySystem : EntryResource {
        public readonly string SystemId;

        public EntrySystem(Uri baseUri, string id, string systemId, string uri) : base(baseUri, id, uri) {
            if (systemId.StartsWith("classpath:/")) {
                // classpath:/path/to/thing is the same as classpath:path/to/thing
                // normalize without the leading slash.
                SystemId = "classpath:" + systemId.Substring(11);
            } else {
                SystemId = systemId;
            }
        }

        public override EntryType GetEntryType() {
            return EntryType.SYSTEM;
        }
    }
}