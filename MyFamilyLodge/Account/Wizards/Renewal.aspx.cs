using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account.Wizards
{
    public partial class Renewal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var a = new MFL.MFLAccount(this.Context);
            if (!IsPostBack)
            {
                this.Label2.Text = a.MFLUsers_Account.balance.ToString();
                var p = a.MFLUsers_Account.MFLUsers_product.First(tp => tp.id == Convert.ToInt32(Request["productId"]));
                this.Label3.Text = MFL.MFLAccount.GetProductType(p.type);
                this.HiddenField1.Value = p.type;


                var dic = MFL.MFLAccount.GetPrize(this.HiddenField1.Value);
                foreach (var kvp in dic)
                {
                    DropDownList3.Items.Add(new ListItem(kvp.Key + "个月", kvp.Key.ToString()));
                }

                DropDownList3_SelectedIndexChanged(null, null);
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var prize = Convert.ToInt32(this.Label1.Text);

            var a = new MFL.MFLAccount(this.Context);
            MFL.MFLAccount.CheckBalance(a.MFLUsers_Account, prize);
            MFL.MFLTransactionCenter.SubmitTransaction(new MFL.Transactions.Renewal()
            {
                Balance = prize,
                Month = Convert.ToInt32(this.DropDownList3.SelectedValue),
                ProductId = Convert.ToInt32(Request["productId"])
            });
        }
        protected override void OnError(EventArgs e)
        {
            base.OnError(e);
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dic = MFL.MFLAccount.GetPrize(this.HiddenField1.Value);
            this.Label1.Text = dic[Convert.ToInt32(this.DropDownList3.SelectedValue)].ToString();
        }
    }
}