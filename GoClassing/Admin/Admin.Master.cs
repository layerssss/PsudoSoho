using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace GoClassing.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var file in Directory.GetFiles(Server.MapPath("/Admin"), "*.aspx"))
            {
                var h = new HyperLink()
                {
                    Text=file.Substring(file.LastIndexOf('\\')+1),
                };
                h.Text = h.Text.Remove(h.Text.LastIndexOf('.'));
                h.BorderWidth = Unit.Pixel(4);
                h.BorderStyle = BorderStyle.Solid;
                h.BorderColor = System.Drawing.Color.Azure;
                h.NavigateUrl = "/Admin/" + h.Text + ".aspx";
                this.Panel1.Controls.Add(h);
            }
        }
    }
}