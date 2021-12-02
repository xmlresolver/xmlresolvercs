namespace Org.XmlResolver {
    public interface IResourceResolver {
        ResolvedResource ResolveUri(string href, string baseUri);
        ResolvedResource ResolveNamespace(string href, string baseUri, string nature, string purpose);
        ResolvedResource ResolveEntity(string name, string publicId, string systemId, string baseUri);
        ResolverConfiguration GetConfiguration();
    }
}