using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
using System.Configuration;
namespace GoClassing.Internal
{
    public partial class Course1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var v = GCCommon.GetVisitor(this.Context);
            this.c = v.VisitCourse(Convert.ToInt32(Request.QueryString["id"]));
            this.mLeft = MFLJson.JsonMachine.JsonContext.Initial("Course", "GCLeft", Internal.GCCommon.HtmlTemplateReader);
            this.mLeft["courseId"] = Json.Number(Convert.ToInt32(Request.QueryString["id"]));
            this.mLeft.Embed(c.GetJson());
            this.mLeft["baseUrl"] = Json.String(ConfigurationManager.AppSettings["baseUrl"]);
            this.Page.Title += "-" + this.c.C.name;
        }
        protected GCCourse c;
        protected MFLJson.JsonMachine.JsonContext mLeft;
    }
}