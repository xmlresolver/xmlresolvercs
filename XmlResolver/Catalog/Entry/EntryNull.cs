namespace XmlResolver.Catalog.Entry;

public class EntryNull : Entry
{
    public EntryNull() : base(new Uri("https://xmlresolver.org/irrelevant"), null)
    {
        // nop;
    }

    public override EntryType GetEntryType() 
    {
        return EntryType.Null;
    }
        
    public override string ToString()
    {
        return $"null entry (not a catalog element)";
    }
}
