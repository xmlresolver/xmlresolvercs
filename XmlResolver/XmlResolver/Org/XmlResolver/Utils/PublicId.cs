using System;

namespace Org.XmlResolver.Utils {
    public class PublicId {
        private PublicId() {
            // you can't make one of these.
        }

        public static string Normalize(string publicId) {
            string normal = publicId.Replace('\t', ' ');
            normal = normal.Replace('\r', ' ');
            normal = normal.Replace('\n', ' ');
            normal = normal.Trim();

            int pos;

            while ((pos = normal.IndexOf("  ")) >= 0) {
                normal = normal.Substring(0, pos) + normal.Substring(pos+1);
            }

            return normal;
       }
        
        
        public static Uri EncodeUrn(string publicId) {
            string urn = PublicId.Normalize(publicId)
                .Replace("%", "%25")
                .Replace(";", "%3B")
                .Replace("'", "%27")
                .Replace("?", "%3F")
                .Replace("#", "%23")
                .Replace("+", "%2B")
                .Replace(" ", "+")
                .Replace("::", ";")
                .Replace(":", "%3A")
                .Replace("//", ":")
                .Replace("/", "%2F");

            return new Uri("urn:publicid:" + urn);
        }

        public static string DecodeUrn(string urn) {
            if (urn.StartsWith("urn:publicid:")) {
                string publicId = urn.Substring(13)
                    .Replace("%2F", "/")
                    .Replace(":", "//")
                    .Replace("%3A", ":")
                    .Replace(";", "::")
                    .Replace("+", " ")
                    .Replace("%2B", "+")
                    .Replace("%23", "#")
                    .Replace("%3F", "?")
                    .Replace("%27", "'")
                    .Replace("%3B", ";")
                    .Replace("%25", "%");
                return publicId;
            } else {
                return urn;
            }
        }
    }
}