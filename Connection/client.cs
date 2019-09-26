﻿using Dna;
using Dna.common;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connection
{
    public class client
    {
        questioner user_item = null;
        private readonly string user_name;
        public client(string user_name)
        {
            user_item = new questioner(this, reference.get_user_info());
            qlist.Add(user_item);
            send_pulse();
            this.user_name = user_name;
        }
        public event Func<Task<string>> password_e = null;
        internal async Task login_item(client_item core)
        {
            switch (core)
            {
                case questioner sw:
                    core = qlist.FirstOrDefault(i => i == sw);
                    break;
                case notifier sw:
                    core = nlist.FirstOrDefault(i => i == sw);
                    break;
            }
            if (core == null)
                return;
            if (core == user_item)
            {
                if (!await autologin())
                    while (true)
                    {
                        var info = await password_e.Invoke();
                        if (await login(info))
                            break;
                    }
                await create_items();
            }
            else
            {
                var td = s.load(user_name);
                var rsv = await user_item.question(new q_get_introcode()
                {
                    token = td.token,
                    divce = td.device
                }) as q_get_introcode.done;
                var rsv2 = await core.q(new q_intrologin()
                {
                    introcode = rsv.introcode,
                    accept_notifications = core is notifier
                });
                if (!(rsv2 is q_intrologin.done))
                    throw new Exception("lvkdlbmfkvkxmkblcc");
            }
        }
        public event Action<chromosome> reconnect_e;
        internal void reconnect(client_item client_item)
        {
            if (!closeF)
                reconnect_e?.Invoke(client_item.info.chromosome);
        }
        async Task<bool> autologin()
        {
            var dv = s.load(user_name);
            if (dv == null)
                return false;
            else
            {
                var rsv = await user_item.q(new q_autologin()
                {
                    divice = dv.device,
                    token = dv.token
                });
                switch (rsv)
                {
                    case q_autologin.done done:
                        {
                            z_user = done.id;
                            return true;
                        }
                    case q_autologin.invalid_token invalid:
                        {
                            s.remove(user_name);
                            return false;
                        }
                }
                throw new Exception("kgjdhhdhvdhjdhbjsjghfgs");
            }
        }

        TaskCompletionSource<s_chromosome_info[]> completionSource = new TaskCompletionSource<s_chromosome_info[]>();
        public async Task<s_chromosome_info> infos(string chromosome)
        {
            if (chromosome == Dna.chromosome.user.ToString())
                return reference.get_user_info();
            var dv = await completionSource.Task;
            return dv.First(i => i.chromosome.ToString() == chromosome);
        }

        bool ready_items = false;
        public event Action<client> login_e;
        async Task create_items()
        {
            if (ready_items)
                return;
            ready_items = true;
            var rsv = await user_item.q(new q_get_chromosome_info());
            if (!(rsv is q_get_chromosome_info.done done))
                throw new Exception("lbjjbnfjbjcjdjbkckb,fd");
            completionSource.SetResult(done.items);
            invoke_login_e();
        }

        async void invoke_login_e()
        {
            await Task.CompletedTask;
            login_e?.Invoke(this);
        }

        public event Action<notify> notify_e;
        internal void notify(notify notify)
        {
            notify_e?.Invoke(notify);
        }
        SemaphoreSlim qlocking = new SemaphoreSlim(1, 1);
        public async Task<answer> question(question question)
        {
            await qlocking.WaitAsync();
            var dv = qlist.FirstOrDefault(i => i.info.chromosome.ToString() == question.z_chromosome);
            if (dv == null)
            {
                dv = new questioner(this, await infos(question.z_chromosome));
                qlist.Add(dv);
            }
            qlocking.Release();
            return await dv.question(question);
        }
        List<notifier> nlist = new List<notifier>();
        SemaphoreSlim nlocking = new SemaphoreSlim(1, 1);
        public async void active_notify(chromosome chromosome)
        {
            await nlocking.WaitAsync();
            var dv = nlist.FirstOrDefault(i => i.info.chromosome == chromosome);
            if (dv == null)
                nlist.Add(new notifier(this, await infos(chromosome.ToString())));
            nlocking.Release();
        }
        async void send_pulse()
        {
            await nlocking.WaitAsync();
            foreach (var i in nlist)
                i.send();
            nlocking.Release();
            await Task.Delay(1000);
            send_pulse();
        }

        List<questioner> qlist = new List<questioner>();
        public long z_user { get; private set; }
        async Task<bool> login(string password)
        {
            Random random = new Random();
            var divce = random.NextDouble();
            var rsv = await user_item.q(new q_login()
            {
                user_name = user_name,
                divce = divce,
                password = password
            });
            switch (rsv)
            {
                case q_login.done sw:
                    {
                        z_user = sw.id;
                        s.save(new token_device()
                        {
                            user_name = user_name,
                            device = divce,
                            token = sw.token
                        });
                        return true;
                    }
                case q_login.invalid sw:
                    {
                        return false;
                    }
            }
            throw new Exception("lgjcjjbjcdjbkdfjkvkdjgj");
        }
        internal bool closeF = false;
        public void close()
        {
            foreach (var i in qlist)
            {
                i.close();
            }
            closeF = true;
        }
    }
}