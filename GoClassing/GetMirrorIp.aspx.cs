using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing
{
    public partial class GetMirrorIp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var wc = new System.Net.WebClient();
            Response.Write(wc.DownloadString("http://tangzx.cl14.53dns.net/getip.aspx"));
            wc.Dispose();
        }
    }
}