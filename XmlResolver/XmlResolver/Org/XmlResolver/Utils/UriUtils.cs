using System;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Reflection;
using System.Text;

namespace Org.XmlResolver.Utils {
    public class UriUtils {
        public static Uri Resolve(Uri baseURI, string uri) {
            if (uri.StartsWith("pack://")) {
                return GetPackUri(uri);
            }
            
            return new Uri(baseURI, uri);
        }

        public static Uri Resolve(Uri baseURI, Uri uri) {
            return new Uri(baseURI, uri);
        }

        public static Uri Cwd() {
            return new Uri(new Uri("file://"), 
                System.IO.Directory.GetCurrentDirectory() + "/");
        }
        
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
        
        public static Uri GetLocationUri(string resourcePath, Assembly assembly = null) {
            StringBuilder uribuilder = new StringBuilder();
            uribuilder.Append("application:///");

            if (assembly == null) {
                assembly = Assembly.GetExecutingAssembly();
            }

            // FIXME: the Microsoft documentation is inconsistent about the format of the path.
            // It says, the path must conform to AssemblyShortName{;Version]{;PublicKey];component/Path
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

        public static Stream GetStream(Uri uri) {
            return GetStream(uri.ToString(), Assembly.GetExecutingAssembly());
        }

        public static Stream GetStream(Uri uri, Assembly asm) {
            return GetStream(uri.ToString(), asm);
        }

        public static Stream GetStream(string uri) {
            return GetStream(uri, Assembly.GetExecutingAssembly());
        }

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

            return File.OpenRead(fn);
        }
        
        private static Stream _getHttpStream(string uri) {
            using (WebClient client = new WebClient()) {
                byte[] response = client.DownloadData(uri);
                return new MemoryStream(response);
            }
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
                fn = asm.GetName().Name + "." + fn;

                /*
                if (true) {
                    Stream stream = asm.GetManifestResourceStream(fn);
                    if (stream == null) {
                        throw new NullReferenceException("Failed to read stream from " + fn);
                    }
                    StreamReader sr = new StreamReader(stream);
                    StringBuilder sb = new StringBuilder();
                    String line;
                    while ((line = sr.ReadLine()) != null) {
                        sb.Append(line);
                    }

                    String foo = sb.ToString();
                    Console.WriteLine(foo);
                }
                */

                return asm.GetManifestResourceStream(fn);
            } else if (uri.StartsWith("pack:///siteoforigin:,,,")) {
                throw new ArgumentException("The siteoforigin authority is not supported: " + uri);
            }
            throw new ArgumentException("Unexpected pack: URI format: " + uri);
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
                    var base64bytes = System.Convert.FromBase64String(data);
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

                    data = System.Web.HttpUtility.UrlDecode(data, System.Text.Encoding.GetEncoding(charset));
                    inputStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
                }

                return inputStream;
            }
            
            throw new UriFormatException(uri + ": comma separator missing");
        }
        
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