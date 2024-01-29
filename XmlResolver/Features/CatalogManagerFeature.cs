namespace XmlResolver.Features;

public class CatalogManagerResolverFeature : ResolverFeature
{
    private readonly CatalogManager? _defaultValue;
        
    public CatalogManagerResolverFeature(string name, CatalogManager? defaultValue) : base(name, typeof(CatalogManager))
    {
        _defaultValue = defaultValue;
    }

    public CatalogManager? GetDefaultValue() {
        return _defaultValue;
    }
}
