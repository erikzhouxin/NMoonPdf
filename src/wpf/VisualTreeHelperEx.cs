using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace System.Data.MoonPdf.Wpf
{
    internal static class VisualTreeHelperEx
    {
        public static T FindChild<T>(DependencyObject o) where T : DependencyObject
        {
            if (o is T)
                return (T)o;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);
                var result = FindChild<T>(child);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
