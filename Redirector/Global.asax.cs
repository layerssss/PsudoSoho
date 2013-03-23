using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Redirector
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.Host.ToLower() == "www.goclassing.com")
            {
                Response.Redirect("http://goclassing.com/");
                return;
            }
            if (Request.Url.Host.ToLower() == "xunnlv.com")
            {
                Response.Write("Hello, you're from " + Request.ServerVariables["REMOTE_ADDR"]);
                Response.End();
                return;
            }
            if (Request.Url.Host.ToLower() == "ispjs.org")
            {
                Response.Redirect("http://ispjs.org/");
                return;
            }
            if (Request.Url.Host.ToLower() == "githubize.it")
            {
                Response.Redirect("http://githubize.it/");
                return;
            }
            if (Request.Url.Host.ToLower().EndsWith("zhixiang.in") || Request.Url.Host.ToLower().EndsWith("layerssss.net"))
            {
                Response.Redirect("http://zhixiang.in/");
                return;
            }
            Response.Write("Hello, you're from " + Request.ServerVariables["REMOTE_ADDR"]+" to "+Request.Url.Host);
            Response.End();
            return;
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

        }
    }
}