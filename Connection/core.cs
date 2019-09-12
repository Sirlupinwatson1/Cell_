﻿using Converter;
using Dna;
using Dna.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connection
{
    public class core
    {
        static converter converter = new converter();
        internal byte[] main_key = null;
        internal byte[] key32 = null;
        internal byte[] iv16 = null;
        internal core(byte[] main_key)
        {
            this.main_key = main_key;
        }

        public TcpClient tcp = null;
        internal async void write(gene gene)
        {
            try
            {
                if (gene == null)
                    await tcp.GetStream().WriteAsync(new byte[4], 0, 4);
                else
                {
                    var data = converter.change(gene);
                    if (key32 != null)
                        data = crypto.Encrypt(data, key32, iv16);
                    data = Combine(BitConverter.GetBytes(data.Length), data);
                    await tcp.GetStream().WriteAsync(data, 0, data.Length);
                }
            }
            catch
            {
                tcp?.Close();
            }
        }

        static int n = 0;
        void create_error()
        {
            n++;
            if (n % 40 == 0)
            {
                Console.Beep();
                throw new Exception("create error");
            }
        }
        internal async Task<gene> read()
        {
            
            var data = new byte[4];
            await tcp.GetStream().ReadAsync(data, 0, data.Length);
            var len = BitConverter.ToInt32(data, 0);
            if (len == 0)
                return new void_answer();
            data = new byte[len];
            await tcp.GetStream().ReadAsync(data, 0, len);
            create_error();
            if (key32 != null)
                data = crypto.Decrypt(data, key32, iv16);
            var dv = converter.change(data) as gene;
            return dv;
        }
        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}