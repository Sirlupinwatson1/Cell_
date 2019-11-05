﻿using Dna;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userserver
{
    class load : myservice<q_loaduser>
    {
        public async override Task<answer> getanswer(q_loaduser question)
        {
            await Task.CompletedTask;
            var dv = db.load(question.userid);
            return new q_loaduser.done()
            {
                user = new s_user()
                {
                    id = dv.id,
                    fullname = dv.fullname
                }
            };
        }
    }
}