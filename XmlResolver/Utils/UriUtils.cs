using System.Globalization;
using System.IO.Packaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace XmlResolver.Utils;

public static class UriUtils
{
    public static readonly int FOLLOW_REDIRECT_LIMIT = 64;
    private static HttpClient _httpClient = new HttpClient();

    /// <summary>
    /// Report if the platform is Windows.
    /// </summary>
    /// <returns>True if the platform is Windows, false otherwise.</returns>
    public static bool IsWindows()
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
    public static Uri Resolve(Uri baseURI, string uri)
    {
        if (uri.StartsWith("pack://"))
        {
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
    /// <param name="baseUri">The base URI.</param>
    /// <param name="uri">The possibly relative URI to make absolute.</param>
    /// <returns>The resolved URI.</returns>
    public static Uri Resolve(Uri baseUri, Uri uri)
    {
        return new Uri(baseUri, uri);
    }

    /// <summary>
    /// Returns the current working directory as a URI.
    /// </summary>
    /// <para>This method returns <see cref="Directory.GetCurrentDirectory"/> as a
    /// <c>file:</c> URI.</para>
    /// <returns>The current directory as a URI.</returns>
    public static Uri Cwd()
    {
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
    /// <param name="uriRef">The URI reference</param>
    /// <returns>The normalized URI reference.</returns>
    public static string? NormalizeUri(string? uriRef)
    {
        if (uriRef == null)
        {
            return null;
        }

        StringBuilder newRef = new StringBuilder();
        byte[] bytes = Encoding.UTF8.GetBytes(uriRef);

        foreach (byte aByte in bytes)
        {
            int ch = aByte & 0xFF;
            if ((ch <= 0x20) // ctrl
                || (ch > 0x7F) // high ascii
                || (ch == 0x22) // "
                || (ch == 0x3C) // <
                || (ch == 0x3E) // >
                || (ch == 0x5C) // \
                || (ch == 0x5E) // ^
                || (ch == 0x60) // `
                || (ch == 0x7B) // {
                || (ch == 0x7C) // |
                || (ch == 0x7D) // }
                || (ch == 0x7F))
            {
                newRef.Append(EncodedByte(ch));
            }
            else
            {
                newRef.Append((char)aByte);
            }
        }

        return newRef.ToString();
    }

    private static string EncodedByte(int b)
    {
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
    public static Uri GetLocationUri(string resourcePath, Assembly? assembly = null)
    {
        StringBuilder uriBuilder = new StringBuilder();
        uriBuilder.Append("application:///");

        if (assembly == null)
        {
            assembly = Assembly.GetExecutingAssembly();
        }

        // FIXME: the Microsoft documentation is inconsistent about the format of the path.
        // It says, the path must conform to AssemblyShortName{;Version]{;PublicKey};component/Path
        // but then goes on to show examples that have a leading "/" before the AssemblyShortName.
        // I can't work out how to construct paths of the latter form, see
        // https://stackoverflow.com/questions/68255149 so I'm just going to run with the former
        // for now.

        AssemblyName name = assembly.GetName();
        uriBuilder.Append(name.Name);
        if (name.Version != null)
        {
            uriBuilder.Append(";");
            uriBuilder.Append(name.Version);
        }

        // FIXME: what about the public key?

        uriBuilder.Append(";component");

        resourcePath = resourcePath.Replace('\\', '/');
        if (!resourcePath.StartsWith("/"))
        {
            resourcePath = "/" + resourcePath;
        }

        Uri appBase = new Uri(uriBuilder.ToString(), UriKind.Absolute);
        Uri appPath = new Uri(resourcePath, UriKind.Relative);
        Uri packUri = PackUriHelper.Create(appBase, appPath);
        return packUri;
    }

    private static Uri GetPackUri(string uri)
    {
        // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
        if (uri.StartsWith("pack://application:,,,"))
        {
            Assembly? asm = null;
            string fn = uri[22..];
            int pos = fn.IndexOf(";component/", StringComparison.Ordinal);
            if (pos >= 0)
            {
                string asmparts = fn[..pos];
                fn = fn.Substring(pos + 10);

                string? name = null;
                string? version = null;
                string? key = null;

                pos = asmparts.IndexOf(";", StringComparison.Ordinal);
                if (pos >= 0)
                {
                    name = asmparts.Substring(0, pos);
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
                    name = name[1..];
                }

                AssemblyName asmName = new AssemblyName(name);
                if (version != null)
                {
                    asmName.Version = new Version(version);
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

                    asmName.SetPublicKeyToken(keyToken);
                }

                asm = Assembly.Load(asmName);
            }

            if (!fn.StartsWith("/"))
            {
                fn = "/" + fn;
            }

            return GetLocationUri(fn, asm);
        }

        if (uri.StartsWith("pack:///siteoforigin:,,,"))
        {
            throw new ArgumentException("The siteoforigin authority is not supported: " + uri);
        }

        // If we fell all the way through to here, just hand it over to the system
        return new Uri(uri);
    }


    /// <summary>
    /// Convenience method for constructing URIs from strings.
    /// </summary>
    /// <para>This method normalizes the number of "/" characters that follow
    /// <c>file:</c> in a file URI, and promotes any <c>href</c> that begins with a
    /// slash to being a <c>file:</c> URI.</para>
    /// <param name="href">A URI or path.</param>
    /// <returns>A normalized file: URI or the string as a URI.</returns>
    public static Uri NewUri(string href)
    {
        if (href.StartsWith("file:"))
        {
            int pos = 5;
            while (pos <= href.Length && href[pos] == '/')
            {
                pos++;
            }

            if (pos > 5)
            {
                pos--;
                href = href.Substring(pos);
            }
            else
            {
                pos = 0;
                href = "/" + href.Substring(5);
            }

            return new Uri("file://" + href);
        }

        if (href.StartsWith("/"))
        {
            return new Uri("file://" + href);
        }

        return new Uri(href);
    }

    public static bool ForbidAccess(string? allowed, string uri, bool mergeHttps)
    {
        if (allowed == null || allowed.Trim() == "")
        {
            return true;
        }

        if (allowed.Trim() == "all")
        {
            return false;
        }

        // Okay, that's the easy cases taken care of, let's do the hard work
        bool sawHttp = false;
        bool sawHttps = false;

        uri = uri.ToLower();
        foreach (string value in allowed.Split(","))
        {
            String protocol = value.Trim().ToLower();
            if (protocol == "all")
            {
                return false;
            }

            if (!protocol.EndsWith(":"))
            {
                protocol += ":";
            }

            sawHttp = sawHttp || (protocol == "http:");
            sawHttps = sawHttps || (protocol == "https:");
            if (uri.StartsWith(protocol))
            {
                return false;
            }
        }

        if (mergeHttps)
        {
            if (sawHttp && !sawHttps && uri.StartsWith("https:"))
            {
                return false;
            }

            if (sawHttps && !sawHttp && uri.StartsWith("http:"))
            {
                return false;
            }
        }

        return true;
    }
}