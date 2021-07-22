using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Org.XmlResolver.Catalog.Entry;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver.Loaders {
    public class ValidatingXmlLoader : CatalogLoader {
        protected static ResolverLogger logger = new ResolverLogger(LogManager.GetCurrentClassLogger());
        protected readonly Dictionary<Uri,EntryCatalog> catalogMap;

        private bool _preferPublic = true;
        private EntryCatalog catalog = null;
        private Locator locator = null;
        
        private readonly Resolver resolver;
        private readonly XmlLoader underlyingLoader;

        public ValidatingXmlLoader() {
            underlyingLoader = new XmlLoader();
            resolver = underlyingLoader.LoaderResolver;
            catalogMap = new Dictionary<Uri, EntryCatalog>();
        }

        public EntryCatalog LoadCatalog(Uri caturi) {
            if (catalogMap.ContainsKey(caturi)) {
                return catalogMap[caturi];
            }

            Stream stream = null;
            try {
                stream = UriUtils.GetStream(caturi);
                return LoadCatalog(caturi, stream);
            }
            catch (Exception) {
                // FIXME: the validating loader should throw an exception for anything other than
                // a file-not-found exception.
                logger.Log(ResolverLogger.ERROR, "Failed to load catalog {0}", caturi.ToString());
                catalog = new EntryCatalog(caturi, null, false);
                catalogMap.Add(caturi, catalog);
                stream.Close();
                return catalog;
            }
        }

        public EntryCatalog LoadCatalog(Uri caturi, Stream data) {
            // This is a bit of a hack, but I don't expect catalogs to be huge and I have
            // to make sure that the stream can be read twice.
            MemoryStream memStream = new MemoryStream();
            data.CopyTo(memStream);
            memStream.Position = 0;
            
            logger.Log(ResolverLogger.WARNING, "XML Resolver does not support catalog validation; assuming valid");
            
            // FIXME: do validation!
            return underlyingLoader.LoadCatalog(caturi, memStream);
        }

        public void SetPreferPublic(bool prefer) {
            underlyingLoader.SetPreferPublic(prefer);
        }

        public bool GetPreferPublic() {
            return underlyingLoader.GetPreferPublic();
        }
    }
}