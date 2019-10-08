﻿using Connection;
using Dna;
using Dna.contact;
using Dna.message;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace messanger
{
    public partial class Form1 : Form
    {
        LiteDatabase db = new LiteDatabase(reference.root("messanger.db"));
        public Form1()
        {
            InitializeComponent();
            FormClosing += Form1_FormClosing;
            txt_chat.ReadOnly = true;
            txt_id.TextChanged += Txt_id_TextChanged;
            txt_partner.TextChanged += Txt_partner_TextChanged;
            txt_id.KeyDown += Txt_id_KeyDown;
            txt_partner.KeyDown += Txt_partner_KeyDown;
            txt_send.KeyDown += Txt_send_KeyDown;
        }
        private void Txt_partner_TextChanged(object sender, EventArgs e)
        {
            run(() =>
            {
                txt_send.Enabled = false;
                txt_send.Text = null;
                txt_partner.BackColor = Color.LightPink;
                partner = contact = default;
                txt_chat.Text = null;
                last_index = 0;
            });
        }
        async void Txt_send_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_send.Text != "")
            {
                e.SuppressKeyPress = true;
                txt_send.Enabled = false;
                var dv = await client.question(new q_send()
                {
                    contact = contact,
                    text = txt_send.Text
                });
                txt_send.Text = null;
            }
        }
        void add(s_message message)
        {
            run(() =>
            {
                if (message.sender == user)
                {
                    txt_chat.SelectionColor = Color.Brown;
                }
                else
                {
                    txt_chat.SelectionColor = Color.Black;
                }

                txt_chat.SelectionFont = new Font("Tahom", 10, FontStyle.Bold);
                txt_chat.AppendText(message.sender + " : ");

                txt_chat.SelectionFont = new Font("Tahom", 10, FontStyle.Regular);
                txt_chat.AppendText(message.text + "\r\n");

                last_index = message.id;
                txt_chat.ScrollToCaret();
            });
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            close_db();
        }
        int last_index = default;
        SemaphoreSlim load_lock = new SemaphoreSlim(1, 1);
        async Task load()
        {
            if (contact == 0)
                return;
            await load_lock.WaitAsync();
            var dv = await client.question(new q_receive()
            {
                contact = contact,
                first_index = last_index + 1
            }) as q_receive.done;
            foreach (var i in dv.messages)
            {
                db_message.Upsert(i);
                add(i);
            }
            load_lock.Release();
            run(ready_to_send);
        }
        void load_local()
        {
            var dv = db_message.FindAll().ToArray();
            foreach (var i in dv)
                add(i);
        }
        private void close_db()
        {
            db?.Dispose();
            db = null;
        }
        long contact = default;
        async void Txt_partner_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_partner.BackColor == Color.LightPink)
            {
                if (!long.TryParse(txt_partner.Text, out partner))
                    return;
                e.SuppressKeyPress = true;
                txt_partner.Enabled = false;
                s_contact dv = null;// db_contact.FindOne(i => i.included(user, partner));
                if (dv == null)
                {
                    var rsv = await client.question(new q_upsert()
                    {
                        partner = partner
                    }) as q_upsert.done;
                    db_contact.Insert(rsv.contact);
                    dv = rsv.contact;
                }
                contact = dv.id;
                load_local();
                await load();
                run(send_pro);
                txt_partner.Enabled = true;
            }
        }
        private void send_pro()
        {
            txt_partner.Enabled = false;
            txt_partner.BackColor = Color.LightBlue;
            ready_to_send();
            Console.Beep();
        }

        private void ready_to_send()
        {
            txt_send.Enabled = true;
            txt_send.Focus();
        }

        private LiteCollection<s_contact> db_contact => db.GetCollection<s_contact>();
        private LiteCollection<s_message> db_message => db.GetCollection<s_message>(contact.ToString());

        client client = default;
        private void Txt_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_id.BackColor == Color.LightPink)
            {
                if (!long.TryParse(txt_id.Text, out user))
                    return;
                txt_id.Enabled = false;
                e.SuppressKeyPress = false;
                client = new client(txt_id.Text);
            }
        }
        void Client_login_e(client obj)
        {
            client.notify_e += Client_notify_e;
            client.active_notify(chromosome.message);
            client.reconnect_e += Client_reconnect_e;
            run(auto);
        }
        private async void auto()
        {
            var dv = await client.question(new Dna.contact.q_loadallcontact()) as q_loadallcontact.done;
            //txt_partner.AutoCompleteCustomSource.AddRange(dv.contacts.Select(i => i.another(user).ToString()).ToArray());
            partner_change();
        }

        async void Client_reconnect_e(chromosome obj)
        {
            if (obj == chromosome.message)
                await load();
        }

        private void partner_change()
        {
            txt_id.Enabled = true;
            txt_id.BackColor = Color.LightBlue;
            txt_partner.Enabled = true;
            Console.Beep();
            txt_partner.Focus();
        }
        void Client_notify_e(notify obj)
        {
            if (obj is n_message rsv)
            {
                if (rsv.contact != contact)
                    return;
                load().Wait();
                Console.Beep(4000, 100);
            }
        }
        private void Txt_id_TextChanged(object sender, EventArgs e)
        {
            user_change();
        }

        long user = default;
        long partner = default;
        private void run(Action action)
        {
            Invoke(new Action(action));
        }

        void user_change()
        {
            client?.close();
            client = null;
            user = partner = contact = default;
            last_index = default;
            txt_id.BackColor = Color.LightPink;
            txt_partner.Text = null;
            txt_partner.BackColor = Color.LightPink;
            txt_partner.Enabled = false;
            txt_chat.Text = null;
            txt_send.Text = null;
            txt_send.Enabled = false;
            last_index = 0;
        }
    }
}