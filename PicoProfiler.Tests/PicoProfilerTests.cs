
using FluentAssertions;

namespace PicoProfiler.Tests;

public class PicoProfilerTests
{
    private static readonly TimeSpan DelayTime = TimeSpan.FromMilliseconds(50);

    [Fact]
    public async Task Calls_End_Action_When_Disposed()
    {
        TimeSpan? elapsed = null;
        
        // ReSharper disable once ConvertToUsingDeclaration
        using (var _ = Profiler.Start(ts => elapsed = ts))
        {
            await Task.Delay(DelayTime);
        }

        Assert.NotNull(elapsed);
        Assert.True(elapsed >= DelayTime);
    }

    [Fact]
    public async Task Creates_Non_Started_Instance()
    {
        var profiler = Profiler.Create();
        
        await Task.Delay(DelayTime);

        Assert.Equal(TimeSpan.Zero, profiler.ElapsedTime);
    }

    [Fact]
    public async Task Counts_Time_When_Started()
    {
        using var profiler = Profiler.Start();

        await Task.Delay(DelayTime);

        Assert.True(profiler.ElapsedTime >= DelayTime);
    }

    [Fact]
    public async Task Stops_Measuring_When_Stopped_Then_Resumes_When_Started()
    {
        using var profiler = Profiler.Start();
        // this will be counted
        await Task.Delay(DelayTime);
        profiler.Pause();

        // this will not be counted
        await Task.Delay(DelayTime);

        profiler.Start();
        //this will be counted again
        await Task.Delay(DelayTime);
        profiler.Finish();
        
        profiler.ElapsedTime.Should().BeGreaterThanOrEqualTo(2 * DelayTime);
        profiler.ElapsedTime.Should().BeLessThan(3 * DelayTime);
    }
} 