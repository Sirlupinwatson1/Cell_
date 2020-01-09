using stemcell;
using Dna.user;
using Dna.userdata;
using localdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace controllibrary
{
    public class userselector : uibase
    {
        StackPanel panel = new StackPanel()
        {
            Margin = new Thickness(20, 0, 20, 20)
        };
        TextBlock heder = new TextBlock() { Padding = new Thickness(10) };
        public override FrameworkElement element => panel;
        Label lable = new Label();
        ComboBox combo = new ComboBox() { MinWidth = 400, Padding = new Thickness(5) };
        private readonly Action<long> userselect;
        public userselector(string text, Action<long> userselect)
        {
            this.userselect = userselect;
            heder.Inlines.Add(new Run()
            {
                FontWeight = FontWeights.Bold,
                FontSize = 17,
                Text = text + " : "
            });
            heder.Inlines.Add(new Run()
            {
                FontWeight = FontWeights.Regular,
                FontSize = 12,
                Text = "User selection"
            });
            panel.Children.Add(heder);
            combo.SelectionChanged += Combo_SelectionChanged;
            panel.Children.Add(combo);
            lable.Content = "User selection : ";
            combo.ItemsSource = alluser.list;
            alluser.list.reset_e += List_reset_e;
            autoselect();
        }

        private void List_reset_e(ocollection<s_user> obj)
        {
            run(refresh);
        }
        private void autoselect()
        {
            if (selected == null)
                combo.SelectedItem = alluser.list.FirstOrDefault();
        }
        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inp)
                return;
            var dv = selected;
            if (dv == null)
                userselect?.Invoke(0);
            else
                userselect?.Invoke(dv.id);
        }
        bool inp = false;
        void refresh()
        {
            autoselect();
            var dv = selected;
            inp = true;
            combo.ItemsSource = null;
            combo.ItemsSource = alluser.list;
            inp = false;
            combo.SelectedItem = dv;
        }
        public s_user selected => combo.SelectedValue as s_user;
    }
}