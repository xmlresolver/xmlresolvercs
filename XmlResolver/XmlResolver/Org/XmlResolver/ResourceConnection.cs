using System;
using System.Globalization;
using System.IO;
using System.Net;
using NLog;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver {
    public class ResourceConnection {
        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

        private Stream _stream = null;
        private Uri _uri = null;
        private int _statusCode = -1;
        private string _contentType = null;
        private string _etag = null;
        private long _lastModified = -1;
        private long _date = -1;
        
        public ResourceConnection(string resolved) : this(resolved, false) {
            // nop
        }

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

        public Uri Uri => _uri;
        public Stream Stream => _stream;
        public string ContentType => _contentType;
        public int StatusCode => _statusCode;
        public string ETag => _etag;
        public long LastModified => _lastModified;
        public long Date => _date;

        public void Close() {
            if (_stream != null) {
                _stream.Close();
                _stream = null;
            }
        }
    }
}