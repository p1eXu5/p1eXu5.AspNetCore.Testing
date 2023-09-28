using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

[ProviderAlias("Test")]
public class TestLoggerProvider : ILoggerProvider
{
    private readonly Func<string, LogLevel, bool>? _filter;
    private readonly ITestContextWriters _testContext;
    private readonly LogOut _logOut;

    public TestLoggerProvider(ITestContextWriters testContext, LogOut logOut)
    {
        _filter = null;
        _testContext = testContext;
        _logOut = logOut;
    }

    public TestLoggerProvider(ITestContextWriters testContext)
        : this(testContext, LogOut.Out | LogOut.Progress)
    {
    }

    public ILogger CreateLogger(string name)
    {
        return new TestLogger(_testContext, name, _filter, _logOut);
    }

    public void Dispose()
    {
    }

    public static ILogger CreateLogger(TextWriter? progress, TextWriter? @out, string name)
    {
        return
            new TestLoggerProvider(new TestContextWriters { Progress = progress, Out = @out })
                .CreateLogger(name);
    }
}