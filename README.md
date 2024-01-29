# xmlresolvercs

A C# implementation of the XML Resolver

## Cloning

The build now includes a collection of schemas and resources that can
be made available in a “data” assembly (at least that’s my current
plan). Because these resources are shared across both the Java and C#
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

## Release notes

Releases are being pushed to [NuGet](https://www.nuget.org/packages/XmlResolver/).

### Backwards incompatible changes

* This is the `legacy_v2` branch. Future development on the `main`
  branch is for XmlResolver v6.x and beyond
