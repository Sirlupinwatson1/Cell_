using controllibrary;
using Dna.message;
using localdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace message
{
    class app : uiapp
    {
        long userid = 0;
        DockPanel panel = new DockPanel();
        members members = new members();
        loadbox loadbox = new loadbox();
        List<chatbox> list = new List<chatbox>();
        public override FrameworkElement element => panel;

        public override void create(long userid)
        {
            loadbox.element.Height = 500;
            this.userid = userid;
            members.create(userid);
            members.select_e += Members_select_e;
            panel.Children.Add(members.element);
            DockPanel.SetDock(members.element, Dock.Left);
            panel.Children.Add(loadbox.element);
            DockPanel.SetDock(loadbox.element, Dock.Right);

        }
        private void Members_select_e(long obj)
        {
            var dv = list.FirstOrDefault(i => i.partnerid == obj);
            if (dv == null)
            {
                dv = new chatbox(userid, obj);
                list.Add(dv);
            }
            loadbox.dialog(dv);
        }
    }
}