using System.Diagnostics;
using Microsoft.Extensions.Logging;
using PicoProfiler;
using PicoProfiler.ConsoleOutput;
using PicoProfiler.Logging;

namespace PicoSampleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        await PlainSample();

        await RunConsoleSample();

        using var loggerFactory = CreateLoggerFactory();
        await RunLoggingSample(loggerFactory.CreateLogger<Program>());
    }

    private static async Task PlainSample()
    {
        Profiler.Start(elapsed => Debug.WriteLine($"Elapsed time is: {elapsed.TotalMilliseconds:.##}"));
        await MyTimeConsumingWork();
    }

    private static async Task RunConsoleSample()
    {
        using var _ = PicoProfilerConsoleOutput.Start();
        await MyTimeConsumingWork();
    }

    private static async Task RunLoggingSample(ILogger logger)
    {
        using var _ = logger.StartProfiler();
        await MyTimeConsumingWork();
    }

    private static ILoggerFactory CreateLoggerFactory() => LoggerFactory
        .Create(lb => lb.AddConsole(cfg => { }));

    private static async Task MyTimeConsumingWork() => await Task.Delay(TimeSpan.FromMilliseconds(374));
}