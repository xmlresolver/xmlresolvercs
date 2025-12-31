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
        var path = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
        if (string.IsNullOrEmpty(path)) {
            path = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVER_ROOT");
            if (string.IsNullOrEmpty(path))
            {
                path = "/tmp"; // It won't work, but it's somewhere...
            }
        }

        while (path.EndsWith("/"))
        {
            path = path.Substring(0, path.Length - 1);
        }

        TestRootPath = path;
        TestRootDirectory = UriUtils.NewUri(TestRootPath + "/");
    }
}
