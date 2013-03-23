using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        protected MFL.MFLAccount Account;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Account = new MFL.MFLAccount(this.Context);
        }

    }
}