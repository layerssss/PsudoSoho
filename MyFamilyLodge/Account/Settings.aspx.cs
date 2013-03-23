using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account
{
    public partial class Settings : System.Web.UI.Page
    {
        protected MFL.MFLUsers_account MFLUsers_Account;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MFLUsers_Account = new MFL.MFLAccount(this.Context).MFLUsers_Account;
        }
    }
}