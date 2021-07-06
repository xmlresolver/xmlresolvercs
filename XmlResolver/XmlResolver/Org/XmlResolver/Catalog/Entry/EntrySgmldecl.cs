using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntrySgmldecl : Entry {
        public EntrySgmldecl(Uri baseUri, string id, string uri) : base(baseUri, id) {
            // nop
        }
        
        public override EntryType GetEntryType() {
            return EntryType.SGML_DECL;
        }
    }
}