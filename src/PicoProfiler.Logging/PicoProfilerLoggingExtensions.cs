using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace PicoProfiler.Logging;

public static class PicoProfilerLoggingExtensions
{
    /// <summary>
    /// Creates a profiler that will output the message to logger when finished
    /// </summary>
    /// <param name="logger">logger instance to output message to</param>
    /// <param name="messageFactory">returns a message template and parameters for logging</param>
    /// <param name="logLevel">log level to use. When null, uses PicoProfilerLoggingConfiguration.Instance.DefaultLogLevel</param>
    /// <returns></returns>
    public static IPicoProfiler CreateProfiler(this ILogger logger, LoggerMessageFactory messageFactory,
        LogLevel? logLevel = null)
    {
        // evaluate it now as config might change later.
        var level = logLevel ?? PicoProfilerLoggingConfiguration.Instance.DefaultLogLevel;

        return Profiler.Create(elapsed =>
        {
            var result = messageFactory(elapsed);
            logger.Log(level, result.messageTemplate, result.parameters);
        });
    }

    /// <summary>
    /// Creates and starts a profiler that will output the message to logger when finished
    /// </summary>
    /// <param name="logger">logger instance to output message to</param>
    /// <param name="messageFactory">returns a message template and parameters</param>
    /// <param name="logLevel">log level to use. When null, uses PicoProfilerLoggingConfiguration.Instance.DefaultLogLevel</param>
    /// <returns></returns>
    public static IPicoProfiler StartProfiler(this ILogger logger, LoggerMessageFactory messageFactory,
        LogLevel? logLevel = null)
    {
        var profiler = CreateProfiler(logger, messageFactory, logLevel);
        profiler.Start();
        return profiler;
    }

    /// <summary>
    /// Creates a logger that will output the message in format "{ActionName} finished in {ElapsedMilliseconds} ms" to the logger.
    /// Message can be adjusted either with providing custom messageFactory or setting default in configuration
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="actionName"></param>
    /// <param name="messageFactory"></param>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public static IPicoProfiler CreateProfiler(this ILogger logger,
        [CallerMemberName]
        string? actionName = null,
        LoggerMessageFactoryWithActionName? messageFactory = null,
        LogLevel? logLevel = null)
    {
        
        var action = messageFactory ?? PicoProfilerLoggingConfiguration.Instance.DefaultActionMessageFactory;
        return CreateProfiler(logger,
            ts => action(actionName, ts),
            logLevel);
    }

    /// <summary>
    /// Creates and starts a logger that will output the message in format "{ActionName} finished in {ElapsedMilliseconds} ms" to the logger.
    /// Message can be adjusted either with providing custom messageFactory or setting default in configuration
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="actionName"></param>
    /// <param name="messageFactory"></param>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public static IPicoProfiler StartProfiler(this ILogger logger,
        [CallerMemberName]
        string? actionName = null,
        LoggerMessageFactoryWithActionName? messageFactory = null,
        LogLevel? logLevel = null)
    {
        var profiler = CreateProfiler(logger, actionName, messageFactory, logLevel);
        profiler.Start();
        return profiler;
    }

}