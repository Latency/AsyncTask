// ****************************************************************************
// Project:  Logging
// File:     FileLogger.cs
// Author:   Latency McLaughlin
// Date:     11/20/2026
// ****************************************************************************

using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Logging.RollingFile;

internal sealed class FileLogger(FileLoggerProvider provider) : ILogger
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileLoggerProvider _provider = provider;

    #if NET461
        public IDisposable BeginScope<TState>(TState state) => _provider.ScopeProvider?.Push(state)!;
    #elif NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => _provider.ScopeProvider?.Push(state);
    #else
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    #endif

        public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var timestamp     = DateTimeOffset.Now;
        var builder       = new StringBuilder($"{timestamp:yyyy-MM-dd HH:mm:ss} [{logLevel}]");
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
        var scopeProvider = _provider.ScopeProvider;
        if (scopeProvider != null)
        {
            scopeProvider.ForEachScope((scope, stringBuilder) => stringBuilder.Append(" => ").Append(scope), builder);
            builder.AppendLine(":");
        }
        else
#endif
            builder.Append(": ");

        builder.AppendLine(formatter(state, exception));

        if (exception != null)
            builder.AppendLine(exception.ToString());

        _provider.Writer.Write(timestamp.LocalDateTime, builder.ToString());
    }
}
