using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace GoClassing.C.All
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.m = MFLJson.JsonMachine.JsonContext.Initial("Courses", "GC",Internal.GCCommon.HtmlTemplateReader);
            this.m["ctypes1"] = MFLJson.Json.Array();
            this.m["ctypes2"] = MFLJson.Json.Array();
            this.m["ctypes3"] = MFLJson.Json.Array();

            if (Request.QueryString["ctype1"] != null)
            {
                this.Title = Request.QueryString["ctype1"] + "-所有课程";
                this.m["ctype1"] = MFLJson.Json.String(Request.QueryString["ctype1"]);
                if (Request.QueryString["ctypes2"] == null && Request.QueryString["ctypes3"] == null)
                {
                    this.m["ctypes2"] = Internal.GCSearch.GetAllCtypes2(Request.QueryString["ctype1"], null)["listItems"];
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctype1", "ctypesLoading[0]='" + Request.QueryString["ctype1"].Replace("'", "") + "';", true);
            }
            else
            {
                this.m["ctypes1"] = Internal.GCSearch.GetAllCtypes1();
            }
            if (Request.QueryString["ctypes2"] != null)
            {
                this.Title = Request.QueryString["ctypes2"] + "-所有课程";
                this.m["ctype2"] = MFLJson.Json.String(Request.QueryString["ctypes2"]);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ctype2", "ctypesLoading[1]='" + Request.QueryString["ctypes2"].Replace("'", "") + "';", true);
            }
            this.m.Embed(Internal.GCSearch.GetAllCourses(Request.QueryString["page"] ?? "0", Request.QueryString["ctype1"], Request.QueryString["ctypes2"],""), "courses");
        }
        protected MFLJson.JsonMachine.JsonContext m;
    }
}