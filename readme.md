# PicoProfiler

PicoProfiler is a tiny abstraction layer over built-in `Stopwatch`, leveraging `IDisposable + using` pattern

It's intended use is to measure the time of execution of given block of code and store it somewhere - usually the log entry, hence the built-in integration with `Microsoft.Extensions.Logging`.

Just wrap the code you want to measure in `using (_logger.StartProfiler())` (or use `using declaration` as below):
```c#
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
```

results in:
```
trce: PicoSampleApp.AlmostRealLifeService[0]
      Processing rule 0 finished in 28.01 ms
trce: PicoSampleApp.AlmostRealLifeService[0]
      Processing rule 1 finished in 15.26 ms
trce: PicoSampleApp.AlmostRealLifeService[0]
      Processing rule 2 finished in 16.06 ms
info: PicoSampleApp.AlmostRealLifeService[0]
      ProcessRules finished in 59.74 ms
```

## Getting started

### Microsoft.Extensions.Logging integration

1. Install package: `dotnet add package PicoProfiler.Logging`

2. Sample usage:
```
 static async Task Main(string[] args)
 {
     using var loggerFactory = CreateLoggerFactory();
     await RunLoggingSample(loggerFactory.CreateLogger<Program>());
 }

 private static async Task RunLoggingSample(ILogger logger)
 {
     using var _ = logger.StartProfiler();
     await MyTimeConsumingWork();
 }

 private static ILoggerFactory CreateLoggerFactory() => LoggerFactory
     .Create(lb => lb.AddConsole(cfg => { }));

 private static async Task MyTimeConsumingWork() => await Task.Delay(TimeSpan.FromMilliseconds(374));
```

3. In the console output, you'll see :
```
info: PicoSampleApp.Program[0]
      RunLoggingSample finished in 381.37 ms
```

#### Notes on usage

Action name can be provided. When not provided, `CallerMemberName` is used - hence the *RunLoggingSample* name is outputted

Default log level and default message template can be adjusted with `LoggerOutputConfiguration.Instance`.

It is also possible to override the log level and message template for each call (see parameters of the methods and overrides).


### Console output

1. Install package: `dotnet add package PicoProfiler`
2. Sample usage code:
```
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
```

3. You'll end up with the following console output: `RunConsoleSample finished in 389,55 ms`


#### Notes on usage

Action name can be provided. When not provided, `CallerMemberName` is used - hence the *RunConsoleSample* name is outputted.

Default message template can be adjusted in `ConsoleOutputConfiguration.Instance`

It is also possible to provide a per-call template (in fact, a message factory method)


### Plain Profiler with no output

The core profiler does not output anywhere - it just calls the provided lambda when finishing (when disposed, it calls `Finish`).

The Console output and Logging output is just a bunch of factory methods that provide an action to format & output message.

The following code will create a plain profiler and call the provided action at the end of the method, outputting to Debug Output
```
private static async Task PlainSample()
{
    Profiler.Start(elapsed => Debug.WriteLine($"Elapsed time is: {elapsed.TotalMilliseconds:.##}"));
    await MyTimeConsumingWork();
}
```

### Profiler API and advanced usages

`Profiler` is a static factory class. 
 * `Create` - creates a new Pico Profiler and returns `IPicoProfiler`. The profiler is not started until you start it explicitly!
 * `Start` - creates *and starts* new Pico Profiler, returns `IPicoProfiler`
 * both factory methods can take an optional action to be called when profiler finishes (either manually by calling `Finish` on the instance, or when it's being disposed) 

* `IPicoProfiler` is an interface to interact with profiler instance. It also implements `IDisposable` so the `using (Profiler.Start(..))` pattern can be leveraged to measure the performance of code block.
 - when profiler instance is being disposed, it calls `Finish` on itself
 - `Start()` starts or resumes the current profiler stopwatch
 - `Pause()` pauses the current profiler stopwatch
 - `Finish()` stops the current profiler stopwatch and runs the action provided (if any)

## Chanelog

 * 0.2.2 fixed default float formatting for displaying elapsed miliseconds
 * 0.2.1 first public release