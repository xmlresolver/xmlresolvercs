namespace XmlResolver.Catalog.Entry;

public class EntryEntity : EntryResource
{
    public readonly string Name;
        
    public EntryEntity(Uri baseUri, string? id, string name, string uri) : base(baseUri, id, uri)
    {
        Name = name;
    }

    public override EntryType GetEntryType()
    {
        return EntryType.Entity;
    }

    public override string ToString()
    {
        return $"entity {Name} {Entry.Rarr} {ResourceUri}";
    }
}
