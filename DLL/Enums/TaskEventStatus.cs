// ****************************************************************************
// Project:  AsyncTask
// File:     TaskEventStatus.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

namespace AsyncTask.Enums
{
    /// <summary>
    ///     TaskEventStatus
    /// </summary>
    public enum TaskEventStatus
    {
        /// <summary>
        ///     The task has been scheduled for execution but has not yet begun executing.
        /// </summary>
        WaitingToRun,

        /// <summary>
        ///     The task is running but has not yet completed.
        /// </summary>
        Running,

        /// <summary>
        ///     The task has timed out and stopped.
        /// </summary>
        TimedOut,

        /// <summary>
        ///     The task completed execution successfully.
        /// </summary>
        RanToCompletion,

        /// <summary>
        ///     The task is scheduled to be canceled.
        /// </summary>
        Cancelling,

        /// <summary>
        ///     The task acknowledged cancellation by throwing an OperationCanceledException
        ///     with its own CancellationToken while the token was in signaled state, or the
        ///     task's CancellationToken was already signaled before the task started executing.
        /// </summary>
        /// <remarks>For more information, see TaskEvent Cancellation.</remarks>
        Canceled,

        /// <summary>
        ///     The task has undergone a thread abort and crashed.
        /// </summary>
        Aborted,

        /// <summary>
        ///     The task completed due to an unhandled exception.
        /// </summary>
        Faulted
    }
}