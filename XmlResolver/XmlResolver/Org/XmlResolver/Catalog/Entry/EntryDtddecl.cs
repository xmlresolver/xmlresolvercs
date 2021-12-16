using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryDtddecl : EntryResource {
        public readonly string PublicId;
        
        public EntryDtddecl(Uri baseUri, string id, string publicId, string uri) : base(baseUri, id, uri) {
            PublicId = publicId;
        }

        public override Catalog.Entry.Entry.EntryType GetEntryType() {
            return Catalog.Entry.Entry.EntryType.DTD_DECL;
        }

        public override string ToString() {
            return $"dtddecl {PublicId} {Entry.Rarr} {ResourceUri}";
        }
    }
}