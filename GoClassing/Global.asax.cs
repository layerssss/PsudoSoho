using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using GoClassing.Internal;
namespace GoClassing
{
    public class Global : System.Web.HttpApplication
    {
        public static Dictionary<string, int> ReplyLimitationOwner;
        public static Dictionary<string, int> ReplyLimitationMember;
        void Application_Start(object sender, EventArgs e)
        {
            {
                var M = 1024 * 1024;
                ReplyLimitationOwner = new Dictionary<string, int>();
                ReplyLimitationOwner.Add("swf", 20 * M);
                ReplyLimitationOwner.Add("mp4", 600 * M);
                ReplyLimitationOwner.Add("mp3", 80 * M);
                ReplyLimitationMember = new Dictionary<string, int>();
                ReplyLimitationMember.Add("swf", 2 * M);
                ReplyLimitationMember.Add("mp4", 50 * M);
                ReplyLimitationMember.Add("mp3", 10 * M);
            }
            // 在应用程序启动时运行的代码
            MFLJson.JsonMachine.JsonContext.TagEmpty = File.ReadAllText(Server.MapPath("/Internal/Templates/Empty.txt"));
            MFLJson.JsonMachine.JsonContext.TagNull = File.ReadAllText(Server.MapPath("/Internal/Templates/Null.txt"));
            Global.root = Server.MapPath("/");
            MFLMirrorLib.Mirror.Init(
                (s1, s2, s3, b1, s4) =>
                {
                    var s = new MFLMirrorServices.WebServicesSoapClient();
                    s.InnerChannel.OperationTimeout = TimeSpan.FromMinutes(5);
                    return s.TryAccessCache(s1, s2, s3, b1, s4);
                },
                (s1, s2) =>
                {
                    var s = new MFLMirrorServices.WebServicesSoapClient();
                    s.Expires(s1, s2);
                },
                System.Configuration.ConfigurationManager.AppSettings["BaseUrl"],
                System.Configuration.ConfigurationManager.AppSettings["SharedSecret"],
                "goclassing");
            mailThread = new Thread(() =>
            {
                mailThreadStopping = false;
                while (!mailThreadStopping)
                {
                    try
                    {
                        var obj = MFLJson.JsonMachine.JsonContext.Initial("MailMsg", "GCMsg", (tn) => File.ReadAllText(root + "Internal\\Templates\\" + tn + ".html"));
                        var d = new gc_localtestEntities();
                        foreach (var u in d.gc_user.Where(tu => tu.msgType == 1))
                        {
                            foreach (var m in u.gc_msg.Where(tm => !tm.mailed && !tm.read))
                            {
                                obj["msgs"] = MFLJson.Json.Array(GCAuthenticated.GetMessage(m));
                                var s = new MemoryStream();
                                obj.RenderToStream(s);
                                s.Seek(0, SeekOrigin.Begin);
                                s.Close();
                                var str = Encoding.UTF8.GetString(s.GetBuffer());
                                try
                                {
                                    GCCommon.SendMail(u.email, m.content.Remove(m.content.Length > 10 ? 10 : m.content.Length), str, bool.Parse(ConfigurationManager.AppSettings["mailDebugging"]), root);
                                }
                                catch { }
                                m.mailed = true;
                            }
                        }
                        var nowHour = DateTime.Now.Hour;
                        foreach (var u in d.gc_user.Where(tu => tu.msgType == 2 && tu.msgClock == nowHour && tu.gc_msg.Count(tm => !tm.mailed && !tm.read) > 0))
                        {
                            var arr = MFLJson.Json.Array();
                            foreach (var m in u.gc_msg.Where(tm => !tm.mailed && !tm.read))
                            {
                                arr.Add(GCAuthenticated.GetMessage(m));
                                m.mailed = true;
                            }
                            obj["msgs"] = arr;
                            var s = new MemoryStream();
                            obj.RenderToStream(s);
                            s.Seek(0, SeekOrigin.Begin);
                            s.Close();
                            var str = Encoding.UTF8.GetString(s.GetBuffer());
                            try
                            {
                                GCCommon.SendMail(u.email, "每日消息汇总", str, bool.Parse(ConfigurationManager.AppSettings["mailDebugging"]), root);
                            }
                            catch { }
                        }
                        d.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        GCCommon.Log(ex.Message, root);
                    }
                    Thread.Sleep(60000);
                }
            });
            mailThread.Name = "MailThread";
            mailThread.Start();
        }
        static Thread mailThread;
        static bool mailThreadStopping;
        void Application_End(object sender, EventArgs e)
        {
            mailThreadStopping = true;
            do
            {
                Thread.Sleep(100);
            } while (mailThreadStopping);

            //  在应用程序关闭时运行的代码

        }
        static string[] staticMimeTypes = new[] { 
            ".css",
            ".png",
            ".js",
            ".html",
            ".gif",
            ".jpg",
            ".mp3",
            ".mp4",
            ".swf"
        };
        private static string root;
        void Application_PreSendRequestContent(object sender, EventArgs e)
        {
            if (Response.StatusCode == 200 && staticMimeTypes.Any(ts=> this.Request.Url.AbsolutePath.ToLower().EndsWith(ts)))
            {
               
                if (Request.Headers["If-Modified-Since"] != null && (new FileInfo(Server.MapPath(Request.Url.AbsolutePath))).Exists)
                {
                    if (DateTime.Parse(Request.Headers["If-Modified-Since"]) == (new FileInfo(Server.MapPath(Request.Url.AbsolutePath))).LastWriteTime)
                    {
                        Response.StatusCode = 304;
                        Response.StatusDescription = "Not Modified";
                        Response.End();
                        return;
                    }
                }
                    if (Request.QueryString["REMOTE_ADDR"] != "222.186.191.104"&&!Request.Url.AbsolutePath.Contains('$'))
                    {
                        if (Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["StaticBoost"]))
                        {
                            MFLMirrorLib.Mirror.HandleHttpRequest(Request.Url.AbsolutePath, true);
                        }
                    }
            }
                
            if (Request.Url.AbsolutePath.ToLower().StartsWith("/avatars/")&&Response.StatusCode==404)
            {
                Response.StatusCode = 200;
                try
                {
                    var d = new gc_localtestEntities();
                    var id = Convert.ToInt32(Request.Url.AbsolutePath.Substring(9, Request.Url.AbsolutePath.IndexOf('.', 9) - 9));
                    var sex = d.gc_user.First(tu => tu.id == id).sex;
                    string str = sex.HasValue ? (sex.Value ? "M" : "F") : "";
                    if (Request.Url.AbsolutePath.ToLower().EndsWith(".small.jpg"))
                    {
                        Response.TransmitFile(Server.MapPath("/avatars/default" + str + ".small.jpg"));
                    }
                    else
                    {
                        Response.TransmitFile(Server.MapPath("/avatars/default" + str + ".jpg"));
                    }
                }
                catch
                {
                    Response.StatusCode = 404;
                }
                Response.End();
            }

            if (Response.StatusCode == 404)
            {
                var d = new gc_localtestEntities();
                var coop = Server.UrlDecode(Request.Url.AbsolutePath.TrimEnd('/').Substring(1));
                
                if (d.gccon_coop.Any(tc => tc.coop == coop))
                {
                    Response.StatusCode = 200;
                    Server.Transfer("/T/All/Default.aspx?search=" + Server.UrlEncode(coop)
                        + (Request["page"] != null ? "&page=" + Request["page"] : ""));
                    return;
                }
            }
            if (Request.Url.AbsolutePath.ToLower().StartsWith("/styles/tags/") && (Response.StatusCode == 404))
            {
                Response.StatusCode = 200;
                Response.TransmitFile(Server.MapPath("/Styles/Tags/" + Server.UrlEncode(Server.UrlDecode(Request.Url.AbsolutePath.Substring("/styles/tags/".Length, Request.Url.AbsolutePath.Length - "/styles/tags/".Length-4)))+".gif"));
                Response.End();
                return;
            }
            if (Request.Url.AbsolutePath.ToLower().StartsWith("/t/") && (Response.StatusCode == 404 || Response.StatusCode == 403))
            {
                Response.StatusCode = 200;
                Server.Transfer("/Internal/Teacher.aspx?name=" + Server.UrlEncode(Request.Url.AbsolutePath.Substring(3)), true);
                Response.End();
                return;
            }
            if (Request.Url.AbsolutePath.ToLower().StartsWith("/c/") && (Response.StatusCode == 404 ))
            {
                var url=Request.Url.AbsolutePath.TrimEnd('/');
                if (url.Length == Internal.GCCommon.CourseIdLen + 3 && url.Substring(3).All(tc => char.IsDigit(tc)))
                {
                    Response.StatusCode = 200;
                    Server.Transfer("/Internal/Course.aspx?id=" + Server.UrlEncode(url.Substring(3)), true);
                    Response.End();
                    return;
                }
                if (url.Length > 4 + Internal.GCCommon.CourseIdLen)
                {
                    var part1 = url.Substring(3, Internal.GCCommon.CourseIdLen);
                    var part2 = url.Substring(3 + Internal.GCCommon.CourseIdLen + 1);
                    if (part1.All(tc => char.IsDigit(tc)))
                    {
                        if (part2.All(tc => char.IsDigit(tc)))
                        {
                            Response.StatusCode = 200;
                            Server.Transfer("/Internal/CoursePost.aspx?id=" + Server.UrlEncode(part1) + "&postId=" + Server.UrlEncode(part2));
                            return;
                        } 
                        if (part2.ToLower()=="members")
                        {
                            Response.StatusCode = 200;
                            Server.Transfer("/Internal/CourseMembers.aspx?id=" + Server.UrlEncode(part1));
                            return;
                        }
                    }
                    
                }
                var i = url.LastIndexOf('/');
                var i2 = url.Remove(i).LastIndexOf('/');
                Response.StatusCode = 200;
                if (i > 3)
                {
                    if (i2 > 3)
                    {
                        return;
                    }
                    Server.Transfer("/C/All/default.aspx?ctype1=" + Server.UrlEncode(Server.UrlDecode(Request.Url.AbsolutePath.Substring(3, i - 3)))
                        + "&ctypes2=" + Server.UrlEncode(Server.UrlDecode(Request.Url.AbsolutePath.TrimEnd('/').Substring(i+1)))
                        + (Request["page"] != null ? "&page=" + Request["page"] : ""), true);
                    return;
                }
                Server.Transfer("/C/All/default.aspx?ctype1=" + Server.UrlEncode(Server.UrlDecode(Request.Url.AbsolutePath.TrimEnd('/').Substring(3)))
                    + (Request["page"] != null ? "&page=" + Request["page"] : ""), true);
                return;
            }
            try
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    var a = (GCCommon.GetVisitor(this.Context) as GCAuthenticated);
                    a.U.timeLastActivity = DateTime.Now;
                    a.D.SaveChanges();
                }
            }
            catch { }
        }
        void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var root = Server.MapPath("/");
            //if (File.Exists(root + "Internal\\gcpublishing.txt"))
            //{
            //    var obj=MFLJson.Json.Object();
            //    GCCommon.CreateUploadTicket("/Internal/gcpublishing.zip", 100000000, 0, obj);
            //    File.WriteAllText(root + "Internal\\gcuploalapurl.txt", (obj["uploadApUrl"] as MFLJson.JsonString).Text);
            //    File.Delete(root + "Internal\\gcpublishing.txt");
            //    return;
            //}
            //if (Request.Url.AbsolutePath.Contains("$"))
            //{
            //    Response.StatusCode = 200;
            //    Response.ContentType = "text/plain";
                
            //    Response.End(); return;
                
            //}
            //if (ConfigurationManager.AppSettings["bind"] == "True" && Request.Url.Host != ConfigurationManager.AppSettings["bindDomain"])
            //{
            //    Response.Redirect("http://" + ConfigurationManager.AppSettings["bindDomain"] + (Request.Url.Port == 80 ? "" : ":" + Request.Url.Port.ToString()) + "/");
            //    return;
            //}
            //if (!Request.Cookies.AllKeys.Contains("TestingUser")&&ConfigurationManager.AppSettings["ready"] == "False" && Request.Url.AbsolutePath == "/"&&Request["ReturnUrl"]==null)
            //{
            //    Response.StatusCode = 200;
            //    Server.Transfer("Internal/NotReady.aspx");
            //    return;
            //}
            
            //if (Request.Url.AbsolutePath.ToLower().StartsWith("/internal/") && !ConfigurationManager.AppSettings["VServerIP"].Split(',').Contains(Request.ServerVariables["REMOTE_ADDR"]))
            //{
            //    Response.Redirect("/");
            //    return;
            //}
            //upload_ticket t;
            //gc_localtestEntities d = new gc_localtestEntities();
            //while ((t = d.upload_ticket.AsEnumerable().FirstOrDefault(tt => DateTime.Now > tt.expires)) != null)
            //{
            //    //过期，返还空间
            //    d.gc_user.First(tu => tu.id == t.space_user_id).spaceUsed -= t.limit;
            //    d.upload_ticket.DeleteObject(t);
            //    d.SaveChanges();

            //    var path = t.path;
            //    if (path != null && path.StartsWith("/Drop/reply"))
            //    {
            //        var id = path.Substring("/Drop/reply".Length);
            //        id = id.Remove(id.LastIndexOf('.'));
            //        var iid = Convert.ToInt32(id);
            //        if (d.gc_reply.Any(tr => tr.id == iid))//转换失败且未删除
            //        {
            //            d.gc_reply.DeleteObject(d.gc_reply.First(tr => tr.id == iid));
            //        }
            //    }
            //}

        }


        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }

    }
}
