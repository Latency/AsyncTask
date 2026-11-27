// ****************************************************************************
// Project:  HSMS.Logging
// File:     NullLogger.cs
// Author:   Latency McLaughlin
// Date:     11/26/2026
// ****************************************************************************

#if !NET5_0_OR_GREATER && !NETSTANDARD2_0_OR_GREATER && !NET461_OR_GREATER
namespace Microsoft.Extensions.Logging.Abstractions;

public sealed class NullLogger : Microsoft.Extensions.Logging.ILogger
{
    public static readonly NullLogger Instance = new();

    private NullLogger() { }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => EmptyScope.Instance;

    public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => false;

    public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel,
                            Microsoft.Extensions.Logging.EventId eventId,
                            TState state,
                            Exception? exception,
                            Func<TState, Exception?, string> formatter)
    { }

    private sealed class EmptyScope : IDisposable
    {
        public static readonly EmptyScope Instance = new();
        public void Dispose() { }
    }
}
#endif
