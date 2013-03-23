using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFL;
namespace MyFamilyLodge.Account
{
    public partial class Lodges : System.Web.UI.Page
    {
        protected MFLAccount Account;
        protected void Page_Load(object sender, EventArgs e)
        {
            var a = new MFL.MFLAccount(this.Context);
            this.Account = a;
        }
        protected string GetLodgeName(MFL_lodge l)
        {
            var ml = new MFLLodge("-invalid", l.ident);
            return ml.LodgeName;
        }
    }
}