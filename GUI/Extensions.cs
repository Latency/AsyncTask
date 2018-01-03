//  *****************************************************************************
//  File:       Extensions.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       02/22/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ORM_Monitor {
  public static class Extensions {
    /// <summary>
    ///   Simple helper extension method to marshall to correct thread if its required
    /// </summary>
    /// <param name="control"></param>
    /// <param name="methodcall"></param>
    /// <param name="priorityForCall"></param>
    /// <param name="parameters"></param>
    public static T InvokeIfRequired<T>(this DispatcherObject control, Func<object[], T> methodcall, DispatcherPriority priorityForCall, params object[] parameters) {
      // CheckAccess returns true if you're on the dispatcher thread.
      return !control.Dispatcher.CheckAccess() ? (T) control.Dispatcher.Invoke(priorityForCall, methodcall, parameters) : methodcall(parameters);
    }


    /// <summary>
    ///   Simple helper extension method to marshall to correct thread if its required
    /// </summary>
    /// <param name="control"></param>
    /// <param name="methodcall"></param>
    /// <param name="priorityForCall"></param>
    /// <param name="parameters"></param>
    public static void InvokeIfRequired(this DispatcherObject control, Action<object[]> methodcall, DispatcherPriority priorityForCall, params object[] parameters) {
      // CheckAccess returns true if you're on the dispatcher thread.
      if (!control.Dispatcher.CheckAccess())
        control.Dispatcher.Invoke(priorityForCall, methodcall, parameters);
      else
        methodcall(parameters);
    }


    /// <summary>
    ///   FindAncestorOrSelf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T FindAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject {
      var p = obj;
      do {
        p = VisualTreeHelper.GetParent(p);
      } while (p != null && !(p is T));
      return p as T;
    }


    /// <summary>
    ///   FindFirstChild
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <returns></returns>
    public static T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement {
      var childrenCount = VisualTreeHelper.GetChildrenCount(element);
      var children = new FrameworkElement[childrenCount];

      for (var i = 0; i < childrenCount; i++) {
        var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
        children[i] = child;
        if (child is T variable)
          return variable;
      }

      for (var i = 0; i < childrenCount; i++) {
        if (children[i] == null)
          continue;
        var subChild = FindFirstChild<T>(children[i]);
        if (subChild != null)
          return subChild;
      }

      return null;
    }
  }
}