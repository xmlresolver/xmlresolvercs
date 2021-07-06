namespace Org.XmlResolver {
    public interface ResourceResolver {
        ResolvedResource ResolveUri(string href, string baseUri);
        ResolvedResource ResolveNamespace(string href, string baseUri, string nature, string purpose);
        ResolvedResource ResolveEntity(string nae, string publicId, string systemId, string baseUri);
        ResolverConfiguration GetConfiguration();
    }
}