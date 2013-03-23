using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFLAdmin
{
    public partial class Default : System.Web.UI.Page
    {
        protected MFL.MFLAdmin MFLAdmin;
        public DateTime Day;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Day = Convert.ToDateTime(Request["day"] ?? DateTime.Today.ToString("yyyy-MM-dd"));
                this.MFLAdmin = new MFL.MFLAdmin(this.Context, Request["lodge"]);
                (this.Master as Site).Day = this.Day.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {

                Response.SetCookie(new HttpCookie("ScriptsToBeExecuted", "$(function(){showErrors('" + ex.Message + "')})"));
                Response.Redirect("/login.aspx?lodge=" + Server.UrlEncode(Request["lodge"]));
            }
        }
    }
}