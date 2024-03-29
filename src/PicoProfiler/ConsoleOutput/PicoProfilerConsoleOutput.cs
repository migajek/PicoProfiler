﻿using System.Runtime.CompilerServices;

namespace PicoProfiler.ConsoleOutput;

public static class PicoProfilerConsoleOutput
{
    public static IPicoProfiler Create([CallerMemberName] string actionName = null,
        ConsoleMessageFactoryWithActionName? messageFactory = null)
    {
        var factory = messageFactory ?? ConsoleOutputConfiguration.Instance.DefaultMessageFactory;

        return Profiler.Create(time => Console.WriteLine(factory(actionName, time)));
    }

    public static IPicoProfiler Start([CallerMemberName] string actionName = null,
        ConsoleMessageFactoryWithActionName? messageFactory = null)
    {
        var profiler = Create(actionName, messageFactory);
        profiler.Start();
        return profiler;
    }
}