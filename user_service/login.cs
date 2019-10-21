﻿using Connection;
using Dna;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user_service
{
    class login : myservice<q_login>
    {
        public async override Task<answer> getanswer(q_login request)
        {
            await Task.CompletedTask;
            var dv = dbuser.FindOne(i => i.token == request.token);
            if (dv == null)
                return new q_login.invalid();
            else
                return new q_login.done() { user = dv.clone() };
        }
    }
}