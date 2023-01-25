using System;
using System.IO;
using System.Net;
using System.Net.Http;
using NLog;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver {
    /// <summary>
    /// Supports opening connections to resolved resources.
    /// </summary>
    /// <para>This is a utility wrapper around <see cref="WebRequest"/>s for http(s) resources.
    /// It returns not just the resource, but also information about the status and headers
    /// that the caching layer can use to determine if the resource should be cached locally.</para>
    public class ResourceConnection {
        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());
        protected static HttpClient httpClient = new HttpClient();

        private Stream _stream = null;
        private Uri _uri = null;
        private HttpStatusCode _statusCode = HttpStatusCode.OK;
        private string _contentType = null;
        private string _etag = null;
        private long _lastModified = -1;
        private long _date = -1;
        
        /// <summary>
        /// Create a connection for the URI.
        /// </summary>
        /// <para>This is the equivalent of calling the two argument constructor
        /// specifying "false" for <c>headOnly</c>.</para>
        /// <param name="resolved">The resolved (absolute) URI to access</param>
        public ResourceConnection(string resolved) : this(resolved, false) {
            // nop
        }

        /// <summary>
        /// Create a connection for the URI.
        /// </summary>
        /// <para>This version of the constructor can be used to specify that
        /// only a HEAD request is required. This form will initialize the status, content type,
        /// and other fields, but not a stream to read the resource.</para>
        /// <param name="resolved">The resolved (absolute) URI to access</param>
        /// <param name="headOnly">Is this only a HEAD request?</param>
        public ResourceConnection(string resolved, bool headOnly) {
            _uri = UriUtils.NewUri(resolved);

            if ("http".Equals(_uri.Scheme) || "https".Equals(_uri.Scheme)) {
                HttpRequestMessage req;
                if (headOnly)
                {
                    req = new HttpRequestMessage(HttpMethod.Head, _uri);
                }
                else
                {
                    req = new HttpRequestMessage(HttpMethod.Get, _uri);
                }

                try
                {
                    HttpResponseMessage resp = httpClient.Send(req);
                    _stream = resp.Content.ReadAsStream();
                    _statusCode = resp.StatusCode;
                    if (resp.Content.Headers.ContentType == null)
                    {
                        _contentType = "application/octet-stream";
                    }
                    else
                    {
                        _contentType = resp.Content.Headers.ContentType.ToString();
                    }
                        
                    if (resp.Headers.ETag != null)
                    {
                        _etag = resp.Headers.ETag.ToString();
                    }

                    if (resp.Content.Headers.LastModified != null)
                    {
                        _lastModified =
                            ((DateTimeOffset)resp.Content.Headers.LastModified).ToUnixTimeMilliseconds();
                    }
                        
                    if (resp.Headers.Date != null)
                    {
                        _date = ((DateTimeOffset)resp.Headers.Date).ToUnixTimeMilliseconds();
                    }
                }
                catch (HttpRequestException)
                {
                    // nop
                }
            }
        }

        /// <summary>
        /// The URI to which a connection was made.
        /// </summary>
        public Uri Uri => _uri;
        
        /// <summary>
        /// The stream for reading the resource. May be null.
        /// </summary>
        public Stream Stream => _stream;
        
        /// <summary>
        /// The content type of the resource. May be null.
        /// </summary>
        public string ContentType => _contentType;
        
        /// <summary>
        /// The status code returned by the request.
        /// </summary>
        public HttpStatusCode StatusCode => _statusCode;
        
        /// <summary>
        /// The ETag of the resource as returned by the server. May be null.
        /// </summary>
        public string ETag => _etag;

        /// <summary>
        /// The last modified timestamp as returned by the server.
        /// </summary>
        public long LastModified => _lastModified;
        
        /// <summary>
        /// The last modified date of the resource. May be null.
        /// </summary>
        /// <para>This value will be null if the server did not return a <c>Date</c>
        /// header or if the format of the date header did not match the expected form
        /// <c>ddd, dd MMM yyyy HH:mm:ss 'GMT'</c>.</para>
        public long Date => _date;

        /// <summary>
        /// Attempts to close the underlying stream, if there is one.
        /// </summary>
        public void Close() {
            if (_stream != null) {
                _stream.Close();
                _stream = null;
            }
        }
    }
}