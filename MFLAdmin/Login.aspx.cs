using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
namespace MFLAdmin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["submit"] == "登录")
            {

                try
                {
                    MFL.SharedConfig.ValidateRecaptcha();
                    MFL.MFLAdmin.Login(this.Context, Request["password"]);
                    Response.Redirect("/" + Request["lodge"] + "/" + (Request["day"] != null ? "?day=" + Request["day"] : ""));
                }
                catch (Exception ex)
                {
                    Response.SetCookie(new HttpCookie("ScriptsToBeExecuted", "$(function(){showErrors('" + ex.Message + "')})"));
                }
            } 

            if (Request["submit"] == "退出")
            {
                MFL.MFLAdmin.Logout(this.Context);
            }
            var day = Convert.ToDateTime(Request["day"] ?? DateTime.Today.ToString("yyyy-MM-dd"));
            (this.Master as Site).Day = day.ToString("yyyy-MM-dd");
        }
    }
}