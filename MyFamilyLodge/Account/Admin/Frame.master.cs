using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account.Admin
{
    public partial class Frame : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var a = new MFL.MFLAccount(this.Context);
            a.CheckAccount();
            if (!a.MFLUsers_Account.isAdmin)
            {
                throw (new Exception("登陆超时"));
            }
        }
    }
}