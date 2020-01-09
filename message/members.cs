using controllibrary;
using Dna.userdata;
using localdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace message
{
    class members : uiapp
    {
        StackPanel panel = new StackPanel() { Background = Brushes.LightGray };
        public override FrameworkElement element => panel;
        syncdb<s_fulluser, member> syncdb;
        public override void create(long userid)
        {
            syncdb = new syncdb<s_fulluser, member>(userid);
            syncdb.add_e += Syncdb_add_e;
            syncdb.list.reset_e += List_reset_e;
            syncdb.search(i => i.contact.minvalid && i.id != userid);
        }
        private void Syncdb_add_e(member obj)
        {
            obj.click_e += Obj_click_e;
        }
        public event Action<long> select_e;
        private void Obj_click_e(member obj)
        {
            foreach (member i in panel.Children)
            {
                i.open(i == obj);
            }
            select_e?.Invoke(obj.id);
        }
        private void List_reset_e(ocollection<member> obj)
        {
            panel.Children.Clear();
            var dv = syncdb.list.ToArray();
            foreach (var i in dv)
            {
                if (i.fulluser.contact.ownersetting == e_contactsetting.favorite)
                    panel.Children.Add(i);
            }
            foreach (var i in dv)
            {
                if (i.fulluser.contact.ownersetting == e_contactsetting.connect)
                    panel.Children.Add(i);
            }
            foreach (var i in dv)
            {
                if (i.fulluser.contact.ownersetting == e_contactsetting.disconnect)
                    panel.Children.Add(i);
            }
        }
    }
}