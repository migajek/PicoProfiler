namespace PicoProfiler.Logging;

public delegate (string messageTemplate, object[] parameters) LoggerMessageFactory(TimeSpan elapsedTime);