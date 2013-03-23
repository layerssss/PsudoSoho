using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
namespace MFLAdmin.Lodge
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (string a in Request.Files)
            {
                var file = Request.Files[a];
                Response.Write("filename:"+file.FileName);
                Response.Flush();
                Thread.Sleep(2000);
                Response.Write(";length:" + file.ContentLength);
                Response.End();
            }
        }
    }
}