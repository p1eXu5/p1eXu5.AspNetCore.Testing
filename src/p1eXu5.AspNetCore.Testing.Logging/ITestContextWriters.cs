namespace p1eXu5.AspNetCore.Testing.Logging;

public interface ITestContextWriters
{
    TextWriter? Progress { get; }

    TextWriter? Out { get; }
}
