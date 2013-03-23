using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoClassing.Admin
{
    public partial class UserProvinces : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            gc_localtestEntities d = new gc_localtestEntities();
            d.gccon_province.AddObject(new gccon_province()
            {
                province = TextBox1.Text
            });
            d.SaveChanges();
            GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            gc_localtestEntities d = new gc_localtestEntities();
            d.gccon_city.AddObject(new gccon_city()
            {
                province = TextBox2.Text,
                city = TextBox3.Text
            });
            d.SaveChanges();
            GridView2.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView2.DataBind();
        }
    }
}