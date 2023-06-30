using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using p1eXu5.AspNetCore.Testing.Logging;

#pragma warning disable IDE0130 // namespace match

namespace Microsoft.Extensions.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddTestLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TestLoggerProvider>());

        return builder;
    }
}