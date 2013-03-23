using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace MFL
{
    public static class SharedConfig
    {
        public static string MFLBaseUrl
        {
            get
            {
                return Debugging ? "http://testing.build.xunnlv.com/" : "http://build.xunnlv.com/";
            }
        }
        public static string AdminBaseUrl
        {
            get
            {
                return Debugging ? "http://testing.admin.xunnlv.com/" : "http://admin.xunnlv.com/";
            }
        }
        public static string LodgeBaseUrl
        {
            get
            {
                return Debugging?"http://testing.hotel.xunnlv.com/":"http://hotel.xunnlv.com/";
            }
        }
        public static string PasswordSalt
        {
            get
            {
                return "xzbiopql;r.xcz;ljkgp][icziopuq";
            }
        }
        public static void ValidateRecaptcha()
        {
            var Context = System.Web.HttpContext.Current;
            var challenge = Context.Request["recaptcha_challenge_field"];
            var response = Context.Request["recaptcha_response_field"];
            WebClient c = new WebClient();
            var ip = Context.Request.ServerVariables["REMOTE_ADDR"];
            var str = "";
            try
            {
                str = c.DownloadString("http://www.google.com/recaptcha/api/verify?privatekey="+RecaptchaPrivateKey
                    + "&remoteip=" + Context.Server.UrlEncode(ip)
                    + "&challenge=" + Context.Server.UrlEncode(challenge)
                    + "&response=" + Context.Server.UrlEncode(response));
            }
            catch {
                throw (new Exception("抱歉，验证码验证失败，请重试。"));
            }
            if (!str.StartsWith("true"))
            {
                throw (new Exception("抱歉，验证码输入错误，请重试。"));
            }
        }

        public static string CookieDomain
        {
            get
            {
                return ".xunnlv.com";
            }
        }

        public static bool Debugging
        {
            get
            {
                return bool.Parse(System.Configuration.ConfigurationManager.AppSettings["Debugging"]);
            }
        }
        public static string RecaptchaPublicKey
        {
            get
            {
                return "6Lc0esoSAAAAABTqLkxt4erCTlpE8V4Jp1KDsIRA";
            }
        }
        public static string RecaptchaPrivateKey
        {
            get
            {
                return "6Lc0esoSAAAAAE68zDigMgA6davZn7LbKelJ6R-V";
            }
        }
    }
}
