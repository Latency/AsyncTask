// ****************************************************************************
// Project:  AsyncTask
// File:     ILogger.cs
// Author:   Latency McLaughlin
// Date:     04/12/2024
// ****************************************************************************

#if !NET5_0_OR_GREATER && !NETSTANDARD2_0_OR_GREATER && !NET461_OR_GREATER
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging;

//
// Summary:
//     Represents a type used to perform logging.
public interface ILogger
{
    //
    // Summary:
    //     Begins a logical operation scope.
    //
    // Parameters:
    //   state:
    //     The identifier for the scope.
    //
    // Type parameters:
    //   TState:
    //     The type of the state to begin scope for.
    //
    // Returns:
    //     A disposable object that ends the logical operation scope on dispose.
    IDisposable? BeginScope<TState>(TState state) where TState : notnull;

    //
    // Summary:
    //     Checks if the given logLevel is enabled.
    //
    // Parameters:
    //   logLevel:
    //     level to be checked.
    //
    // Returns:
    //     true if enabled; false otherwise.
    bool IsEnabled(LogLevel logLevel);

    //
    // Summary:
    //     Writes a log entry.
    //
    // Parameters:
    //   logLevel:
    //     Entry will be written on this level.
    //
    //   eventId:
    //     Id of the event.
    //
    //   state:
    //     The entry to be written. Can be also an object.
    //
    //   exception:
    //     The exception related to this entry.
    //
    //   formatter:
    //     Function to create a System.String message of the state and exception.
    //
    // Type parameters:
    //   TState:
    //     The type of the object to be written.
    void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);
}
#endif