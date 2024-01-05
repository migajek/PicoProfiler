using Microsoft.Extensions.Logging;

namespace PicoProfiler.Tests.TestLogger;

internal class TestLoggerFixture
{
    private readonly TestLoggerProvider _provider = new();
    private ILoggerFactory _factory = null!;

    public IReadOnlyCollection<TestLoggerProvider.LogEntry> LogEntries
    {
        get
        {
            // disposing of factory is the only way to flush.
            RecreateFactory();
            return _provider.LogEntries.AsReadOnly();
        }
    }

    public TestLoggerFixture()
    {
        RecreateFactory();
    }

    private void RecreateFactory()
    {
        _factory?.Dispose();
        _factory = LoggerFactory.Create(x => x.SetMinimumLevel(LogLevel.Trace).AddProvider(_provider));
    }

    public ILogger CreateLogger() => _factory.CreateLogger<TestLoggerFixture>();
}