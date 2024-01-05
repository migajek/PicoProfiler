using Microsoft.Extensions.Logging;
using PicoProfiler.Logging;
using PicoProfiler.Tests.TestLogger;

namespace PicoProfiler.Tests;

public class PicoProfilerLoggerTests
{
    [Fact]
    public void Outputs_To_Log()
    {
        var fixture = new TestLoggerFixture();
        
        var logger = fixture.CreateLogger();
        using (logger.StartProfiler())
        {
        }

        Assert.Equal(1, fixture.LogEntries.Count);
    }

    [Fact]
    public void Uses_Provided_LogLevel()
    {
        var fixture = new TestLoggerFixture();

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler(logLevel: LogLevel.Debug))
        {
        }

        Assert.Equal(LogLevel.Debug, fixture.LogEntries.Single().Level);
    }

    [Fact]
    public void Uses_Default_ActionName()
    {
        var fixture = new TestLoggerFixture();

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler())
        {
        }

        Assert.Contains(nameof(Uses_Default_ActionName), fixture.LogEntries.Single().Message);
    }

    [Fact]
    public void Uses_Provided_ActionName()
    {
        var fixture = new TestLoggerFixture();

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler("fetching"))
        {
        }

        Assert.Contains("fetching", fixture.LogEntries.Single().Message);
    }

    [Fact]
    public void Uses_Configured_ActionName()
    {
        var fixture = new TestLoggerFixture();
        LoggerOutputConfiguration.Instance.DefaultLogLevel = LogLevel.Critical;

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler())
        {
        }

        Assert.Equal(LogLevel.Critical, fixture.LogEntries.Single().Level);
    }

    [Fact]
    public void Uses_Default_Message()
    {
        var fixture = new TestLoggerFixture();

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler())
        {
        }

        Assert.StartsWith($"{nameof(Uses_Default_Message)} finished in", fixture.LogEntries.Single().Message);
    }

    [Fact]
    public void Uses_Provided_Message()
    {
        var fixture = new TestLoggerFixture();

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler(messageFactory: (name, time) => ("action {act} done!", new object[]{name})))
        {
        }

        Assert.Equal($"action {nameof(Uses_Provided_Message)} done!", fixture.LogEntries.Single().Message);
    }

    [Fact]
    public void Uses_Configured_Message()
    {
        var fixture = new TestLoggerFixture();
        LoggerOutputConfiguration.Instance.DefaultActionMessageFactory =
            (name, time) => ("action {act} done!", new object[] { name });

        var logger = fixture.CreateLogger();
        using (logger.StartProfiler())
        {
        }

        Assert.Equal($"action {nameof(Uses_Configured_Message)} done!", fixture.LogEntries.Single().Message);
    }
}