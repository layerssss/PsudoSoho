using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
namespace WebWaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            wc = new WebClient();
            if (args.Length == 2)
            {
                prefix = args[1];
            }

        }
        static WebClient wc;
        static string prefix;
        public void Visit(string url)
        {
            try
            {
                wc.Encoding = Encoding.UTF8;
                var str = wc.DownloadString(url);

            }
            catch(WebException ex) {
                Console.WriteLine("{0} {1}", ex.Status, url);
            }
        }
    }
}
