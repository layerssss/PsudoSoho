using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing
{
    public partial class Root : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Title != "")
            {
                this.Page.Title += "-";
            }
            this.Page.Title += "上课网";
            if (this.Page.User.Identity.IsAuthenticated)
            {
                var a = Internal.GCCommon.GetVisitor(this.Context) as Internal.GCAuthenticated;
                var c=a.U.gc_msg.Where(tm => !tm.read).Count();
                (this.LoginView1.FindControl("numMsg") as Label).Text =  c.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "$(function(){storage('"+Internal.GCCommon.FormatSpace(a.U.spaceTotal)+"','"+Internal.GCCommon.FormatSpace(a.U.spaceUsed)+"',"+a.U.spaceUsed*100/a.U.spaceTotal+")});", true);
            }
            if (Request.Cookies.AllKeys.Contains("Msg"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "$(function(){MFLDialog(\"提示\",\"" + Server.UrlDecode(Server.UrlDecode(Request.Cookies["Msg"].Value)) + "\");});", true);
                Response.SetCookie(new HttpCookie("Msg") { Expires = DateTime.Now.AddDays(-1) });
            }
        }
    }
}