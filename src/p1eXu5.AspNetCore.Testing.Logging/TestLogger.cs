using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

[Flags]
public enum LogOut
{
    NotSet = 0,
    Progress = 0x0001,
    Out = 0x0010,
}

public class TestLogger : ILogger
{
    private readonly ITestContextWriters _testContext;
    private readonly Func<string, LogLevel, bool>? _filter;
    private readonly string _name;
    private readonly LogOut _logOut;

    public TestLogger(ITestContextWriters testContext, string name, LogOut logOut = LogOut.Progress)
        : this(testContext, name, filter: null)
    {
        _logOut = logOut;
    }

    public TestLogger(ITestContextWriters testContext, string name, Func<string, LogLevel, bool>? filter, LogOut logOut = LogOut.Progress)
    {
        _name = string.IsNullOrEmpty(name) ? nameof(TestLogger) : name;
        _testContext = testContext;
        _filter = filter;
        _logOut = logOut;
    }

    public IDisposable BeginScope<TState>(TState state)
         where TState : notnull
    {
        return NullDisposable.Instance;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = formatter(state, exception);

        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        message = $"[{DateTime.Now:HH:mm:ss:fff} {LogLevelShort(logLevel)}]: {_name}{Environment.NewLine}       {message}";

        if (exception != null)
        {
            message += Environment.NewLine + Environment.NewLine + exception.ToString();
        }

        WriteMessage(message);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return RunningInNUnitContext() && logLevel != LogLevel.None
        && (_filter is null || _filter(_name, logLevel));
    }

    private bool RunningInNUnitContext()
    {
        return _testContext.Progress is not null || _testContext.Out is not null;
    }

    private void WriteMessage(string message)
    {
        if ((_logOut & LogOut.Progress) > 0)
        {
            _testContext.Progress?.WriteLine(message);
        }

        if ((_logOut & LogOut.Out) > 0)
        {
            _testContext.Out?.WriteLine(message);
        }
    }

    private static string LogLevelShort(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "trace",
        LogLevel.Debug => "debug",
        LogLevel.Information => "info",
        LogLevel.Warning => "warn",
        LogLevel.Error => "error",
        LogLevel.Critical => "critical",
        _ => "none",
    };

    private sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Instance = new NullDisposable();

        public void Dispose()
        {
        }
    }
}

public class TestLogger<T> : TestLogger, ILogger<T>
{
    public TestLogger(ITestContextWriters testContext, LogOut logOut = LogOut.Progress)
        : base(testContext, typeof(T).Name, logOut)
    {
    }
}