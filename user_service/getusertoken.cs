﻿using Connection;
using Dna;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace userserver
{
    class getusertoken : myservice<q_getusertoken>
    {
        public async override Task<answer> getanswer(q_getusertoken question)
        {
            await Task.CompletedTask;
            var user = db.findone(i => i.callerid == question.callerid);
            if (user == null || user.activecode != question.activecode)
                return new q_getusertoken.invalidactivecode();
            if (!user.active)
            {
                user.active = true;
                s.db.upsert(user, true);
                notify(e_chromosome.profile);
            }
            return new q_getusertoken.done()
            {
                token = user.token,
                user = user.id
            };
        }
    }
}