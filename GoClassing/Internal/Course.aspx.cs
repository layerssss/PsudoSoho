using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
using System.IO;
using System.Configuration;
namespace GoClassing.Internal
{
    public partial class Course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var v = GCCommon.GetVisitor(this.Context);
            this.c = v.VisitCourse(Convert.ToInt32(Request.QueryString["id"]));
            this.mTag = MFLJson.JsonMachine.JsonContext.Initial("Course", "GCTag", Internal.GCCommon.HtmlTemplateReader);
            this.mTag["courseShowId"] = Json.String(Request.QueryString["id"]);
            this.Page.Title = this.c.C.name;
        }
        protected GCCourse c;
        protected MFLJson.JsonMachine.JsonContext mTag;
    }
}