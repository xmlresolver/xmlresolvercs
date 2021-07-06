using System;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryNull : Entry {
        public EntryNull() : base(new Uri("https://xmlresolver.org/irrelevant"), null) {
            // nop;
        }

        public override EntryType GetEntryType() {
            return EntryType.NULL;
        }
    }
}