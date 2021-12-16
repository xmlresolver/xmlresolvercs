using System;
using System.Globalization;
using System.IO;
using System.Net;
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

        private Stream _stream = null;
        private Uri _uri = null;
        private int _statusCode = -1;
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
                try {
                    HttpWebRequest req = WebRequest.CreateHttp(_uri);
                    if (headOnly) {
                        req.Method = "HEAD";
                    }
                    else {
                        req.Method = "GET";
                    }

                    HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
                    _stream = resp.GetResponseStream();
                    _statusCode = (int) resp.StatusCode;
                    if (resp.ContentType == string.Empty) {
                        _contentType = "application/octet-stream";
                    }
                    else {
                        _contentType = resp.ContentType;
                    }

                    string retag = null;
                    string date = null;
                    for (int pos = 0; pos < resp.Headers.Count; pos++) {
                        string header = resp.Headers.GetKey(pos);
                        if (retag == null && "etag".Equals(header.ToLower())) {
                            retag = resp.Headers.GetValues(pos)[0];
                        }

                        if (date == null && "date".Equals(header.ToLower())) {
                            date = resp.Headers.GetValues(pos)[0];
                        }
                    }

                    _etag = retag;
                    _lastModified = ((DateTimeOffset) resp.LastModified).ToUnixTimeMilliseconds();

                    if (date != null) {
                        DateTime dt = DateTime.ParseExact(date,
                            "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                            CultureInfo.InvariantCulture.DateTimeFormat,
                            DateTimeStyles.AssumeUniversal);
                        _date = ((DateTimeOffset) dt).ToUnixTimeMilliseconds();
                    }
                }
                catch (WebException) {
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
        public int StatusCode => _statusCode;
        
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