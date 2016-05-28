using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace WotDossier.Web.Logging
{
    public class NLogLoggerProvider : ILoggerProvider
    {
        private readonly LogFactory _logFactory;
        private bool _disposed = false;

        public NLogLoggerProvider(LogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        public ILogger CreateLogger(string name)
        {
            return new Logger(_logFactory.GetLogger(name));
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _logFactory.Flush(null);
                _logFactory.Dispose();
            }
        }

        private class Logger : ILogger
        {
            private readonly global::NLog.Logger _logger;

            public Logger(global::NLog.Logger logger)
            {
                _logger = logger;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, System.Func<TState, System.Exception, string> formatter)
            {
                var nLogLogLevel = GetLogLevel(logLevel);
                var message = string.Empty;
                if (formatter != null)
                {
                    message = formatter(state, exception);
                }
                else
                {
                    message = formatter(state, exception);
                }
                if (!string.IsNullOrEmpty(message))
                {
                    var eventInfo = LogEventInfo.Create(nLogLogLevel, _logger.Name, exception, CultureInfo.CurrentCulture, message);
                    eventInfo.Properties["EventId"] = eventId;
                    _logger.Log(eventInfo);
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return _logger.IsEnabled(GetLogLevel(logLevel));
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                if (state == null)
                {
                    throw new ArgumentNullException(nameof(state));
                }

                return NestedDiagnosticsContext.Push(state.ToString());
            }

            private global::NLog.LogLevel GetLogLevel(LogLevel logLevel)
            {
                switch (logLevel)
                {
                    //case LogLevel.Trace: return global::NLog.LogLevel.Trace;
                    case LogLevel.Debug: return global::NLog.LogLevel.Debug;
                    case LogLevel.Information: return global::NLog.LogLevel.Info;
                    case LogLevel.Warning: return global::NLog.LogLevel.Warn;
                    case LogLevel.Error: return global::NLog.LogLevel.Error;
                    case LogLevel.Critical: return global::NLog.LogLevel.Fatal;
                }
                return global::NLog.LogLevel.Debug;
            }
        }
    }
}
