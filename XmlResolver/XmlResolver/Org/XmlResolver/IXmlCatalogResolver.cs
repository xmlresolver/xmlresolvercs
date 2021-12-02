using System;

namespace Org.XmlResolver {
    public interface IXmlCatalogResolver {
        public Uri LookupUri(string uri);
        public Uri LookupPublic(string systemId, string publicId);
        public Uri LookupSystem(string systemId);
        public Uri LookupDoctype(string entityName, string systemId, string publicId);
        public Uri LookupEntity(string entityName, string systemId, string publicId);
        public Uri LookupNotation(string notationName, string systemId, string publicId);
        public Uri LookupDocument();
        public Uri LookupNamespaceUri(string uri, string nature, string purpose);
    }
}