using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryLinktype : EntryResource {
        public readonly string Name;
        
        public EntryLinktype(Uri baseUri, string id, string name, string uri) : base(baseUri, id, uri) {
            Name = name;
        }

        public override Catalog.Entry.Entry.EntryType GetEntryType() {
            return Catalog.Entry.Entry.EntryType.LINKTYPE;
        }
        
        public override string ToString() {
            return $"linktype {Name} {Entry.Rarr} {ResourceUri}";
        }
    }
}