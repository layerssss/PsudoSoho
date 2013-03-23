using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account
{
    public partial class Frame : System.Web.UI.MasterPage
    {
        protected MFL.MFLUsers_account MFLUsers_account;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void OnInit(EventArgs e)
        {
            this.Page.Error += new EventHandler(Page_Error);
            var a = new MFL.MFLAccount(this.Context);
            a.CheckAccount();
            this.MFLUsers_account = a.MFLUsers_Account;
            switch (Request["submit"])
            {
                case "退出":
                    a.Logout();
                    a.CheckAccount();
                    break;
                case "反激活":
                    Response.Redirect("/Account/Wizards/Deactive.aspx?productId=" + Request["productId"]);
                    return;
                case "激活":
                    Response.Redirect("/Account/Wizards/Active.aspx?productId=" + Request["productId"]);
                    return;
                case "续费":
                    Response.Redirect("/Account/Wizards/Renewal.aspx?productId=" + Request["productId"]);
                    return;
                case "申请退款":
                    Response.Redirect("/Account/Wizards/Refund.aspx?productId=" + Request["productId"]);
                    return;
                case "修改密码":
                    this.MFLUsers_account.password = Request["password"];
                    a.D.SaveChanges();
                    a.Logout();
                    a.CheckAccount();
                    Response.Redirect("/Account/Wizards/Success.aspx");
                    break;
                case "修改资料":
                    MFLUsers_account.pName = Request["pName"];
                    MFLUsers_account.pProvince = Request["pProvince"];
                    MFLUsers_account.pCity = Request["pCity"];
                    MFLUsers_account.pTraffic = Request["pTraffic"];
                    MFLUsers_account.pSex = Boolean.Parse(Request["pSex"]);
                    a.D.SaveChanges();
                    Response.Redirect("/Account/Wizards/Success.aspx");
                    break;
            }
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
        }

        void Page_Error(object sender, EventArgs e)
        {
            var ex=Server.GetLastError();
            Response.Redirect("/Account/Wizards/Error.aspx?message=" + Server.UrlEncode(ex.Message));
            Server.ClearError();
        }
        protected override void OnError(EventArgs e)
        {
            base.OnError(e);
        }
    }
}