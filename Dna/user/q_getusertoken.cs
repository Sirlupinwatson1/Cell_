﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dna.user
{
    public class q_getusertoken : question
    {
        public string callerid = default;
        public string activecode = default;
        public override e_permission z_permission => e_permission.free;
        public class done : answer
        {
            public long token = default;
            public long user = default;
        }
        public class invalidactivecode : answer { }
    }
}