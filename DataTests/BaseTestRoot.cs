using System;
using XmlResolver.Utils;

namespace DataTests;

public class BaseTestRoot
{
    protected readonly string TestRootPath;
    protected readonly Uri TestRootDirectory;

    public BaseTestRoot()
    {
        // FIXME: Deal with Windows paths
        string? path = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVERDATA_PATH");
        if (string.IsNullOrEmpty(path))
        {
            path = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVER_PATH");
        }

        if (string.IsNullOrEmpty(path))
        {
            path = "/Volumes/Projects/xmlresolver/cs"; // It won't work for you, but ...
        }

        while (path.EndsWith("/"))
        {
            path = path.Substring(0, path.Length - 1);
        }

        TestRootPath = path;
        TestRootDirectory = UriUtils.NewUri(TestRootPath + "/");
    }
}