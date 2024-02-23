using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

public class TestLoggerFactory : ILoggerFactory
{
    private readonly ITestContextWriters _testContextWriters;
    private readonly LogOut _logOut;

    public TestLoggerFactory(ITestContextWriters testContextWriters, LogOut logOut = LogOut.All)
    {
        _testContextWriters = testContextWriters;
        _logOut = logOut;
    }

    public static ILoggerFactory CreateWith(TextWriter? progressWriter, TextWriter? outWriter)
        => new TestLoggerFactory(TestContextWriters.DefaultWith(progressWriter, outWriter));

    public void AddProvider(ILoggerProvider provider)
    {
    }

    public ILogger CreateLogger(string categoryName)
        => new TestLogger(_testContextWriters, categoryName, _logOut);

    public void Dispose()
    {
    }
}
