using stemcell;
using Dna;
using Dna.user;
using Dna.userdata;
using localdb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace user
{
    class userbody
    {
        public StackPanel panel = new StackPanel() { Width = 400 };
        ListBox lstaccounts = new ListBox() { MinHeight = 100 };
        Button btnlogout = new Button()
        {
            Content = "Sign out of selected account",
            Padding = new Thickness(8),
            MinWidth = 120,
            Margin = new Thickness(2)
        };
        TextBlock lbl = new TextBlock()
        {
            Padding = new Thickness(5),
            Margin = new Thickness(5)
        };
        TextBox txt = new TextBox() { Padding = new Thickness(5), FlowDirection = FlowDirection.LeftToRight };
        StackPanel btnpanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10),
            Height = 40
        };
        Button btnback = new Button()
        {
            Content = "Phone number correction",
            Padding = new Thickness(5),
            MinWidth = 120,
            Margin = new Thickness(0, 0, 5, 0)
        };
        Button btnsubmit = new Button()
        {
            Content = "Send",
            Padding = new Thickness(5),
            MinWidth = 120
        };
        public userbody()
        {
            panel.Margin = new Thickness(10);
            panel.Children.Add(lstaccounts);
            panel.Children.Add(btnlogout);
            lbl.Inlines.Add(subject);
            lbl.Inlines.Add(describtion);
            panel.Children.Add(lbl);
            panel.Children.Add(txt);
            btnpanel.Children.Add(btnback);
            btnpanel.Children.Add(btnsubmit);
            panel.Children.Add(btnpanel);
            txt.PreviewTextInput += Txtphone_PreviewTextInput;
            txt.TextChanged += Txt_TextChanged;
            txt.KeyDown += Txt_KeyDown;
            btnsubmit.Click += Btnsubmit_Click;
            lstaccounts.SelectionChanged += Lstaccounts_SelectionChanged;
            btnback.Click += Btnback_Click;
            btnlogout.Click += Btnlogout_Click;
            reset();
            reset2();
            lstaccounts.ItemsSource = alluser.list;
        }
        void Btnlogout_Click(object sender, RoutedEventArgs e)
        {
            basic.logout(selected.id);
        }
        private void Btnback_Click(object sender, RoutedEventArgs e)
        {
            var dv = phone;
            phone = null;
            txt.Text = dv;
            txt.CaretIndex = txt.Text.Length;
        }
        s_user selected => lstaccounts.SelectedValue as s_user;
        private void Lstaccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reset2();
        }
        private void reset2()
        {
            btnlogout.IsEnabled = selected != null;
        }
        async void Btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            btnpanel.IsEnabled = false;
            switch (state)
            {
                case e_state.submitphone:
                    {
                        await basic.sendactivecode(txt.Text);
                        phone = txt.Text;
                        txt.Text = null;
                    }
                    break;
                case e_state.submitcode:
                    {
                        var dv = await basic.login(phone, txt.Text);
                        if (dv)
                        {
                            phone = null;
                            txt.Text = null;
                        }
                        else
                        {
                            MessageBox.Show("Activation code not entered correctly");
                        }
                    }
                    break;
            }
            reset();
            btnpanel.IsEnabled = true;
        }
        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Btnsubmit_Click(null, null);
        }
        private void Txtphone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (phone == null)
            {
                long val;
                if (!long.TryParse(e.Text, out val))
                {
                    e.Handled = true;
                    SystemSounds.Asterisk.Play();
                }
                if (txt.Text.Length >= 11)
                    e.Handled = true;
            }
            else
            {
                long val;
                if (!long.TryParse(e.Text, out val))
                {
                    e.Handled = true;
                    SystemSounds.Asterisk.Play();
                }
                if (txt.Text.Length >= 5)
                    e.Handled = true;
            }
        }

        string phone = default;
        void reset()
        {
            subject.Text = describtion.Text = null;
            if (phone == null)
                phonepro();
            else
                activecodepro();
        }

        Run subject = new Run() { FontWeight = FontWeights.Bold };
        Run describtion = new Run() { FontWeight = FontWeights.Normal };
        private void activecodepro()
        {
            subject.Text = "Activation code : ";
            describtion.Text = "This code will be sent to your mobile in seconds.";
            btnback.IsEnabled = true;
            btnback.Content = "Phone number correction";
            txt.Focus();
            if (txt.Text.Length == 5)
            {
                btnsubmit.IsEnabled = true;
                btnsubmit.Content = "login";
                state = e_state.submitcode;
            }
            else
            {
                btnsubmit.IsEnabled = true;
                btnsubmit.Content = null;
                state = e_state.none;
            }
        }
        enum e_state
        {
            submitphone,
            submitcode,
            none
        }
        e_state state = e_state.none;
        private void phonepro()
        {
            subject.Text = "Phone number : ";
            describtion.Text = "Add new account via mobile number";
            btnback.IsEnabled = false;
            btnback.Content = null;
            txt.Focus();
            if (txt.Text.Length == 11)
            {
                btnsubmit.Content = "Send activation code";
                btnsubmit.IsEnabled = true;
                state = e_state.submitphone;
            }
            else
            {
                btnsubmit.Content = null;
                btnsubmit.IsEnabled = false;
                state = e_state.none;
            }
        }
        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            reset();
        }
    }
}