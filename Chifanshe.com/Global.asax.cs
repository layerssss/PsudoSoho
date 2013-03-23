using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ispJs;
using System.IO;
namespace Chifanshe.com
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            WebApplication.RegisterActions("Chifanshe.com", typeof(Auth));
            WebApplication.RegisterActions("Chifanshe.com", typeof(Administrator));
            WebApplication.RegisterActions("Chifanshe.com", typeof(Actions));

            WebApplication.RegisterRenderer("Admin/Stores.isp.js", new Admin.Stores());

            WebApplication.RegisterSubPage("Admin/store.isp.js");
            WebApplication.RegisterRenderer("Admin/store.isp.js", new Admin.store());

            WebApplication.RegisterRenderer("Admin/Default.htm.isp.js", new Admin.Default());

            WebApplication.RegisterSubPage("dian.isp.js");
            WebApplication.RegisterRenderer("dian.isp.js", new dian());

            WebApplication.RegisterRenderer("Default.htm.isp.js", new Default());
            WebApplication.RegisterRenderer("Sheiquna.isp.js", new Sheiquna());
            WebApplication.HandleStart(Server);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            WebApplication.HandleBeginRequest();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {

            var path = Request.Path;
            var exts = new[] { "js", "css", "jpeg", "jpg", "gif", "png", "swf" };
            var ext = path.Substring(path.LastIndexOf('.') + 1);
            if (!(path.Contains('.') && exts.Contains(ext)) && File.Exists(Server.MapPath(path)))
            {
                Response.ContentType = "text/html";
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            WebApplication.HandleEnd();
        }
        public static cfs.chifanshecom Entity
        {
            get
            {
                return new cfs.chifanshecom(
                    new MySql.Data.MySqlClient.MySqlConnection(
                    "Server=dev.xunnlv.com;Database=chifanshe_com"));
            }
        }
    }
}