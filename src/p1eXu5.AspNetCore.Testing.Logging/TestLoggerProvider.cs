using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

[ProviderAlias("Test")]
public class TestLoggerProvider : ILoggerProvider
{
    private readonly Func<string, LogLevel, bool>? _filter;
    private readonly ITestContext _testContext;

    public TestLoggerProvider(ITestContext testContext)
    {
        _filter = null;
        _testContext = testContext;
    }

    public ILogger CreateLogger(string name)
    {
        return new TestLogger(_testContext, name, _filter);
    }

    public void Dispose()
    {
    }
}