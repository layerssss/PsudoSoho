using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFL;
namespace MyFamilyLodge.Membership
{
    public partial class Frame : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ret = Request["ReturnUrl"];
            if (ret == null)
            {
                if (Request.UrlReferrer != null)
                {
                    ret = Request.UrlReferrer.OriginalString;
                }
                if (ret==null||ret.ToLower().StartsWith(MFL.SharedConfig.MFLBaseUrl.ToLower()+"membership/"))
                {
                    ret = "/Account/";
                }                
            }
            try
            {
                MFLAccount a = new MFLAccount(this.Context);

                if (a.MFLUsers_Account != null&&Request["submit"]!="退出")
                {
                    Response.Redirect(ret,false);
                    return;
                }
                switch (Request["submit"])
                {
                    case "注册":
                            if (Request["password"] != Request["password2"])
                            {
                                throw (new Exception("两次输入的密码不一样。"));
                            }
                            a.Register(Request["username"], Request["password"]);
                        break;
                    case "登陆":
                        a.Login(Request["username"], Request["password"]);
                        break;
                    case "退出":
                        a.Logout();
                        break;
                    default:
                        return;
                }
                Response.Redirect(ret,false);
                return;
            }
            catch (Exception ex)
            {
                Response.SetCookie(new HttpCookie("ScriptsToBeExecuted", "$(function(){showErrors('" + ex.Message + "')})"));
            }
            
        }

    }
}