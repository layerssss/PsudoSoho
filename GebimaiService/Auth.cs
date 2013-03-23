using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class Auth
    {
        /// <summary>
        /// 【单一形式接口】供OAuth2.0协议使用的接口；成功后自动跳转到/OAuthFinished.htm
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="remember">if set to <c>true</c> [记住密码].</param>
        [ispJs.Action]
        public void OAuthValidate(string code, string provider, bool remember)
        {
            string username, aId, aToken, name, avatarUrl, gender, data;
            DateTime aTokenExp;
            try
            {
                Global.SocialProviders[provider].Validate(code, remember, out username, out aId, out aToken, out aTokenExp, out name, out avatarUrl, out gender, out data);
            }
            catch
            {
                return;
            }
            var d = Global.Entities;
            var user = d.users.FirstOrDefault(tu => tu.authProvider == provider && tu.authId == aId);
            if (user == null)
            {
                user = new global::user()
                {
                    authId = aId,
                    authProvider = provider,
                    avatarUrl = "",
                    gender = "",
                    name = "",
                    username = ""
                };
                d.users.InsertOnSubmit(user);
                d.SubmitChanges();
                secret s = new secret()
                {
                    userid = user.id,
                    authToken = "",
                    authTokenExpire = DateTime.Now,
                    logon = 0
                };
                d.secrets.InsertOnSubmit(s);
                d.SubmitChanges();
            }

            user.username = username;
            GetSecret(user.id, d).logon = new Random().Next();
            GetSecret(user.id, d).authToken = aToken;
            GetSecret(user.id, d).authTokenExpire = aTokenExp;
            GetSecret(user.id, d).data=data;
            user.name = name;
            user.avatarUrl = avatarUrl;
            user.gender = gender;
            d.SubmitChanges();
            var ckuid = new HttpCookie("gcuid", user.username.ToString())
            {
                Domain = Global.CookieDomain
            };
            var ckuhash = new HttpCookie("gchash", user.username + GetSecret(user.id, d).logon.ToString())
            {
                Domain = Global.CookieDomain
            };
            if (remember)
            {
                ckuid.Expires = DateTime.Now.AddMonths(1);
                ckuhash.Expires = DateTime.Now.AddMonths(1);
            }
            ispJs.WebApplication.SafeDelete("", "*.user");
            HttpContext.Current.Response.Cookies.Add(ckuid);
            HttpContext.Current.Response.Cookies.Add(ckuhash);
            HttpContext.Current.Response.Redirect("/OAuthFinished.htm");
        }
        /// <summary>
        /// 退出当前用户的所有会话。
        /// </summary>
        [ispJs.Action]
        public void Logout()
        {
            var d = Global.Entities;
            var uid = Username;
            if (uid == null)
            {
                throw (new Exception("您当前没有登陆."));
            }
            var user = d.users.First(tu => tu.username == uid);
            GetSecret(user.id, d).logon = new Random().Next();
            d.SubmitChanges();

        }

        /// <summary>
        /// 【单一行为接口】跳转到用户首页
        /// </summary>
        [ispJs.Action]
        public void JumpHomepage()
        {
            ispJs.WebApplication.Response.Redirect("/" + Auth.Username + ".user");
        }
        /// <summary>
        /// 获取登陆窗体URL。
        /// </summary>
        /// <param name="remember">if set to <c>true</c> [记住密码].</param>
        /// <param name="provider">The provider.</param>
        /// <param name="url">The URL.</param>
        [ispJs.Action]
        public void GetLoginUrl(bool remember,string provider,out string url){
            url = Global.SocialProviders[provider].GetLoginUrl(remember);
        }
        public static string Username
        {
            get
            {
                var d=Global.Entities;
                var ckin = HttpContext.Current.Request.Cookies;
                var ckuid = ckin["gcuid"];
                var ckuhash = ckin["gchash"];
                if (ckuid == null || ckuhash == null)
                {
                    return null;
                }
                var uid = ckuid.Value;
                var uhash = ckuhash.Value;
                var u =d .users.FirstOrDefault(tu => tu.username == uid);
                if (u == null)
                {

                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("gcuid") { Expires = DateTime.Now.AddDays(-1), Domain = Global.CookieDomain });
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("gchash") { Expires = DateTime.Now.AddDays(-1), Domain = Global.CookieDomain });
                    return null;
                }
                if (uhash != uid + GetSecret(u.id,d).logon.ToString())
                {
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("gcuid") { Expires = DateTime.Now.AddDays(-1), Domain = Global.CookieDomain });
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("gchash") { Expires = DateTime.Now.AddDays(-1), Domain = Global.CookieDomain });
                    return null;
                }
                return uid;
            }
        }
        public static secret GetSecret(int userId,gebimaicom d)
        {
            return d.secrets.First(ts => ts.userid == userId);
        }
        /// <summary>
        /// 获取当前的用户信息。
        /// </summary>
        /// <param name="me">当前用户的信息，如果未登录则为null</param>
        /// <param name="message">The message.</param>
        [ispJs.Action]
        public void GetStatus(out global::user me, out string message, out global::sender senderInfo,out global::admin adminInfo)
        {
            senderInfo = null;
            adminInfo = null;
            var d = Global.Entities;
            var c = ispJs.WebApplication.Request.Cookies["errormsg"];
            if (c == null || c.Value == "null")
            {
                message = null;
            }
            else
            {
                message = c.Value;
                ispJs.WebApplication.Response.Cookies.Add(new HttpCookie("errormsg") { Expires = DateTime.Now.AddDays(-1), Path = "/" });
            }
            var uid = Username;
            if (uid == null)
            {
                me = null;
                return;
            }

            var u = d.users.First(tu => tu.username == uid);
            me = u;
            senderInfo = u.senders.FirstOrDefault();
            adminInfo = u.admins.FirstOrDefault();

        }
        /// <summary>
        /// 【单一形式接口】供OAuth2.0协议使用的接口；用于接收信息推送。
        /// </summary>
        /// <param name="provider">The provider.</param>
        [ispJs.Action]
        public void OAuthNotify(string provider)
        {
            Global.SocialProviders[provider].Notify();
        }

        /// <summary>
        /// 获取可用provider列表
        /// </summary>
        /// <param name="providers">The providers.</param>
        [ispJs.Action]
        public void GetProvidersList(out string[] providers)
        {
            providers = Global.SocialProviders.Keys.ToArray();
        }
    }
}