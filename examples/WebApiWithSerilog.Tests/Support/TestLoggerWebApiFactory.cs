using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using p1eXu5.AspNetCore.Testing.Logging;
using Serilog;

namespace WebApiWithSerilog.Tests.Support;

internal class TestLoggerWebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");

        // Example of "p1eXu5.AspNetCore.Testing.Serilog" configuration.
        // Not all logs are shown.
        //
        // builder.UseSetting("Serilog:Using:2", "p1eXu5.AspNetCore.Testing.Serilog");
        // builder.UseSetting("Serilog:WriteTo:2:Name", "NUnit");
        // builder.ConfigureAppConfiguration((ctx, b) =>
        // {
        //     ctx.Configuration["Serilog:WriteTo:2"] = null;
        // });

        builder.ConfigureServices(services =>
        {
            services.AddSerilog(
                (services, lc) => lc.MinimumLevel.Debug(),
                writeToProviders: true // to use Microsoft logger providers
            );

            services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                // cfg.SetMinimumLevel(LogLevel.Warning); // is not accounting
                cfg.AddTestLogger(TestContextWriters.GetInstance<TestContext>(), LogOut.All);
            });
        });
    }
}
