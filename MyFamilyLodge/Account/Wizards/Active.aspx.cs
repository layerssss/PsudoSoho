using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account.Wizards
{
    public partial class ActiveLodge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MFL.MFLAccount.ValidateLodgeIdent(this.TextBox1.Text);
            try
            {
                MFL.Transactions.ServiceDelegation.Lodge.DeleteFile("~/" + this.TextBox1.Text);
                MFL.Transactions.ServiceDelegation.Admin.ActiveLodge(this.TextBox1.Text);
                MFL.Transactions.ServiceDelegation.Admin.DeactiveLodge(this.TextBox1.Text);
            }
            catch
            {
                throw (new Exception(string.Format("该旅馆标识属于保留关键字，请换用另一个标识重试。")));
            }
            MFL.MFLTransactionCenter.SubmitTransaction(new
                MFL.Transactions.ActiveProduct()
                {
                    LodgeName=this.TextBox1.Text,
                    ProductId=Convert.ToInt32(Request["productId"])
                });
        }
    }
}