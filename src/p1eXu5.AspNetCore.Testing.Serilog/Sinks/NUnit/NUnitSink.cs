using System;
using System.IO;
using p1eXu5.AspNetCore.Testing.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace p1eXu5.AspNetCore.Testing.Serilog.Sinks.NUnit
{
    public class NUnitSink : ILogEventSink
    {
        private readonly ITestContextWriters _testContextWriters;
        private readonly MessageTemplateTextFormatter _formatter;

        public NUnitSink(ITestContextWriters testContextWriters, MessageTemplateTextFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            _testContextWriters = testContextWriters;
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var @out = _testContextWriters.Out;
            var progress = _testContextWriters.Progress;

            if (@out is not null || progress is not null)
            {
                using var writer = new StringWriter();

                _formatter.Format(logEvent, writer);
                var msg = writer.ToString();

                @out?.WriteLine(msg);
                progress?.WriteLine(msg);
            }
        }
    }
}
