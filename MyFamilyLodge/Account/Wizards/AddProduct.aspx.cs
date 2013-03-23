using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFL;
namespace MyFamilyLodge.Account.Wizards
{
    public partial class AddProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var a = new MFL.MFLAccount(this.Context);
            if (!IsPostBack)
            {
                this.Label2.Text = a.MFLUsers_Account.balance.ToString();
                DropDownList1_SelectedIndexChanged(null, null);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var prize = Convert.ToInt32(this.Label1.Text);
            MFL.MFLTransactionCenter.SubmitTransaction(new MFL.Transactions.BuyProduct()
            {
                Balance = prize,
                Month = Convert.ToInt32(this.DropDownList3.SelectedValue),
                ProductionType = this.DropDownList1.SelectedValue
            });
        }
        protected override void OnError(EventArgs e)
        {
            base.OnError(e);
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DropDownList3.Items.Clear();
            var dic = MFLAccount.GetPrize(this.DropDownList1.SelectedValue);
            foreach (var kvp in dic)
            {
                DropDownList3.Items.Add(new ListItem(kvp.Key + "个月", kvp.Key.ToString()));
            }
            DropDownList3_SelectedIndexChanged(null, null);
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dic = MFL.MFLAccount.GetPrize(this.DropDownList1.SelectedValue);
            this.Label1.Text = dic[Convert.ToInt32(this.DropDownList3.SelectedValue)].ToString();
        }
    }
}