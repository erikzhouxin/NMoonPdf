using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace System.Data.MoonPdf.Wpf
{
	/// <summary>
	/// Common interface for the two different display types, single pages (SinglePageMoonPdfPanel) and continuous pages (ContinuousMoonPdfPanel)
	/// </summary>
	internal interface IMoonPdfPanel
	{
		ScrollViewer ScrollViewer { get; }
		UserControl Instance { get; }
		float CurrentZoom { get; }
		void Load(IPdfSource source, string password = null);
        void Unload();
        void Zoom(double zoomFactor);
		void ZoomIn();
		void ZoomOut();
		void ZoomToWidth();
		void ZoomToHeight();
		void GotoPage(int pageNumber);
		void GotoPreviousPage();
		void GotoNextPage();
		int GetCurrentPageIndex(ViewType viewType);
	}
}
