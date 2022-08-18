using System;

namespace Org.XmlResolver.Utils {
    /// <summary>
    /// Performs RFC 3151 encoding and decoding of public identifier and URNs.
    /// </summary>
    public class PublicId {
        private PublicId() {
            // you can't make one of these.
        }

        /// <summary>
        /// Normalizes whitespace in a public identifier.
        /// </summary>
        /// <para>Whitespace normalization replaces tabs, new lines, and carriage returns with spaces,
        /// trims off leading and trailing whitespace, and replaces all occurrences of more than one
        /// consecutive space with a single space. Returns null if the publicId is null.</para>
        /// <param name="publicId">The public identifier.</param>
        /// <returns>The normalized identifier.</returns>
        public static string Normalize(string publicId)
        {
            if (publicId == null) {
                return null;
            }
            
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
        
        /// <summary>
        /// Encodes a public identifier.
        /// </summary>
        /// <para>Performs RFC 3151 encoding of a public identifier to yield a URN.
        /// Returns null if the publicId is null.</para>
        /// <param name="publicId">The public identifier.</param>
        /// <returns>A <c>urn:publicid:</c> URN.</returns>
        public static Uri EncodeUrn(string publicId) {
            if (publicId == null) {
                return null;
            }
            
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

        /// <summary>
        /// Decodes a <c>urn:publicid:</c> URN into a public identifier.
        /// </summary>
        /// <para>Performs RFC 3151 decoding of a URN to yield a public identifier.
        /// Returns null if the urn is null.</para>
        /// <para>If the specified <c>urn</c> does not appear to be a
        /// <c>urn:publicid:</c> URN, it is returned unchanged.</para>
        /// <param name="urn">The <c>urn:publicid:</c> URN.</param>
        /// <returns>The public identifier.</returns>
        public static string DecodeUrn(string urn) {
            if (urn == null) {
                return null;
            }

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