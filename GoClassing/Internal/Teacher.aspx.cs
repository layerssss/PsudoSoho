using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
using System.IO;
namespace GoClassing.Internal
{
    public partial class Teacher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var v = GCCommon.GetVisitor(this.Context);
            if (Request.QueryString["name"] == "")
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("/T/"+(v as GCAuthenticated).U.originalusername+"/");
                    return;
                }
                Response.Redirect("/T/Get_Started/");
                return;
            }

            
            var u = v.VisitUser(Request.QueryString["name"].ToLower().TrimEnd('/'));
            this.m = MFLJson.JsonMachine.JsonContext.Initial("Teacher", "GC", Internal.GCCommon.HtmlTemplateReader);
            this.m.Embed(u.GetJson(), "");
            this.m.Add("settings", u.GetAllSettings());
            this.m.Add("baseurl", Json.String(System.Configuration.ConfigurationManager.AppSettings["baseUrl"]));
            this.Title = (this.m["truename"] as JsonString).Text;
            try
            {
                this.m.Add("profiles", u.GetProfiles());
            }
            catch(Exception ex){
                this.m.Add("profiles", Json.String(ex.Message));
            } 
            try
            {
                this.m.Add("moreprofiles", u.GetMoreProfiles());
            }
            catch (Exception ex)
            {
                this.m.Add("moreprofiles", Json.String(ex.Message));
            }
            try
            {
                this.m.Add("courses", u.GetCourses("0", 3)["listItems"]);
            }
            catch (Exception ex)
            {
                this.m.Add("courses", Json.String(ex.Message));
            }
            try
            {
                this.m.Add("pcourses", u.GetPaticipatedCourses("0", 3)["listItems"]);
            }
            catch (Exception ex)
            {
                this.m.Add("pcourses", Json.String(ex.Message));
            }
            try
            {
                this.m.Add("friends", u.GetFriends("0", 6)["listItems"]);
            }
            catch (Exception ex)
            {
                this.m.Add("friends", Json.String(ex.Message));
            }
            
        }
        protected MFLJson.JsonMachine.JsonContext m;
    }
}