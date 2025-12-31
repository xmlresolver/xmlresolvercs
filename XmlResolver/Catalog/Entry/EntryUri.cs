using System;

namespace XmlResolver.Catalog.Entry {
    public class EntryUri : EntryResource {
        public readonly string Name;
        public readonly string Nature;
        public readonly string Purpose;
    
        public EntryUri(Uri baseUri, string id, string name, string uri, string nature, string purpose):
            base(baseUri, id, uri) {

            if (name.StartsWith("classpath:/")) {
                // classpath:/path/to/thing is the same as classpath:path/to/thing
                // normalize without the leading slash.
                Name = "classpath:" + name.Substring(11);
            } else {
                Name = name;
            }

            Nature = nature;
            Purpose = purpose;
        }

        public override EntryType GetEntryType() {
            return EntryType.URI;
        }
        
        public override string ToString() {
            string str = $"uri {Name} {Entry.Rarr} {ResourceUri}";
            if (Nature != null || Purpose != null) {
                str += " (";
            }
            if (Nature != null) {
                str += $"nature={Nature}";
            }

            if (Purpose != null) {
                if (Nature != null) {
                    str += "; ";
                }

                str += $"purpose={Purpose}";
            }
            if (Nature != null || Purpose != null) {
                str += ")";
            }

            return str;
        }
    }
}