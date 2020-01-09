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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace message
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string title = "Messenger";
        panel<app> panel = new panel<app>(title);
        public MainWindow()
        {
            InitializeComponent();
            Title = title;
            Content = panel.element;
            SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}