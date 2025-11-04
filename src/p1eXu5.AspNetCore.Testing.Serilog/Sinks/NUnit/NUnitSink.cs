using p1eXu5.AspNetCore.Testing.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace p1eXu5.AspNetCore.Testing.Serilog.Sinks.NUnit;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class NUnitSink : ILogEventSink
{
    private ITestTextWriter? _out;

    private readonly ITestContextWriters _testContextWriters;
    private readonly MessageTemplateTextFormatter _formatter;
    private readonly LogOut _logOut;

    public NUnitSink(ITestContextWriters testContextWriters, MessageTemplateTextFormatter formatter, LogOut logOut = LogOut.All)
    {
        ArgumentNullException.ThrowIfNull(formatter);
        _testContextWriters = testContextWriters;
        _formatter = formatter;
        _logOut = logOut;
    }

    public void Emit(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        ITestTextWriter? @out = (_logOut & LogOut.Out) > 0 ? _testContextWriters.Out : null;
        ITestTextWriter? progress = (_logOut & LogOut.Progress) > 0 ? _testContextWriters.Progress : null;

        bool isOutSet = @out is not null;

        if (isOutSet || progress is not null)
        {
            using var writer = new StringWriter();

            _formatter.Format(logEvent, writer);
            var msg = writer.ToString();

            if (isOutSet && _out is null)
            {
                _out = @out;
            }
            else if (isOutSet && !object.ReferenceEquals(_out, @out))
            {
                @out = _out;
            }
            
            @out?.WriteLine(msg);
            progress?.WriteLine(msg);
        }
    }
}
