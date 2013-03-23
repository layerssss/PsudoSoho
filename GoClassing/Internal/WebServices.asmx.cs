using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Net;
using System.Configuration;
namespace GoClassing.Internal
{
    /// <summary>
    /// WebServices 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://goclassing.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WebServices : System.Web.Services.WebService
    {
        public struct Ticket
        {
            public Guid Id;
            public string Path;
            public int Limit;
            public int SpaceUserId;
        }
        [WebMethod]
        public Ticket TakeTicket(Guid ticketId)
        {
            var d = new gc_localtestEntities();
            var t = d.upload_ticket.First(tt => tt.id == ticketId);
            var fname = new Ticket()
            {
                Id=t.id,
                Limit=t.limit,
                Path=t.path,
                SpaceUserId=t.space_user_id
            };
            d.upload_ticket.DeleteObject(t);
            d.SaveChanges();
            return fname;
        }
        [WebMethod]
        public void RetrieveTarget(string filename,string path,int limit,int spaceUserId)
        {
            var d = new gc_localtestEntities();
            try
            {
                if (filename != null)
                {
                    filename = filename.ToLower();
                    var c = new WebClient();
                    if (path.StartsWith("/Drop/reply"))
                    {
                        var id = path.Substring("/Drop/reply".Length);
                        id = id.Remove(id.IndexOf('.'));
                        var iid = Convert.ToInt32(id);
                        if (!d.gc_reply.Any(tr => tr.id == iid))//reply已删除
                        {
                            throw (new Exception());
                        }
                        var r=d.gc_reply.First(tr => tr.id == iid);
                        if (filename.EndsWith(".mp4"))
                        {
                            r.ftype = "video";
                        }
                        if(filename.EndsWith(".swf")){
                            r.ftype = filename.Remove(filename.LastIndexOf('.'));
                            r.ftype = r.ftype.Substring(r.ftype.LastIndexOf('.') + 1);
                            r.ftype = r.ftype.ToLower().TrimEnd('x');
                        }
                        if (filename.EndsWith(".mp3"))
                        {
                            r.ftype = "sound";
                        }
                        if (r.gc_post.gc_reply.OrderBy(tr => tr.time).First() == r)
                        {
                            r.gc_post.ftype = r.ftype;
                        }
                        if (r.ftype != "processing")
                        {
                            (new GCCourse(r.gc_post.gc_course.id, new gc_localtestEntities())).HandleFeeds(r);
                        }
                        
                    }
                    var fi = new FileInfo(Server.MapPath(path));
                    var compare = GCCommon.Md5Part(DateTime.UtcNow.DayOfYear + System.Configuration.ConfigurationManager.AppSettings["SharedSecret"]);
                    MFLMirrorServices.WebServicesSoapClient mc = new MFLMirrorServices.WebServicesSoapClient();
                    if (fi.Exists)
                    {//已有文件，返还空间
                        d.gc_user.First(tu => tu.id == spaceUserId).spaceUsed -= fi.Length;
                        fi.Delete();

                        MFLMirrorLib.Mirror.Expires(
                            path);
                        mc.Expires(compare,path);
                    }
                    c.DownloadFile(ConfigurationManager.AppSettings["repositoryUrl"] + filename, Server.MapPath(path));
                    MFLMirrorLib.Mirror.HandleHttpRequest(path, false);
                    fi = new FileInfo(Server.MapPath(path));
                    d.gc_user.First(tu => tu.id == spaceUserId).spaceUsed += fi.Length;
                    //耗费空间
                }
                else
                {
                    if (path!=null&&path.StartsWith("/Drop/reply"))
                    {
                        var id = path.Substring("/Drop/reply".Length);
                        id = id.Remove(id.LastIndexOf('.'));
                        var iid = Convert.ToInt32(id);
                        if (d.gc_reply.Any(tr => tr.id == iid))//转换失败且未删除
                        {
                            d.gc_reply.DeleteObject(d.gc_reply.First(tr => tr.id == iid));
                        }
                    }
                }
            }
            catch { }
            d.gc_user.First(tu => tu.id == spaceUserId).spaceUsed -= limit;
            d.SaveChanges();
            //返还空间
        }
        [WebMethod]
        public void Error(string msg)
        {
            GCCommon.Log("VServerError:" + msg, Server.MapPath("/"));
        }
        [WebMethod]
        public Guid GetNewTicket(string path,int limit,int space_user_id)
        {
            var t = new upload_ticket()
            {
                expires=DateTime.Now.AddMinutes(5),
                id=Guid.NewGuid(),
                path=path,
                limit=limit,
                space_user_id=space_user_id
            };
            var d = new gc_localtestEntities();
            d.gc_user.First(tu => tu.id == space_user_id).spaceUsed += limit;
            d.upload_ticket.AddObject(t);
            d.SaveChanges();
            return t.id;
        }
    }
}
