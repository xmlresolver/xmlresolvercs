using System.Text.RegularExpressions;

namespace XmlResolver.Catalog.Entry;

public abstract class Entry
{
    protected static ResolverLogger Logger = new(NLog.LogManager.GetCurrentClassLogger());

    public enum EntryType {
        Null, Catalog, DelegatePublic, DelegateSystem, DelegateUri,
        Doctype, Document, DtdDecl, Entity, Group, Linktype, NextCatalog,
        Notation, Public, RewriteSystem, RewriteUri, SgmlDecl,
        System, SystemSuffix,
        Uri, UriSuffix
    }
        
    public static readonly string Rarr = "→";

    // Cheap and cheerful NCNAME test
    private static Regex _ncnameRe = new Regex(@"^[A-Za-z0-9_]+$");

    public readonly Uri BaseUri;
    public readonly string? Id;
    public readonly Dictionary<string,string> Extra = new();

    public Entry(Uri baseUri, string? id) {
        Id = id;
        if (baseUri.IsAbsoluteUri) {
            BaseUri = baseUri;
        } else {
            throw new NotSupportedException("Base URI of catalog entry must be absolute: " + baseUri);
        }
    }

    public void SetProperty(string name, string value) {
        if (_ncnameRe.IsMatch(name)) {
            Extra[name] = value;
        } else {
            Logger.Log(ResolverLogger.Error, "Property name invalid: " + name);
        }
    }

    public void SetProperties(Dictionary<String,String> props) {
        foreach (var entry in props) {
            SetProperty(entry.Key, entry.Value);
        }
    }

    public string GetProperty(string name) {
        return Extra[name];
    }

    public Dictionary<String,String> GetProperties() {
        return new Dictionary<string, string>(Extra);
    }

    public abstract EntryType GetEntryType();
}