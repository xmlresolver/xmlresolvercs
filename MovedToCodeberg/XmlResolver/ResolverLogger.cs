using NLog;

namespace XmlResolver;

/// <summary>
/// A wrapper around an underlying logger.
/// </summary>
/// <para>This wrapper allows the resolver to be configured so that it will log
/// different classes of information at different levels.</para>
public class ResolverLogger
{
    private readonly Logger _logger;

    /// <summary>
    /// Create a new <c>ResolverLogger</c> around the specified logger.
    /// </summary>
    /// <param name="logger">The underlying logger.</param>
    public ResolverLogger(Logger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// An error message.
    /// </summary>
    /// <param name="message">The message, as a format string</param>
    /// <param name="args">Format string arguments</param>
    public void Error(string message, params object[] args)
    {
        if (args.Length == 0)
        {
            _logger.Error(message);
        }
        else
        {
            _logger.Error(message, string.Format(message, args));
        }
    }

    /// <summary>
    /// A warning message.
    /// </summary>
    /// <param name="message">The message, as a format string</param>
    /// <param name="args">Format string arguments</param>
    public void Warn(string message, params object[] args)
    {
        if (args.Length == 0)
        {
            _logger.Warn(message);
        }
        else
        {
            _logger.Warn(message, string.Format(message, args));
        }
    }

    /// <summary>
    /// An informational message.
    /// </summary>
    /// <param name="message">The message, as a format string</param>
    /// <param name="args">Format string arguments</param>
    public void Info(string message, params object[] args)
    {
        if (args.Length == 0)
        {
            _logger.Info(message);
        }
        else
        {
            _logger.Info(message, string.Format(message, args));
        }
    }

    /// <summary>
    /// A debug message.
    /// </summary>
    /// <param name="message">The message, as a format string</param>
    /// <param name="args">Format string arguments</param>
    public void Debug(string message, params object[] args)
    {
        if (args.Length == 0)
        {
            _logger.Debug(message);
        }
        else
        {
            _logger.Debug(message, string.Format(message, args));
        }
    }
}
