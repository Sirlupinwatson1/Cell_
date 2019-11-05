﻿using Connection;
using Dna;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace userserver
{
    class server : mainserver
    {
        public server()
        {
            ini();
        }
        public override service[] elements
        {
            get
            {
                return new service[]
                {
                    new getchromosome(),
                    new getusertoken(),
                    new login(),
                    new load(),
                    new loadalluser(),
                    new sendactivecode(),
                    new getservertoken(),
                    new rename(),
                    new loadentity()
                };
            }
        }
        public override byte[] privatekey => resource.user_private_key;
        public override IPEndPoint endpoint => new IPEndPoint(reference.localip(), 10001);
        public override string password => "kfkbfkbfmbmgkbkcmbmfmbkf";

        public override e_chromosome chromosome => e_chromosome.user;

        private void ini()
        {
            s.db.upsert(new r_user()
            {
                fullname = "firstuser",
                general = false,
                id = 1000 * 1000 * 100
            }, false);
            createitem(e_chromosome.user, "kfkbfkbfmbmgkbkcmbmfmbkf");
            createitem(e_chromosome.profile, "kgjjjfjbjvjcnvjfjbkndfjbjcnbjcn");
            createitem(e_chromosome.usercontact, "mgjdjbjdbkdbkgfvjdjdnvbjdmd");
        }
        private static void createitem(e_chromosome chromosome, string password)
        {
            myservice<q_getusertoken>.dbserverinfo.Upsert(new r_serverinfo()
            {
                id = (int)chromosome,
                password = password
            });
        }
    }
}