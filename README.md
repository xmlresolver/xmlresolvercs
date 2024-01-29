# xmlresolvercs

A C# implementation of the XML Resolver

See [The XML Resolver Project](https://xmlresolver.org/) for more details.

## APIs

This API implements `System.Xml.XmlResolver`.

The resolver can read catalogs from the local filesystem, from ZIP
files, or from assemblies bundled with your application.

## Version 6.x

Version 6.x is a significant refactoring and is not backwards compatible with version 2.x.
(The underlying functionality is the same, but the API is slightly different.)
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

## A note about version numbers

The XML Resolver API is often integrated into other projects and
products. On the one hand, this means that it’s valuable to publish
new releases early so that integrators can test them. On the other
hand, integrators quite reasonably want to make production releases
with only the most stable versions.

In an effort to make this easier, starting with version 6.x, the XML
Resolver releases will use an even/odd pattern version number strategy
to identify development and stable branches.

If the second number in the verion is even, that’s a work-in-progress,
stabalization release. Please test it, and report bugs. If the second
number is odd, that’s a stable release. (Test that and report bugs
too, obviously!)

In other words 6.0.x are stabalization releases. When the API is
deemed stable, there will be a 6.1.0 release. If more features are
developed or significant changes are undertaken, those will be
published in a series of 6.2.x releases before stabalizing in a 6.3.0
release. Etc.

## Cloning

The build now includes a collection of schemas and resources that can
be made available in a “data” assembly.
Because these resources are shared across both the Java and C#
projects, they’re in a separate repo and that repo is a “submodule” of
this one.

What that means in practice is that after you clone this repository,
you must also run:

```
git submodule sync
git submodule update --init
```

That will make sure that the submodule is checked out and the data
files are available.

### Loading assembly catalogs

Assembly catalogs are loaded using their `AssemblyName`, not their
path:

```
  config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS,
    "MyAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
```

You can leave the version out of the name.

## Release notes

Releases are being pushed to [NuGet](https://www.nuget.org/packages/XmlResolver/).

