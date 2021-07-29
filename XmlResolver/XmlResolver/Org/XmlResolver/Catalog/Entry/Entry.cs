using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Org.XmlResolver.Catalog.Entry {
    public abstract class Entry {
        protected static ResolverLogger logger = new(NLog.LogManager.GetCurrentClassLogger());

        public enum EntryType {
            NULL, CATALOG, DELEGATE_PUBLIC, DELEGATE_SYSTEM, DELEGATE_URI,
            DOCTYPE, DOCUMENT, DTD_DECL,ENTITY, GROUP, LINKTYPE, NEXT_CATALOG,
            NOTATION, PUBLIC, REWRITE_SYSTEM, REWRITE_URI, SGML_DECL,
            SYSTEM, SYSTEM_SUFFIX,
            URI, URI_SUFFIX
        }

        // Cheap and cheerful NCNAME test
        private static Regex NCNAME_RE = new Regex(@"^[A-Za-z0-9_]+$");

        public readonly Uri BaseUri;
        public readonly string Id;
        public readonly Dictionary<string,string> Extra = new();

        public Entry(Uri baseUri, String id) {
            Id = id;
            if (baseUri.IsAbsoluteUri) {
                BaseUri = baseUri;
            } else {
                throw new NotSupportedException("Base URI of catalog entry must be absolute: " + baseUri);
            }
        }

        public void SetProperty(String name, String value) {
            if (NCNAME_RE.IsMatch(name)) {
                Extra[name] = value;
            } else {
                logger.Log(ResolverLogger.ERROR, "Property name invalid: " + name);
            }
        }

        public void SetProperties(Dictionary<String,String> props) {
            foreach (var entry in props) {
                SetProperty(entry.Key, entry.Value);
            }
        }

        public String GetProperty(String name) {
            return Extra[name];
        }

        public Dictionary<String,String> GetProperties() {
            return new Dictionary<string, string>(Extra);
        }

        public abstract EntryType GetEntryType();
    }
}