using NLog;
using XmlResolver.Catalog.Entry;

namespace XmlResolver.Catalog.Query;

public class QueryResult
{
    protected static Logger Logger = LogManager.GetCurrentClassLogger();

    public static readonly QueryResult EmptyResult = new QueryResult();
    private Uri? _result = null;
    private readonly bool _resolved;

    protected QueryResult()
    {
        _resolved = false;
    }

    public QueryResult(Uri uri)
    {
        _resolved = true;
        _result = uri;
    }

    public virtual bool Query()
    {
        return false;
    }

    public virtual bool Resolved()
    {
        return _resolved;
    }

    public Uri? ResultUri()
    {
        return _result;
    }

    public virtual List<Uri> UpdateCatalogSearchList(EntryCatalog catalog, List<Uri> catalogs)
    {
        // <nextCatalog>
        List<Uri> next = new();
        foreach (var raw in catalog.Entries(Entry.Entry.EntryType.NextCatalog)) {
            next.Add(((EntryNextCatalog) raw).Catalog);
        }

        foreach (var cat in catalogs) {
            next.Add(cat);
        }

        return next;
    }
}
