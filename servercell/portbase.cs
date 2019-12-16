﻿using Dna;
using Dna.user;
using stemcell;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace servercell
{
    abstract class portbase
    {
        public long userid { get; private set; }
        protected readonly TcpClient tcp;
        private readonly byte[] privatekey;
        private readonly bool decrypt;
        private readonly bool loginrequired;
        public portbase(TcpClient tcp, byte[] privatekey, bool decrypt, bool loginrequired = true)
        {
            this.tcp = tcp;
            this.privatekey = privatekey;
            this.decrypt = decrypt;
            this.loginrequired = loginrequired;
            login();
        }
        protected byte[] key32 { get; private set; }
        protected byte[] iv16 { get; private set; }
        async void login()
        {
            try
            {
                byte[] data = new byte[128];
                await tcp.GetStream().ReadAsync(data, 0, data.Length);
                data = crypto.Decrypt(data, privatekey);
                var token = BitConverter.ToInt64(data, 0);
                if (decrypt)
                {
                    key32 = crypto.split(data, 4, 32);
                    iv16 = crypto.split(data, 36, 16);
                }
                if (token == 0)
                {
                    if (loginrequired)
                        throw new Exception("fkdbfkbkfmbkgjbjfnjbnvjbmm");
                }
                else
                {
                    var dv = await mainserver.question(new q_login() { token = token }) as q_login.done;
                    if (dv.error_invalid)
                    {
                        writebyte(netid.invalidtoken);
                        await Task.Delay(100);
                        tcp.Close();
                        return;
                    }
                    userid = dv.userid;
                }
                writebyte(netid.login);
                start();
            }
            catch
            {
                tcp.Close();
            }
        }
        protected abstract void start();
        protected async Task<byte> receivebyte()
        {
            byte[] data = new byte[1];
            await tcp.GetStream().ReadAsync(data, 0, data.Length);
            return data[0];
        }
        protected void writebyte(byte data)
        {
            tcp.GetStream().WriteByte(data);
        }
    }
}