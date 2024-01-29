using System.Reflection;
using XmlResolver;
using XmlResolver.Utils;

namespace Tests;

public class XmlResolverTest
{
    protected readonly string TestRootPath;
    protected readonly Uri TestRootDirectory;
    protected readonly List<Uri> PropertyFiles = new();
    protected readonly List<string> CatalogFiles = new();
    private Assembly? _assembly;
    private XmlResolverConfiguration? _config;

    protected Assembly Asm
    {
        get
        {
            _assembly ??= Assembly.GetExecutingAssembly();
            return _assembly;
        }
    }

    protected XmlResolverConfiguration Config
    {
        get
        {
            _config ??= new(PropertyFiles, CatalogFiles);
            return _config!;
        }
    }

    protected XmlResolverTest()
    {
        string? path = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVER_PATH");
        if (path is null or "")
        {
            path = "/Volumes/Projects/xmlresolver/cs"; // It won't work for you, but...
        }

        path = path.Replace("\\", "/");
        if (path.EndsWith("/"))
        {
            path = path[..^1];
        }

        TestRootPath = path;
        TestRootDirectory = UriUtils.NewUri(TestRootPath + "/");
    }

    protected Stream GetStream(Uri uri)
    {
        Stream? data = ResourceAccess.GetStream(Config, uri);
        if (data == null)
        {
            throw new NullReferenceException("Failed to load uri: " + uri);
        }

        return data;
    }
}