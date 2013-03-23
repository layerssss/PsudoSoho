using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoclassingStatic
{
    public class Services
    {
        [JSoonLib.Action]
        public void PullFromDev(string hash, string src, string filename)
        {
            GCServiceBase.Validator.Validate(hash);
            var wc = new System.Net.WebClient();
            wc.DownloadFile(string.Format("http://dev.goclassing.com/Dev/GetFile?hash={0}&path={1}",
                HttpUtility.UrlEncode(hash),
                HttpUtility.UrlEncode(src)),
                HttpContext.Current.Server.MapPath("~/App_Data/" + filename));
            var url = "";
            url=string.Format("http://dev.goclassing.com/Dev/DelFile?hash={0}&path={1}",
                    HttpUtility.UrlEncode(hash),
                    HttpUtility.UrlEncode(src));
                wc.DownloadString(url);
        }
        [JSoonLib.Action]
        public void DownloadFile(string filehash, string filename, string alias, string contentType = "application/octet-stream")
        {
            GCServiceBase.Validator.Validate(filehash, filename);
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.ContentType = contentType;
            if (alias.Length > 0)
            {
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + alias.Replace(' ','-'));
            }
            HttpContext.Current.Response.WriteFile(HttpContext.Current.Server.MapPath("~/App_Data/") + filename);
            HttpContext.Current.Response.End();
        }
        [JSoonLib.Action]
        public void Echo(string ping, out string reply)
        {
            reply = ping;
            throw (new Exception("bah!"));
        }
    }
}