using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.MockRepository;

public class TestLogWriter : ITestLogWriter
{
    private readonly ILogger _testLogger;

    public TestLogWriter(ILogger testLogger)
    {
        _testLogger = testLogger;
    }

    public void LogDebug(string message)
        => _testLogger.LogDebug(message);

    public void LogDebug(string message, object arg)
        => _testLogger.LogDebug(message, arg);

    public void LogError(Exception exception, string message, object arg)
        => _testLogger.LogError(exception, message, arg);
}
