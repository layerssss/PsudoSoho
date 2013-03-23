using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net;
using System.Threading;
using System.IO;
using System.Web.Routing;
using GCPicBroker;
namespace MFLMirror
{
    public class Global : System.Web.HttpApplication
    {
        public static void Log(string msg,params object[] obj)
        {
            lock (Global.logLocker)
            {
                try
                {
                    File.AppendAllLines(logpath, new[] { DateTime.Now.ToString(), string.Format(msg, obj) }, System.Text.Encoding.Default);
                }
                catch { }
                
            }
            
        }
        static object logLocker;
        public override void Dispose()
        {
            base.Dispose();
        }
        static string logpath;
        protected void Application_Start(object sender, EventArgs e)
        {
            Global.logLocker = new object();
            logpath = Server.MapPath("/log.txt");
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["Released"]))
            {
                uploader = new PicUploader(Server.MapPath("~/uploading.tmp"),
                    "MFLAdmin", "http://admin.xunnlv.com/GCPicBrokerProvider.aspx");
                uploader.Event += new PicUploader.EventHandler(uploader_Event);
            }
        }

        void uploader_Event(string providerName, string imgFilename)
        {
            Log("{0}:{1}", providerName, imgFilename);
        }
        static PicUploader uploader;
        protected void Session_Start(object sender, EventArgs e)
        {
            
        }
        public static string[] RegisteredHostIP;

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Log(Request.RawUrl);
            if (Request.Path.ToLower().StartsWith("/hello"))
            {
                Response.StatusCode = 200;
                Response.Write("Hello, too!");
                Response.End();
                return;
            }


            ////Artifical static contents
            //var fi = new FileInfo(Request.PhysicalPath);
            //if (fi.Exists)
            //{
            //    if (fi.Extension.ToLower() == ".mp4")
            //    {
            //        try
            //        {
            //            Response.StatusCode = 200;
            //            Response.ContentType = "video/mp4";
            //            Response.TransmitFile(fi.FullName);
            //        }
            //        catch (Exception ex)
            //        {
            //            Response.StatusCode = 500;
            //            Response.ContentType = "text/plain";
            //            Response.Write(ex.Message);
            //            Response.End();
            //        }
            //        return;
            //    }
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["Released"]))
            {
                uploader.Dispose();
            }
        }
    }
}