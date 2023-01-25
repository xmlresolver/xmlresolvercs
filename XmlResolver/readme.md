# README

A C# implementation of the XML Resolver.

This package implements an OASIS catalog-based XML resource resolver.

See [The XML Resolver Project](https://xmlresolver.org/) for more details.

## APIs

This API implements `System.Xml.XmlResolver`.

The resolver can read catalogs from the local filesystem, from ZIP
files, or from assemblies bundled with your application.

## Change Log

This release fixes a bug in how assembly catalogs are located, updates
the release to use .NET 6, and replaces the now deprecated `WebClient`
API with the `System.Net.Http` URI.

## Backwards incompatible changes

Version 2.0.0 introduces a few backwards incompatible changes.

### Loading assembly catalogs

Starting with version 2.0.0, assembly catalogs are loaded using their
`AssemblyName`, not their path. Where previously you might have specified

```
  config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "MyAssembly.dll");
```

you must now use the assembly name:

```
  config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS,
    "MyAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
```

You can leave the version out of the name.

### The ResourceConnection class

The `StatusCode` property of the `ResourceConnection` class is no longer an integer.
For compatibility with `System.Net.Http` it is a `HttpStatusCode`.
