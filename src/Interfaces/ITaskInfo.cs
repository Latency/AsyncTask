// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskInfo.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.ComponentModel;
#if NETNET461_OR_GREATER || NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif

namespace AsyncTask.Interfaces;

public interface ITaskInfo : INotifyPropertyChanged
{
    #if NET461_OR_GREATER || NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    [Required]
    #endif
    string                  Name                   { get; set; }
    SynchronizationContext? SynchronizationContext { get; set; }
    TimeSpan?               Timeout                { get; set; }
    CancellationToken       Token                  { get; }
    TimeSpan                PollInterval           { get; set; }
    ITaskList?              TaskList               { get; set; }
    ILogger?                Logger                 { get; set; }
}