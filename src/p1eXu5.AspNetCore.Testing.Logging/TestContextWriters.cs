namespace p1eXu5.AspNetCore.Testing.Logging;

/// <inheritdoc cref="ITestContextWriters"/>
public class TestContextWriters : ITestContextWriters
{
    private static TestContextWriters? _default;

    private Func<TextWriter?>? _progressWriterFactory;
    private Func<TextWriter?>? _outWriterFactory;

    private TextWriter? _progressWriter;
    private TextWriter? _outWriter;

    public TestContextWriters(TextWriter? progressWriter, TextWriter? outWriter)
        => SetWriters(progressWriter, outWriter);

    public TestContextWriters(Func<TextWriter?>? progressWriterFactory, Func<TextWriter?>? outWriterFactory)
        => SetWriters(progressWriterFactory, outWriterFactory);

    public static TestContextWriters Default => _default ??= new((TextWriter?)null, null);

    /// <inheritdoc/>
    public TextWriter? Progress
    {
        get
        {
            if (_progressWriterFactory is { })
            {
                return _progressWriterFactory();
            }

            return _progressWriter;
        }
    }

    /// <inheritdoc/>
    public TextWriter? Out
    {
        get
        {
            if (_outWriterFactory is { })
            {
                return _outWriterFactory();
            }

            return _outWriter;
        }
    }

    public static TestContextWriters DefaultWith(TextWriter? progressWriter, TextWriter? outWriter)
    {
        _default ??= new(progressWriter, outWriter);
        return _default;
    }

    public static TestContextWriters DefaultWith(Func<TextWriter?>? progressWriterFactory, Func<TextWriter?>? outWriterFactory)
    {
        _default ??= new(progressWriterFactory, outWriterFactory);
        return _default;
    }

    /// <summary>
    /// NUnit TestContext may be different for OneTimeSetup-methods and test methods.
    /// </summary>
    /// <param name="progressWriter"></param>
    /// <param name="outWriter"></param>
    public void SetWriters(TextWriter? progressWriter, TextWriter? outWriter)
    {
        _progressWriter = progressWriter;
        _outWriter = outWriter;
    }

    public void SetWriters(Func<TextWriter?>? progressWriterFactory, Func<TextWriter?>? outWriterFactory)
    {
        _progressWriterFactory = progressWriterFactory;
        _outWriterFactory = outWriterFactory;
    }
}
