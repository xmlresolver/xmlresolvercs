using System.Collections;
using System.Text;

namespace XmlResolver.Catalog.Entry;

public class EntryCatalog : Entry
{
    private readonly object _syncLock = new object();

    protected static readonly List<Entry> None = new();
    protected ArrayList entries = new();
    protected Dictionary<EntryType, List<Entry>> typedEntries = new();
    protected Locator? locator = null;
        
    public readonly bool preferPublic;
        
    public EntryCatalog(Uri baseUri, string? id, bool prefer): base(baseUri, id)
    {
        preferPublic = prefer;
    }

    public void SetLocator(Locator locator)
    {
        this.locator = locator;
    }
        
    public override EntryType GetEntryType()
    {
        return EntryType.Catalog;
    }

    public List<Entry> Entries()
    {
        lock (_syncLock) {
            var list = new Entry[entries.Count];
            var pos = 0;
            foreach (var entry in entries)
            {
                list[pos] = (Entry) entry;
                pos += 1;
            }

            return new List<Entry>(list);
        }
    }

    public List<Entry> Entries(EntryType entryType)
    {
        lock (_syncLock) {
            if (typedEntries.ContainsKey(entryType))
            {
                var list = new List<Entry>();
                foreach (var entry in typedEntries[entryType])
                {
                    list.Add(entry);
                }
                return list;
            } else {
                return None;
            }
        }
    }

    protected virtual void Add(Entry entry)
    {
        lock (_syncLock) {
            entries.Add(entry);

            if (!typedEntries.ContainsKey(entry.GetEntryType())) {
                typedEntries.Add(entry.GetEntryType(), new());
            }
            typedEntries[entry.GetEntryType()].Add(entry);
        }
    }

    public void Remove(Entry entry)
    {
        lock (_syncLock) {
            entries.Remove(entry);

            if (typedEntries.ContainsKey(entry.GetEntryType())) {
                typedEntries[entry.GetEntryType()].Remove(entry);
                    
            }
        }
    }

    protected virtual void Error(string message, params string[] rest)
    {
        StringBuilder sb = new();
        sb.Append(BaseUri);
        if (locator != null && locator.LineNumber > 0) {
            sb.Append(":");
            sb.Append(locator.LineNumber);
            if (locator.LinePosition > 0) {
                sb.Append(":");
                sb.Append(locator.LinePosition);
            }
        }
        sb.Append(':');
        sb.Append(string.Format(message, rest));
        Logger.Log(ResolverLogger.Error, sb.ToString());
    }
        
    public virtual EntryGroup AddGroup(Uri baseUri, string? id, bool prefer)
    {
        var group = new EntryGroup(baseUri, id, prefer);
        Add(group);
        return group;
    }
        
    public virtual EntryPublic? AddPublic(Uri baseUri, string? id, string? publicId, string? uri, bool prefer) {
        EntryPublic? entry = null;
        if (publicId != null && uri != null) {
            entry = new EntryPublic(baseUri, id, publicId, uri, prefer);
            Add(entry);
        } else {
            Error("Invalid public entry (missing publicId or uri attribute)");
        }
        return entry;
    }

    public virtual EntrySystem? AddSystem(Uri baseUri, string? id, string? systemId, string? uri)
    {
        EntrySystem? entry = null;
        if (systemId != null && uri != null) {
            entry = new EntrySystem(baseUri, id, systemId, uri);
            Add(entry);
        } else {
            Error("Invalid system entry (missing systemId or uri attribute)");
        }
        return entry;
    }

    public virtual EntrySystemSuffix? AddSystemSuffix(Uri baseUri, string? id, string? suffix, string? uri)
    {
        EntrySystemSuffix? entry = null;
        if (suffix != null && uri != null) {
            entry = new EntrySystemSuffix(baseUri, id, suffix, uri);
            Add(entry);
        } else {
            Error("Invalid systemSuffix entry (missing systemIdSuffix or uri attribute)");
        }
        return entry;
    }

    public virtual EntryRewriteSystem? AddRewriteSystem(Uri baseUri, string? id, string? startString, string? prefix)
    {
        EntryRewriteSystem? entry = null;
        if (startString != null && prefix != null) {
            entry = new EntryRewriteSystem(baseUri, id, startString, prefix);
            Add(entry);
        } else {
            Error("Invalid rewriteSystem entry (missing systemIdStartString or prefix attribute)");
        }
        return entry;
    }

    public virtual EntryDelegateSystem? AddDelegateSystem(Uri baseUri, string? id, string? startString, string? catalog)
    {
        EntryDelegateSystem? entry = null;
        if (startString != null && catalog != null) {
            entry = new EntryDelegateSystem(baseUri, id, startString, catalog);
            Add(entry);
        } else {
            Error("Invalid delegateSystem entry (missing systemIdStartString or catalog attribute)");
        }
        return entry;
    }

    public virtual EntryDelegatePublic? AddDelegatePublic(Uri baseUri, string? id, string? startString, string? catalog, bool prefer)
    {
        EntryDelegatePublic? entry = null;
        if (startString != null && catalog != null) {
            entry = new EntryDelegatePublic(baseUri, id, startString, catalog, prefer);
            Add(entry);
        } else {
            Error("Invalid delegatePublic entry (missing publicIdStartString or catalog attribute)");
        }
        return entry;
    }

    public virtual EntryUri? AddUri(Uri baseUri, string? id, string? name, string? uri, string? nature, string? purpose)
    {
        EntryUri? entry = null;
        if (name != null && uri != null) {
            entry = new EntryUri(baseUri, id, name, uri, nature, purpose);
            Add(entry);
        } else {
            Error("Invalid uri entry (missing name or uri attribute)");
        }
        return entry;
    }

    public virtual EntryRewriteUri? AddRewriteUri(Uri baseUri, string? id, string? start, string? prefix)
    {
        EntryRewriteUri? entry = null;
        if (start != null && prefix != null) {
            entry = new EntryRewriteUri(baseUri, id, start, prefix);
            Add(entry);
        } else {
            Error("Invalid rewriteURI entry (missing uriStartString or prefix attribute)");
        }
        return entry;
    }

    public virtual EntryUriSuffix? AddUriSuffix(Uri baseUri, string? id, string? suffix, string? uri)
    {
        EntryUriSuffix? entry = null;
        if (suffix != null && uri != null) {
            entry = new EntryUriSuffix(baseUri, id, suffix, uri);
            Add(entry);
        } else {
            Error("Invalid uriSuffix entry (missing uriStartString or uri attribute)");
        }
        return entry;
    }

    public virtual EntryDelegateUri? AddDelegateUri(Uri baseUri, string? id, string? startString, string? catalog)
    {
        EntryDelegateUri? entry = null;
        if (startString != null && catalog != null) {
            entry = new EntryDelegateUri(baseUri, id, startString, catalog);
            Add(entry);
        } else {
            Error("Invalid delegateURI entry (missing uriStartString or catalog attribute)");
        }
        return entry;
    }

    public virtual EntryNextCatalog? AddNextCatalog(Uri baseUri, string? id, string? catalog)
    {
        EntryNextCatalog? entry = null;
        if (catalog != null) {
            entry = new EntryNextCatalog(baseUri, id, catalog);
            Add(entry);
        } else {
            Error("Invalid nextCatalog entry (missing catalog attribute)");
        }
        return entry;
    }

    public virtual EntryDoctype? AddDoctype(Uri baseUri, string? id, string? name, string? uri)
    {
        EntryDoctype? entry = null;
        if (name != null && uri != null) {
            entry = new EntryDoctype(baseUri, id, name, uri);
            Add(entry);
        } else {
            Error("Invalid doctype entry (missing name or uri attribute)");
        }
        return entry;
    }

    public virtual EntryDocument? AddDocument(Uri baseUri, string? id, string? uri)
    {
        EntryDocument? entry = null;
        if (uri != null) {
            entry = new EntryDocument(baseUri, id, uri);
            Add(entry);
        } else {
            Error("Invalid document entry (missing uri attribute)");
        }
        return entry;
    }

    public virtual EntryDtdDecl? AddDtdDecl(Uri baseUri, string? id, string? publicId, string? uri)
    {
        EntryDtdDecl? entry = null;
        if (publicId != null && uri != null) {
            entry = new EntryDtdDecl(baseUri, id, publicId, uri);
            Add(entry);
        } else {
            Error("Invalid dtddecl entry (missing publicId or uri attribute)");
        }
        return entry;
    }

    public virtual EntryEntity? AddEntity(Uri baseUri, string? id, string? name, string? uri)
    {
        EntryEntity? entry = null;
        if (name != null && uri != null) {
            entry = new EntryEntity(baseUri, id, name, uri);
            Add(entry);
        } else {
            Error("Invalid entity entry (missing name or uri attribute)");
        }
        return entry;
    }

    public virtual EntryLinktype? AddLinktype(Uri baseUri, string? id, string? name, string? uri)
    {
        EntryLinktype? entry = null;
        if (name != null && uri != null) {
            entry = new EntryLinktype(baseUri, id, name, uri);
            Add(entry);
        } else {
            Error("Invalid linktype entry (missing name or uri attribute)");
        }
        return entry;
    }

    public virtual EntryNotation? AddNotation(Uri baseUri, string? id, string? name, string? uri)
    {
        EntryNotation? entry = null;
        if (name != null && uri != null) {
            entry = new EntryNotation(baseUri, id, name, uri);
            Add(entry);
        } else {
            Error("Invalid notation entry (missing name or uri attribute)");
        }
        return entry;
    }

    public virtual EntrySgmlDecl? AddSgmlDecl(Uri baseUri, string? id, string? uri)
    {
        EntrySgmlDecl? entry = null;
        if (uri != null) {
            entry = new EntrySgmlDecl(baseUri, id, uri);
            Add(entry);
        } else {
            Error("Invalid sgmldecl entry (uri attribute)");
        }
        return entry;
    }
    
    public override string ToString()
    {
        return $"catalog prefer={(preferPublic ? "public" : "system")}";
    }

}