using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

#if NETSTANDARD2_0
namespace XmlResolver.Utils
{
    internal static class NetStandardExtensions
    {
        public static int IndexOf(this string s, char value, StringComparison comparisonType)
        {
            return s.IndexOf($"{value}", comparisonType);
        }

        public static Span<string> Split(this string s, string sep, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        {
            char[] sepChars = [.. s];
            return s.Split(sepChars, stringSplitOptions);
        }

        public static bool StartsWith(this string s, char c)
        {
            if (s.Length > 0)
            {
                if (s[0] == c)
                {
                    return true;
                }
            }
            return false;
        }

        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage message)
        {
            return client.SendAsync(message).Result;
        }

        public static Stream ReadAsStream(this HttpContent content)
        {
            return content.ReadAsStreamAsync().Result;
        }
    }
}
#endif