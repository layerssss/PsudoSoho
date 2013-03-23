using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing.Admin
{
    public partial class ProfileName : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gc_localtestEntities d = new gc_localtestEntities();
            d.gccon_profilename.AddObject(new gccon_profilename() { name = TextBox1.Text,defaultSort=0 });
            d.SaveChanges();
            this.Page.DataBind();
        }
    }
}