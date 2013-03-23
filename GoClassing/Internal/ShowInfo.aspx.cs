using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing.Internal
{
    public partial class ShowInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var key in Request.ServerVariables.AllKeys)
            {
                Response.Write(key + "=" + Request.ServerVariables[key] + ";");
            }
        }
    }
}