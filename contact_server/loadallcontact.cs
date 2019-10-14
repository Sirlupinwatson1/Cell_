﻿using Connection;
using Dna;
using Dna.contact;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contact_server
{
    class loadallcontact : myservice<q_loadallcontact>
    {
        public async override Task<answer> getanswer(q_loadallcontact question)
        {
            nullcheck(ref question.fullnamefilter);
            var db = dbcontact(question.z_user);
            s_contact[] rt = null;
            if (question.mysettingfilter == e_connectionsetting.none)
            {
                var dv = await mainserver.q(new q_loadalluser() { name_filter = question.fullnamefilter }) as q_loadalluser.done;
                rt = join(dv.users, db.FindAll().ToArray());
            }
            else
            {
                var dv = db.FindAll();
                if (question.mysettingfilter != e_connectionsetting.any)
                    dv = db.Find(i => i.mysetting == question.mysettingfilter);
                var users = await p.getusers(question.fullnamefilter, dv.Select(i => i.partner).ToArray());
                rt = join(users, dv.ToArray());
            }
            return new q_loadallcontact.done()
            {
                contacts = rt
            };
        }
        s_contact[] join(s_user[] users, r_contact[] contacts)
        {
            List<s_contact> l = new List<s_contact>();
            foreach (var i in users)
            {
                var contact = contacts.FirstOrDefault(j => j.partner == i.id);
                l.Add(new s_contact()
                {
                    id = contact?.contact ?? 0,
                    partner = i,
                    mysetting = contact?.mysetting ?? e_connectionsetting.none
                });
            }
            return l.ToArray();
        }
    }
}