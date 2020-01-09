using stemcell;
using controllibrary;
using Dna.user;
using Dna.userdata;
using localdb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace searchuser
{
    public class bodysearch : grid
    {
        lableheder hdr_contact = new lableheder() { MinWidth = 40 };
        columncheck clm_contact = new columncheck(nameof(item.contact), style.checkbox(), style.checkbox());

        textheder hdr_fullname = new textheder("Full name");
        columntext clm_fulname = new columntext(nameof(item.fullname), style.textblock(), style.textbox());

        textheder hdr_tell = new textheder("Phone");
        columntext clm_code = new columntext(nameof(item.tell), style.textblock(), style.textbox());

        textheder hdr_city = new textheder("City");
        columntext clm_city = new columntext(nameof(item.city), style.textblock(), style.textbox());

        textheder hdr_des = new textheder("Description") { MinWidth = 200 };
        columntext clm_des = new columntext(nameof(item.description), style.textblock(), style.textbox());


        public bodysearch()
        {
            body.SelectionMode = DataGridSelectionMode.Single;
            add(hdr_contact, clm_contact);
            add(hdr_fullname, clm_fulname);
            add(hdr_des, clm_des);
            add(hdr_city, clm_city);
            add(hdr_tell, clm_code);
        }
        long userid = default;
        dbend<s_fulluser> syncdb;
        public override void create(long userid)
        {
            this.userid = userid;
            syncdb = new dbend<s_fulluser>(userid);
            syncdb.update_e += Syncdb_update_e;
            syncdb.delete_e += Syncdb_delete_e;
            body.ItemsSource = items;
        }

        private void Syncdb_delete_e(long obj)
        {
            var dv = items.FirstOrDefault(i => i.id == obj);
            if (dv == null)
                return;
            dv.updatecontact(false);
            items.reset();
        }

        private void Syncdb_update_e(s_fulluser obj)
        {
            var dv = items.FirstOrDefault(i => i.id == obj.id);
            if (dv == null)
                return;
            dv.update(userid, obj.user);
            dv.updatecontact(obj.contact.valid);
            items.reset();
        }

        ocollection<item> items = new ocollection<item>();
        protected async override void resetsearch(bool online)
        {
            if (!online)
                return;
            loadbox.mainwaiting();
            var rsv = await client.question(userid, new q_searchuser()
            {
                fullname = hdr_fullname.text,
                city = hdr_city.text,
                description = hdr_des.text,
                tell = hdr_tell.text
            }) as q_searchuser.done;
            items.Clear();
            foreach (var i in rsv.users)
            {
                var dv = new item();
                dv.update(userid, i);
                items.Add(dv);
            }
            syncdb.search(i => NewMethod(i, rsv));
            loadbox.mainrelease();
        }

        private static bool NewMethod(s_fulluser i, q_searchuser.done rsv)
        {
            return rsv.users.Any(j => i.id == j.id);
        }
    }
}