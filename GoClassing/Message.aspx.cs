using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoClassing.Internal;
namespace GoClassing
{
    public partial class Message : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var v = (GCCommon.GetVisitor(this.Context) as GCAuthenticated);
            var m = v.U.gc_msg.First(tm => tm.id == Convert.ToInt32(Request.QueryString["id"]));
            var nurl = m.nexturl;
            IEnumerable<gc_msg> q;
            while ((q = v.U.gc_msg.Where(tm => tm.nexturl == nurl && !tm.read)).Any())
            {
                q.First().read = true;
            }
            v.D.SaveChanges();
            Response.Redirect(nurl);
        }
    }
}