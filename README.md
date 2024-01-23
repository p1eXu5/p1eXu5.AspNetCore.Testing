p1eXu5.AspNetCore.Testing
=========================

## p1eXu5.AspNetCore.Testing.Logging

### Create ILoggerFactory

- F#, NUnit

```fsharp
let private loggerFactory =
    { new ILoggerFactory with
        member _.AddProvider(_: ILoggerProvider) = ()
        member _.CreateLogger(categoryName: string) =
            TestLogger(TestContextWriters(Progress = TestContext.Progress, Out = TestContext.Out), categoryName, LogOut.All)
            :> ILogger
        member _.Dispose() = ()
    }
```


```fsharp
[<SetUpFixture>]
module SetUpFixture =

    [<OneTimeSetUp>]
    let init () =
        let tcw = TestContextWriters.DefaultWith(TestContext.Progress,TestContext.Out)
        let handlerLogger = TestLogger<LogHttpRequestDelegatingHandler>(tcw, LogOut.All) :> ILogger<LogHttpRequestDelegatingHandler>
```

Or:

```fsharp
[<SetUpFixture>]
module SetUpFixture =

    [<OneTimeSetUp>]
    let init () =
        TestContextWriters.Default.SetWriters(TestContext.Progress, TestContext.Out)
```

- Serilog (use `p1eXu5.AspNetCore.Testing.Serilog`)

```fsharp
type FooWebApplicationFactory() =
    inherit WebApplicationFactory<Program>()

    override _.ConfigureWebHost(builder: IWebHostBuilder) =
        builder
            .UseSetting("ASPNETCORE_ENVIRONMENT", "Development")
            .UseSetting("IsInTest", "true")
            .UseSetting("InMemory", "true") |> ignore

        builder.ConfigureServices(fun (services: IServiceCollection) ->
            services.AddSerilog(
                (new LoggerConfiguration())
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "Foo Web API")
                    .WriteTo.NUnitOutput(TestContextWriters.Default)
                    .CreateLogger()
            )
            |> ignore
        )
        |> ignore
```