namespace PicoProfiler;

public static class Profiler
{
    /// <summary>
    /// Creates a new instance of Profiler, but doesn't start it
    /// </summary>
    /// <param name="endAction">Action that will be called when profiler Finishes tracking</param>
    /// <returns></returns>
    public static IPicoProfiler Create(ProfilerEndAction? endAction = null) => new ProfilerImpl(endAction);

    /// <summary>
    /// Creates and starts a new Profiler
    /// </summary>
    /// <param name="endAction">Action that will be called when profiler Finishes tracking</param>
    /// <returns></returns>
    public static IPicoProfiler Start(ProfilerEndAction? endAction = null)
    {
        var profiler = Create(endAction);
        profiler.Start();
        return profiler;
    }
}