namespace PicoProfiler.Logging;

public delegate (string messageTemplate, object[] parameters) LoggerMessageFactoryWithActionName(string actionName, TimeSpan elapsedTime);