using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoonPdfViewer
{
    /// <summary>
    /// HelpContent.xaml 的交互逻辑
    /// </summary>
    public partial class HelpContent : UserControl
    {
        public HelpContent()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = System.IO.Path.GetFullPath("Resources/HelpPdf.pdf");
                this.PnlMoonPdf.OpenFile(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
