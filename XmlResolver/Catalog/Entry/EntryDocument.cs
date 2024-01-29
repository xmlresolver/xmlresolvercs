namespace XmlResolver.Catalog.Entry;

public class EntryDocument : EntryResource
{
    public EntryDocument(Uri baseUri, string? id, string uri) : base(baseUri, id, uri)
    {
        // nop
    }

    public override EntryType GetEntryType()
    {
        return EntryType.Document;
    }

    public override string ToString()
    {
        return $"document {Entry.Rarr} {ResourceUri}";
    }
}
