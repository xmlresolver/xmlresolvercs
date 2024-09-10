using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using XmlResolver.Utils;

namespace XmlResolver;

public static class ResourceAccess
{
    public const int FollowRedirectLimit = 64;
    private static HttpClient _httpClient = new HttpClient();
    
    public static ResourceResponse GetResource(ResourceRequest request)
    {
        Uri? uri = request.GetAbsoluteUri();
        if (uri == null && request.Uri != null)
        {
            uri = new Uri(request.Uri);
        }

        if (uri == null)
        {
            throw new NullReferenceException("URI must not be null in GetResource");
        }

        if (!uri.IsAbsoluteUri)
        {
            uri = UriUtils.Resolve(UriUtils.Cwd(), uri.ToString());
        }

        switch (uri.Scheme)
        {
            case "data":
                return _getDataResource(request, uri);
            case "file":
                return _getFileResource(request, uri);
            case "http":
            case "https":
                return _getHttpResource(request, uri);
            case "pack":
                return _getPackResource(request, uri);
            default:
                throw new ArgumentException("Unexpected URI scheme: " + request.Uri);
        }
    }

    public static ResourceResponse GetResource(ResourceResponse response)
    {
        Uri? uri = response.ResolvedUri;
        if (uri == null)
        {
            uri = response.Request.GetAbsoluteUri();
            if (uri == null && response.Request.Uri != null)
            {
                uri = new Uri(response.Request.Uri);
            }
        }

        if (uri == null)
        {
            throw new NullReferenceException("URI must not be null in GetResource");
        }

        if (!uri.IsAbsoluteUri)
        {
            uri = UriUtils.Resolve(UriUtils.Cwd(), uri.ToString());
        }

        try
        {
            switch (uri.Scheme)
            {
                case "data":
                    return _getDataResource(response.Request, uri);
                case "file":
                    return _getFileResource(response.Request, uri);
                case "http":
                case "https":
                    return _getHttpResource(response.Request, uri);
                case "pack":
                    return _getPackResource(response.Request, uri);
                default:
                    throw new ArgumentException("Unexpected URI scheme: " + uri);
            }
        }
        catch (Exception)
        {
            return response;
        }
    }

    private static ResourceResponse _getFileResource(ResourceRequest request, Uri resourceUri)
    {
        string uri = resourceUri.ToString();
        string fn;

        if (uri.StartsWith("file:///"))
        {
            fn = uri[7..];
        }
        else if (uri.StartsWith("file://"))
        {
            fn = uri[6..];
        }
        else
        {
            fn = uri[5..];
        }

        // What about /C:/path/to/thing?
        if (fn.Length >= 3 && fn[2] == ':')
        {
            fn = fn.Substring(1);
        }

        // If this looks like /C:/path/to/thing, make it C:/path/to/thing
        if (fn.Length >= 3 && fn[0] == '/' && fn[2] == ':')
        {
            fn = fn.Substring(1);
        }

        ResourceResponse resp = new ResourceResponse(request, resourceUri);
        if (request.IsOpenStream)
        {
            resp.Stream = File.OpenRead(fn);
        }

        return resp;
    }

    private static ResourceResponse _getHttpResource(ResourceRequest request, Uri resourceUri)
    {
        var seen = new HashSet<Uri>();
        var count = FollowRedirectLimit;
        var resolvedUri = resourceUri;
        HttpStatusCode status = HttpStatusCode.OK;
        bool done = false;

        while (!done)
        {
            count--;
            done = count <= 0;

            var httpClient = _httpClient;
            var req = new HttpRequestMessage(HttpMethod.Get, resolvedUri);
            var resp = httpClient.Send(req);

            status = resp.StatusCode;
            if (seen.Contains(resolvedUri))
            {
                throw new HttpRequestException("Redirect loop", null, status);
            }

            seen.Add(resolvedUri);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var headers = new Dictionary<string, List<string>>();
                foreach (var header in resp.Headers)
                {
                    List<string> value = new List<string>();
                    foreach (var hvalue in header.Value)
                    {
                        value.Add(hvalue);
                    }

                    headers.Add(header.Key, value);
                }

                string ctype;
                if (resp.Content.Headers.ContentType is null)
                {
                    ctype = "application/octet-stream";
                }
                else
                {
                    ctype = resp.Content.Headers.ContentType.ToString();
                }

                ResourceResponse rresp = new ResourceResponse(request, resp.RequestMessage!.RequestUri);
                if (request.IsOpenStream)
                {
                    rresp.Stream = resp.Content.ReadAsStream();
                }

                rresp.ContentType = ctype;
                rresp.Headers = headers;
                rresp.StatusCode = 200;
                return rresp;
            }

            if (resp.StatusCode is HttpStatusCode.Moved or HttpStatusCode.Redirect)
            {
                resolvedUri = resp.Content.Headers.ContentLocation ?? throw new HttpRequestException("Redirect without location", null, status);
            }
            else
            {
                done = true;
            }
        }

        if (count <= 0)
        {
            throw new HttpRequestException("Too many redirects", null, status);
        }

        throw new HttpRequestException("Failed to read resource", null, status);
    }

    private static ResourceResponse _getPackResource(ResourceRequest request, Uri resourceUri)
    {
        string uri = resourceUri.ToString();
        
        // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
        if (uri.StartsWith("pack://application:,,,"))
        {
            Assembly asm = request.Assembly;

            string fn = uri.Substring(22);
            int pos = fn.IndexOf(";component/", StringComparison.Ordinal);
            if (pos >= 0)
            {
                string asmparts = fn[..pos];
                fn = fn[(pos + 10)..];

                string? name = null;
                string? version = null;
                string? key = null;

                pos = asmparts.IndexOf(";", StringComparison.Ordinal);
                if (pos >= 0)
                {
                    name = asmparts[..pos];
                    asmparts = asmparts[(pos + 1)..];

                    pos = asmparts.IndexOf(";", StringComparison.Ordinal);
                    if (pos >= 0)
                    {
                        version = asmparts[..pos];
                        asmparts = asmparts[(pos + 1)..];
                    }
                    else
                    {
                        if (!"".Equals(asmparts))
                        {
                            version = asmparts;
                            asmparts = "";
                        }
                    }

                    if (!"".Equals(asmparts))
                    {
                        key = asmparts;
                    }
                }
                else
                {
                    name = asmparts;
                }

                if (name.StartsWith("/"))
                {
                    name = name.Substring(1);
                }

                AssemblyName asmname = new AssemblyName(name);
                if (version != null)
                {
                    asmname.Version = new Version(version);
                }

                if (key != null)
                {
                    int byteCount = key.Length / 2;
                    byte[] keyToken = new byte[byteCount];
                    for (int i = 0; i < byteCount; i++)
                    {
                        string byteString = key.Substring(i * 2, 2);
                        keyToken[i] = byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }

                    asmname.SetPublicKeyToken(keyToken);
                }

                asm = Assembly.Load(asmname);
            }

            if (fn.StartsWith("/"))
            {
                fn = fn.Substring(1);
            }

            fn = fn.Replace("/", ".");

            Uri rsrcUri = new Uri(uri);
            ResourceResponse resp = new ResourceResponse(request, rsrcUri);
            if (request.IsOpenStream)
            {
                resp.Stream = asm.GetManifestResourceStream(fn);
            }

            resp.ContentType = "application/octet-stream";
            return resp;
        }
        else if (uri.StartsWith("pack:///siteoforigin:,,,"))
        {
            throw new ArgumentException("The siteoforigin authority is not supported: " + uri);
        }
        else if (uri.StartsWith("pack://file%3"))
        {
            return _getPackFileResource(request, resourceUri);
        }

        throw new ArgumentException("Unexpected pack: URI format: " + uri);
    }

    private static ResourceResponse _getPackFileResource(ResourceRequest request, Uri resourceUri)
    {
        string uri = resourceUri.ToString();
        string fileStr = uri[7..];
        int pos = fileStr.IndexOf("/", StringComparison.Ordinal);
        if (pos < 0)
        {
            throw new ArgumentException("Unsupported pack: URI format: " + uri);
        }

        string path = fileStr[(pos + 1)..];
        fileStr = fileStr[..pos];
        fileStr = fileStr.Replace("%3a", ":");
        fileStr = fileStr.Replace("%3A", ":");
        fileStr = fileStr.Replace(",", "/");

        Uri fileuri = UriUtils.NewUri(fileStr);
        fileStr = fileuri.AbsolutePath;

        // Assume this is a zip file...
        ZipArchive zipRead = ZipFile.OpenRead(fileStr);
        foreach (ZipArchiveEntry entry in zipRead.Entries)
        {
            if (entry.FullName.Equals(path))
            {
                ResourceResponse resp = new ResourceResponse(request, fileuri);
                if (request.IsOpenStream)
                {
                    resp.Stream = entry.Open();
                }

                resp.ContentType = "application/octet-stream";
                return resp;
            }
        }

        return new ResourceResponse(request, fileuri);
    }

    private static ResourceResponse _getDataResource(ResourceRequest request, Uri resourceUri)
    {
        string uri = resourceUri.ToString();
        
        // This is a little bit crude; see RFC 2397
        var reqUri = new Uri(uri);

        // Can't use URI accessors because they percent decode the string incorrectly.
        var path = uri[5..];
        var pos = path.IndexOf(",", StringComparison.Ordinal);

        if (pos < 0)
        {
            throw new UriFormatException(uri + ": comma separator missing");
        }

        Stream? inputStream = null;
        var mediaType = path[..pos];
        String data = path[(pos + 1)..];
        if (mediaType.EndsWith(";base64"))
        {
            // Base64 decode it
            var base64Bytes = Convert.FromBase64String(data);
            inputStream = new MemoryStream(base64Bytes);
        }
        else
        {
            // URL decode it
            string charset = "UTF-8";
            pos = mediaType.IndexOf(";charset=", StringComparison.Ordinal);
            if (pos > 0)
            {
                charset = mediaType[(pos + 9)..];
                pos = charset.IndexOf(";", StringComparison.Ordinal);
                if (pos >= 0)
                {
                    charset = charset[..pos];
                }
            }

            data = HttpUtility.UrlDecode(data, Encoding.GetEncoding(charset));
            inputStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
        }

        ResourceResponse resp = new ResourceResponse(request, reqUri);
        if (request.IsOpenStream)
        {
            resp.Stream = inputStream;
        }

        resp.ContentType = mediaType;
        return resp;
    }

    /// <summary>
    /// Returns a stream for the given URI.
    /// </summary>
    /// <para>Equivalent to <c>GetStream(uri.ToString(), Assembly.GetExecutingAssembly())</c>.
    /// </para>
    /// <para>Will throw exceptions if errors occur attempting to resolve the reference
    /// or open the stream.</para>
    /// <param name="uri">The URI.</param>
    /// <returns>The stream, or null if the stream could not be opened.</returns>
    public static Stream? GetStream(IResolverConfiguration config, Uri uri)
    {
        ResourceRequest request = new ResourceRequest(config)
        {
            Uri = uri.ToString(),
            IsOpenStream = true
        };

        return _getStream(request);
    }

    /// <summary>
    /// Returns a stream for the given URI in the context of the given assembly.
    /// </summary>
    /// <para>Equivalent to <c>GetStream(uri.ToString(), asm)</c>.
    /// </para>
    /// <para>Will throw exceptions if errors occur attempting to resolve the reference
    /// or open the stream.</para>
    /// <param name="uri">The URI.</param>
    /// <param name="asm">The relevant assembly.</param>
    /// <returns>The stream, or null if the stream could not be opened.</returns>
    public static Stream? GetStream(IResolverConfiguration config, Uri uri, Assembly asm)
    {
        ResourceRequest request = new ResourceRequest(config)
        {
            Uri = uri.ToString(),
            Assembly = asm,
            IsOpenStream = true
        };
        return _getStream(request);
    }

    /// <summary>
    /// Returns a stream for the given URI.
    /// </summary>
    /// <para>Equivalent to <c>GetStream(uri, Assembly.GetExecutingAssembly())</c>.
    /// </para>
    /// <para>Will throw exceptions if errors occur attempting to resolve the reference
    /// or open the stream.</para>
    /// <param name="uri">The URI.</param>
    /// <returns>The stream, or null if the stream could not be opened.</returns>
    public static Stream? GetStream(IResolverConfiguration config, string uri)
    {
        return GetStream(config, new Uri(uri));
    }

    /// <summary>
    /// Returns a stream for the given URI.
    /// </summary>
    /// <para>The URI must be absolute. Only <c>file:</c>, <c>http:</c> and <c>https:</c>,
    /// <c>pack:</c>, and <c>data:</c> URIs are supported.</para>
    /// <para>The assembly is only relevant for <c>pack:</c> scheme URIs.</para>
    /// <para>In addition to <c>ArgumentException</c>, any exceptions raised attempting to open the
    /// resource will also be thrown. In particular, <c>FileNotFoundException</c> or similar exceptions for unreadable
    /// file: URIs and <c>HttpRequestException</c> for http(s) resources that return an error code other than 200.
    /// (The underlying Http machinery follows redirects, so this shouldn't apply to 3xx error codes under
    /// most circumstances.)</para>
    /// <param name="uri">The URI.</param>
    /// <param name="asm">The relevant assembly.</param>
    /// <returns>The stream, or null if the stream could not be opened.</returns>
    /// <exception cref="ArgumentException">If the URI is not absolute or has an unsupported scheme.</exception>
    public static Stream? GetStream(IResolverConfiguration config, string uri, Assembly asm)
    {
        return GetStream(config, new Uri(uri), asm);
    }

    private static Stream? _getStream(ResourceRequest request)
    {
        ResourceResponse resp = GetResource(request);
        return resp.Stream;
    }
}