namespace p1eXu5.AspNetCore.Testing.Logging;

/// <inheritdoc cref="ITestContextWriters"/>
public class TestContextWriters : ITestContextWriters
{
    private static TestContextWriters? _default;

    public static TestContextWriters Default => _default ??= new(null, null);

    public static TestContextWriters DefaultWith(TextWriter? progressWriter, TextWriter? outWriter)
    {
        _default ??= new(progressWriter, outWriter);
        return _default;
    }

    public TestContextWriters(TextWriter? progressWriter, TextWriter? outWriter)
    {
        SetWriters(progressWriter, outWriter);
    }


    /// <inheritdoc/>
    public TextWriter? Progress { get; set; }

    /// <inheritdoc/>
    public TextWriter? Out { get; set; }

    /// <summary>
    /// NUnit TestContext may be different for OneTimeSetup-methods and test methods.
    /// </summary>
    /// <param name="progressWriter"></param>
    /// <param name="outWriter"></param>
    public void SetWriters(TextWriter? progressWriter, TextWriter? outWriter)
    {
        Progress = progressWriter;
        Out = outWriter;
    }
}
