// ****************************************************************************
// Project:  HSMS.Logging
// File:     LogEntry.cs
// Author:   Latency McLaughlin
// Date:     06/30/2021
// ****************************************************************************

using System.Diagnostics.CodeAnalysis;
using Logging.Models;
using Microsoft.Extensions.Logging;

namespace Logging;

public sealed class LogEntry
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    /// <param name="fullName"></param>
    /// <param name="args"></param>
    public LogEntry(LogLevel logLevel,
        #if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        #endif
        string? message, bool fullName, params object?[] args)
    {
        var stackInfo = new StackInfo(fullName, args);

        #if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        foreach (var (name, tuple) in stackInfo.ArgumentList)
            ExtendedProperties.Add(name, tuple);
        #else
        foreach (var arg in stackInfo.ArgumentList)
            ExtendedProperties.Add(arg.Key, arg.Value);
        #endif

        Arguments = args ?? [];

        LogLevel = logLevel;

        Message = logLevel switch
        {
            LogLevel.Trace => $"{message ?? string.Empty}{stackInfo}",
            _              => message ?? string.Empty
        };
    }


    public IDictionary<string, object?> ExtendedProperties { get; set; } = new Dictionary<string, object?>();
    public string                       Message            { get; set; }
    public object?[]                    Arguments          { get; set; }
    public LogLevel                     LogLevel           { get; set; }
}