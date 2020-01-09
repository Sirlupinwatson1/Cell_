using controllibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace searchuser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string title = "Search for users";
        panel<bodysearch> panel = new panel<bodysearch>(title);
        internal static MainWindow ds;
        public MainWindow()
        {
            ds = this;
            InitializeComponent();
            Title = title;
            SizeToContent = SizeToContent.WidthAndHeight;
            Content = panel.element;
        }
    }
}