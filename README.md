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

## p1eXu5.AspNetCore.Testing.Serilog

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

## p1eXu5.AspNetCore.Testing

### MockRepository

```fsharp
// Fixture module

open Microsoft.Extensions.Logging
open NUnit.Framework
open p1eXu5.AspNetCore.Testing.Logging
open p1eXu5.AspNetCore.Testing
open p1eXu5.AspNetCore.Testing.MockRepository

type internal FooWebApi =
    {
        Client: FooApiClient
        MockRepository: MockRepository
    }

let mutable private client = Unchecked.defaultof<_>

let mutable private _mockRepository = Unchecked.defaultof<_>

let mutable internal _instance = Unchecked.defaultof<_>

[<OneTimeSetUp>]
let init () =
    let tcw =
        TestContextWriters.DefaultWith(TestContext.Progress, TestContext.Out)

    let initLogger = TestLogger<FooWebApi>(tcw, LogOut.All) :> ILogger<DrugsWebApi>
    let lorWriter = TestLogWriter(initLogger)

    let factory =
        (new FooWebApplicationFactory())
            .WithWebHostBuilder(fun builder ->
                builder.AddMockRepository(
                    [Service<IFooRepository>()],
                    lorWriter,
                    (fun mr -> _mockRepository <- mr)
                )
            )

    let fooApiClientLogger = TestLogger<DrugsApiClient>(tcw, LogOut.All) :> ILogger<DrugsApiClient>

    _instance <-
        {
            Client = FooApiClient.init client fooApiClientLogger
            MockRepository = _mockRepository
        }

[<OneTimeTearDown>]
    let dispose () =
        match box client with
        | null -> ()
        | _ ->
            client.Dispose()

// Test module

open System.Threading
open System.Linq

open NUnit.Framework
open FsUnit.TopLevelOperators
open NSubstitute
open p1eXu5.AspNetCore.Testing
open p1eXu5.AspNetCore.Testing.Logging
open p1eXu5.FSharp.Testing.ShouldExtensions

[<SetUp>]
    let setWriters () =
        TestContextWriters.Default.SetWriters(TestContext.Progress, TestContext.Out)

[<Test>]
let ``search endpoint, when foo repo is empty, returns ok`` () =
    task {
        let fooRepoMock = FooWebApi._instance.MockRepository.TrySubstitute<IFooRepository>()
        let _ =
            fooRepoMock
                .SearchFooAsync(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
                .ReturnsForAnyArgs(Enumerable.Empty<FooCard>())

        let! response = FooWebApi._instance.Client.SearchAsync "foo" CancellationToken.None

        response |> should be (ofCase <@ ApiRequestResult<FooCard list>.Ok @>)
        let! _ = fooRepoMock.ReceivedWithAnyArgs(1).SearchFooAsync(Unchecked.defaultof<_>, Unchecked.defaultof<_>)

        return ()
    }
```