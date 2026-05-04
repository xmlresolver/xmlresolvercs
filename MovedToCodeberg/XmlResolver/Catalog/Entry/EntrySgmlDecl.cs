using System;

namespace XmlResolver.Catalog.Entry {
    public class EntrySgmldecl : EntryResource {
        public EntrySgmldecl(Uri baseUri, string id, string uri) : base(baseUri, id, uri) {
            // nop
        }
        
        public override EntryType GetEntryType() {
            return EntryType.SGML_DECL;
        }
        
        public override string ToString() {
            return $"sgmldecl {Entry.Rarr} {ResourceUri}";
        }
    }
}