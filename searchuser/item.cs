using stemcell;
using controllibrary;
using Dna;
using Dna.userdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchuser
{
    public class item : s_user
    {
        bool contactf = default;
        long owner;
        public bool contact
        {
            get { return contactf; }
            set { set(value); }
        }
        public override void update(long owner, s_entity entity)
        {
            this.owner = owner;
            base.update(owner, entity as s_user);
        }
        public void updatecontact(bool val)
        {
            contactf = val;
        }
        async void set(bool value)
        {
            if (id == owner)
                return;
            if (value)
            {
                if (await messagebox.maindilog((null, addmessage), "Add", "Cancel") == 0)
                {
                    await client.question(owner, new q_upsertcontact() { partner = id, setting = e_contactsetting.connect });
                }
            }
            else
            {
                if (await messagebox.maindilog((null, removemessage), "Delete", "Cancel") == 0)
                {
                    await client.question(owner, new q_upsertcontact() { partner = id, setting = e_contactsetting.disconnect });
                }
            }
        }
        private string addmessage => "Are you sure you want to " + fullname + " add to your contact?";
        private string removemessage => "Are you sure you want to " + fullname + " remove from your contact list?";
    }
}
