﻿using Dna;
using Dna.common;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connection
{
    class client_side : core
    {
        internal readonly chromosome chromosome;
        private readonly IPEndPoint endPoint;
        internal client client = null;
        public client_side(chromosome chromosome, IPEndPoint endPoint, byte[] main_key) : base(main_key)
        {
            this.chromosome = chromosome;
            this.endPoint = endPoint;
        }
        public async Task connect()
        {
            tcp = new TcpClient();
            await tcp.ConnectAsync(endPoint.Address, endPoint.Port);
            var keys = crypto.create_symmetrical_keys();
            write(new f_set_key()
            {
                key32 = crypto.Encrypt(keys.key32, main_key),
                iv16 = crypto.Encrypt(keys.iv16, main_key)
            });
            await read();
            key32 = keys.key32;
            iv16 = keys.iv16;
            if (!(chromosome == chromosome.user || chromosome == chromosome.central))
            {
                if (!(await client.question(new f_get_introcode()) is f_get_introcode.done rsv))
                    throw new Exception("kfkbhdhbjgkxlsmjfcks");

                write(new f_set_introcode() { introcode = rsv.introcode });

                if (!(await read() is f_set_introcode.done))
                    throw new Exception("lgkdkbmrfjjcksmbmbkhfd");
            }
            reading();
        }
        public event Action<notify> notify_e;
        request_task sent = null;
        async void reading()
        {
            var rsv = await read();
            if (rsv == null)
                return;
            if (rsv is notify dv)
            {
                notify_e?.Invoke(dv);
            }
            else
            {
                await locking.WaitAsync();
                if (sent == null)
                    throw new Exception("lgjfjbjdfjbhhfhvhc");
                sent.task.SetResult(rsv as response);
                sent = null;
                send();
                locking.Release();
            }
            reading();
        }
        class request_task
        {
            public request request = null;
            public TaskCompletionSource<response> task = new TaskCompletionSource<response>();
        }
        List<request_task> list = new List<request_task>();
        SemaphoreSlim locking = new SemaphoreSlim(1, 1);
        public async Task<response> question(request request)
        {
            var dv = new request_task()
            {
                request = request,
            };
            await locking.WaitAsync();
            if (tcp == null)
                await connect();
            list.Add(dv);
            send();
            locking.Release();
            return await dv.task.Task;
        }
        void send()
        {
            if (sent != null || list.Count == 0)
                return;
            sent = list.First();
            list.Remove(sent);
            write(sent.request);
        }
    }
}