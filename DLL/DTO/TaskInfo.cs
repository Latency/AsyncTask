// ****************************************************************************
// Project:  AsyncTask
// File:     TaskInfo.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using AsyncTask.Interfaces;

namespace AsyncTask.DTO
{
    // ReSharper disable once UnusedMember.Global
    public class TaskInfo : ITaskInfo
    {
        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;


        /// <summary>
        ///     Name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///     Scheduler
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }


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
        public ITaskList TaskList { get; set; }


        /// <summary>
        ///     Logger
        /// </summary>
        public ILogger Logger { get; set; }


        /// <summary>
        ///     PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}