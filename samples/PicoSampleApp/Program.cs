using PicoProfiler.ConsoleOutput;

namespace PicoSampleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        await RunConsoleSample();
    }

    private static async Task RunConsoleSample()
    {
        using var _ = PicoProfilerConsoleOutput.Start();
        await MyTimeConsumingWork();
    }

    private static async Task MyTimeConsumingWork() => await Task.Delay(TimeSpan.FromMilliseconds(374));
}