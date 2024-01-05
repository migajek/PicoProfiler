using Microsoft.Extensions.Logging;

namespace PicoProfiler.Logging;

public class PicoProfilerLoggingConfiguration
{
    public static PicoProfilerLoggingConfiguration Instance = new ();

    public LogLevel DefaultLogLevel { get; set; }= LogLevel.Information;

    public LoggerMessageFactoryWithActionName DefaultActionMessageFactory = 
        (actionName, time) => ("{ActionName} finished in {ElapsedMilliseconds:.##} ms",
            new object[] { actionName, time.TotalMilliseconds });

    private PicoProfilerLoggingConfiguration()
    {
    }
}