using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account
{
    public partial class Default : System.Web.UI.Page
    {
        protected MFL.MFLUsers_account MFLUsers_account;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MFLUsers_account = new MFL.MFLAccount(this.Context).MFLUsers_Account;
        }
    }
}