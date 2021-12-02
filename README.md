# xmlresolvercs

A C# implementation of the XML Resolver

If you’ve got experience with C# projects and you happen to notice me
doing anything foolish, please do let me know.

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

* 0.2.0 Several interfaces have been renamed to follow C# conventions.
  Methods related to the namespace resolver have been added to them
  (given that I was breaking them anyway).
* 0.1.0 Initial release.
