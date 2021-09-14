using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.MoonPdf.Wpf
{
    internal static class DpiHelper
    {
        public const float DEFAULT_DPI = 96;

        public static Dpi GetCurrentDpi()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();

            var dpi = new Dpi();
            dpi.HorizontalDpi = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSX);
            dpi.VerticalDpi = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSY);

            return dpi;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        public enum DeviceCap
        {
            /// <summary>
            /// Logical pixels inch in X
            /// </summary>
            LOGPIXELSX = 88,
            /// <summary>
            /// Logical pixels inch in Y
            /// </summary>
            LOGPIXELSY = 90
        }      
    }

    internal class Dpi
    {
        public float HorizontalDpi { get; set; }
        public float VerticalDpi { get; set; }
    }
}
