using System;

namespace XmlResolver.Catalog.Entry {
    public class EntryGroup : Entry {
        public readonly bool preferPublic;
                
        public EntryGroup(Uri baseUri, string id, bool prefer) : base(baseUri, id) {
            preferPublic = prefer;
        }
        
        public override EntryType GetEntryType() {
            return EntryType.GROUP;
        }
        
        public override string ToString() {
            return $"group prefer={(preferPublic ? "public" : "system")}";
        }
    }
}