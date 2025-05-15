namespace p1eXu5.AspNetCore.Testing.Logging;

public interface ITestTextWriter : IDisposable
{
    void WriteLine(string message);
}

