using System;
using System.IO;
using Org.XmlResolver.Catalog.Entry;

namespace Org.XmlResolver.Loaders {
    public interface ICatalogLoader {
        public EntryCatalog LoadCatalog(Uri caturi);
        public EntryCatalog LoadCatalog(Uri caturi, Stream data);
        public void SetPreferPublic(bool prefer);
        public bool GetPreferPublic();
        public void SetArchivedCatalogs(bool archived);
        public bool GetArchivedCatalogs();
    }
}