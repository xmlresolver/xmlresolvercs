using System;
using System.IO;
using System.Text.RegularExpressions;
using NLog;
using Org.XmlResolver.Utils;

#nullable enable

namespace Org.XmlResolver {
    public class Resolver : System.Xml.XmlResolver, INamespaceResolver {
        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        private readonly CatalogResolver _resolver;

        public Resolver() {
            _resolver = new CatalogResolver();
        }

        public Resolver(XmlResolverConfiguration config) {
            _resolver = new CatalogResolver(config);
        }

        public Resolver(CatalogResolver resolver) {
            this._resolver = resolver;
        }

        public CatalogResolver CatalogResolver => _resolver;
        
        public ResolverConfiguration GetConfiguration() {
            return _resolver.GetConfiguration();
        }

        public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn) {
            ResolvedResource rsrc = _resolver.ResolveEntity(null, null, absoluteUri.ToString(), null);
            if (rsrc == null) {
                try {
                    Stream stream = UriUtils.GetStream(absoluteUri);
                    return stream;
                }
                catch (Exception) {
                    // If the absoluteUri looks like a public identifier, ¯\_(ツ)_/¯
                    if ("file".Equals(absoluteUri.Scheme)) {
                        string cwd = Directory.GetCurrentDirectory();
                        string pubid = absoluteUri.ToString(); // Can't use AbsolutePath because it escapes things
                        pubid = Regex.Replace(pubid, @"file:/+", "/");
                        if (pubid.StartsWith(cwd)) {
                            pubid = pubid.Substring(cwd.Length+1);
                            pubid = Regex.Replace(pubid, @"/", @"//");
                            rsrc = _resolver.ResolveEntity(null, pubid, null, null);
                            if (rsrc != null) {
                                return rsrc.GetInputStream();
                            }
                        }
                    }
                }
                
                return null;
            }
            return rsrc.GetInputStream();
        }

        public object? GetEntity(Uri absoluteUri, string nature, string purpose) {
            ResolvedResource rsrc = _resolver.ResolveNamespace(absoluteUri.ToString(), null, nature, purpose);
            if (rsrc == null) {
                return null;
            }

            return rsrc.GetInputStream();
        }

        public object? GetEntity(string href, Uri baseUri, string nature, string purpose) {
            string? uristr = null;
            if (baseUri != null) {
                uristr = baseUri.ToString();
            }
            ResolvedResource rsrc = _resolver.ResolveNamespace(href, uristr, nature, purpose);
            if (rsrc == null && baseUri != null) {
                Uri absUri = new Uri(baseUri, href);
                return GetEntity(absUri, nature, purpose);
            }

            if (rsrc != null) {
                return rsrc.GetInputStream();
            }

            return null;
        }
    }
}