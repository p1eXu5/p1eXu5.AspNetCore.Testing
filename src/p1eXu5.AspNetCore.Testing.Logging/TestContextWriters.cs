namespace p1eXu5.AspNetCore.Testing.Logging;

/// <inheritdoc cref="ITestContextWriters"/>
public class TestContextWriters : ITestContextWriters
{
    /// <inheritdoc/>
    public TextWriter? Progress { get; set; }

    /// <inheritdoc/>
    public TextWriter? Out { get; set; }
}
