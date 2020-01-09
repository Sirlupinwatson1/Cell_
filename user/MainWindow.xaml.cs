using stemcell;
using Dna;
using Dna.common;
using Dna.userdata;
using localdb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Threading;
using controllibrary;

namespace user
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        userbody body;
        Mutex mutex = new Mutex(false, "caaa");
        clientspeed clientspeed = new clientspeed(e_chromosome.user);
        public MainWindow()
        {
            if (!mutex.WaitOne(0))
            {
                mutex.ReleaseMutex();
                MessageBox.Show("Central application is open.");
                Close();
                return;
            }
            mutex.WaitOne();
            Closing += MainWindow_Closing;
            Directory.CreateDirectory(reference.root(""));
            Directory.CreateDirectory(reference.root("", "message"));
            Directory.CreateDirectory(reference.root("", "allapps"));
            InitializeComponent();
            Title = "Central";
            SizeToContent = SizeToContent.WidthAndHeight;
            ini();
            allapps.start();
            uibase.run(runing);
        }

        async void runing()
        {
            Title = "Central " + clientspeed.n + " pulse";
            await Task.Delay(500);
            uibase.run(runing);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mutex.ReleaseMutex();
        }

        async void ini()
        {
            alluser.addremove_e += Alluser_addremove_e;
            body = new userbody();
            Content = body.panel;
            await Task.Delay(2000);
            WindowState = WindowState.Minimized;
        }

        List<dbendcenter> dbs = new List<dbendcenter>();
        private void Alluser_addremove_e(bool arg1, long arg2)
        {
            if (arg1)
            {
                dbendcenter dv = new dbendcenter(arg2);
                dbs.Add(dv);
            }
            else
            {
                var dv = dbs.FirstOrDefault(i => i.userid == arg2);
                dv.close();
                dbs.Remove(dv);
            }
        }
    }
}