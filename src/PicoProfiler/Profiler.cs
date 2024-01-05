using System.Diagnostics;

namespace PicoProfiler;

internal class ProfilerImpl: IPicoProfiler
{
    private readonly Stopwatch _stopwatch = new();

    private readonly ProfilerEndAction? _endAction;

    public ProfilerImpl(ProfilerEndAction? endAction)
    {
        _endAction = endAction;
    }

    public void Dispose()
    {
        Finish();
    }

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Pause()
    {
        _stopwatch.Stop();
    }

    public void Finish()
    {
        _stopwatch.Stop();
        _endAction?.Invoke(ElapsedTime);
    }

    public TimeSpan ElapsedTime => _stopwatch.Elapsed;
}