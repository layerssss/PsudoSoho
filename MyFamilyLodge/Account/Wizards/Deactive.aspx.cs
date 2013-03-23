using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account.Wizards
{
    public partial class Deactive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            MFL.MFLTransactionCenter.SubmitTransaction(
                new MFL.Transactions.DeactiveProduct()
                {
                    ProductId = Convert.ToInt32(Request["productId"])
                });
        }
    }
}