using System;

namespace Org.XmlResolver {
    public interface XmlCatalogResolver {
        public Uri LookupUri(string uri);
        public Uri LookupPublic(string systemId, string publicId);
        public Uri LookupSystem(string systemId);
        public Uri LookupDoctype(string entityName, string systemId, string publicId);
        public Uri LookupEntity(string entityName, string systemId, string publicId);
        public Uri LookupNotation(string notationName, string systemId, string publicId);
        public Uri LookupDocument();
    }
}