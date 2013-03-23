using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
namespace GoclassingDevClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new WebClient();
            c.Encoding = Encoding.UTF8;
            FileInfo f = new FileInfo(args[0]);
            var git = f.Directory;
            while (!git.GetDirectories(".git").Any())
            {
                git = git.Parent;
            }
            Console.WriteLine(c.DownloadString(
                string.Format(
                "http://dev.goclassing.com/Dev/UpdateFile?reponame={0}&filename={1}&hashString={2}",
                git.Name,
                f.FullName.Substring(git.FullName.Length).TrimStart('\\').Replace('\\','/'),
                GCServiceBase.Validator.GetHash())));
        }
    }
}
