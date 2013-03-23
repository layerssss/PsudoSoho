using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
namespace GoclassingDev
{
    public class Dev
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="hash">The hash.</param>
        [ispJs.Action]
        public void GetFile(string path,string hash)
        {
            GCServiceBase.Validator.Validate(hash);
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.WriteFile("/srv/" + path);
            
            HttpContext.Current.Response.End();
        }
        [ispJs.Action]
        public void DelFile(string path, string hash)
        {
            GCServiceBase.Validator.Validate(hash);
            System.IO.File.Delete("/srv/" + path);
        }
        [ispJs.Action]
        public void Checkout( 
            //string hash,
            out string output, out string error)
        {
            output = "";
            error = "";
            var p = Process.Start(new ProcessStartInfo("/usr/bin/svn", " checkout http://dev.xunnlv.com/svn/service.gebimai.com/ /srv/service.gebimai.com --force")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
            p.WaitForExit();
            output += "service.gebimai.com:"+p.StandardOutput.ReadToEnd();
            error += "service.gebimai.com:" + p.StandardError.ReadToEnd(); 

            p = Process.Start(new ProcessStartInfo("/usr/bin/svn", " checkout http://dev.xunnlv.com/svn/Chifanshe.com/ /srv/chifanshe.com --force")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
            p.WaitForExit();
            output += "chifanshe.com:" + p.StandardOutput.ReadToEnd();
            error += "chifanshe.com:" + p.StandardError.ReadToEnd();
        }
        [ispJs.Action]
        public void GithubHook(string payload)
        {
            
            var ip=ispJs.WebApplication.Request.ServerVariables["REMOTE_ADDR"];
            var c=new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            if (ip != "207.97.227.253" && ip != "50.57.128.197" && ip != "108.171.174.178")
            {
                return;
            }
            try
            {
                var o = JObject.Parse(payload);

                var name = (string)o["repository"]["name"];
                foreach (var commit in o["commits"])
                {
                    foreach (var added in commit["added"])
                    {
                        var t = ((string)added);
                        UpdateFile(name, t, GCServiceBase.Validator.GetHash());
                    }
                    foreach (var modified in commit["modified"])
                    {
                        var t = ((string)modified);
                        UpdateFile(name, t, GCServiceBase.Validator.GetHash());
                    }
                }
                System.IO.File.Delete(ispJs.WebApplication.Server.MapPath("/error.txt"));

            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(ispJs.WebApplication.Server.MapPath("/error.txt"), string.Format(
                    "error:\r\n{0};payload:\r\n{1}",
                    ex.ToString(),
                    payload));

            }
        }
        [ispJs.Action]
        public void UpdateFile(string reponame, string filename,string hashString)
        {
            GCServiceBase.Validator.Validate(hashString);
            var c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            var path = "";
            var prefix = "";
            switch (reponame)
            {
                case "ispjs.org":
                    path = "/srv/ispjs.org/";
                    prefix = "ispJs.org/";
                    break;
                case "goclassing.com":
                    path = "/srv/goclassing.com/";
                    prefix = "";
                    break;
                case "zhixiang.in":
                    path = "/srv/zhixiang.in/";
                    prefix = "zhixiang.in/";
                    break;
                default:
                    throw(new Exception("repo not found "+reponame));
            }
            if (filename.StartsWith(prefix))
            {
                var t2 = filename.Substring(prefix.Length);
                try
                {
                    ispJs.Utility.CreateFolderFor(path + t2);
                }
                catch { }
                c.DownloadFile("https://raw.github.com/layerssss/"
                    + reponame
                    + "/master/"
                    + filename,
                    path + t2);
            }
            else
            {
            }
        }
    }
}