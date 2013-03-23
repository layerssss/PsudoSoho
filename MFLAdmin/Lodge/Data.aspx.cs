using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFL;
using MFLJson;
using System.IO;
namespace MFLAdmin.Lodge
{
    public partial class Data : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var lodge = new MFLLodge(Server.MapPath("~/"), Request.QueryString["lodge"]);
            var sta = new MFLStatic(Server.MapPath("~/"));
            JsonObject obj = Json.Object("success", Json.True());
            var argsInt = new List<string>();
            var argsString = new List<string>();
            var actions = new List<string>();
            bool acted = false;
            Action<string, Action> regAction = (string actName, Action act) =>
            {
                if (Request.QueryString["action"] == actName)
                {
                    act();
                    acted = true;
                }
                actions.Add(actName);
            };
            Func<string, int> pi = (p) =>
            {
                argsInt.Add(p);
                return Convert.ToInt32(Request.QueryString[p] ?? Request.Form[p]);
            };
            Func<string, string> ps = (p) =>
            {
                argsInt.Add(p);
                return Request.QueryString[p] ?? Request.Form[p];
            };
            try
            {
                //System.Threading.Thread.Sleep(200);
                regAction("获取特性", () =>
                {
                    obj["listItems"] = lodge.GetAttributes();
                });
                regAction("获取图标", () =>
                {
                    obj["listItems"] = sta.GetIcons();
                });
                regAction("修改特性", () =>
                {
                    lodge.UpdateAttribute(pi("id"), ps("name"), ps("icon"));
                });
                regAction("修改选项", () =>
                {
                    var id=pi("id");
                    lodge.UpdateOption(id, ps("name"), ps("icon"));
                });
                regAction("创建特性", () =>
                {
                    obj["newId"] = Json.Number(lodge.NewAttribute(ps("name"), ps("icon")));
                    obj["message"] = Json.String("特性已创建，现在可以为该特性添加选项。");
                });
                regAction("创建选项", () =>
                {
                    obj["newId"] = Json.Number(lodge.NewOption(pi("attributeId"), ps("name"), ps("icon")));
                });
                regAction("获取选项", () =>
                {
                    obj["listItems"] = lodge.GetOptions(pi("id"));
                });
                regAction("删除特性", () =>
                {
                    lodge.DelAttribute(pi("id"));
                });
                regAction("删除选项", () =>
                {
                    lodge.DelOption(pi("id"));
                });
                regAction("获取房间", () =>
                {
                    obj["listItems"] = lodge.GetRooms();
                });
                regAction("删除房间", () =>
                {
                    lodge.DelRoom(pi("id"));
                });
                regAction("创建房间", () =>
                {
                    obj["newId"] = Json.Number(lodge.NewRoom(ps("name"), ps("icon"), ps("prize")));
                    obj["message"] = Json.String("房间已创建，接下来请为新房间选择特性并添加照片。");
                });
                regAction("修改房间", () =>
                {
                    lodge.UpdateRoom(pi("id"), ps("name"), ps("icon"), ps("prize"));
                });
                regAction("修改房间特性", () =>
                {
                    lodge.UpdateRoomAttribute(pi("roomId"), pi("optionId"), pi("attributeId"));
                });
                regAction("获取房间特性", () =>
                {
                    obj["listItems"] = lodge.GetRoomAttributes(pi("id"), false);
                });
                regAction("获取旅馆设定", () =>
                {
                    obj["listItems"] = Json.Array(lodge.GetAllProperties());
                    obj.Embed(lodge.GetLodgeStatus());
                });
                regAction("修改旅馆设定", () =>
                {
                    lodge.UpdateProperty("lodgeName", ps("lodgeName"));
                    lodge.UpdatePropertyNumber("lodgeLongtitude", ps("lodgeLongtitude"));
                    lodge.UpdatePropertyNumber("lodgeLatitude", ps("lodgeLatitude"));
                    lodge.UpdateProperty("lodgeTraffic", ps("lodgeTraffic"));
                    lodge.UpdateProperty("lodgeDescription", ps("lodgeDescription"));
                    lodge.UpdateProperty("lodgeContact", ps("lodgeContact"));
                    lodge.UpdateTemplateName(ps("templateName"));
                    lodge.UpdateTemplateProperties(this.Context);
                    obj["message"] = Json.String("旅馆选项修改成功");
                });
                regAction("旅馆相册添加照片", () =>
                {
                    lodge.LodgeNewPhoto(Request.Form["photoTitle"], Request.Form["photoContent"], Request.Form["photoType"], Request.Files["photoFile"]);
                    obj["message"] = Json.String("旅馆照片添加成功");
                });
                regAction("旅馆相册获取照片", () =>
                {
                    obj["listItems"] = lodge.LodgeGetPhotos();
                });
                regAction("旅馆相册删除照片", () =>
                {
                    lodge.LodgeDelPhoto(pi("id"));
                });
                regAction("旅馆相册修改照片", () =>
                {
                    lodge.LodgeUpdatePhoto(pi("photoId"), ps("photoTitle"), ps("photoContent"), ps("photoType"));
                });
                regAction("房间相册添加照片", () =>
                {
                    lodge.RoomNewPhoto(Convert.ToInt32(Request.Form["roomId"]), Request.Form["photoTitle"], Request.Form["photoContent"], Request.Files["photoFile"]);
                    obj["message"] = Json.String("房间照片添加成功");
                });
                regAction("房间相册获取照片", () =>
                {
                    obj["listItems"] = lodge.RoomGetPhotos(pi("roomId"));
                });
                regAction("房间相册删除照片", () =>
                {
                    lodge.RoomDelPhoto(pi("roomId"), pi("id"));
                });
                regAction("房间相册修改照片", () =>
                {
                    lodge.RoomUpdatePhoto(pi("roomId"), pi("photoId"), ps("photoTitle"), ps("photoContent"));
                });
                regAction("获取相册状态", () =>
                {
                    obj["albumStatus"] = lodge.GetAlbumStatus();
                });
                regAction("获取所有相册分类", () =>
                {
                    obj["lodgePhotoTypes"] = lodge.LodgeGetPhotoTypes();
                });
                regAction("旅馆相册获取分类", () =>
                {
                    obj["listItems"] = lodge.LodgeGetDetailPhotoTypes();
                });
                regAction("获取模版选项", () =>
                {
                    obj["listItems"] = lodge.GetTemplateProperties(ps("templateName"));
                });
                regAction("获取模版", () =>
                {
                    obj["listItems"] = sta.GetTemplates();
                });
                regAction("获取所有特性名称", () =>
                {
                    obj["listItems"] = lodge.SearchAttributeNames();
                });
                regAction("获取旅馆信息", () =>
                {
                    lock (Global.Pending)
                    {
                        obj["refreshStatus"] = Json.String("就绪");
                        if (Global.Pending.Contains(lodge.MFL_lodge.ident))
                        {
                            obj["refreshStatus"] = Json.String(string.Format("正在更新队列中等待，在您之前仍有{0}个请求..", Global.Pending.ToList().IndexOf(lodge.LodgeName)));
                            obj["refreshing"] = Json.True();
                        }
                        else
                        {
                            if (Global.Publishing == lodge.MFL_lodge.ident)
                            {
                                obj["refreshStatus"] = Json.String("正在更新..");
                                obj["refreshing"] = Json.True();
                            }
                        }
                    }
                    var fi = new FileInfo(Server.MapPath("~/static/" + lodge.MFL_lodge.ident + ".lastPublishedTime"));
                    if (fi.Exists)
                    {
                        obj["lastRefreshedTime"] = Json.String(File.ReadAllLines(fi.FullName)[0]);
                    }
                    else
                    {
                        obj["lastRefreshedTime"] = Json.String("不可用");
                    }
                    obj["lodgeName"] = Json.String(lodge.LodgeName);
                    obj["staHistory"] = Json.Number(lodge.StaGetHistory());
                    obj["staToday"] = Json.Number(lodge.StaGetToday());
                    obj["numRooms"] = Json.Number(lodge.GetRooms().Count);
                    obj["productType"] = Json.String(MFLAccount.GetProductType(lodge.MFL_lodge.MFLUsers_product.type));
                    obj.Embed(lodge.GetAlbumStatus());

                });
                regAction("更新前台页面", () =>
                {
                    lock (Global.Pending)
                    {
                        if (Global.Pending.Contains(lodge.MFL_lodge.ident))
                        {
                            throw (new Exception(string.Format("正在更新队列中等待，在您之前仍有{0}个请求..", Global.Pending.ToList().IndexOf(lodge.MFL_lodge.ident))));
                        }
                        if (Global.Publishing == lodge.MFL_lodge.ident)
                        {
                            throw (new Exception("正在更新.."));
                        }
                        Global.Pending.Enqueue(lodge.MFL_lodge.ident);
                    }
                    //System.Threading.Thread.Sleep(1000);
                });
                regAction("设定旅馆管理员密码", () =>
                {
                    lodge.ChangeAdminPwd(ps("lodgeAdminPwd"));
                    obj["message"] = Json.String("管理员密码设定成功");
                });
                regAction("获取操作", () =>
                {
                    obj["actions"] = Json.Array(actions.Select(ta => Json.String(ta)));
                });
                obj["lodgeChanged"] = Json.Bool(lodge.LodgeChanged);
                if (!acted)
                {
                    throw (new Exception(string.Format("操作“{0}”未实现。", Request.QueryString["action"])));
                }
            }
            catch (Exception ex)
            {
                obj["success"] = Json.False();
                obj["message"] = Json.String("执行“" + Request.QueryString["action"] + "”操作时发生错误，错误原因是：" + ex.Message);
            }
            if (Request.QueryString["listArgs"] == "true")
            {
                obj["argInt"] = Json.Array(argsInt.Select(ta => Json.String(ta)));
                obj["argString"] = Json.Array(argsString.Select(ta => Json.String(ta)));
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