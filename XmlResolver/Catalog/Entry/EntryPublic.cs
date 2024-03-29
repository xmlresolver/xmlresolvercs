namespace XmlResolver.Catalog.Entry;

public class EntryPublic : EntryResource
{
    public readonly string PublicId;
    public readonly bool PreferPublic;

    public EntryPublic(Uri baseUri, string? id, string publicId, string uri, bool prefer) : base(baseUri, id, uri) 
    {
        PublicId = publicId;
        PreferPublic = prefer;
    }

    public override EntryType GetEntryType()
    {
        return EntryType.Public;
    }
        
    public override string ToString()
    {
        return $"public {PublicId} {Rarr} {ResourceUri}";
    }
}
