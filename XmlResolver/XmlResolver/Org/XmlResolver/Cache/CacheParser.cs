using System;
using System.Text.RegularExpressions;
using NLog;

namespace Org.XmlResolver.Cache {
    public class CacheParser {
        protected static ResolverLogger logger = new(NLog.LogManager.GetCurrentClassLogger());

        private static readonly Regex sizeK = new Regex("^[0-9]+k$", RegexOptions.IgnoreCase);
        private static readonly Regex sizeM = new Regex("^[0-9]+m$", RegexOptions.IgnoreCase);
        private static readonly Regex sizeG = new Regex("^[0-9]+g$", RegexOptions.IgnoreCase);
        private static readonly Regex timeS = new Regex("^[0-9]+s$", RegexOptions.IgnoreCase);
        private static readonly Regex timeM = sizeM; // same
        private static readonly Regex timeH = new Regex("^[0-9]+h$", RegexOptions.IgnoreCase);
        private static readonly Regex timeD = new Regex("^[0-9]+d$", RegexOptions.IgnoreCase);
        private static readonly Regex timeW = new Regex("^[0-9]+w$", RegexOptions.IgnoreCase);

        public static long ParseLong(string longStr, long defVal) {
            if (longStr == null) {
                return defVal;
            }

            try {
                return Convert.ToInt64(longStr);
            }
            catch (Exception) {
                logger.Log(ResolverLogger.ERROR, "Bad numeric value: {0}", longStr);
                return defVal;
            }
        }

        public static long ParseLong(string longStr, long units, long defVal) {
            try {
                long val = Convert.ToInt64(longStr);
                return val * units;
            } catch (Exception) {
                logger.Log(ResolverLogger.ERROR, "Bad numeric value: {0}", longStr);
                return defVal;
            }
        }

        public static long ParseSizeLong(String longStr, long defVal) {
            if (longStr == null) {
                return defVal;
            }

            long units = 1;
            if (sizeK.IsMatch(longStr)) {
                units = 1024;
            } else if (sizeM.IsMatch(longStr)) {
                units = 1024 * 1000;
            } else if (sizeG.IsMatch(longStr)) {
                units = 1024 * 1000 * 1000;
            }

            if (units == 1) {
                return ParseLong(longStr, units, defVal);
            }

            return ParseLong(longStr.Substring(0, longStr.Length - 1), units, defVal);
        }

        public static long ParseTimeLong(string longStr, long defVal) {
            if (longStr == null) {
                return defVal;
            }

            long units = 1;
            if (timeS.IsMatch(longStr)) {
                longStr = longStr.Substring(0, longStr.Length - 1);
            } else if (timeM.IsMatch(longStr)) {
                units = 60;
            } else if (timeH.IsMatch(longStr)) {
                units = 60 * 60;
            } else if (timeD.IsMatch(longStr)) {
                units = 60 * 60 * 24;
            } else if (timeW.IsMatch(longStr)) {
                units = 60 * 60 * 24 * 7;
            }

            if (units == 1) {
                return ParseLong(longStr, units, defVal);
            }

            return ParseLong(longStr.Substring(0, longStr.Length - 1), units, defVal);
        }
    }
}