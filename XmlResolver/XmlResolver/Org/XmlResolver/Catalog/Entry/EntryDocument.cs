using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryDocument : EntryResource {
        public EntryDocument(Uri baseUri, string id, string uri) : base(baseUri, id, uri) {
            // nop
        }

        public override Catalog.Entry.Entry.EntryType GetEntryType() {
            return Catalog.Entry.Entry.EntryType.DOCUMENT;
        }
    }
}