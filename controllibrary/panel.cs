using stemcell;
using localdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;

namespace controllibrary
{
    public class panel<T> : uibase where T : uiapp, new()
    {
        StackPanel stack = new StackPanel();
        userselector userselector;
        loadbox loadbox = new loadbox();
        public override FrameworkElement element => stack;

        Mutex mutex = new Mutex(false, "caaa");
        public panel(string text)
        {
            while (mutex.WaitOne(0))
            {
                mutex.ReleaseMutex();
                MessageBox.Show("Please open the central application.");
            }
            userselector = new userselector(text, userselect);
            alluser.addremove_e += Alluser_remove_e;
            stack.Children.Add(userselector.element);
            stack.Children.Add(loadbox.element);
        }
        private void Alluser_remove_e(bool state, long obj)
        {
            if (!state)
            {
                var dv = list.FirstOrDefault(i => i.userid == obj);
                if (dv != null)
                {
                    dv.close();
                    list.Remove(dv);
                }
            }
        }
        List<T> list = new List<T>();
        private void userselect(long userid)
        {
            loadbox.child = null;
            if (userid == 0)
                return;
            var uiapp = list.FirstOrDefault(i => i.userid == userid);
            if (uiapp == null)
            {
                uiapp = new T();
                uiapp.create_(userid);
                list.Add(uiapp);
            }
            loadbox.child = uiapp.element;
        }
    }
}