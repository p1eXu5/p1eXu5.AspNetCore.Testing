namespace p1eXu5.AspNetCore.Testing.MockRepository;

public interface ITestLogWriter
{
    void LogDebug(string message);

    void LogDebug(string message, object arg);

    void LogWarning(string message);

    void LogWarning(string message, object arg);

    void LogError(Exception exception, string message, object arg);
}