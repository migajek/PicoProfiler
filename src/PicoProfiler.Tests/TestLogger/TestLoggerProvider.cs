using Microsoft.Extensions.Logging;

namespace PicoProfiler.Tests.TestLogger;

internal class TestLoggerProvider : ILoggerProvider
{
    public record struct LogEntry(LogLevel Level, string Message);

    public List<LogEntry> LogEntries { get; } = new();

    public void Add(LogLevel level, string message) => LogEntries.Add(new LogEntry(level, message));

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(this);
    }
}