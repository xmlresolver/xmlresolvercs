namespace XmlResolver.Catalog.Entry;

public class EntryGroup : Entry 
{
    public readonly bool PreferPublic;
                
    public EntryGroup(Uri baseUri, string? id, bool prefer) : base(baseUri, id) 
    {
        PreferPublic = prefer;
    }
        
    public override EntryType GetEntryType()
    {
        return EntryType.Group;
    }
        
    public override string ToString() 
    {
        return $"group prefer={(PreferPublic ? "public" : "system")}";
    }
}
