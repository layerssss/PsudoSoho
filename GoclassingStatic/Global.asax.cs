using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using JSoonLib;
namespace GoclassingStatic
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            WebApplication.PathResolving = (str) => str.Substring("/static".Length);
            WebApplication.RegisterActions("GoclassingStatic", typeof(Services));
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
    }
}