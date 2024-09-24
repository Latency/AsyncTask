// ****************************************************************************
// Project:  AsyncTask
// File:     TaskInfo.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

#if NETNET461_OR_GREATER || NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AsyncTask.Interfaces;

namespace AsyncTask.Models;

// ReSharper disable once UnusedMember.Global
public class TaskInfo : ITaskInfo
{
    /// <summary>
    ///     Name
    /// </summary>
    #if NET461_OR_GREATER || NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    [Required]
    #endif
    public string Name { get; set; } = string.Empty;


    /// <summary>
    ///     Scheduler
    /// </summary>
    public SynchronizationContext? SynchronizationContext { get; set; }


    /// <summary>
    ///     Timeout
    /// </summary>
    public TimeSpan? Timeout { get; set; }


    /// <summary>
    ///     Token
    /// </summary>
    public CancellationToken Token { get; internal set; }


    /// <summary>
    ///     Poll Interval
    /// </summary>
    /// <remarks>
    ///     Poll every 1 second by default.
    /// </remarks>
    public TimeSpan PollInterval { get; set; } = new();


    /// <summary>
    ///     TaskList
    /// </summary>
    public ITaskList? TaskList { get; set; }


    /// <summary>
    ///     Logger
    /// </summary>
    public ILogger? Logger { get; set; }


    /// <summary>
    ///     PropertyChanged
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;


    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new(propertyName));
}