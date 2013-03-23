using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing.Home.Messages
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TextBox1.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            this.TextBox2.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}