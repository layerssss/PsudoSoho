using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MFLJson;
using System.Net;
using System.Configuration;
using System.IO;
using System.Text;
namespace GoClassing.Internal
{
    public static class GCCommon
    {
        public static int CourseIdLen = 5;
        public static Func<string, string> HtmlTemplateReader = (templateName) => { return GCTemplateReader.Get(templateName + ".html"); };
        public static string[] PreservedUsername = new[] { "all","admin","any","administrator","goclassing","help","system"};
        public static string[] DocTypes = new[] { "doc", "docx", "xsl", "xslx", "ppt", "pptx", "pdf" };
        public static string[] MediaTypes = new[] { "3gp", "avi", "wmv", "mp4", "rmvb", "rm", "mkv", "mov", "mp4","flv" };
        public static string[] SoundTypes = new[] { "ogg", "mp3", "wav", "wma","flac" };
        public static string GetTime(DateTime time)
        {
            var span = DateTime.Now - time;
            if (span.TotalSeconds < 60)
            {
                return (int)span.TotalSeconds + "秒前";
            }
            if (span.TotalMinutes < 60)
            {
                return (int)span.TotalMinutes + "分钟前";
            }
            if (span.TotalHours < 6)
            {
                return (int)span.TotalHours + "小时前";
            }
            if (time.DayOfYear ==DateTimeOffset.Now.DayOfYear)
            {
                return "今天" + time.ToShortTimeString();
            }
            if (time.DayOfYear == DateTimeOffset.Now.AddDays(-1).DayOfYear)
            {
                return "昨天" + time.ToShortTimeString();
            }
            if (time.DayOfYear == DateTimeOffset.Now.AddDays(-2).DayOfYear)
            {
                return "前天" + time.ToShortTimeString();
            }
            if (span.TotalDays < 30)
            {
                return (int)span.TotalDays + "天前";
            }
            return time.ToShortDateString();
        }
        public static JsonObject GetPagedList<T, Tkey>(IQueryable<T> item, Func<T, Tkey> keySelector, Func<T, JsonObject> func, string pagestr, int pageSize)
        {
            return GetPagedList(item, keySelector, func, pagestr, pageSize, false);
        }
        public static JsonObject GetPagedList<T,Tkey>(IQueryable<T> item,Func<T,Tkey> keySelector, Func<T, JsonObject> func,string pagestr,int pageSize,bool ascOrder)
        {
            if (pagestr == null)
            {
                return  Json.Object("listItems",(ascOrder?Json.Array(item.OrderBy(keySelector).Select(func)): Json.Array(item.OrderByDescending(keySelector).Select(func))),"pages",Json.String(null));
            }
            var page =  Convert.ToInt32(pagestr);
            if (page < 0)
            {
                if(HttpContext.Current.Request.UrlReferrer!=null){
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UrlReferrer.PathAndQuery,true);
                    return null;
                }
                else
                {
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsolutePath + "?page=0");
                    return null;
                }
            }
            var count = (item.Count() + pageSize - 1) / pageSize;
            var pages = Json.Object();
            if (page != 0)
            {
                pages.Add(UI.PagePrev, Json.Number(page - 1));
            }
            else
            {
                pages.Add(UI.PagePrev, Json.Number(-1));
            }
            for (var i = 0; i < count; i++)
            {
                if (page == i)
                {
                    pages.Add((i + 1).ToString(), Json.Number(-2));
                }
                else
                {
                    pages.Add((i + 1).ToString(), Json.Number(i));
                }
            }
            if (count == 0 || page == count - 1)
            {
                pages.Add(UI.PageNext, Json.Number(-1));
            }
            else
            {
                pages.Add(UI.PageNext, Json.Number(page + 1));
            }
            return ascOrder? 
                Json.Object("listItems",Json.Array(item.OrderBy(keySelector).Skip(pageSize * page).Take(pageSize).Select(func)),
                "pages", pages):
                Json.Object("listItems", Json.Array(item.OrderByDescending(keySelector).Skip(pageSize * page).Take(pageSize).Select(func)),
                "pages", pages);
        }
        public static bool VerifyRecaptcha(HttpContext context,ref string msg)
        {
            var challenge =context.Request["recaptcha_challenge_field"];
            var response = context.Request["recaptcha_response_field"];
            if (challenge == null)
            {
                return false;
            }
            WebClient c = new WebClient();
            var ip=context.Request.ServerVariables["REMOTE_ADDR"];
            var str = c.DownloadString("http://www.google.com/recaptcha/api/verify?privatekey=6LdopMcSAAAAAJFTjVwbZLfdIvXEOmjLs8qY0r0g&remoteip=" + context.Server.UrlEncode(ip) + "&challenge=" + context.Server.UrlEncode(challenge) + "&response=" + context.Server.UrlEncode(response));
            if (str.StartsWith("true"))
            {
                return true;
            }
            else
            {
                msg = str;
                return false;
            }
        }
        public static void SendMail(string receiver, string title, string content)
        {
            SendMail(receiver, title, content, false, HttpContext.Current.Server.MapPath("/"));
        }
        public static void SendMail(string receiver, string title, string content,bool debug,string root)
        {

            var temp = File.ReadAllText(root+"Internal\\Templates\\Mail.html");
            title = "[上课网]" + title;
            temp = temp.Replace("$content$", content);
            temp = temp.Replace("$title$", title);
            temp = temp.Replace("$receiver$", receiver);
            temp = temp.Replace("$baseUrl$", ConfigurationManager.AppSettings["baseUrl"]);
            if (debug)
            {
                File.WriteAllText(root+"Internal\\Messages\\" + receiver + DateTime.Now.ToString("-yyyyMMddhhmmss") + ".html", temp);
            }
            else
            {
                
                try
                {

                    if (Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["mailUseVServer"]))
                    {
                        VServerServices.WebServicesSoapClient c = new VServerServices.WebServicesSoapClient();
                        c.SendMail(receiver, title, temp);
                        c.Close();
                    }
                    else
                    {
                        System.Net.Mail.SmtpClient c = new System.Net.Mail.SmtpClient();
                        c.Send(new System.Net.Mail.MailMessage(System.Configuration.ConfigurationManager.AppSettings["defaultMailSender"], receiver,
                            title, temp) { IsBodyHtml = true });
                        c.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Log("Error sending mail:" + ex.Message, root);
                }
            }

        }
        public static void ClientRegisterASPSESSID(HttpContext current,System.Web.UI.Page page)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, typeof(System.Web.UI.Page), "ASPSESSID", "var ASPSESSID='" + current.Session.SessionID + "';", true);
        }
        public static void OnlyAuthenticated(HttpContext content)
        {

            try
            {
                if (!content.User.Identity.IsAuthenticated)
                {
                    content.Response.Redirect("/?ReturnUrl=" + content.Server.UrlEncode(content.Request.Url.PathAndQuery));
                }
            }
            catch (NullReferenceException)
            {
                content.Response.Redirect("/?ReturnUrl=" + content.Server.UrlEncode(content.Request.Url.PathAndQuery));
            }
        }
        public static string Md5Part(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = get_md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            get_md5.Dispose();
            return ByteArrayToHexString(bytes, 0, 16);
        }
        public static string ByteArrayToHexString(byte[] buf, int offset, int len)
        {
            len += offset;
            var sb = new StringBuilder();
            for (int i = offset; i < len; i++)
            {
                sb.Append(buf[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public static string GetMailVerifyHash(string mail)
        {
            return Md5Part(System.Configuration.ConfigurationManager.AppSettings["mainVerifySalt"] + mail);
        }
        public static string NotNull(string field)
        {
            var val = ((HttpContext.Current.Request[field] ?? HttpContext.Current.Request[field+'2'])?? "").Trim();
            if (val=="")
            {
                throw (new GCException(HttpContext.Current.Request[field] != null ? field : field + '2', GCExceptionType.FieldReqired));
            }
            return val;
        }
        public static GCVisitor GetVisitor(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return new GCGuest(new gc_localtestEntities());
            }
            var loginname = context.User.Identity.Name;
            var a = new GCAuthenticated(new gc_localtestEntities());
            a.U = a.D.gc_user.First(tu => tu.username == loginname);
            return a;
        }
        public static void Login(string username, string password,bool remember,HttpContext context,string redirect)
        {
            if (!context.Request.IsSecureConnection)
            {
                //GCException.GCConfirm(UI.LoggingViaHttp);
            }var d=(new gc_localtestEntities());
            var u = d.gc_user.FirstOrDefault(tu => tu.username == username || tu.email == username);
            if(u==null){
                GCException.GCStopFieldError("username","该用户不存在");
            }
            if(u.password!=password){

                GCException.GCStopFieldError("password", "密码错误");
            }
            if (!u.mailverified)
            {
                GCException.GCReCaptcha();
                GCCommon.SendRegisterMail(u.email, context, u.username, redirect);
                GCException.GCStopMessage(UI.MailNeedVerification);
            }
            if (u.truename == "[匿名用户]")
            {
                if (!d.gc_msg.Any(tm =>
                    tm.user_id == u.id &&
                    !tm.read &&
                    tm.nexturl == "/T/"))
                {
                    d.gc_msg.AddObject(new gc_msg()
                    {
                        content = UI.MsgTruenameNotConfig,
                        fromid = 0,
                        mailed = false,
                        nexturl = "/T/",
                        read = false,
                        time = DateTime.Now,
                        user_id = u.id
                    });
                    d.SaveChanges();
                }
            }
            if (u.pwdAnswer == null || u.pwdAnswer.Trim()=="")
            {
                if (!d.gc_msg.Any(tm =>
                       tm.user_id == u.id &&
                       !tm.read &&
                       tm.nexturl == "/Home/Security/#Question"))
                {
                    d.gc_msg.AddObject(new gc_msg()
                    {
                        content = UI.MsgPwdAnserNotConfig,
                        fromid = 0,
                        mailed = false,
                        nexturl = "/Home/Security/#Question",
                        read = false,
                        time = DateTime.Now,
                        user_id = u.id
                    });
                    d.SaveChanges();
                }
            }
            if (ConfigurationManager.AppSettings["ready"] == "False")
            {
                context.Response.SetCookie(new HttpCookie("TestingUser")
                {
                    Expires = DateTime.Now.AddMonths(2),
                    Value = "true"
                });
            }
            System.Web.Security.FormsAuthentication.SetAuthCookie(u.originalusername, remember);
            if (remember)
            {
                HttpContext.Current.Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddMonths(1);
            }
        }
        public static void Logout(HttpContext context)
        {
            System.Web.Security.FormsAuthentication.SignOut();
        }
        public static void Register(string username,string email,string password1,string password2,bool agreement,string redirect,HttpContext context)
        {
            if (password1 != password2)
            {
                GCException.GCStopFieldError("password2", "两次输入的密码不相同");
            }
            if (!agreement)
            {
                GCException.GCStopFieldError("agreement", "您必须接受《上课网免费服务使用条款》才可继续");
            }
            if (GCCommon.PreservedUsername.Contains(username))
            {

                GCException.GCStopFieldError("regusername", "抱歉，您输入的用户名已经被系统保留，请换一个试试");
            }
            GCException.GCReCaptcha();
            try
            {
                System.Web.Security.Membership.CreateUser(username, password1, email);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("用户名") || ex.Message.Contains("username"))
                {
                    GCException.GCStopFieldError("regusername", ex.Message);
                }
                if (ex.Message.Contains("电子邮件")||ex.Message.Contains("mail"))
                {
                    GCException.GCStopFieldError("regemail", ex.Message);
                }
                if (ex.Message.Contains("密码") || ex.Message.Contains("password"))
                {
                    GCException.GCStopFieldError("password1", "您输入的密码不符合要求，密码最小长度为6位");
                }
                GCException.GCStopMessage(ex.Message);
            }
            var f = 0;
            foreach (var a in Enum.GetNames(typeof(GCUserFlag)))
            {
                f |= 1 << (int)(GCUserFlag)Enum.Parse(typeof(GCUserFlag), a);
            }
            var d = new gc_localtestEntities();
            d.gc_user.First(tu => tu.originalusername == username).flagsettings = f;
            d.SaveChanges();
            GCCommon.SendRegisterMail(email,context,username, redirect);
        }
        private static void SendRegisterMail(string email, HttpContext context, string username, string redirect)
        {
                var url = "$baseUrl$MailVerifying.aspx?mail=" + context.Server.UrlEncode(email) + "&hash=" + context.Server.UrlEncode(GCCommon.GetMailVerifyHash(email)) + "&redirect=" + context.Server.UrlEncode(redirect);
                Internal.GCCommon.SendMail(email, UI.MailVerification, string.Format(UI.MailVerificationContent, url, username));
        }
        public static void VerifyMail(string mail, string hash)
        {
            try
            {
                if (hash != Internal.GCCommon.GetMailVerifyHash(mail))
                {
                    throw (new Exception());
                }
                var d = new gc_localtestEntities();
                var u=d.gc_user.First(tu => tu.email == mail);
                u.mailverified = true;
                d.SaveChanges();
                System.Web.Security.FormsAuthentication.SetAuthCookie(u.username, false);
                GCCommon.SendMail(mail, UI.MailVerificated, string.Format(UI.MailVerificatedContent,u.username,mail));
                return;
            }
            catch
            {
                GCException.GCStopMessage(UI.VerificationHashWrong);
            }
        }
        public static string GetPwdQuestion(string loginname)
        {
            var u = (new gc_localtestEntities()).gc_user.FirstOrDefault(tu => (tu.username == loginname || tu.email == loginname));
            if (u == null)
            {
                GCException.GCStopFieldError("rloginname", "找不到该用户名或电子邮件地址");
            }
            if (!u.mailverified)
            {
                GCException.GCStopFieldError("rloginname", "您的电子邮件地址尚未被验证，不能进行密码重设操作");
            }
            if (u.pwdQuestion==null||u.pwdQuestion=="")
            {
                GCException.GCStopFieldError("rloginname", "您尚未设置安全验证问题");
            }
            return u.pwdQuestion;
        }
        public static string RetrievePassword(string loginname, string pwdAnser)
        {
            GCException.GCReCaptcha();
            var d=(new gc_localtestEntities());
            GetPwdQuestion(loginname);
            var u = d.gc_user.FirstOrDefault(tu => (tu.username == loginname || tu.email == loginname));
            if (u.pwdAnswer!=pwdAnser)
            {
                GCException.GCStopFieldError("r2pwdanswer", "安全验证问题答案错误");
            }
            u.password=System.Web.Security.Membership.GeneratePassword(8,1);
            d.SaveChanges();
            SendMail(u.email, UI.MailPwdRetrieval, string.Format(UI.MailPwdRetrievalContent, 
                u.password,
                u.username
                ));
            return u.email.Remove(1) + "******" + u.email.Substring(u.email.IndexOf("@"));
        }
        public static void VerifyOldPwdAnser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                GCException.GCStopMessage("登录超时，请刷新页面后重新登录再试");
            }
            GCException.GCReCaptcha();
            var uname=HttpContext.Current.User.Identity.Name;
            var D = new gc_localtestEntities();
            var u = D.gc_user.First(tu => tu.username == uname);
            if (u.pwdAnswer != null && u.pwdAnswer != GCCommon.NotNull("oldPwdAnswer"))
            {
                GCException.GCStopFieldError("oldPwdAnswer", "安全问题答案错误。");
            }
        }
        public static void CreateUploadTicket(string path,int limit,int spaceUserId,JsonObject obj)
        {
            
            var d = new gc_localtestEntities();

            upload_ticket t;


            var u = d.gc_user.First(tu => tu.id == spaceUserId);
            u.spaceUsed += limit;
            if (u.spaceUsed > u.spaceTotal)
            {
                GCException.GCStopMessage("您的剩余上传空间不不足(" + GCCommon.FormatSpace(u.spaceTotal-u.spaceUsed+limit) + ")，要进行该操作必须剩余至少" + GCCommon.FormatSpace(limit) + "的空间。");
            }
             t= new upload_ticket()
            {
                path = path,
                id=Guid.NewGuid(),
                expires=DateTime.Now.AddSeconds(5),
                limit=limit,
                space_user_id=spaceUserId
            };
            d.upload_ticket.AddObject(t);
            d.SaveChanges();
            obj["uploadLimit"] = Json.Number(limit);
            obj["uploadApUrl"] = Json.String(ConfigurationManager.AppSettings["uploadApUrl"] + "?TicketId=" + HttpContext.Current.Server.UrlEncode(t.id.ToString()));
        }
        public static string FormatSpace(long limit)
        {
            if (limit < 1024)
            {
                return limit + "B";
            }
            if (limit < 1024 * 1024)
            {
                return limit / 1024 + "KB";
            }
            if (limit < 1024 * 1024 * 1024)
            {
                return limit / (1024 * 1024) + "MB";
            }
            var g = (limit / (1024 * 1024 * 1024 / 10)).ToString();
            return g.Remove(g.Length - 1) + '.' + g.Substring(g.Length - 1) + "GB";
        }
        public static string FormatSpace(string path)
        {
            try
            {
                return FormatSpace(new FileInfo(path).Length);
            }
            catch
            {
                return "未知";
            }
        }
        public static void Log(string p, string root)
        {
            lock (GCCommon.logLocker)
            {
                File.AppendAllText(root.TrimEnd('\\') + "\\Internal\\Errors\\" + DateTime.Now.ToString("yyyyMMddhh") + ".txt", "\r\n" + p);
            }
        }
        static object logLocker = new object();
        public static string FormatTimeSpan(int p)
        {
            var str = "";
            if (p > 3600)
            {
                str += p / 3600 + "小时";
                p %= 3600;
            }
            if (p > 60)
            {
                str += p / 60 + "分钟";
                p %= 60;
            }
            str += p + "秒";
            return str;
        }
    }
}