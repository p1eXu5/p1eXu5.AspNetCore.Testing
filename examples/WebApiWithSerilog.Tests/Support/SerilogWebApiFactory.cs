using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApiWithSerilog.Tests;

internal class SerilogWebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");

        builder.UseSetting("Serilog:Using:2", "p1eXu5.AspNetCore.Testing.Serilog");
        builder.UseSetting("Serilog:WriteTo:2:Name", "NUnit");
        builder.UseSetting("Serilog:WriteTo:2:Args:logOut", "2"); // 1 - Progress, 2 - Out, 3 - LogOut.All (default)

        builder.ConfigureAppConfiguration((ctx, b) =>
        {
            ctx.Configuration["Serilog:WriteTo:0"] = ""; // disable Console
            ctx.Configuration["Serilog:WriteTo:1"] = ""; // disable Debug
            ctx.Configuration["Serilog:WriteTo:2"] = null; // reset to null, cause is set as ""
        });
    }
}
