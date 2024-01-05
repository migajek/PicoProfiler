namespace PicoProfiler.ConsoleOutput;

public sealed class ConsoleOutputConfiguration
{
    public static ConsoleOutputConfiguration Instance = new();

    private ConsoleOutputConfiguration()
    {
    }

    public ConsoleMessageFactoryWithActionName DefaultMessageFactory { get; set; }
        = (actionName, elapsedTime) => "";
}