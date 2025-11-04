using System.Net.Http.Json;
using WebApiWithSerilog.Tests.Support;

namespace WebApiWithSerilog.Tests;

public sealed class IntegrationTests
{
    private static readonly TestLoggerWebApiFactory _testLoggerWebApiFactory = new();
    private static readonly SerilogWebApiFactory _serilogWebApiFactory = new();
    private static readonly RawWebApiFactory _rawWebApiFactory = new();

    [Test]
    public async Task TestLogger_WebApi_Test()
    {
        TestContext.Out.WriteLine("In '{0}':", nameof(TestLogger_WebApi_Test));

        using var client = _testLoggerWebApiFactory.CreateClient();

        await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        TestContext.Out.WriteLine("'{0}' finished.", nameof(TestLogger_WebApi_Test));
    }

    [Test]
    public async Task Serilog_WebApi_Test()
    {
        TestContext.Out.WriteLine("In {0}:", nameof(Serilog_WebApi_Test));

        using var client = _serilogWebApiFactory.CreateClient();

        await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        TestContext.Out.WriteLine("'{0}' finished.", nameof(Serilog_WebApi_Test));
    }

    [Test]
    public async Task Raw_WebApi_Test()
    {
        TestContext.Out.WriteLine("In {0}:", nameof(Raw_WebApi_Test));

        using var client = _rawWebApiFactory.CreateClient();

        await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        TestContext.Out.WriteLine("'{0}' finished.", nameof(Raw_WebApi_Test));
    }
}
