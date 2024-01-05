namespace PicoProfiler;

/// <summary>
/// Surface API for controlling Profiler
/// </summary>
public interface IPicoProfiler: IDisposable
{
    /// <summary>
    /// Starts or continues tracking
    /// </summary>
    void Start();

    /// <summary>
    /// Pauses tracking
    /// </summary>
    void Pause();

    /// <summary>
    /// Finishes tracking and causes the EndAction to be called
    /// </summary>
    void Finish();

    /// <summary>
    /// Sum of elapsed time
    /// </summary>
    TimeSpan ElapsedTime { get; }
}