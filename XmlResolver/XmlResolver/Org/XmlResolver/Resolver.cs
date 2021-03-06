using System;
using System.IO;
using System.Text.RegularExpressions;
using NLog;
using Org.XmlResolver.Utils;

#nullable enable

namespace Org.XmlResolver {
    /// <summary>
    /// The catalog resolver implementation of the main URI-related interfaces.
    /// </summary>
    public class Resolver : System.Xml.XmlResolver, INamespaceResolver {
        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        private readonly CatalogResolver _resolver;

        /// <summary>
        /// Creates a new resolver with a default configuration.
        /// </summary>
        public Resolver() {
            _resolver = new CatalogResolver();
        }

        /// <summary>
        /// Creates a new resolver with the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public Resolver(XmlResolverConfiguration config) {
            _resolver = new CatalogResolver(config);
        }

        /// <summary>
        /// Creates a new resolver using the specified underlying resolver.
        /// </summary>
        /// <para>The underlying resolver is used directly, it is not copied. If the underlying resolver is used in
        /// several resolvers, any changes made will simultaneously effect all the resolvers.
        /// </para>
        /// <param name="resolver">The underlying resolver.</param>
        public Resolver(CatalogResolver resolver) {
            this._resolver = resolver;
        }

        /// <summary>
        /// Returns the underlying <see cref="CatalogResolver"/> used by this resolver.
        /// </summary>
        public CatalogResolver CatalogResolver => _resolver;
        
        /// <summary>
        /// Returns the configuration of the underlying resolver.
        /// </summary>
        /// <returns>The configuration object.</returns>
        public IResolverConfiguration GetConfiguration() {
            return _resolver.GetConfiguration();
        }

        /// <summary>
        /// Implements <see cref="System.Xml.XmlResolver.GetEntity"/>.
        /// </summary>
        /// <para>The role and return type are ignored because they have no useful purpose in the system
        /// version of the API.</para>
        /// <para>There's an awful failing in the way that the system parser uses this API.
        /// Presented with an entity that has both system and public identifiers, it calls
        /// <c>GetEntity</c> initially passing the public identifier as the absolute URI.
        /// This is complete madness, but if attempting to use the URI throws an exception and
        /// the URI looks like it might be a public identifier, we try to resolve with it.</para>
        /// <para>In principle, an XML document can't have only a public identifier, but this API forces
        /// us to pretend that it might because the public and system identifiers are passed independently
        /// in two separate API calls.</para>
        /// <para>Note that this method will attempt to open the specified URI, for example, over the web,
        /// if resolution fails to find something local in the catalog.</para>
        /// <param name="absoluteUri">The URI.</param>
        /// <param name="role">The role, which is ignored.</param>
        /// <param name="ofObjectToReturn">The type of object to return, which is ignored.</param>
        /// <returns>A stream if the resource was located or null otherwise.</returns>
        public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn) {
            ResolvedResource rsrc = _resolver.ResolveEntity(null, null, absoluteUri.ToString(), null);
            if (rsrc == null) {
                try {
                    Stream stream = UriUtils.GetStream(absoluteUri);
                    return stream;
                }
                catch (Exception) {
                    // Give up.
                    return null;
                }
            }
            return rsrc.GetInputStream();
        }

        /// <summary>
        /// Implements the <see cref="IResourceResolver.GetEntity"/> interface.
        /// </summary>
        /// <param name="absoluteUri">The absolute URI.</param>
        /// <param name="nature">The nature.</param>
        /// <param name="purpose">The purpose.</param>
        /// <returns>A stream if the resource was located or null otherwise.</returns>
        public object? GetEntity(Uri absoluteUri, string nature, string purpose) {
            ResolvedResource rsrc = _resolver.ResolveNamespace(absoluteUri.ToString(), null, nature, purpose);
            if (rsrc == null) {
                return null;
            }

            return rsrc.GetInputStream();
        }

        /// <summary>
        /// Implements the <see cref="IResourceResolver.GetEntity"/> interface.
        /// </summary>
        /// <param name="href">The URI.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="nature">The nature.</param>
        /// <param name="purpose">The purpose.</param>
        /// <returns>A stream if the resource was located or null otherwise.</returns>
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

        /// <summary>
        /// Resolve the relative URI against the base.
        /// </summary>
        /// <para>Note: this is part of the System.Xml.UriResolver interface. This is about resolving
        /// the relative and absolute portions of a URI, it is not about resolving the URI to a resource.</para>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The resolved absolute URI or null.</returns>
        /// <exception cref="NotSupportedException">If resolution produces a relative URI</exception>
        public override Uri ResolveUri(Uri? baseUri, string? relativeUri) {
            // There's a horrible bug in System.Xml where it attempts to resolve public identifiers
            // as if they were relative URIs. It turns them into things like this:
            //    http://example.com/path/-//DTD//Example 1.0//EN
            // That's not in the catalog, of course, so we end up doing a web request for it, even
            // though it's bound to fail.
            //
            // You might think, if we were clever, we could do a publicId lookup in the catalog
            // for it. And you'd be right. Unfortunately, the resulting resource will either
            // have no system identifier (not allowed by XML) or will have the aforementioned
            // mangled URI as its system identifier. Using the mangled URI will cause problems
            // if relative URIs are resolved against it, especially if it happens to be a pack://
            // URI that was the result of loading a resource from an assembly.
            //
            // The best we can do is return "null" without attempting to access the resource
            // with the broken URI.

            bool publicId = false;
            // Does the relativeUri look like a public identifier? Technically, does it look like a formal
            // public identifier (FPI)? This code relies on the structure of FPIs to identify them. Any
            // random string of characters *might* be a public identifier, but there's nothing we can do
            // about that. And the convention of formatting them as FPIs is pretty nearly universal.
            if (relativeUri != null) {
                int cpos = relativeUri.IndexOf(":", StringComparison.InvariantCulture);
                int pos = relativeUri.IndexOf("//", StringComparison.InvariantCulture);
                // If it doesn't contain '//' or if it begins 'scheme://', then it's not a public identifier.
                // (Here we use "position of :" < "position of //" as a proxy for checking the scheme.
                // Close enough and faster than regex.)
                if (pos >= 0 && (cpos < 0 || cpos > pos)) {
                    String id = relativeUri.Substring(pos + 2);
                    pos = id.IndexOf("//", StringComparison.InvariantCulture);
                    if (pos >= 0) {
                        // If it contains a second //, and...
                        pos = id.LastIndexOf("//", StringComparison.InvariantCulture);
                        id = id.Substring(pos + 2);
                        // If there's no "." (no hint of a filename extension) after the last //
                        publicId = !id.Contains("."); // call it a public identifier
                    }
                }
            }
            
            if (publicId) {
                return null;
            }

            return base.ResolveUri(baseUri, relativeUri);
        }
    }
}