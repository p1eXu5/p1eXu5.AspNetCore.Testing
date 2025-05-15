using System.Reflection;

namespace p1eXu5.AspNetCore.Testing.Logging;

/// <inheritdoc cref="ITestContextWriters"/>
public sealed class TestContextWriters : ITestContextWriters
{
    private static TestContextWriters? _default;

    private MethodInfo? _outWriterMethodInfo;
    private FieldInfo? _progressWriterMethodInfo;

    private TextWriter? _outWriter;
    private TextWriter? _progressWriter;

    private bool _isDisposed = false;

    private TestContextWriters(Type testContextType)
    {
        // if type is static
        if (testContextType.IsClass)
        {
            var flags = BindingFlags.Public | BindingFlags.Static;

            _outWriterMethodInfo = testContextType.GetProperty("Out", flags | BindingFlags.GetProperty)?.GetGetMethod();
            _progressWriterMethodInfo = testContextType.GetField("Progress", flags);
        }
    }

    private TestContextWriters(TextWriter? outWriter, TextWriter? progressWriter)
    {
        _outWriter = outWriter;
        _progressWriter = progressWriter;
    }

    public static TestContextWriters GetInstance<TTestContext>() => _default ??= new(typeof(TTestContext));

    public void Dispose()
    {
        if (_isDisposed) return;

        _outWriterMethodInfo = null;
        _progressWriterMethodInfo = null;
        _outWriter = null;
        _progressWriter = null;
    }

    public ITestContextWriters Freeze()
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(TestContextWriters));

        if (_outWriter is { } || _progressWriter is { })
        {
            return this;
        }

        var inst = new TestContextWriters(
            _outWriterMethodInfo?.Invoke(null, null) as TextWriter,
            _progressWriterMethodInfo?.GetValue(null) as TextWriter);

        inst._outWriterMethodInfo = null;
        inst._progressWriterMethodInfo = null;

        return inst;
    }

    /// <inheritdoc/>
    public ITestTextWriter? Out
    {
        get
        {
            var textWriter = _outWriter ?? _outWriterMethodInfo?.Invoke(null, null) as TextWriter;
            if (textWriter is { })
            {
                return new TestTextWriter(textWriter);
            }

            return null;
        }
    }

    /// <inheritdoc/>
    public ITestTextWriter? Progress
    {
        get
        {
            var textWriter = _progressWriter ?? _progressWriterMethodInfo?.GetValue(null) as TextWriter;
            if (textWriter is { })
            {
                return new TestTextWriter(textWriter);
            }

            return null;
        }
    }

    private sealed class TestTextWriter : ITestTextWriter
    {
        private readonly TextWriter _textWriter;

        internal TestTextWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public void Dispose()
        { }

        public void WriteLine(string message) => _textWriter.WriteLine(message);
    }
}

