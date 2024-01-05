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
        // 1. Plain sample
        await PlainSample();

        // 2. Console output
        await RunConsoleSample();

        // 3. Logging output
        using var loggerFactory = CreateLoggerFactory();
        await RunLoggingSample(loggerFactory.CreateLogger<Program>());

        // 4. real life usage scenarios
        await new AlmostRealLifeService(loggerFactory.CreateLogger<AlmostRealLifeService>()).HandleMessage();
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
        .Create(lb => lb.SetMinimumLevel(LogLevel.Trace).AddConsole(cfg => { }));

    private static async Task MyTimeConsumingWork() => await Task.Delay(TimeSpan.FromMilliseconds(374));
}