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


