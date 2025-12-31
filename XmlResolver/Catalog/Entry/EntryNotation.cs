using System;

namespace XmlResolver.Catalog.Entry {
    public class EntryNotation : EntryResource {
        public readonly string Name;
        
        public EntryNotation(Uri baseUri, string id, string name, string uri) : base(baseUri, id, uri) {
            Name = name;
        }

        public override EntryType GetEntryType() {
            return EntryType.NOTATION;
        }
        
        public override string ToString() {
            return $"notation {Name} {Entry.Rarr} {ResourceUri}";
        }
    }
}