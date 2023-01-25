using System.Collections.Generic;
using System.Text;
using NLog;

namespace Org.XmlResolver {
    /// <summary>
    /// A wrapper around an underlying logger.
    /// </summary>
    /// <para>This wrapper allows the resolver to be configured so that it will log
    /// different classes of information at different levels.</para>
    public class ResolverLogger {
        public const string REQUEST = "request";
        public const string RESPONSE = "response";
        public const string TRACE = "trace";
        public const string ERROR = "error";
        public const string CACHE = "cache";
        public const string CONFIG = "config";
        public const string WARNING = "warning";

        const int DEBUG = 1;
        const int INFO = 2;
        const int WARN = 3;

        private readonly Logger _logger;
        private readonly Dictionary<string, int> categories = new();
        private string _catalogLogging = null;

        /// <summary>
        /// Create a new <c>ResolverLogger</c> around the specified logger.
        /// </summary>
        /// <param name="logger">The underlying logger.</param>
        public ResolverLogger(Logger logger) {
            _logger = logger;
        }

        public static string CatalogLogging { get; set; }
         
        public string GetCategory(string cat) {
            if (categories.ContainsKey(cat)) {
                if (INFO.Equals(categories[cat])) {
                    return "info";
                } else if (WARN.Equals(categories[cat])) {
                    return "warn";
                }
            }
            return "debug";
        }

        public void SetCategory(string cat, string level) {
            if ("info".Equals(level)) {
                categories.Add(cat, INFO);
            } else if ("warn".Equals(level)) {
                categories.Add(cat, WARN);
            } else {
                categories.Add(cat, DEBUG);
                if (!"debug".Equals(level)) {
                    _logger.Info(string.Format("Incorrect logging level specified: {0} treated as debug", level));
                }
            }
        }

        public void Log(string cat, string message, params string[] args) {
            updateLoggingCategories();

            StringBuilder sb = new();
            sb.Append(cat);
            sb.Append(":");

            if (args.Length == 0) {
                sb.Append(message);
            } else {
                sb.Append(string.Format(message, args));
            }

            var defLevel = categories.GetValueOrDefault("*", DEBUG);
            var level = categories.GetValueOrDefault(cat, defLevel);

            switch (level) {
                case WARN:
                    _logger.Warn(sb.ToString());
                    break;
                case INFO:
                    _logger.Info(sb.ToString());
                    break;
                default:
                    _logger.Debug(sb.ToString());
                    break;
            }
        }

        private void updateLoggingCategories() {
            if (CatalogLogging == null && _catalogLogging == null) {
                return;
            }

            if (CatalogLogging == null) {
                categories.Clear();
                return;
            }

            if (CatalogLogging.Equals(_catalogLogging)) {
                return;
            }

            _catalogLogging = CatalogLogging;
            categories.Clear();
            foreach (var prop in _catalogLogging.Split(",")) {
                int pos = prop.IndexOf(":");
                if (pos > 0) {
                    var cat = prop.Substring(0, pos).Trim();
                    var level = prop.Substring(pos + 1).Trim();
                    SetCategory(cat, level);
                }
            }
        }
    }
}