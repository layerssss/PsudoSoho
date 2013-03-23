using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFLAdmin.Lodge
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var a = new MFL.MFLAccount(this.Context);
            a.CheckAccount();
            var l=Request["lodge"]??"test";
            if (!a.MFLUsers_Account.MFLUsers_product.SelectMany(tp=>tp.MFL_lodge).Any(tl => tl.ident == l))
            {
                a.Logout();
                a.CheckAccount();
            }
        }
    }
}