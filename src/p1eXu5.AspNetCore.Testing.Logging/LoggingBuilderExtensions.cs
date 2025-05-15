using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.Logging;

public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Extension method to add a test logger to the logging builder. Freezes <paramref name="testContext"/>.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="testContext">The test context writers.</param>
    /// <param name="logOut">The log out.</param>
    /// <returns>The logging builder.</returns>
    public static ILoggingBuilder AddTestLogger(this ILoggingBuilder builder, ITestContextWriters testContext, LogOut logOut)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, TestLoggerProvider>(_ =>
                new TestLoggerProvider(testContext.Freeze(), logOut)));

        return builder;
    }
}