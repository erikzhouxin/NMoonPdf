using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Data.MoonPdf.Wpf
{
    internal static class BitmapExtensionMethods
    {
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap bmp)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            int bufferSize = bmpData.Stride * bmp.Height;
            var bms = new System.Windows.Media.Imaging.WriteableBitmap(bmp.Width, bmp.Height, bmp.HorizontalResolution, bmp.VerticalResolution, PixelFormats.Bgr32, null);
            bms.WritePixels(new Int32Rect(0, 0, bmp.Width, bmp.Height), bmpData.Scan0, bufferSize, bmpData.Stride);
            bmp.UnlockBits(bmpData);

            return bms;
        }
    }
}
