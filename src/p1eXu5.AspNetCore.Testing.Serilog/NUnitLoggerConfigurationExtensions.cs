using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

using p1eXu5.AspNetCore.Testing.Logging;
using p1eXu5.AspNetCore.Testing.Serilog.Sinks.NUnit;

namespace p1eXu5.AspNetCore.Testing.Serilog;

public static class NUnitLoggerConfigurationExtensions
{
    const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext}{NewLine}    {Message:lj}{NewLine}    {Exception}";

    public static LoggerConfiguration NUnitOutput(
        this LoggerSinkConfiguration sinkConfiguration,
        ITestContextWriters testContextWriters,
        LogOut logOut = LogOut.Progress,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider? formatProvider = null,
        LoggingLevelSwitch? levelSwitch = null,
        string outputTemplate = DefaultOutputTemplate
    ) {
        if (sinkConfiguration == null)
        {
            throw new ArgumentNullException(nameof(sinkConfiguration));
        }

        var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

        return sinkConfiguration.Sink(new NUnitSink(testContextWriters, formatter, logOut), restrictedToMinimumLevel, levelSwitch);
    }
}