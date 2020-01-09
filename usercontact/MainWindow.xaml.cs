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

namespace usercontact
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string titel = "Contacts";
        panel<bodyusercontact> panel = new panel<bodyusercontact>(titel);
        public MainWindow()
        {
            InitializeComponent();
            Title = titel;
            Content = panel.element;
            SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}
