using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing
{
    public partial class MailVerifying : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "verify", "$(function(){verify(\"" + Request.QueryString["mail"].Replace('\"', ' ') + "\",\"" + Request.QueryString["hash"].Replace('\"', ' ') + "\",\""+Request.QueryString["redirect"]+"\");});", true);
        }
    }
}