﻿using Connection;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contact_server
{
    class p
    {
        public static async Task<s_user> get_user(long user)
        {
            var dv = await server.q(new q_load() { userid = user }) as q_load.done;
            return dv.user;
        }
        public static async Task<s_user[]> getusers(string user_name_filter, params long[] user)
        {
            var dv = await server.q(new q_loadalluser()
            {
                name_filter = user_name_filter,
                ids_filter = user
            }) as q_loadalluser.done;
            return dv.users;
        }
    }
}