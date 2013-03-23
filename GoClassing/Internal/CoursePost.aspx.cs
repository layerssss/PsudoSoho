using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
namespace GoClassing.Internal
{
    public partial class CoursePost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.m=MFLJson.JsonMachine.JsonContext.Initial("Course", "GCPost", Internal.GCCommon.HtmlTemplateReader);
            this.m["postId"] = Json.Number(Convert.ToInt32(Request["postId"]));
            this.m["courseId"] = Json.Number(Convert.ToInt32(Request.QueryString["id"]));
            
            try
            {
                this.m.Embed(GCCommon.GetVisitor(this.Context).VisitCourse(Convert.ToInt32(Request["id"])).GetPost(Convert.ToInt32(Request["postId"])));
            }
            catch(GCException ex)
            {
                Response.SetCookie(new HttpCookie("Msg", Server.UrlEncode(ex.Message)));
                Response.Redirect("/C/" + Request["id"] + "/");
            }
            this.Title = string.Format("[{0}]{1}", this.m["tag"], this.m["title"]);
            
        }
        protected MFLJson.JsonMachine.JsonContext m;
    }
}