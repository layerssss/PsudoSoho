using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
namespace MFLAdmin
{
    public partial class State : System.Web.UI.Page
    {
        //static int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = Json.Object("success", Json.True());
            if (Request["public"] == "true")
            {
                Response.ContentType = "text/javascript";
                var lodge = new MFL.MFLLodge(Server.MapPath("~/"), Request["lodge"]);
                lodge.StaVisit();
                var count = lodge.StaGetHistory();
                Response.Write("MFLCount=" + count + ';');
                return;
            }
            try
            {
                //i++;
                //System.Threading.Thread.Sleep((new Random()).Next(1000));
                //if (i % 4 == 0)
                //{
                //    throw (new Exception("假设这是随机发生的网络错误……太邪恶了……"));
                //}
                var admin = new MFL.MFLAdmin(this.Context, Request["lodge"]);
                switch (Request["action"])
                {
                    case "更改入住状态":
                        {
                            var year=Convert.ToInt32(Request["year"]);
                            var month=Convert.ToInt32(Request["month"]);
                            admin.SetRoomState(
                                year,
                                month,
                                Convert.ToInt32(Request["day"]),
                                Convert.ToInt32(Request["roomId"]),
                                new MFL.MFLAdmin.RoomState()
                                {
                                    Contact = Request["contact"],
                                    Email = Request["email"],
                                    Full = Request["state"] == "full",
                                    Memo = Request["memo"],
                                    People = Convert.ToInt32('0'+Request["people"]),
                                    QQ = Request["qq"],
                                    Tel = Request["tel"]
                                });
                            lock (Global.Pending)
                            {
                                Global.Pending.Enqueue(string.Format("~{0}~{1:D4}{2:D2}"
                                    , Request["lodge"]
                                    , year,
                                    month));
                            }
                        }
                        break;
                    default:
                        {
                            var year = Convert.ToInt32(Request["year"]);
                            var month = Convert.ToInt32(Request["month"]);
                            var path = Server.MapPath(string.Format("~/static/{0}-state-{1:D4}-{2:D2}.state", admin.LodgeName, year, month));
                            if (System.IO.File.Exists(path))
                            {
                                Response.TransmitFile(path);
                                Response.End();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                obj["success"] = Json.False();
                obj["message"] = Json.String("执行“" + Request.QueryString["action"] + "”操作时发生错误，错误原因是：" + ex.Message);
            }
            if (Request.QueryString["formatted"] != "true")
            {
                if (Request.QueryString["cType"] == "html")
                {
                    Response.Write("<html><head></head><body>");
                    Response.Write((obj as IJson).ToString());
                    Response.Write("</body></html>");
                    Response.ContentType = "text/html";
                }
                else
                {
                    Response.Write((obj as IJson).ToString());
                    Response.ContentType = "application/json";
                }
            }
            else
            {
                Response.ContentType = "text/plain";
                Response.Write((obj as IJson).ToFormattedString(""));//.Replace(" ", " ").Replace("\r\n", "<br />"));
            }
            Response.End();
        }
    }
}