using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

public class TestLoggerFactory : ILoggerFactory
{
    private ILoggerProvider _loggerProvider;

    public TestLoggerFactory(ITestContextWriters testContextWriters, LogOut logOut = LogOut.All)
    {
        _loggerProvider = new TestLoggerProvider(testContextWriters, logOut);
    }

    public static ILoggerFactory Create<TTestContext>()
        => new TestLoggerFactory(TestContextWriters.GetInstance<TTestContext>());

    public void AddProvider(ILoggerProvider provider)
    {
        _loggerProvider = provider;
    }

    public ILogger CreateLogger(string categoryName)
        => _loggerProvider.CreateLogger(categoryName);

    public void Dispose() => _loggerProvider.Dispose();
}
