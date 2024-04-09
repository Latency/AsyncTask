// ****************************************************************************
// Project:  AsyncTask
// File:     LoggerHandlers.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using AsyncTask.Interfaces;
using AsyncTask.Models;

namespace AsyncTask.Logging;

public abstract partial class LoggerHandlers
{
    private static IMessageEventArgs _(string msg, Exception? ex = null) => new MessageEventArgs
    {
        Message   = $"{msg}{Environment.NewLine}{(ex != null ? $"{ex.Message}{Environment.NewLine}" : string.Empty)}",
        Exception = ex
    };

    #region Callbacks
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public virtual void OnTrace      (string msg) => TraceHandler?.Invoke(_(msg));
    public virtual void OnDebug      (string msg) => DebugHandler?.Invoke(_(msg));
    public virtual void OnInformation(string msg) => InformationHandler?.Invoke(_(msg));
    public virtual void OnWarning    (string msg) => WarningHandler?.Invoke(_(msg));
    public virtual void OnCritical   (string msg) => CriticalHandler?.Invoke(_(msg));
    public virtual void OnNone       (string msg) => NoneHandler?.Invoke(_(msg));
    public virtual void OnError      (string msg, Exception? ex) => ErrorHandler?.Invoke(_(msg, ex));
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Callbacks
}