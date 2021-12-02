using System;

#nullable enable
namespace Org.XmlResolver {
    public interface INamespaceResolver {
        public object? GetEntity(Uri absoluteUri, string nature, string purpose);
        public object? GetEntity(String href, Uri baseUri, string nature, string purpose);
    }
}