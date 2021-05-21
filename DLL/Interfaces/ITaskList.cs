// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskList.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System.Collections.Generic;

namespace AsyncTask.Interfaces
{
    public interface ITaskList
    { }

    public interface ITaskList<TKey, TValue> : IDictionary<TKey, TValue>, ITaskList where TKey : notnull
    {
    }
}