using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public abstract class QueryCatalog : QueryResult
{
    public QueryCatalog() : base() {
        // nop;
    }

    public override bool Query() {
        return true;
    }

    public QueryResult Search(CatalogManager manager)
    {
        List<Uri> catalogs = new(manager.Catalogs());
        while (catalogs.Count > 0)
        {
            Uri uri = catalogs[0];
            catalogs.RemoveAt(0);
            EntryCatalog catalog = manager.LoadCatalog(uri);
            bool done = false;
            QueryCatalog query = this;
            while (!done)
            {
                QueryResult result = query.lookup(manager, catalog);
                done = result.Resolved();
                catalogs = result.UpdateCatalogSearchList(catalog, catalogs);

                if (result.Query())
                {
                    query = (QueryCatalog)result;
                }
                else
                {
                    done = true;
                    if (result.Resolved())
                    {
                        return result;
                    }
                }
            }
        }

        return EmptyResult;
    }

    internal abstract QueryResult lookup(CatalogManager manager, EntryCatalog catalog);
}
