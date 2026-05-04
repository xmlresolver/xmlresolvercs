using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using NLog;
using XmlResolver.Utils;

namespace XmlResolver;

/// <summary>
/// Static class containing methods to access resources.
/// </summary>
/// <para>Supports http, https, file, data, and pack URI schemes.</para>
public static class ResourceAccess
{
    /// <summary>
    /// Maximum number of redirects to follow.
    /// </summary>
    public static readonly int FOLLOW_REDIRECT_LIMIT = 64;

    private static readonly ResolverLogger Logger = new(LogManager.GetCurrentClassLogger());
    private static readonly HttpClient HttpClient = new HttpClient();
    
    /// <summary>
    /// Get the resource represented by a request.
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The resource response</returns>
    public static IResourceResponse GetResource(IResourceRequest request)
    {
        return GetResource(request, Assembly.GetExecutingAssembly());
    }
    
    /// <summary>
    /// Get the resource represented by a response.
    /// </summary>
    /// <param name="response">The response</param>
    /// <returns>The resource response, with a Stream</returns>
    public static IResourceResponse GetResource(IResourceResponse response)
    {
        return GetResource(response, Assembly.GetExecutingAssembly());
    }
    
    /// <summary>
    /// Get the resource represented by a request in the context of a particular Assembly.
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="asm">The assembly</param>
    /// <returns>The response, with a Stream</returns>
    /// <exception cref="NullReferenceException">If the request URI is null</exception>
    public static IResourceResponse GetResource(IResourceRequest request, Assembly asm)
    {
        var uri = request.GetAbsoluteUri();
        if (uri == null && request.Uri != null)
        {
            uri = new Uri(request.Uri!);
        }

        if (uri == null)
        {
            throw new NullReferenceException("Uri must not be null in GetResource");
        }

        if (!uri.IsAbsoluteUri)
        {
            uri = UriUtils.Resolve(UriUtils.Cwd(), uri.ToString());
        }

        var resp = GetResource(request, uri);
        
        // FIXME: in Java this is getURI()
        Logger.Debug("GetResource: {0}: {1}", resp.Resolved, resp.ResolvedUri);
        return resp;
    }

    /// <summary>
    /// Get the resource represented by a response in the context of a particular Assembly.
    /// </summary>
    /// <param name="response">The response</param>
    /// <param name="asm">The assembly</param>
    /// <returns>The response, with a Stream</returns>
    /// <exception cref="NullReferenceException">If the response URI is null</exception>
    public static IResourceResponse GetResource(IResourceResponse response, Assembly asm)
    {
        var uri = response.ResolvedUri;
        if (uri == null)
        {
            uri = response.Request.GetAbsoluteUri();
            if (uri == null && response.Request.Uri != null)
            {
                uri = new Uri(response.Request.Uri!);
            }
        }

        if (uri == null)
        {
            throw new NullReferenceException("Uri must not be null in GetResource");
        }

        if (!uri.IsAbsoluteUri)
        {
            uri = UriUtils.Resolve(UriUtils.Cwd(), uri.ToString());
        }

        var resp = GetResource(response.Request, uri, asm);
        
        Logger.Debug("GetResource: {0}: {1}", resp.Resolved, resp.ResolvedUri);
        return resp;
    }

    private static IResourceResponse GetResource(IResourceRequest request, Uri uri)
    {
        return GetResource(request, uri, Assembly.GetExecutingAssembly());
    }

    private static IResourceResponse GetResource(IResourceRequest request, Uri uri, Assembly asm)
    {
        switch (uri.Scheme)
        {
            case "http":
            case "https":
                return _getHttpResource(request, uri);
            case "file":
                return _getFileResource(request, uri);
            case "data":
                return _getDataResource(request, uri);
            case "pack":
                return _getPackResource(request, uri, asm);
            default:
                throw new ArgumentException("Unexpected URI scheme: " + uri);
        }
    }

    private static IResourceResponse _getHttpResource(IResourceRequest request, Uri uri)
    {
        var (resolvedUri, resp) = _getHttpResponse(uri);
        
        // If the status code wasn't 200, _getHttpResponse would have thrown an exception
        
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

        var rsrc = new ResourceResponse(request, resolvedUri);
        
        var pos = ctype.IndexOf("charset=", StringComparison.Ordinal);
        if (pos > 0)
        {
            var charset = ctype.Substring(pos + 8);
            pos = charset.IndexOf(";", StringComparison.Ordinal);
            if (pos > 0)
            {
                charset = charset.Substring(0, pos);
            }
            rsrc.Encoding = charset;
        }
        rsrc.ContentType = ctype;
        rsrc.SetHeaders(headers);
        rsrc.Stream = resp.Content.ReadAsStream();
        rsrc.StatusCode = 200;
        return rsrc;
    }

    private static (Uri resolvedUri, HttpResponseMessage resp) _getHttpResponse(Uri uri)
    {
        var seen = new HashSet<Uri>();
        var count = FOLLOW_REDIRECT_LIMIT;
        var resolvedUri = uri;
        HttpStatusCode status = HttpStatusCode.OK;
        bool done = false;

        while (!done)
        {
            count--;
            done = count <= 0;
                
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, resolvedUri);
            HttpResponseMessage resp = HttpClient.Send(req);

            status = resp.StatusCode;
            if (seen.Contains(resolvedUri))
            {
                throw CreateHttpRequestException("Redirect loop", status);
            }

            seen.Add(resolvedUri);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                return (resolvedUri, resp);
            }

            if (resp.StatusCode == HttpStatusCode.Moved || resp.StatusCode == HttpStatusCode.Redirect)
            {
                resolvedUri = resp.Content.Headers.ContentLocation
                              ?? throw CreateHttpRequestException("Redirect without location", status); 
            }
            else
            {
                done = true;
            }
        }

        if (count <= 0)
        {
            throw CreateHttpRequestException("Too many redirects", status);
        }

        throw CreateHttpRequestException("Failed to read resource", status);

        HttpRequestException CreateHttpRequestException(string text, HttpStatusCode statusCode)
        {
#if NETSTANDARD2_0
            return new HttpRequestException($"{text} ({statusCode})");
#else
            return new HttpRequestException(text, null, statusCode);
#endif
        }
    }
    
    private static IResourceResponse _getFileResource(IResourceRequest request, Uri uri)
    {
        var resp = new ResourceResponse(request, uri);
        resp.Stream = _getFileStream(uri.ToString());
        return resp;
    }

    private static FileStream _getFileStream(string uri)
    {
        var fn = "";
        if (uri.StartsWith("file:///"))
        {
            fn = uri.Substring(7);
        } 
        else if (uri.StartsWith("file://"))
        {
            fn = uri.Substring(6);
        }
        else
        {
            fn = uri.Substring(5);
        }

        // If this looks like /C:/path/to/thing, make it C:/path/to/thing
        if (fn.Length >= 3 && fn[0] == '/' && fn[2] == ':') {
            fn = fn.Substring(1);
        }

        return File.OpenRead(fn);
    }

    private static IResourceResponse _getDataResource(IResourceRequest request, Uri reqUri)
    {
        var resp = new ResourceResponse(request, reqUri);
        var (mediaType, stream) = _getDataStream(reqUri);
        resp.Stream = stream;
        resp.ContentType = mediaType;
        return resp;
    }

    private static (string mediaType, Stream stream) _getDataStream(Uri reqUri)
    {
        // This is a little bit crude; see RFC 2397
        var uri = reqUri.ToString();
        
        // Can't use URI accessors because they percent decode the string incorrectly.
        var path = uri.Substring(5);
        var pos = path.IndexOf(',', StringComparison.Ordinal);

        if (pos < 0)
        {
            throw new UriFormatException(uri + ": comma separator missing");
        }
            
        var mediaType = path[..pos];
        var data = path[(pos + 1)..];
        if (mediaType.EndsWith(";base64")) {
            // Base64 decode it
            mediaType = mediaType.Substring(0, mediaType.Length - 7);
            var base64Bytes = Convert.FromBase64String(data);
            return (mediaType, new MemoryStream(base64Bytes));
        }

        // URL decode it
        var charset = "UTF-8";
        pos = mediaType.IndexOf(";charset=", StringComparison.Ordinal);
        if (pos > 0) {
            charset = mediaType.Substring(pos + 9);
            mediaType = mediaType.Substring(0, pos);
            pos = charset.IndexOf(';', StringComparison.Ordinal);
            if (pos >= 0) {
                charset = charset.Substring(0, pos);
            }
        }

        data = HttpUtility.UrlDecode(data, Encoding.GetEncoding(charset));
        return (mediaType, new MemoryStream(Encoding.UTF8.GetBytes(data)));
    }

    private static IResourceResponse _getPackResource(IResourceRequest request, Uri reqUri, Assembly asm)
    {
        var resp = new ResourceResponse(request, reqUri)
        {
            Stream = _getPackStream(reqUri, asm)
        };
        return resp;
    }

    private static Stream _getPackStream(Uri reqUri, Assembly asm)
    {
        var uri = reqUri.ToString();
        
        // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
        if (uri.StartsWith("pack://application:,,,")) {
            string fn = uri.Substring(22);
            int pos = fn.IndexOf(";component/", StringComparison.Ordinal);
            if (pos >= 0) {
                string asmparts = fn.Substring(0, pos);
                fn = fn.Substring(pos + 10);

                string name = null;
                string version = null;
                string key = null;

                pos = asmparts.IndexOf(';', StringComparison.Ordinal);
                if (pos >= 0) {
                    name = asmparts.Substring(0, pos);
                    asmparts = asmparts.Substring(pos + 1);

                    pos = asmparts.IndexOf(';', StringComparison.Ordinal);
                    if (pos >= 0) {
                        version = asmparts.Substring(0, pos);
                        asmparts = asmparts.Substring(pos + 1);
                    }
                    else {
                        if (!"".Equals(asmparts)) {
                            version = asmparts;
                            asmparts = "";
                        }
                    }

                    if (!"".Equals(asmparts)) {
                        key = asmparts;
                    }
                }
                else {
                    name = asmparts;
                }

                if (name.StartsWith('/')) {
                    name = name.Substring(1);
                }

                AssemblyName asmname = new AssemblyName(name);
                if (version != null) {
                    asmname.Version = new Version(version);
                }

                if (key != null) {
                    var byteCount = key.Length / 2;
                    var keyToken = new byte[byteCount];
                    for (var i = 0; i < byteCount; i++)
                    {
                        var byteString = key.Substring(i * 2, 2);
                        keyToken[i] = byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }

                    asmname.SetPublicKeyToken(keyToken);
                }

                asm = Assembly.Load(asmname);
            }

            if (fn.StartsWith('/')) {
                fn = fn.Substring(1);
            }

            fn = fn.Replace("/", ".");

            return asm.GetManifestResourceStream(fn);
        } else if (uri.StartsWith("pack:///siteoforigin:,,,")) {
            throw new ArgumentException("The siteoforigin authority is not supported: " + uri);
        } else if (uri.StartsWith("pack://file%3")) {
            return _getPackFileStream(uri);
        }
        throw new ArgumentException("Unexpected pack: URI format: " + uri);
        
    }
    
    private static Stream _getPackFileStream(string uri)
    {
        string filestr = uri.Substring(7);
        int pos = filestr.IndexOf('/',  StringComparison.Ordinal);
        if (pos < 0) {
            throw new ArgumentException("Unsupported pack: URI format: " + uri);
        }

        string path = filestr.Substring(pos + 1);
        filestr = filestr.Substring(0, pos);
        filestr = filestr.Replace("%3a", ":");
        filestr = filestr.Replace("%3A", ":");
        filestr = filestr.Replace(",", "/");

        Uri fileuri = UriUtils.NewUri(filestr);
        filestr = fileuri.AbsolutePath;
        
        // Assume this is a zip file...
        ZipArchive zipRead = ZipFile.OpenRead(filestr);
        foreach (ZipArchiveEntry entry in zipRead.Entries) {
            if (entry.FullName.Equals(path))
            {
                return entry.Open();
            }
        }

        throw new ArgumentException("Could not get pack file stream: " + uri);
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
    public static Stream GetStream(Uri uri) {
        return GetStream(uri.ToString(), Assembly.GetExecutingAssembly());
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
    public static Stream GetStream(Uri uri, Assembly asm) {
        return GetStream(uri.ToString(), asm);
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
    public static Stream GetStream(string uri) {
        return GetStream(uri, Assembly.GetExecutingAssembly());
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
    public static Stream GetStream(string uri, Assembly asm)
    {
        if (uri.StartsWith("file:/")) {
            return _getFileStream(uri);
        }
            
        if (uri.StartsWith("http:/") || uri.StartsWith("https:/")) {
            var httpUri = new Uri(uri);
            var (resolvedUri, resp) = _getHttpResponse(httpUri);
            return resp.Content.ReadAsStream();
        } 
            
        if (uri.StartsWith("pack:/")) {
            return _getPackStream(new Uri(uri), asm);
        }

        if (uri.StartsWith("data:"))
        {
            var (_, stream) = _getDataStream(new Uri(uri));
            return stream;
        }

        throw new ArgumentException("Unexpected URI scheme: " + uri);
    }
}