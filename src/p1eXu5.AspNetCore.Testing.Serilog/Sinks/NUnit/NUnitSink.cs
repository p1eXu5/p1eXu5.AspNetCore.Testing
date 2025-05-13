﻿using p1eXu5.AspNetCore.Testing.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace p1eXu5.AspNetCore.Testing.Serilog.Sinks.NUnit;

public class NUnitSink : ILogEventSink
{
    private readonly ITestContextWriters _testContextWriters;
    private readonly MessageTemplateTextFormatter _formatter;
    private readonly LogOut _logOut;

    public NUnitSink(ITestContextWriters testContextWriters, MessageTemplateTextFormatter formatter, LogOut logOut = LogOut.Progress)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }
        _testContextWriters = testContextWriters;
        _formatter = formatter;
        _logOut = logOut;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent == null)
        {
            throw new ArgumentNullException(nameof(logEvent));
        }

        var @out = (_logOut & LogOut.Out) > 0 ? _testContextWriters.Out : null;
        var progress = (_logOut & LogOut.Progress) > 0 ? _testContextWriters.Progress : null;

        if (@out is not null || progress is not null)
        {
            using var writer = new StringWriter();

            _formatter.Format(logEvent, writer);
            var msg = writer.ToString();

            @out?.WriteLine(msg);
            progress?.WriteLine(msg);
        }
    }
}
