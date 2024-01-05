using Microsoft.Extensions.Logging;

namespace PicoProfiler.Logging;

public class LoggerOutputConfiguration
{
    public static LoggerOutputConfiguration Instance = new ();

    public LogLevel DefaultLogLevel { get; set; }= LogLevel.Information;

    public LoggerMessageFactoryWithActionName DefaultActionMessageFactory = 
        (actionName, time) => ("{ActionName} finished in {ElapsedMilliseconds:.##} ms",
            new object[] { actionName, time.TotalMilliseconds });

    private LoggerOutputConfiguration()
    {
    }
}