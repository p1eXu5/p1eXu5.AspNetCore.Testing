namespace p1eXu5.AspNetCore.Testing.Logging;

public interface ITestContext
{
    TextWriter? Progress { get; }

    TextWriter? Out { get; }
}
