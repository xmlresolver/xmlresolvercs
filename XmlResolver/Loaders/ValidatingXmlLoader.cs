using NLog;
using XmlResolver.Catalog.Entry;
using XmlResolver.Utils;

namespace XmlResolver.Loaders;

public class ValidatingXmlLoader : ICatalogLoader
{
    protected static ResolverLogger logger = new ResolverLogger(LogManager.GetCurrentClassLogger());
    protected readonly Dictionary<Uri, EntryCatalog> catalogMap;

    private EntryCatalog? catalog = null;

    private readonly XmlResolver resolver;
    private readonly XmlLoader underlyingLoader;

    public ValidatingXmlLoader()
    {
        underlyingLoader = new XmlLoader();
        resolver = underlyingLoader.LoaderResolver;
        catalogMap = new Dictionary<Uri, EntryCatalog>();
    }

    public EntryCatalog LoadCatalog(Uri caturi)
    {
        if (catalogMap.ContainsKey(caturi))
        {
            return catalogMap[caturi];
        }

        Stream? stream = null;
        try
        {
            stream = ResourceAccess.GetStream(underlyingLoader.Config, caturi);
            if (stream != null) {
                EntryCatalog thisCat = LoadCatalog(caturi, stream);
                stream.Close();
                return thisCat;
            }

            return new EntryCatalog(caturi, null, false);
        }
        catch (Exception)
        {
            // FIXME: the validating loader should throw an exception for anything other than
            // a file-not-found exception.
            logger.Log(ResolverLogger.Error, "Failed to load catalog {0}", caturi.ToString());
            catalog = new EntryCatalog(caturi, null, false);
            catalogMap.Add(caturi, catalog);
            if (stream != null)
            {
                stream.Close();
            }
            return catalog;
        }
    }

    public EntryCatalog LoadCatalog(Uri caturi, Stream data)
    {
        // This is a bit of a hack, but I don't expect catalogs to be huge and I have
        // to make sure that the stream can be read twice.
        MemoryStream memStream = new MemoryStream();
        data.CopyTo(memStream);
        memStream.Position = 0;

        logger.Log(ResolverLogger.Warning, "XML Resolver does not support catalog validation; assuming valid");

        // FIXME: do validation!
        return underlyingLoader.LoadCatalog(caturi, memStream);
    }

    public void SetPreferPublic(bool prefer)
    {
        underlyingLoader.SetPreferPublic(prefer);
    }

    public bool GetPreferPublic()
    {
        return underlyingLoader.GetPreferPublic();
    }

    public void SetArchivedCatalogs(bool archived)
    {
        underlyingLoader.SetArchivedCatalogs(archived);
    }

    public bool GetArchivedCatalogs()
    {
        return underlyingLoader.GetArchivedCatalogs();
    }
}