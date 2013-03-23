using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace GoClassing.T.All
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.m = MFLJson.JsonMachine.JsonContext.Initial("Teachers", "GC", Internal.GCCommon.HtmlTemplateReader);
            this.m["title"] = MFLJson.Json.String("所有用户");
            if (Request.QueryString["search"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "search", "gcSearch=function(s){location.href='/T/All/#'+s};stopAjax=true;", true);
                this.m["title"] = MFLJson.Json.String("“" + Request.QueryString["search"] + "”的成员<img src=\"/Styles/Tags/" + Request.QueryString["search"] + ".gif\" title=\"" + Request.QueryString["search"] + "\" alt=\"" + Request.QueryString["search"] + "\">");
            }
            this.m.Embed(Internal.GCSearch.GetAllUsers(Request.QueryString["page"] ?? "0", Request.QueryString["search"]??""));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "data", "data=" + (this.m as MFLJson.IJson).ToString() + ";", true);
        }
        protected MFLJson.JsonMachine.JsonContext m;
    }
}