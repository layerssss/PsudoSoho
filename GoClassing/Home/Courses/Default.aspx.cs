using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing.Home.Courses
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["createCourse"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "c", "$(function(){$('.createCourseBtn').trigger('click');});", true);
            }
        }
    }
}