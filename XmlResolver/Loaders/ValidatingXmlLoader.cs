using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using XmlResolver.Catalog.Entry;
using XmlResolver;

namespace XmlResolver.Loaders;

public class ValidatingXmlLoader : ICatalogLoader {
    private static readonly ResolverLogger Logger = new(LogManager.GetCurrentClassLogger());
    private readonly Dictionary<Uri,EntryCatalog> _catalogMap = new();
    private EntryCatalog _catalog = null;
    private readonly XmlLoader _underlyingLoader = new();

    public EntryCatalog LoadCatalog(Uri caturi) {
        if (_catalogMap.TryGetValue(caturi, out var catalog)) {
            return catalog;
        }

        Stream stream = null;
        try {
            stream = ResourceAccess.GetStream(caturi);
            return LoadCatalog(caturi, stream);
        }
        catch (Exception) {
            // FIXME: the validating loader should throw an exception for anything other than
            // a file-not-found exception.
            Logger.Debug("Failed to load catalog {0}", caturi.ToString());
            _catalog = new EntryCatalog(caturi, null, false);
            _catalogMap.Add(caturi, _catalog);
            stream?.Close();
            return _catalog;
        }
    }

    public EntryCatalog LoadCatalog(Uri caturi, Stream data) {
        // This is a bit of a hack, but I don't expect catalogs to be huge and I have
        // to make sure that the stream can be read twice.
        MemoryStream memStream = new MemoryStream();
        data.CopyTo(memStream);
        memStream.Position = 0;
        
        Logger.Debug("XmlResolver does not support catalog validation; assuming valid");
        
        // FIXME: do validation!
        return _underlyingLoader.LoadCatalog(caturi, memStream);
    }

    public void SetPreferPublic(bool prefer) {
        _underlyingLoader.SetPreferPublic(prefer);
    }

    public bool GetPreferPublic() {
        return _underlyingLoader.GetPreferPublic();
    }

    public void SetArchivedCatalogs(bool archived) {
        _underlyingLoader.SetArchivedCatalogs(archived);
    }

    public bool GetArchivedCatalogs() {
        return _underlyingLoader.GetArchivedCatalogs();
    }
}
