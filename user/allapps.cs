using stemcell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace user
{
    class allapps
    {
        const string baseroot = @"rulingcode/Cell_";
        static string root = "";
        const string dll = baseroot + @"/abcd/bin/Debug/netstandard2.0";
        const string exe = baseroot + @"/abcd/bin/Debug";
        public static void start()
        {
            root = Assembly.GetAssembly(typeof(allapps)).CodeBase.Replace(@"file:///", "");
            if (!root.Contains(@"Repos/" + baseroot))
                return;
            root = root.Split(new string[] { baseroot }, StringSplitOptions.RemoveEmptyEntries)[0];

            copystandard("stemcell", ".dll");
            copystandard("Dna", ".dll");
            copystandard("localdb", ".dll");

            copynet("controllibrary", ".dll");
            copynet("message", ".exe");
            copynet("profile", ".exe");
            copynet("searchuser", ".exe");
            copynet("usercontact", ".exe");
            copynet("user", ".exe");
        }
        static void copystandard(string val, string pas)
        {
            var source = root + dll.Replace("abcd", val) + @"/" + val + pas;
            var end = reference.root(val + pas, "allapps");
            File.WriteAllBytes(end, File.ReadAllBytes(source));
        }
        static void copynet(string val, string pas)
        {
            var source = root + exe.Replace("abcd", val) + @"/" + val + pas;
            var end = reference.root(val + pas, "allapps");
            File.WriteAllBytes(end, File.ReadAllBytes(source));
        }
    }
}