# README

A C# implementation of the XML Resolver.

This package implements an OASIS catalog-based XML resource resolver.

See [The XML Resolver Project](https://xmlresolver.org/) for more details.

## APIs

This API implements `System.Xml.XmlResolver`.

The resolver can read catalogs from the local filesystem, from ZIP
files, or from assemblies bundled with your application.

## Change Log

Version 6.x is a significant refactoring and is not backwards compatible with version 2.x.
(The underlying functionality is the same, but the API is different.)
The version 2.x sources are now in the
[legacy_v2](https://github.com/xmlresolver/xmlresolvercs/tree/legacy_v2) branch. Important
bug fixes will be applied to the 2.x release for some time, but new development is
focused on the 6.x release.

Three main considerations drove the refactoring:

1. Correcting design errors, mostly attempting to make the whole thing more C#-like.
   The classes are now `XmlResolver.XmlResolver…` and not `Org.XmlResolver…`.
2. Simplification of the design (removing the caching feature, for example)
3. Bringing the [Java](https://github.com/xmlresolver/xmlresolver) and C# implementations
   into better alignment.

### What’s changed?(tl;dr)

Where previously you would have instantiated an
`Org.Xmlresolver.Resolver` and used it as the entity resolver for `System.Xml.XmlResolver`
you should now instantiate an `XmlResolver.XmlResolver`. This new object has methods for
performing catalog lookup and resource resolution. It also has a method that
returns the `System.Xml.XmlResolver` resolver API:

```
XmlResolver.XmlResolver.getXmlResolver()
```

Behind the scenes, the API has been reworked so that most operations
consist of constructing a request for some resource, asking the `XmlResolver` to either
(just) look it up in the catalog or resolve it, and returning a response.

This release fixes a bug in how assembly catalogs are located, updates
the release to use .NET 8, and replaces the now deprecated `WebClient`
API with the `System.Net.Http` URI.
