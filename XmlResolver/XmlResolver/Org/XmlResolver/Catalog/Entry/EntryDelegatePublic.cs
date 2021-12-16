using System;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Catalog.Entry {
    public class EntryDelegatePublic : Entry {
        public readonly bool PreferPublic;
        public readonly string PublicIdStart;
        public readonly Uri Catalog;
        
        public EntryDelegatePublic(Uri baseUri, string id, string start, string catalog, bool prefer) : base(baseUri, id) {
            PreferPublic = prefer;
            PublicIdStart = start;
            Catalog = UriUtils.Resolve(baseUri, catalog);
        }

        public override Catalog.Entry.Entry.EntryType GetEntryType() {
            return XmlResolver.Catalog.Entry.Entry.EntryType.DELEGATE_PUBLIC;
        }
                    
        public override string ToString() {
            return $"delegatePublic {PublicIdStart} {Entry.Rarr} {Catalog}";
        }
    }
}