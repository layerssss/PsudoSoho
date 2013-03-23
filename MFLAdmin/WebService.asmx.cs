using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using YupooAPI.NET;
namespace MFLAdmin
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void DeactiveLodge(string hash,string lodgeName)
        {
            GCServiceBase.Validator.Validate(hash);
            if (File.Exists(Server.MapPath("~/" + lodgeName + "/IsLodge.true")))
            {
                Directory.Delete(Server.MapPath("~/" + lodgeName + "/"), true);
            }
            foreach (var file in Directory.GetFiles(Server.MapPath("~/static"), lodgeName + ".*"))
            {
                File.Delete(file);
            }
            foreach (var file in Directory.GetFiles(Server.MapPath("~/static"), lodgeName + "-*.*"))
            {
                if (file.EndsWith(".yupooid", StringComparison.OrdinalIgnoreCase))
                {
                    var yid = File.ReadAllText(file);
                    YPhoto u = new YPhoto("de89c7232cad62b6", "4f34a3335937ad3cc7714ac5603166b1", "http://v.yupoo.com/");
                    try
                    {
                        u.Delete("c9fc5cde774deef50596e11a6b0a344c", yid);
                    }
                    catch { }
                }
                File.Delete(file);
            }
        }
        [WebMethod]
        public bool ActiveLodge(string hash,string lodgeName)
        {
            GCServiceBase.Validator.Validate(hash);
            var path = (Server.MapPath("~/" + lodgeName + "/"));
            if(Directory.Exists(path)){
                throw (new Exception());
            }
            Directory.CreateDirectory(path);
            File.WriteAllText(path+"Default.aspx","<%@Page Language=\"C#\"%><%Server.Transfer(\"~/Default.aspx?lodge="+Server.UrlEncode(lodgeName)+"\"+(Request[\"day\"]==null?\"\":\"&day=\"+Server.UrlEncode(Request[\"day\"])));%>",System.Text.Encoding.UTF8);
            File.WriteAllText(path + "IsLodge.true", "true", System.Text.Encoding.UTF8);
            return true;
        }
    }
}