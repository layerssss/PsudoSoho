using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GebimaiService
{
    public partial class GetMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var uname = Auth.Username;
                if (uname == null)
                {
                    throw (new Exception("亲，要登录先啊有木有！"));
                }
                while (true)
                {

                    var d = Global.Entities;
                    var u = d.users.First(tu => tu.username == uname);
                    if (u.message != null)
                    {
                        Response.Write(u.message);
                        Response.End();
                        break;
                    }
                    if (!Response.IsClientConnected)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch
            {
            }
        }
    }
}