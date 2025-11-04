using System.Reflection;
using p1eXu5.AspNetCore.Testing.Logging;
using p1eXu5.AspNetCore.Testing.Serilog.Sinks.NUnit;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Serilog;

public static class NUnitLoggerConfigurationExtensions
{
    const string DEFAULT_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext}{NewLine}    {Message:lj}{NewLine}    {Exception}";

    /// <summary>
    /// To use from configuration:
    /// <code>
    /// protected override void ConfigureWebHost(IWebHostBuilder builder)
    /// {
    ///     // any index can be used
    ///     builder.UseSetting("Serilog:Using:2", "p1eXu5.AspNetCore.Testing.Serilog");
    ///     builder.UseSetting("Serilog:WriteTo:2:Name", "NUnit");
    ///     builder.ConfigureAppConfiguration((ctx, b) =>;
    ///     {
    ///         ctx.Configuration["Serilog:WriteTo:2"] = null; // or Configuration["Serilog:WriteTo:2"].Value = "" and Nunit sink wil not be set
    ///     }
    /// }
    /// </code>
    /// <para>
    /// <b>Not all logs are emitted. Use AddTestLogger instead:</b>
    /// </para>
    /// <code>
    /// protected override void ConfigureWebHost(IWebHostBuilder builder)
    /// {
    ///     builder.ConfigureServices(services =>
    ///     {
    ///         services.AddSerilog(
    ///             (IServiceProvider services, LoggerConfiguration lc) =>
    ///                 lc.MinimumLevel.Debug(),
    ///             writeToProviders: true);
    ///             
    ///         services.AddLogging(cfg =>
    ///         {
    ///             cfg.ClearProviders();
    ///             // cfg.SetMinimumLevel(LogLevel.Warning); // is not accounting
    ///             cfg.AddTestLogger(TestContextWriters.GetInstance&lt;TestContext&gt;(), LogOut.All);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="sinkConfiguration"></param>
    /// <param name="testContextAssemblyName"></param>
    /// <param name="testContextTypeName"></param>
    /// <param name="logOut"></param>
    /// <param name="restrictedToMinimumLevel"></param>
    /// <param name="formatProvider"></param>
    /// <param name="levelSwitch"></param>
    /// <param name="outputTemplate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static LoggerConfiguration NUnit(
        this LoggerSinkConfiguration sinkConfiguration,
        string testContextAssemblyName = "nunit.framework",
        string testContextTypeName = "TestContext",
        LogOut logOut = LogOut.Progress,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider? formatProvider = null,
        LoggingLevelSwitch? levelSwitch = null,
        string outputTemplate = DEFAULT_OUTPUT_TEMPLATE
    )
    {
        Assembly assembly =
            AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == testContextAssemblyName);

        Type testContextType = assembly.GetExportedTypes().First(t => t.Name == testContextTypeName);

        ArgumentNullException.ThrowIfNull(sinkConfiguration);

        var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

        return sinkConfiguration.Sink(
            new NUnitSink(TestContextWriters.GetInstance(testContextType), formatter, logOut),
            restrictedToMinimumLevel,
            levelSwitch);
    }

    /// <summary>
    /// To use:
    /// <code>
    /// protected override void ConfigureWebHost(IWebHostBuilder builder)
    /// {
    ///     builder.ConfigureServices(services =>
    ///     {
    ///         services.AddSerilog((IServiceProvider services, LoggerConfiguration lc) =>
    ///             WriteTo.NUnitOutput(TestContextWriters.GetInstance&lt;TestContext&gt;()
    ///             // ...);
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="sinkConfiguration"></param>
    /// <param name="testContextWriters"></param>
    /// <param name="logOut"></param>
    /// <param name="restrictedToMinimumLevel"></param>
    /// <param name="formatProvider"></param>
    /// <param name="levelSwitch"></param>
    /// <param name="outputTemplate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static LoggerConfiguration NUnit(
        this LoggerSinkConfiguration sinkConfiguration,
        ITestContextWriters testContextWriters,
        LogOut logOut = LogOut.Progress,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider? formatProvider = null,
        LoggingLevelSwitch? levelSwitch = null,
        string outputTemplate = DEFAULT_OUTPUT_TEMPLATE
    )
    {
        ArgumentNullException.ThrowIfNull(sinkConfiguration);

        var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

        return sinkConfiguration.Sink(new NUnitSink(testContextWriters, formatter, logOut), restrictedToMinimumLevel, levelSwitch);
    }
}