using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Org.XmlResolver.Utils {
    /// <summary>
    /// Convenience class of methods for dealing with URIs and obtaining data from resources identified
    /// with URIs.
    /// </summary>
    ///
    /// <para>The methods in <c>URIUtils</c> are designed to simplify access, but they are
    /// considered relatively low level. It is the caller's responsibility to catch exceptions
    /// that may arise (because resources aren't found, for example, or aren't accessible.</para>
    /// 
    public class UriUtils {
        protected static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Report if the platform is Windows.
        /// </summary>
        /// <returns>True if the platform is Windows, false otherwise.</returns>
        public static Boolean isWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
        
        /// <summary>
        /// Resolve a string against a base URI, taking special care of pack: URIs.
        /// </summary>
        /// <para>URIs in the <c>pack:</c> scheme can be used to access resources in assemblies,
        /// see https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
        /// </para>.
        /// <para>If the <c>uri</c> appears to be a <c>pack:</c> URI, an attempt is
        /// made to find the relevant assembly. If it is found, <see cref="GetLocationUri"/> is
        /// used to return a location. Only <c>pack://application:</c> URIs are supported,
        /// <c>pack://siteoforigin:</c> URIs are not.</para>
        /// <para>For URIs of other schemes, this method calls <c>new Uri(baseURI, uri)</c>.
        /// </para>
        /// <para>Will throw exceptions if the URI scheme is not supported or if resolution results
        /// in an exception.</para>
        /// 
        /// <param name="baseURI">The base URI.</param>
        /// <param name="uri">The possibly relative URI to make absolute.</param>
        /// <returns>The resolved URI.</returns>
        public static Uri Resolve(Uri baseURI, string uri) {
            if (uri.StartsWith("pack://")) {
                return GetPackUri(uri);
            }
            
            return new Uri(baseURI, uri);
        }

        /// <summary>
        /// Resolve a URI against a base URI.
        /// </summary>
        /// <para>This is purely a convenience method so that <c>UriUtils.Resolve()</c> can be
        /// called with either a string or Uri. It simply calls <c>new Uri(baseURI, uri)</c>
        /// on its inputs.</para>
        /// <param name="baseURI">The base URI.</param>
        /// <param name="uri">The possibly relative URI to make absolute.</param>
        /// <returns>The resolved URI.</returns>
        public static Uri Resolve(Uri baseURI, Uri uri) {
            return new Uri(baseURI, uri);
        }

        /// <summary>
        /// Returns the current working directory as a URI.
        /// </summary>
        /// <para>This method returns <see cref="Directory.GetCurrentDirectory"/> as a
        /// <c>file:</c> URI.</para>
        /// <returns>The current directory as a URI.</returns>
        public static Uri Cwd() {
            return new Uri(new Uri("file://"), 
                Directory.GetCurrentDirectory() + "/");
        }
        
        /// <summary>
        /// Normalize a URI. 
        /// </summary>
        /// <para>The XML Catalogs specification describes a normalization process for system and public
        /// identifiers. This normalization attempts to make comparisons more robust and is required
        /// for encoding public identifiers as URNs.</para>
        ///
        /// <para>Normalization consists of replacing all occurrences of characters less than
        /// or equal to space (0x20), greater than or equal to DEL (0x7F), or one of
        /// <c>&quot;</c>, <c>&lt;</c>, <c>&gt;</c>, <c>\</c>, <c>^</c>, <c>`</c>, <c>{</c>, <c>|</c>,
        /// or <c>}</c> with its hex-encoded value.</para> 
        ///
        /// <para>If the <c>uriref</c> is <c>null</c>, <c>null</c> is returned.
        /// <param name="uriref">The URI reference</param>
        /// <returns>The normalized URI reference.</returns>
        public static String NormalizeUri(string uriref) {
            if (uriref == null) {
                return null;
            }

            StringBuilder newRef = new StringBuilder();
            byte[] bytes = Encoding.UTF8.GetBytes(uriref);

            foreach (byte aByte in bytes) {
                int ch = aByte & 0xFF;
                if ((ch <= 0x20)    // ctrl
                    || (ch > 0x7F)  // high ascii
                    || (ch == 0x22) // "
                    || (ch == 0x3C) // <
                    || (ch == 0x3E) // >
                    || (ch == 0x5C) // \
                    || (ch == 0x5E) // ^
                    || (ch == 0x60) // `
                    || (ch == 0x7B) // {
                    || (ch == 0x7C) // |
                    || (ch == 0x7D) // }
                    || (ch == 0x7F)) {
                    newRef.Append(EncodedByte(ch));
                } else {
                    newRef.Append((char) aByte);
                }
            }

            return newRef.ToString();
        }

        private static String EncodedByte(int b) {
            String hex = b.ToString("X2");
            return "%" + hex;
        }
        
        /// <summary>
        /// Return a URI for a resource located in an assembly.
        /// </summary>
        /// <para>Generates a URI from the combination of a resource path and an assembly.
        /// If the assembly is null, the currently executing assembly is used.</para>
        /// <para>Note: The Microsoft documentation is inconsistent about the format of the path.
        /// It says the path must conform to <c>AssemblyShortName{;Version]{;PublicKey};component/Path</c>
        /// but then goes on to show examples that have a leading "/" before the AssemblyShortName.
        /// I can't work out how to construct paths of the latter form, see
        /// https://stackoverflow.com/questions/68255149 so this method returns the former for now.
        /// 
        /// <param name="resourcePath">The resource path.</param>
        /// <param name="assembly">The assembly, or null for the current assembly</param>
        /// <returns>Returns a pack: URI for the resource within the assembly.</returns>
        public static Uri GetLocationUri(string resourcePath, Assembly assembly = null) {
            StringBuilder uribuilder = new StringBuilder();
            uribuilder.Append("application:///");

            if (assembly == null) {
                assembly = Assembly.GetExecutingAssembly();
            }

            // FIXME: the Microsoft documentation is inconsistent about the format of the path.
            // It says, the path must conform to AssemblyShortName{;Version]{;PublicKey};component/Path
            // but then goes on to show examples that have a leading "/" before the AssemblyShortName.
            // I can't work out how to construct paths of the latter form, see
            // https://stackoverflow.com/questions/68255149 so I'm just going to run with the former
            // for now.

            AssemblyName name = assembly.GetName();
            uribuilder.Append(name.Name);
            if (name.Version != null) {
                uribuilder.Append(";");
                uribuilder.Append(name.Version);
            }
            
            // FIXME: what about the public key?

            uribuilder.Append(";component");

            resourcePath = resourcePath.Replace('\\', '/');
            if (!resourcePath.StartsWith("/")) {
                resourcePath = "/" + resourcePath;
            }

            Uri appBase = new Uri(uribuilder.ToString(), UriKind.Absolute);
            Uri appPath = new Uri(resourcePath, UriKind.Relative);
            Uri packUri = PackUriHelper.Create(appBase, appPath);
            return packUri;
        }
        
        private static Uri GetPackUri(string uri) {
            // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
            if (uri.StartsWith("pack://application:,,,")) {
                Assembly asm = null;
                string fn = uri.Substring(22);
                int pos = fn.IndexOf(";component/");
                if (pos >= 0) {
                    string asmparts = fn.Substring(0, pos);
                    fn = fn.Substring(pos + 10);

                    string name = null;
                    string version = null;
                    string key = null;

                    pos = asmparts.IndexOf(";");
                    if (pos >= 0) {
                        name = asmparts.Substring(0, pos);
                        asmparts = asmparts.Substring(pos + 1);

                        pos = asmparts.IndexOf(";");
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

                    if (name.StartsWith("/")) {
                        name = name.Substring(1);
                    }

                    if (name != null) {
                        AssemblyName asmname = new AssemblyName(name);
                        if (version != null) {
                            asmname.Version = new Version(version);
                        }

                        if (key != null) {
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
                }

                if (!fn.StartsWith("/")) {
                    fn = "/" + fn;
                }

                return GetLocationUri(fn, asm);
            }
            
            if (uri.StartsWith("pack:///siteoforigin:,,,")) {
                throw new ArgumentException("The siteoforigin authority is not supported: " + uri);
            }

            // If we fell all the way through to here, just hand it over to the system
            return new Uri(uri);
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
        public static Stream GetStream(string uri, Assembly asm) {
            if (uri.StartsWith("file:/")) {
                return _getFileStream(uri);
            }
            
            if (uri.StartsWith("http:/") || uri.StartsWith("https:/")) {
                return _getHttpStream(uri);
            } 
            
            if (uri.StartsWith("pack:/")) {
                return _getPackStream(uri, asm);
            }

            if (uri.StartsWith("data:")) {
                return _getDataStream(uri);
            }

            throw new ArgumentException("Unexpected URI scheme: " + uri);
        }

        private static Stream _getFileStream(string uri) {
            String fn;

            if (uri.StartsWith("file:///")) {
                fn = uri.Substring(7);
            } else if (uri.StartsWith("file://")) {
                fn = uri.Substring(6);
            }
            else {
                fn = uri.Substring(5);
            }

            // What about /C:/path/to/thing?
            if (fn.Length >= 3 && fn[2] == ':')
            {
                fn = fn.Substring(1);
            }

            // If this looks like /C:/path/to/thing, make it C:/path/to/thing
            if (fn.Length >= 3 && fn[0] == '/' && fn[2] == ':') {
                fn = fn.Substring(1);
            }
            
            return File.OpenRead(fn);
        }
        
        private static Stream _getHttpStream(string uri) {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage resp = httpClient.Send(req);

            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException("Failed to read resource", null, resp.StatusCode);
            }
            return resp.Content.ReadAsStream();
        }
        
        private static Stream _getPackStream(string uri, Assembly asm) {
            // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
            if (uri.StartsWith("pack://application:,,,")) {
                string fn = uri.Substring(22);
                int pos = fn.IndexOf(";component/");
                if (pos >= 0) {
                    string asmparts = fn.Substring(0, pos);
                    fn = fn.Substring(pos + 10);

                    string name = null;
                    string version = null;
                    string key = null;

                    pos = asmparts.IndexOf(";");
                    if (pos >= 0) {
                        name = asmparts.Substring(0, pos);
                        asmparts = asmparts.Substring(pos + 1);

                        pos = asmparts.IndexOf(";");
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

                    if (name.StartsWith("/")) {
                        name = name.Substring(1);
                    }

                    if (name != null) {
                        AssemblyName asmname = new AssemblyName(name);
                        if (version != null) {
                            asmname.Version = new Version(version);
                        }

                        if (key != null) {
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
                }

                if (fn.StartsWith("/")) {
                    fn = fn.Substring(1);
                }

                fn = fn.Replace("/", ".");

                Stream stream = asm.GetManifestResourceStream(fn);
                return stream;
            } else if (uri.StartsWith("pack:///siteoforigin:,,,")) {
                throw new ArgumentException("The siteoforigin authority is not supported: " + uri);
            } else if (uri.StartsWith("pack://file%3")) {
                return _getPackFileStream(uri);
            }
            throw new ArgumentException("Unexpected pack: URI format: " + uri);
        }

        private static Stream _getPackFileStream(string uri) {
            string filestr = uri.Substring(7);
            int pos = filestr.IndexOf("/");
            if (pos < 0) {
                throw new ArgumentException("Unsupported pack: URI format: " + uri);
            }

            string path = filestr.Substring(pos + 1);
            filestr = filestr.Substring(0, pos);
            filestr = filestr.Replace("%3a", ":");
            filestr = filestr.Replace("%3A", ":");
            filestr = filestr.Replace(",", "/");

            Uri fileuri = NewUri(filestr);
            filestr = fileuri.AbsolutePath;
            
            // Assume this is a zip file...
            ZipArchive zipRead = ZipFile.OpenRead(filestr);
            foreach (ZipArchiveEntry entry in zipRead.Entries) {
                if (entry.FullName.Equals(path)) {
                    return entry.Open();
                }
            }

            return null;
        }
        
        private static Stream _getDataStream(string uri) {
            // This is a little bit crude; see RFC 2397

            // Can't use URI accessors because they percent decode the string incorrectly.
            String path = uri.ToString().Substring(5);
            int pos = path.IndexOf(",");
            if (pos >= 0) {
                Stream inputStream = null;
                String mediatype = path.Substring(0, pos);
                String data = path.Substring(pos + 1);
                if (mediatype.EndsWith(";base64")) {
                    // Base64 decode it
                    var base64bytes = Convert.FromBase64String(data);
                    inputStream = new MemoryStream(base64bytes);
                }
                else {
                    // URL decode it
                    String charset = "UTF-8";
                    pos = mediatype.IndexOf(";charset=");
                    if (pos > 0) {
                        charset = mediatype.Substring(pos + 9);
                        pos = charset.IndexOf(";");
                        if (pos >= 0) {
                            charset = charset.Substring(0, pos);
                        }
                    }

                    data = HttpUtility.UrlDecode(data, Encoding.GetEncoding(charset));
                    inputStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
                }

                return inputStream;
            }
            
            throw new UriFormatException(uri + ": comma separator missing");
        }
        
        /// <summary>
        /// Convenience method for constructing URIs from strings.
        /// </summary>
        /// <para>This method normalizes the number of "/" characters that follow
        /// <c>file:</c> in a file URI, and promotes any <c>href</c> that begins with a
        /// slash to being a <c>file:</c> URI.</para>
        /// <param name="href">A URI or path.</param>
        /// <returns>A normalized file: URI or the string as a URI.</returns>
        public static Uri NewUri(string href) {
            if (href.StartsWith("file:")) {
                int pos = 5;
                while (pos <= href.Length && href[pos] == '/') {
                    pos++;
                }

                if (pos > 5) {
                    pos--;
                    href = href.Substring(pos);
                }
                else {
                    pos = 0;
                    href = "/" + href.Substring(5);
                }

                return new Uri("file://" + href);
            } 
            
            if (href.StartsWith("/")) {
                return new Uri("file://" + href);
            }
            
            return new Uri(href);
        }
    }
}