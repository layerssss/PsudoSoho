using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
namespace GoClassing.Home
{
    public partial class Home : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var d = new gc_localtestEntities();
            this.ctype1Panel.Controls.Add(new Literal()
            {
                Mode = LiteralMode.PassThrough,
                Text = "<a href=\"/C/All/\" class=\"ftype ftype-ctype1\">(全部)</a>"
            });
            foreach (var c in d.gccon_ctype1)
            {
                this.ctype1Panel.Controls.Add(new Literal()
                {
                    Mode=LiteralMode.PassThrough,
                    Text="<a href=\"/C/"+c.type+"\" class=\"ftype ftype-ctype1\">"+c.type+"</a>"
                });
            }
            this.m = MFLJson.JsonMachine.JsonContext.Initial("Home.Master", "GC", Internal.GCCommon.HtmlTemplateReader);
            this.m["onlineFriends"] = Internal.GCCommon.GetVisitor(this.Context).GetMyFriends(null, "", true)["listItems"];
        }
        public MFLJson.JsonMachine.JsonContext m;
        
    }
}