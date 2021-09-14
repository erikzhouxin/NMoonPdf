using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace System.Data.MoonPdf.Wpf
{
    internal class PdfImageProvider : IItemsProvider<IEnumerable<PdfImage>>
    {
        private IPdfSource pdfSource;
        private int count = -1;
        private int totalPages;
        private bool preFetch;
        private string password;

        public PageDisplaySettings Settings { get; private set; }

        public PdfImageProvider(IPdfSource pdfSource, int totalPages, PageDisplaySettings settings, bool preFetch = true, string password = null)
        {
            this.pdfSource = pdfSource;
            this.totalPages = totalPages;
            this.Settings = settings;
            this.preFetch = preFetch; // preFetch is relevant for continuous page display
            this.password = password;
        }

        public int FetchCount()
        {
            if (count == -1)
                count = MuPdfWrapper.CountPages(pdfSource, this.password);

            return count;
        }

        public IList<IEnumerable<PdfImage>> FetchRange(int startIndex, int count)
        {
            var imagesPerRow = this.Settings.ImagesPerRow;
            var viewType = this.Settings.ViewType;

            startIndex = (startIndex * imagesPerRow) + 1;

            if (preFetch)
                count = count * imagesPerRow;

            if (viewType == ViewType.BookView)
            {
                if (startIndex == 1)
                    count = Math.Min(this.totalPages, preFetch ? (1 /*first page*/ + imagesPerRow) : 0);
                else
                    startIndex--;
            }

            var end = Math.Min(FetchCount(), startIndex + count - 1);
            var list = new List<IEnumerable<PdfImage>>();
            var rowList = new List<PdfImage>(imagesPerRow);
            var offset = viewType == ViewType.BookView ? 1 : 0;

            for (int i = Math.Min(FetchCount(), startIndex); i <= Math.Min(FetchCount(), Math.Max(startIndex, end)); i++)
            {
                var margin = new Thickness(0, 0, this.Settings.HorizontalOffsetBetweenPages, 0);

                using (var bmp = MuPdfWrapper.ExtractPage(pdfSource, i, this.Settings.ZoomFactor, this.password))
                {
                    if (Settings.Rotation != ImageRotation.None)
                    {
                        var flipType = System.Drawing.RotateFlipType.Rotate90FlipNone;

                        if (Settings.Rotation != ImageRotation.Rotate90)
                            flipType = Settings.Rotation == ImageRotation.Rotate180 ? System.Drawing.RotateFlipType.Rotate180FlipNone : System.Drawing.RotateFlipType.Rotate270FlipNone;

                        bmp.RotateFlip(flipType);
                    }

                    var bms = bmp.ToBitmapSource();
                    // Freeze bitmap to avoid threading problems when using AsyncVirtualizingCollection,
                    // because FetchRange is NOT called from the UI thread
                    bms.Freeze();

                    if ((i == 1 && viewType == ViewType.BookView) || (i + offset) % 2 == 0)
                        margin.Right = 0; // set right margin to zero for first page and for all pages that are on the right side

                    var img = new PdfImage { ImageSource = bms, Margin = margin };

                    // if first page and viewtype bookview, add the first page and continue with next
                    if (viewType == ViewType.BookView && i == 1)
                    {
                        list.Add(new[] { img });
                        continue;
                    }

                    rowList.Add(img);
                }

                // if all images per row were added or the end of the pdf is reached, add the remaining PdfImages from rowList to the final list
                if (rowList.Count % imagesPerRow == 0 || i == end)
                {
                    list.Add(rowList);

                    if (i == end && rowList.Count % imagesPerRow != 0)
                    {
                        var last = rowList.Last();
                        last.Margin = new Thickness(0);
                    }

                    if (i < end)
                        rowList = new List<PdfImage>(imagesPerRow);
                }
            }

            return list;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ImageRotation
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Rotate90,
        /// <summary>
        /// 
        /// </summary>
        Rotate180,
        /// <summary>
        /// 
        /// </summary>
        Rotate270
    }
}
