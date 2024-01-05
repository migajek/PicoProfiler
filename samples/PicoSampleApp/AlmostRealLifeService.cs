using Microsoft.Extensions.Logging;
using PicoProfiler.Logging;

namespace PicoSampleApp;

internal class AlmostRealLifeService
{
    private readonly ILogger<AlmostRealLifeService> _logger;

    public AlmostRealLifeService(ILogger<AlmostRealLifeService> logger)
    {
        _logger = logger;
    }

    public async Task HandleMessage()
    {
        var data = await FetchHelperData();
        await ProcessRules(data);
    }

    private async Task<object[]> FetchHelperData()
    {
        using var _ = _logger.StartProfiler();
        await Task.Delay(283); // fair roll dice, as you know it.
        
        var results = Enumerable.Range(0, 3).Cast<object>().ToArray();
        _logger.LogInformation("Returning {HelperDataCount} results", results.Length);
        return results;
    }

    private async Task ProcessRules(object[] objects)
    {
        using var _ = _logger.StartProfiler();

        foreach (var o in objects)
        {
            using (_logger.StartProfiler($"Processing rule {o}", logLevel: LogLevel.Trace))
            {
                await Task.Delay(15);
            }
        }
    }
}