// ****************************************************************************
// Project:  GUI
// File:     Find.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System.Windows;
using System.Windows.Media;

namespace ORM_Monitor.Extensions
{
    public static class Extensions
    {
        /// <summary>
        ///     FindAncestorOrSelf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject
        {
            var p = obj;
            do
            {
                p = VisualTreeHelper.GetParent(p);
            }
            while (p != null && !(p is T));

            return p as T;
        }


        /// <summary>
        ///     FindFirstChild
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);
            var children = new FrameworkElement[childrenCount];

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                children[i] = child;
                if (child is T variable)
                    return variable;
            }

            for (var i = 0; i < childrenCount; i++)
            {
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