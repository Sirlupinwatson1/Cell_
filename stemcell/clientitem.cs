﻿using core;
using Dna;
using Dna.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace stemcell
{
    public class clientitem
    {
        internal bool inp = default;
        public readonly string chromosome;
        public readonly long userid;
        internal bool isnotifier;
        public clientitem(string chromosome, long userid)
        {
            this.chromosome = chromosome;
            this.userid = userid;
        }
        tcpclient tcpclient;
        SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        async Task<bool> login()
        {
            if (tcpclient != null)
                return false;
            var info = await basic.getchromosome(chromosome);
            var ipend = reference.getendpoint(info.endpoint);
            tcpclient = new tcpclient(ipend.Address, ipend.Port, info.publickey);
            if (userid != 0)
            {
                userlogin dv = await getlogin();
                var rsv = await question_(new q_login()
                {
                    token = dv.token,
                    notifier = isnotifier
                });
                if (!(rsv is q_login.done))
                {
                    Console.Beep(1000, 500);
                    throw new Exception("bkdkbmfbcmfmbmmbm");
                }
                return true;
            }
            return false;
        }
        async Task<userlogin> getlogin()
        {
            userlogin userlogin = null;
            userlogin = s.dbuserlogin.FindOne(i => i.id == userid);
            while (userlogin == null)
            {
                userlogin = s.dbuserlogin.FindOne(i => i.id == userid);
                if (userlogin == null)
                {
                    var dv = s.dbuserlogin.FindAll().ToArray();
                }
                await Task.Delay(200);
            }
            return userlogin;
        }
        public async Task<answer> question(question question)
        {
            try
            {
                await locker.WaitAsync();
                await login();
                var answer = await question_(question);
                locker.Release();
                return answer;
            }
            catch (Exception e)
            {
                var dv = e.Message;
                dv = null;
                Console.Beep();
                tcpclient?.close();
                tcpclient = null;
                locker.Release();
                return await this.question(question);
            }
        }
        const int notifyconst = -1;
        public async Task<int> getnotify()
        {
            try
            {
                await locker.WaitAsync();
                if (await login())
                {
                    locker.Release();
                    return notifyconst;
                }
                var dv = await tcpclient.getnotify();
                locker.Release();
                return dv;
            }
            catch (Exception e)
            {
                var dv = e.Message;
                dv = null;
                Console.Beep();
                tcpclient?.close();
                tcpclient = null;
                locker.Release();
                return await getnotify();
            }
        }
        public async void sendpulse()
        {
            try
            {
                await locker.WaitAsync();
                await login();
                tcpclient.sendpulse();
                locker.Release();
            }
            catch (Exception e)
            {
                var dv = e.Message;
                dv = null;
                Console.Beep();
                locker.Release();
                sendpulse();
            }
        }
        public virtual void close() { }
        public async Task<answer> question_(question question)
        {
            var data = converter.change(question);
            data = await tcpclient.question(data);
            var answer = converter.change(data) as answer;
            return answer;
        }
    }
}