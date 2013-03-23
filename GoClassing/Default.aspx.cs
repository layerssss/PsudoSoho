using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoClassing.Internal;
namespace GoClassing
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["QuickLogin"] == "true")
            {
                Internal.GCCommon.Login(Request.QueryString["Username"], Request.QueryString["Password"], false, this.Context, "/Home/Security/");
                Response.Redirect(Request.Url.PathAndQuery.Replace("QuickLogin=true", ""));
                return;
            }
            if (Request.QueryString["PasswordRetrieval"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "$(function(){wizard(\"r1\");});", true);
            }
            else
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    if (this.User.Identity.IsAuthenticated)
                    {

                        Response.Redirect(Request.QueryString["ReturnUrl"]);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "redirect=\"" +
                        Request.QueryString["ReturnUrl"] + "\";$(function(){openLogin();});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "redirect=\"/Home/\";", true);
                    if (this.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("/Home/");
                    }
                }
            }
        }
    }
}