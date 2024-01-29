namespace XmlResolver.Catalog.Entry;

public class EntryDtdDecl : EntryResource
{
    public readonly string? PublicId;
        
    public EntryDtdDecl(Uri baseUri, string? id, string? publicId, string uri) : base(baseUri, id, uri)
    {
        PublicId = publicId;
    }

    public override EntryType GetEntryType()
    {
        return EntryType.DtdDecl;
    }

    public override string ToString()
    {
        return $"dtddecl {PublicId} {Rarr} {ResourceUri}";
    }
}
