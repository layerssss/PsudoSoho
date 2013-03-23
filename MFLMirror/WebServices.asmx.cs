using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.IO;
namespace MFLMirror
{
    /// <summary>
    /// WebServices 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://MirrorServices.MFL/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WebServices : System.Web.Services.WebService
    {

        [WebMethod]
        public bool TryAccessCache(string dayHash,string path,string url,bool secret,string validation)
        {
            if (!validateHash(dayHash))
            {
                return false;
            }
            var fi = new FileInfo(Server.MapPath(path));
            if (fi.Exists)
            {
                Global.Log("Access:{0}", path);
                return true;
            }
            Global.Log("Denied:{0}", path);
            {
                if (secret)
                {
                    MMEntities d = new MMEntities();
                    d.MM_secret.AddObject(new MM_secret());
                    d.SaveChanges();
                }
                try
                {
                    if (!fi.Directory.Exists)
                    {
                        CreateDirectory(fi.Directory);
                    }
                    Global.Log("DownloadBegin:{0} {1}", path,url);
                    var wc = new WebClient();
                    wc.DownloadFile(url, Server.MapPath(path));
                    Global.Log("DownloadEnds:{0} {1}", path, url);
                    return true;
                }
                catch (Exception ex)
                {
                    Global.Log(ex.Message);
                    return false;
                }
            }
        }

        public static void CreateDirectory(DirectoryInfo di)
        {

            Global.Log("Create:{0}", di.FullName);
            if (!di.Parent.Exists)
            {
                CreateDirectory(di.Parent);
            }
            di.Create();
        }
        [WebMethod]
        public void Expires(string dayHash, string path)
        {
            if (!validateHash(dayHash))
            {
                return;
            }
            path = path.ToLower();
            File.Delete(Server.MapPath(path));
            MMEntities d = new MMEntities();
            var secret = d.MM_secret.FirstOrDefault(ts => ts.path == path);
            if (secret != null)
            {
                d.MM_secret.DeleteObject(secret);
                d.SaveChanges();
            }
        }
        bool validateHash(string dayHash)
        {
            dayHash = dayHash.ToLower();
            var compare=MD5(DateTime.UtcNow.DayOfYear+System.Configuration.ConfigurationManager.AppSettings["SharedSecret"]);
            if (compare != dayHash)
            {
                Global.Log("Access denied from " + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + " hash:" + dayHash + " compare:" + compare);
            }
            return compare == dayHash;
        }
        public static string MD5(string str)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = get_md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            get_md5.Dispose();
            return ByteArrayToHexString(bytes, 0, 16);

        }
        static string ByteArrayToHexString(byte[] buf, int offset, int len)
        {
            len += offset;
            var sb = new System.Text.StringBuilder();
            for (int i = offset; i < len; i++)
            {
                sb.Append(buf[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
