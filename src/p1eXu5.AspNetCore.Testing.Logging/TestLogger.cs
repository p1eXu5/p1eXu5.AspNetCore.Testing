using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

[Flags]
public enum LogOut
{
    NotSet = 0,
    Progress = 0b0001,
    Out      = 0b0010,
    All      = 0b0011,
}

public class TestLogger : ILogger
{
    private readonly ITestContextWriters _testContext;
    private readonly Func<string, LogLevel, bool>? _filter;
    private readonly string _categoryName;
    private readonly LogOut _logOut;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestLogger"/> class.
    /// </summary>
    /// <param name="testContextWriters">Implementation of <see cref="ITestContextWriters"/>.<br/>Instance of <see cref="TestContextWriters"/> can be used.</param>
    /// <param name="categoryName"></param>
    /// <param name="logOut">Where to write logs. Default - <see cref="LogOut.Progress"/>.</param>
    public TestLogger(ITestContextWriters testContextWriters, string categoryName, LogOut logOut = LogOut.Progress)
        : this(testContextWriters, categoryName, filter: null)
    {
        _logOut = logOut;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestLogger"/> class.
    /// </summary>
    /// <param name="testContextWriters">Implementation of <see cref="ITestContextWriters"/>.<br/>Instance of <see cref="TestContextWriters"/> can be used.</param>
    /// <param name="categoryName"></param>
    /// <param name="filter"></param>
    /// <param name="logOut">Where to write logs. Default - <see cref="LogOut.Progress"/>.</param>
    public TestLogger(ITestContextWriters testContextWriters, string categoryName, Func<string, LogLevel, bool>? filter, LogOut logOut = LogOut.Progress)
    {
        _categoryName = string.IsNullOrEmpty(categoryName) ? nameof(TestLogger) : categoryName;
        _testContext = testContextWriters;
        _filter = filter;
        _logOut = logOut;
    }

    /// <summary>
    /// Returns <see cref="NullDisposable.Instance"/>.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable BeginScope<TState>(TState state)
         where TState : notnull
    {
        return NullDisposable.Instance;
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="logLevel"></param>
    /// <param name="eventId"></param>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <param name="formatter"></param>
    /// <exception cref="ArgumentNullException"></exception>
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

        message = $"{DateTime.Now:HH:mm:ss:fff} [{LogLevelShort(logLevel, 3)}] {_categoryName}{Environment.NewLine}    {message}{Environment.NewLine}";

        if (exception != null)
        {
            message += Environment.NewLine + Environment.NewLine + exception.ToString();
        }

        WriteMessage(message);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return RunningInNUnitContext() && logLevel != LogLevel.None
        && (_filter is null || _filter(_categoryName, logLevel));
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

    private static string LogLevelShort(LogLevel logLevel, int length = -1) => (logLevel) switch
    {
        LogLevel.Trace =>
            length switch {
                3 => "VRB",
                _ => "TRACE",
            },
        LogLevel.Debug =>
            length switch
            {
                3 => "DBG",
                _ => "DEBUG",
            },
        LogLevel.Information =>
            length switch
            {
                3 => "INF",
                _ => "INFO",
            },
        LogLevel.Warning =>
            length switch
            {
                3 => "WRN",
                _ => "WARN",
            },
        LogLevel.Error =>
            length switch
            {
                3 => "ERR",
                _ => "ERROR",
            },
        LogLevel.Critical =>
            length switch
            {
                3 => "CRT",
                _ => "CRITICAL",
            },
        _ =>
            length switch
            {
                3 => "???",
                _ => "???",
            },
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
        : base(testContext, typeof(T).FullName ?? typeof(T).Name, logOut)
    {
    }

    public TestLogger(ITestContextWriters testContextWriters, string categoryName, Func<string, LogLevel, bool>? filter, LogOut logOut = LogOut.Progress)
        : base(testContextWriters, categoryName, filter, logOut)
    {
    }
}