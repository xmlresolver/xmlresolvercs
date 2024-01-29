namespace XmlResolver.Catalog.Entry;

public class EntrySgmlDecl : EntryResource
{
    public EntrySgmlDecl(Uri baseUri, string? id, string uri) : base(baseUri, id, uri)
    {
        // nop
    }
        
    public override EntryType GetEntryType()
    {
        return EntryType.SgmlDecl;
    }
        
    public override string ToString()
    {
        return $"sgmldecl {Rarr} {ResourceUri}";
    }
}
