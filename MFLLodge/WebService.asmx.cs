using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
namespace MFLLodge
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
        public void DeleteFolder(string hashStr,string path)
        {
            GCServiceBase.Validator.Validate(hashStr);
            if (Directory.Exists(Server.MapPath(path)))
            {
                Directory.Delete(Server.MapPath(path), true);
            }
        }
        [WebMethod]
        public void DeleteFile(string hashStr, string path)
        {
            GCServiceBase.Validator.Validate(hashStr);
            File.Delete(Server.MapPath(path));
        }
        [WebMethod]
        public void EmptyFolder(string hashStr, string path, string pattern)
        {
            GCServiceBase.Validator.Validate(hashStr);
            if (Directory.Exists(Server.MapPath(path)))
            {
                foreach (var file in Directory.GetFiles(Server.MapPath(path), pattern))
                {
                    File.Delete(file);
                }
            }
        }
        [WebMethod]
        public void WriteFile(string hashStr, string path, string content)
        {
            GCServiceBase.Validator.Validate(hashStr);
            var fi = new FileInfo(Server.MapPath(path));
            if (!fi.Directory.Exists)
            {
                createDirectory(fi.Directory);
            }
            File.WriteAllText(Server.MapPath(path), content, System.Text.Encoding.UTF8);
        }

        static void createDirectory(DirectoryInfo di)
        {
            try
            {
                if (!di.Parent.Exists)
                {
                    createDirectory(di.Parent);
                }
            }
            catch { }
            di.Create();
        }
    }
}
