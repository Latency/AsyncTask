//  *****************************************************************************
//  File:      TaskRecordSetBase.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using ORM_Monitor.Extensions;

namespace ORM_Monitor.Models {
  // ReSharper disable once InconsistentNaming
  public abstract class TaskRecordSetBase : DependencyObject
  {
    #region Fields
    // -----------------------------------------------------------------------

    /// <summary>
    ///  DependancyCollection
    /// </summary>
    private static readonly Dictionary<string, DependencyProperty> DependancyCollection = new();

    /// <summary>
    ///  Mutex
    /// </summary>
    private static readonly object Mutex = new();

    // -----------------------------------------------------------------------
    #endregion Fields


    /// <inheritdoc />
    /// <summary>
    ///  Constructor
    /// </summary>
    protected TaskRecordSetBase() {
      if (DependancyCollection.Count > 0)
        return;
      // Map the actual property to the DependencyProperty type.
      foreach (var prop in typeof(TaskRecordSet).GetProperties()) {
        var propertyType = prop.PropertyType;
        DependancyCollection.Add(
          prop.Name, DependencyProperty.Register(
            prop.Name, propertyType, GetType(), new UIPropertyMetadata(
              propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null, (sender, e) => sender.InvokeIfRequired(
                objs => {
                  typeof(DispatcherObject).GetProperty((string)objs[1])?.SetValue(objs[0], objs[2]);
                }, DispatcherPriority.Send, sender, prop.Name, e
              )
            )
          )
        );
      }
    }


    #region Methods
    // -----------------------------------------------------------------------

    /// <summary>
    ///  Get
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    protected T Get<T>(string property) {
      return (T)this.InvokeIfRequired(
        objs => {
          lock (Mutex) {
            return objs[0] is string key ? GetValue(DependancyCollection[key]) : null;
          }
        }, DispatcherPriority.Send, property
      );
    }


    /// <summary>
    ///  Set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="property"></param>
    /// <param name="value"></param>
    protected void Set<T>(string property, T value) {
      this.InvokeIfRequired(
        objs => {
          lock (Mutex) {
            if (objs[0] is not string key)
              return;
            var v = objs[1] is T variable ? variable : default(T);
            SetValue(DependancyCollection[key], v);
          }
        }, DispatcherPriority.Send, property, value
      );
    }

    // -----------------------------------------------------------------------
    #endregion Methods
  }
}