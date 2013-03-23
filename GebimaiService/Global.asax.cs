using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using ispJs;
using System.Threading;
namespace GebimaiService
{
    public class Global : System.Web.HttpApplication
    {
        public static object Safelock = new object();
        public static Dictionary<string, ISocialProvider> SocialProviders = new Dictionary<string, ISocialProvider>();
        public static gebimaicom Entities
        {
            get
            {
                return new gebimaicom(new MySql.Data.MySqlClient.MySqlConnection(Global.ConnectinoString));
            }
        }
        public static string HttpRoot
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HttpRoot"];
            }
        }
        public static string ConnectinoString = "Server=dev.xunnlv.com;Database=gebimai_com";

        public static string CookieDomain
        {
            get
            {
                return "gebimai.com";
            }
        }
        protected void Application_Start(object sender, EventArgs e)
        {

            Global.SocialProviders.Add("weibo", new WeiboProvider());

            ispJs.WebApplication.ActionCommentsXmlPath = Server.MapPath("/bin/GebimaiService.XML");
            ispJs.WebApplication.ErrorPath = "/Error/";
            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(Dev));
            ispJs.WebApplication.RegisterRenderer("Dev/Actions.htm.isp.js", new _Dev.Devs());
            ispJs.WebApplication.RegisterRenderer("Dev/Default.htm.isp.js", new _Dev.Devs());
            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(Error));

            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(Admin));
            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(Sender));
            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(Auth));
            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(Public));
            ispJs.WebApplication.RegisterActions("GebimaiService", typeof(User));


            WebApplication.RegisterSubPage("user.isp.js");
            WebApplication.RegisterRenderer("user.isp.js", new user());


            WebApplication.RegisterSubPage("sender.isp.js");
            WebApplication.RegisterRenderer("sender.isp.js", new sender());

            WebApplication.RegisterSubPage("admin.isp.js");
            WebApplication.RegisterRenderer("admin.isp.js", new admin());

            WebApplication.RegisterSubPage("orders.isp.js");
            WebApplication.RegisterRenderer("orders.isp.js", new orders());

            WebApplication.RegisterSubPage("list.isp.js");
            WebApplication.RegisterRenderer("list.isp.js", new list());

            WebApplication.RegisterSubPage("Dev/codex.isp.js");
            WebApplication.RegisterRenderer("Dev/codex.isp.js", new _Dev.codex());

            WebApplication.LogPath = "log.txt";


            ispJs.WebApplication.OnConsolePageReading = (isp, subpage) =>
            {
                new _Dev.Devs().Page_Read(subpage);
            };

            ispJs.WebApplication.HandleStart(Server);
            BotThread = new Thread(BotThreadStart);
            BotThreadStopping = false;
            try
            {
                File.Delete(Server.MapPath("/log.txt"));
            }
            catch
            {
            }
            WeiboProvider.InitBot();
            BotThread.Start();

        }
        public static Thread BotThread;
        public static bool BotThreadStopping{
            get{
                lock(Safelock){
                    return botThreadStopping;
                }
            }
            set{
                lock (Safelock)
                {
                    botThreadStopping = value;
                }
            }
        }
        public static bool botThreadStopping=false;
        static void BotThreadStart()
        {
            var loop=0;
            while (!BotThreadStopping)
            {
                bool results = false;
                if (loop == 0)
                {
                    try
                    {
                        WeiboProvider.PingBot(out results);
                    }
                    catch (Exception ex)
                    {
                        ispJs.GlobalLog.Fire(ex, "BOTERROR:{0}", ex.Message);
                    }
                    loop = results ? 1 : 4;
                }
                Thread.Sleep(15000);
                loop--;
            }
            BotThreadStopping = false;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {

            var path = Request.Path;
            var exts = new[] { "js", "css", "jpeg", "jpg", "gif", "png", "swf" };
            var ext = "";
            try
            {
                ext = path.Substring(path.LastIndexOf('.') + 1);
            }
            catch { }
            if (!(path.Contains('.') && exts.Contains(ext)))
            {
                Response.ContentType = "text/html";
            }
            if (path == "/Error")
            {
                Response.ContentType = "text/html";
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ispJs.WebApplication.HandleBeginRequest();
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
            BotThreadStopping = true;
            do
            {
                Thread.Sleep(300);
            } while (BotThreadStopping);
            ispJs.WebApplication.HandleEnd();

        }
        public static bool IsLocal
        {
            get
            {
                return HttpRoot.Contains("t.");
            }
        }
    }
}