using Microsoft.Extensions.Logging;

namespace PicoProfiler.Tests.TestLogger;

internal class TestLogger : ILogger
{
    private readonly TestLoggerProvider _provider;

    public TestLogger(TestLoggerProvider provider)
    {
        _provider = provider;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _provider.Add(logLevel, formatter(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => default;
}