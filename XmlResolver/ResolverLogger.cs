using System.Text;
using NLog;

namespace XmlResolver;

/// <summary>
/// A wrapper around the NLog logger.
/// </summary>
/// <para>The XML Resolver logger can be configured to trace various aspects of the resolution
/// process.</para>
public class ResolverLogger
{
    public const string Request = "request";
    public const string Response = "response";
    public const string Trace = "trace";
    public const string Error = "error";
    public const string Config = "config";
    public const string Warning = "warning";

    private const int Debug = 1;
    private const int Info = 2;
    private const int Warn = 3;

    private readonly Logger _logger;
    private readonly Dictionary<string, int> _categories = new();
    private string? _catalogLogging = null;

    /// <summary>
    /// Create a new <c>ResolverLogger</c> around the specified logger.
    /// </summary>
    /// <param name="logger">The underlying logger.</param>
    public ResolverLogger(Logger logger) {
        _logger = logger;
    }

    /// <summary>
    /// Get or set the logging category.
    /// </summary>
    public static string? CatalogLogging { get; set; }
         
    public string GetCategory(string cat) {
        if (_categories.ContainsKey(cat)) {
            if (Info.Equals(_categories[cat])) {
                return "info";
            } else if (Warn.Equals(_categories[cat])) {
                return "warn";
            }
        }
        return "debug";
    }

    public void SetCategory(string cat, string level) {
        if ("info".Equals(level)) {
            _categories.Add(cat, Info);
        } else if ("warn".Equals(level)) {
            _categories.Add(cat, Warn);
        } else {
            _categories.Add(cat, Debug);
            if (!"debug".Equals(level)) {
                _logger.Info($"Incorrect logging level specified: {level} treated as debug");
            }
        }
    }

    public void Log(string cat, string message, params string[] args) {
        UpdateLoggingCategories();

        StringBuilder sb = new();
        sb.Append(cat);
        sb.Append(':');

        sb.Append(args.Length == 0 ? message : string.Format(message, args));

        var defLevel = _categories.GetValueOrDefault("*", Debug);
        var level = _categories.GetValueOrDefault(cat, defLevel);

        switch (level) {
            case Warn:
                _logger.Warn(sb.ToString());
                break;
            case Info:
                _logger.Info(sb.ToString());
                break;
            default:
                _logger.Debug(sb.ToString());
                break;
        }
    }

    private void UpdateLoggingCategories() {
        if (CatalogLogging == null && _catalogLogging == null) {
            return;
        }

        if (CatalogLogging == null) {
            _categories.Clear();
            return;
        }

        if (CatalogLogging.Equals(_catalogLogging)) {
            return;
        }

        _catalogLogging = CatalogLogging;
        _categories.Clear();
        foreach (var prop in _catalogLogging.Split(",")) {
            int pos = prop.IndexOf(":", StringComparison.Ordinal);
            if (pos > 0) {
                var cat = prop[..pos].Trim();
                var level = prop[(pos + 1)..].Trim();
                SetCategory(cat, level);
            }
        }
    }
}