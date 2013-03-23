using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account.Admin
{
    public partial class Defaultaspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TextBox1.Text = "";
            this.Label1.Text = (string)this.GridView1.SelectedDataKey[0];
            this.Panel1.Visible = this.GridView1.SelectedIndex != -1;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var a=new MFL.MFLAccount(this.Context);
            var id = Convert.ToInt32((string)this.GridView1.SelectedDataKey[0]);
            var tc = new MFL.MFLTransactionCenter();
            var t = MFL.Transactions.MFLTransactionOpration.LoadFrom(tc.D.MFLTC_transaction.First(tt => tt.id == id), tc.D);
            t.Reject(a.MFLUsers_Account.username, TextBox1.Text);
            this.GridView1.DataBind();
            this.GridView1_SelectedIndexChanged(null, null);
        }
    }
}