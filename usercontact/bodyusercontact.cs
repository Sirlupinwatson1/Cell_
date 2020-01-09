using controllibrary;
using Dna.userdata;
using localdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usercontact
{
    class bodyusercontact : grid
    {
        textheder hdr_fullname = new textheder("Contact");
        columntext fullname = new columntext("user.fullname", style.textblock(), style.textbox());
        comboheder hdr_mysetting = new comboheder("My Settings");
        columncombo mysetting = new columncombo("ownersetting", new adapter(), style.combobox(), style.combobox());
        comboheder hdr_partnersetting = new comboheder("Partner settings");
        columncombo partnersetting = new columncombo("contact.partnersetting", new adapter(), style.combobox(false), style.combobox(false));
        syncdb<s_fulluser, item> db;
        public override void create(long userid)
        {
            add(hdr_fullname, fullname);
            add(hdr_mysetting, mysetting);
            add(hdr_partnersetting, partnersetting);
            db = new syncdb<s_fulluser, item>(userid);
            body.ItemsSource = db.list;
        }
        protected override void resetsearch(bool online)
        {
            db.search(i => check(i));
        }
        bool check(s_fulluser fulluser)
        {
                if (fulluser.user.fullname.Contains(hdr_fullname.text))
                    return true;
            return false;
        }
    }
}