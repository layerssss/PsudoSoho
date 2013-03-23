using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net;
/// <summary>
///WeiboProvider 的摘要说明
/// </summary>
namespace GebimaiService
{
    public class WeiboProvider : ISocialProvider
    {

        static System.Net.WebClient c = new System.Net.WebClient();
        static string getStr(bool post, string url, params string[] kvp)
        {
            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < kvp.Length; i++)
            {
                sb.Append(kvp[i]);
                sb.Append('=');
                i++;
                sb.Append(HttpUtility.UrlEncode(kvp[i]));
                sb.Append('&');
            }
            sb = sb.Remove(sb.Length - 1, 1);
            var str = "Network error:";
            for (var i = 0; i < 1; i++)
            {
                try
                {
                    if (post)
                    {
                        var req = WebRequest.Create(ap + url);
                        req.Method = "POST";
                        req.ContentType = "application/x-www-form-urlencoded";
                        var data = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                        req.GetRequestStream().Write(data, 0, data.Length);
                        req.GetRequestStream().Close();
                        var res = req.GetResponse();
                        var sr = new System.IO.StreamReader(res.GetResponseStream());
                        var text = sr.ReadToEnd();
                        sr.Close();
                        return text;
                        //return c.UploadString(ap + url, sb.ToString());
                    }
                    else
                    {
                        return c.DownloadString(ap + url + "?" + sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        var sr = new System.IO.StreamReader((ex as WebException).Response.GetResponseStream());
                        str += sr.ReadToEnd();
                        sr.Close();
                    }
                    catch { }
                    //str += ex.ToString();
                }
            }
            throw (new System.Net.WebException(str));
        }
        static string  post(string url, params string[] kvp)
        {
            return getStr(true, url, kvp);
        }
         static string  get(string url, params string[] kvp)
        {
            return getStr(false, url, kvp);
        }

        static string ap = "https://api.weibo.com/";
        public static string AppID = "1183298141";
        static string appSecret = "5d8a2202eb74a8e34054822b38bbebc5";
        static string appAuthToken = "";
        static DateTime appAuthTokenExpire;
        static string appRefreshToken = "";
        static string VERIFY_TOKEN = "poiuytrewq";
        public WeiboProvider()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
            c.Encoding = System.Text.Encoding.UTF8;
        }

        #region ISocialProvider 成员

        public void Validate(string code, bool remember, out string id, out string authId, out string authToken, out DateTime authTokenExpre, out string name, out string avatarUrl, out string gender,out string data)
        {
            var o = JObject.Parse(post("oauth2/access_token",
                "client_id", AppID,
                "client_secret", appSecret,
                "grant_type", "authorization_code",
                "redirect_uri", Global.HttpRoot + "OAuth/OAuthValidate?provider=weibo&remember=" + remember.ToString().ToLower() + "&redirect=" + HttpUtility.UrlEncode("/OAuthFinished.htm"),
                "code", code));
            authToken = (string)o["access_token"];
            authTokenExpre = DateTime.Now.AddSeconds((int)o["expires_in"]);
            data = (string)o["refresh_token"];
            o = JObject.Parse(get("2/account/get_uid.json",
                "access_token", authToken));
            authId = ((long)o["uid"]).ToString();
            o = JObject.Parse(get("2/users/show.json",
                "access_token", authToken,
                "uid", authId));
            id = authId + ".weibo";
            name = (string)o["screen_name"];
            avatarUrl = (string)o["profile_image_url"];
            gender = (string)o["gender"];
        }

        public void Notify()
        {
        }

        #endregion

        #region ISocialProvider 成员


        public string GetLoginUrl(bool remember)
        {
            return string.Format(ap + "oauth2/authorize?client_id={0}&redirect_uri={1}&display=popup",
                AppID,
                HttpUtility.UrlEncode(Global.HttpRoot + "Auth/OAuthValidate?provider=weibo&remember=" + remember.ToString().ToLower() + "&redirect=" + HttpUtility.UrlEncode("/OAuthFinished.htm"))
                );
        }

        #endregion

        internal static void PingBot(out bool results)
        {
            
                var d = Global.Entities;
            JObject o;
            //if((appAuthTokenExpire-DateTime.Now).TotalMinutes<10){
            //    o = JObject.Parse(post("oauth2/access_token",
            //        "client_id", AppID,
            //        "client_secret", appSecret,
            //        "grant_type", "refresh_token",
            //        "refresh_token", appRefreshToken));
            //    var secret = d.secrets.First(ts => ts.authToken == appAuthToken);
            //    secret.authToken = (string)o["access_token"];
            //    secret.authTokenExpire = DateTime.Now.AddSeconds((long)o["expires_in"]);
            //    secret.data = (string)o["refresh_token"];
            //    d.SubmitChanges();
            //    appAuthToken = secret.authToken;
            //    appAuthTokenExpire = secret.authTokenExpire;
            //    appRefreshToken = secret.data;
            //}



            results = false;
            if (Global.IsLocal)
            {
                return;
            }
            bool isStatus = true;
            o = JObject.Parse(get("2/statuses/mentions.json",
                "since_id", laststatusID.ToString(),
                "access_token", appAuthToken));

            var wbst = o["statuses"].LastOrDefault();
            if (wbst == null)
            {
                o = JObject.Parse(get("2/comments/to_me.json",
                    "since_id", lastcommentID.ToString(),
                    "access_token", appAuthToken));
                wbst = o["comments"].LastOrDefault();
                isStatus = false; 
            }
            if (wbst != null)
            {

                var text = ((string)wbst["text"]).Trim();
                if (text.Contains("回复@隔壁买:"))
                {
                    text = text.Substring(text.LastIndexOf("回复@隔壁买:") + 7).Trim();
                }
                ispJs.GlobalLog.Fire(null, " text: {0}",text);
                {
                    results = true;
                    var replyText = new[] { "亲，我还在成长之中，请勿难为我。" };
                    switch (text)
                    {
                        case "ping":
                            replyText = new[] { "pong" };
                            break;
                        default:
                            try
                            {
                                replyText = Logic.Reply(text, ((long)wbst["user"]["id"]).ToString());
                            }
                            catch(Exception ex) {
                                replyText = new[] { ex.Message };
                            }
                            break;
                    }
                    foreach (var t in replyText)
                    {
                        try
                        {
                            if (isStatus)
                            {
                                post("2/comments/create.json",
                                    "id", ((long)wbst["id"]).ToString(),
                                    "comment", t + " [" + DateTime.Now.ToLongTimeString() + ']',
                                    "access_token", appAuthToken);
                            }
                            else
                            {
                                post("2/comments/reply.json",
                                    "id", ((long)wbst["status"]["id"]).ToString(),
                                    "cid", ((long)wbst["id"]).ToString(),
                                    "comment", t + " [" + DateTime.Now.ToLongTimeString() + ']',
                                    "access_token", appAuthToken);
                            }
                        }
                        catch { }
                    }
                }
                if (isStatus)
                {
                    laststatusID = (long)wbst["id"];
                }
                else
                {
                    lastcommentID = (long)wbst["id"];
                }
                var s = d.wbstates.First();
                s.lastcommentID = lastcommentID;
                s.laststatusID = laststatusID;
                d.SubmitChanges();
            }

            
        }
        static long lastcommentID;
        static long laststatusID;
        internal static void InitBot()
        {
            var d = Global.Entities;
            var s =d.wbstates.First();
            var uid = s.operatoruserID;
            appAuthToken = d.secrets.First(tu => tu.id == uid).authToken;
            appAuthTokenExpire = d.secrets.First(tu => tu.id == uid).authTokenExpire;
            appRefreshToken = d.secrets.First(tu => tu.id == uid).data;
            lastcommentID = s.lastcommentID;
                laststatusID=s.laststatusID;
        }


        internal static string PostStatus(int uid, string title, string imgUrl, string url, int num)
        {
            var sec = Global.Entities.secrets.First(ts => ts.userid == uid).authToken;
            var o = JObject.Parse(post("2/statuses/update.json",
                "access_token", sec,
                "status", string.Format("@隔壁买 给我来#{0}件# #{1}#( {2} ){3}",
                num, title, url, DateTime.Now.ToLongTimeString())));
            var id = (long)o["id"];
            var wuid = (long)o["user"]["id"];
            o=JObject.Parse(get("2/statuses/querymid.json",
                "access_token",sec,
                "type","1",
                "id",id.ToString()));
            return "http://weibo.com/"+wuid+'/'+(string)o["mid"];
        }
        internal static void SendNotification(string content, int uid)
        {
            var d = Global.Entities;
            d.users.First(tu => tu.id == uid).message = content;
            d.SubmitChanges();

        }
    }
}