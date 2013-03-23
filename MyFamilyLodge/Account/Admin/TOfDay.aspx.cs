using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFamilyLodge.Account.Admin
{
    public partial class TOfDay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.TextBox1.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.GridView1.DataBind();
        }
    }
}