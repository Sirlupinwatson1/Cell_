using stemcell;
using controllibrary;
using Dna.user;
using Dna.userdata;
using localdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace profile
{
    class profilebody : uiapp
    {
        StackPanel panel = new StackPanel()
        {
            Margin = new Thickness(20, 0, 20, 20)
        };
        Label lblfullname = new Label() { Content = "Full name : " };
        TextBox txtfullname = new TextBox();

        Label lbldescribtion = new Label() { Content = "Description" };
        textbox txtdescribtion = new textbox();

        Label lblcity = new Label() { Content = "City : " };
        TextBox txtcity = new TextBox() { MaxLength = 20 };

        Label lblnationalcode = new Label() { Content = "National ID : " };
        TextBox txtnationalcode = new TextBox() { FlowDirection = FlowDirection.LeftToRight };

        Label lbltell = new Label() { Content = "Phone Call : " };
        TextBox txttell = new TextBox() { FlowDirection = FlowDirection.LeftToRight };

        Label lblgender = new Label() { Content = "nature : " };
        ComboBox cmbgender = new ComboBox()
        {
            ItemsSource = new string[] { "Unknown", "Man", "Female", "Business" }
        };

        Button btnsave = new Button()
        {
            Content = "Submit information",
            Padding = new Thickness(5),
            HorizontalAlignment = HorizontalAlignment.Center,
            MinWidth = 200
        };
        client client;
        long userid = default;
        syncdb<s_fulluser> syncdb;
        public override void create(long userid)
        {
            this.userid = userid;
            syncdb = new syncdb<s_fulluser>(userid);
            syncdb.notify(userid, reset);
            designing();
            client = new client(userid);
            btnsave.Click += Btnsave_Click;
        }

        s_user user = default;
        private void reset(s_fulluser obj)
        {
            user = obj.user;
            txtcity.Text = user.city;
            txtdescribtion.Text = user.description;
            txtfullname.Text = user.fullname;
            txtnationalcode.Text = user.nationalcode;
            txttell.Text = user.tell;
            cmbgender.SelectedIndex = (int)user.nature;
        }
        private void designing()
        {
            panel.Children.Add(lblfullname);
            panel.Children.Add(txtfullname);
            panel.Children.Add(lbldescribtion);
            panel.Children.Add(txtdescribtion);
            panel.Children.Add(lblcity);
            panel.Children.Add(txtcity);
            panel.Children.Add(lbltell);
            panel.Children.Add(txttell);
            panel.Children.Add(lblnationalcode);
            panel.Children.Add(txtnationalcode);
            panel.Children.Add(lblgender);
            panel.Children.Add(cmbgender);
            panel.Children.Add(btnsave);
            foreach (FrameworkElement i in panel.Children)
            {
                i.Margin = new Thickness(2);
            }
            btnsave.Margin = new Thickness(10);
        }
        async void Btnsave_Click(object sender, RoutedEventArgs e)
        {
            user.city = txtcity.Text;
            user.description = txtdescribtion.Text;
            user.fullname = txtfullname.Text;
            user.nationalcode = txtnationalcode.Text;
            user.nature = (e_nature)cmbgender.SelectedIndex;
            user.tell = txttell.Text;
            await save();
        }

        async Task save()
        {
            loadbox.mainwaiting();
            if (!await valid())
                return;
            var rsv = await client.question(new q_upsertuser() { user = user }) as q_upsertuser.done;
            if (rsv.error_duplicate)
                await messagebox.maindilog((null, "This name has already been registered. Please change it"));
            loadbox.mainrelease();
        }
        async Task<bool> valid()
        {
            string text = txtfullname.Text;
            if (text.Length < 5)
            {
                await messagebox.maindilog("The selected name is less than the allowed length");
                return false;
            }
            if (text.Length > 25)
            {
                await messagebox.maindilog("Selected name exceeds the maximum length");
                return false;
            }
            if (text.Contains("  "))
            {
                await messagebox.maindilog("Use only one space between words");
                return false;
            }
            if (text.First() == ' ' || text.Last() == ' ')
            {
                await messagebox.maindilog("Don't leave any space at the beginning or end of the name");
                return false;
            }
            foreach (var i in text)
            {
                if (i == ' ')
                    continue;
                if (!char.IsLetter(i))
                {
                    await messagebox.maindilog("The name uses an unauthorized character");
                    return false;
                }
            }
            return true;
        }
        public override FrameworkElement element => panel;
    }
}