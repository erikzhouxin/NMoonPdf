using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.MoonPdf.Wpf
{
    internal static class ListExtension
    {
        public static IEnumerable<T> Take<T>(this IList<T> list, int start, int length)
        {
            for (int i = start; i < Math.Min(list.Count, start + length); i++)
            {
                yield return list[i];
            }
        }
    }
}
