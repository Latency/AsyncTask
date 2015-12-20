//  *****************************************************************************
//  File:      TaskHandler.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System.Linq;
using System.Threading;

namespace ORM_Monitor.Models {
  /// <summary>
  ///   TaskHandler
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class TaskHandler<T> {
    /// <summary>
    ///   TaskEventHandler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TaskEventHandler(TaskHandler<T> sender, TaskEventArgs<T> e);


    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="cts"></param>
    protected TaskHandler(CancellationTokenSource cts) {
      _cts = cts;
    }


    /// <summary>
    ///   IsSubscribed
    /// </summary>
    public bool IsSubscribed {
      get { return Handler != null && Handler.GetInvocationList().Any(x => x.Method.Name.Equals(_delegateName)); }
    }


    /// <summary>
    ///   MyHandler
    /// </summary>
    protected event TaskEventHandler MyHandler {
      add {
        _delegateName = value.Method.Name;
        Handler += value;
      }
      remove {
        _delegateName = null;
        Handler -= value;
      }
    }


    /// <summary>
    ///   Invoke
    /// </summary>
    /// <param name="expression"></param>
    public void Invoke(T expression) {
      Handler?.Invoke(this, new TaskEventArgs<T>(_cts, expression));
    }

    // -----------------------------------------------------------------------

    #region Fields

    private event TaskEventHandler Handler;
    private readonly CancellationTokenSource _cts;
    private string _delegateName;

    // -----------------------------------------------------------------------

    #endregion Fields
  }
}