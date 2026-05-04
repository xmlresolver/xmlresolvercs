using System;

namespace XmlResolver.Catalog.Entry {
    public class EntryNull : Entry {
        public EntryNull() : base(new Uri("https://xmlresolver.org/irrelevant"), null) {
            // nop;
        }

        public override EntryType GetEntryType() {
            return EntryType.NULL;
        }
        
        public override string ToString() {
            return $"null entry (not a catalog element)";
        }
    }
}