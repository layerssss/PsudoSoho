using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Threading;
namespace MFLMirrorLib
{
    public static class Mirror
    {
        public static string MirrorBaseUrl{
            get{
                return "http://tangzx.cl14.53dns.net/";
            }
        }
        public static string MirrorIp{
            get{
                return "222.186.191.104";
            }
        }
        private static string baseUrl;
        private static string identity;
        private static List<string> dontCache;
        private static List<string> mirrorCachedPath;
        private static Queue<string> pendingPath;
        private static bool threadStopping;
        private static Thread thread;
        private static string sharedSecret;
        private static Func<string, string, string, bool, string, bool> serviceTryAccessCache;
        private static Action<string,string> serviceExpires;
        public static void Init(Func<string, string, string, bool, string, bool> serviceTryAccessCacheHandler, Action<string, string> serviceExpiresHandler, string baseUrl, string sharedSecret, string identity, params string[] dontCache)
        {
            Mirror.OnError += new Action<string>(Mirror_OnError);
            Mirror.serviceTryAccessCache = serviceTryAccessCacheHandler;
            Mirror.serviceExpires = serviceExpiresHandler;
            Mirror.baseUrl = baseUrl;
            Mirror.sharedSecret = sharedSecret;
            Mirror.identity = identity;
            Mirror.dontCache = new List<string>();
            Mirror.dontCache.AddRange(dontCache.Select(ts => ts.ToLower()));
            Mirror.pendingPath = new Queue<string>();
            Mirror.mirrorCachedPath = new List<string>();
            Mirror.thread = new Thread(Mirror.threadStart);
            Mirror.thread.Start();
        }

        static void Mirror_OnError(string obj)
        {
            
        }
        public static void threadStart()
        {
            Mirror.threadStopping = false;
            while (!Mirror.threadStopping)
            {
                var curPath = "";
                lock (Mirror.pendingPath)
                {
                    if (Mirror.pendingPath.Any())
                    {
                        curPath = Mirror.pendingPath.Dequeue();
                    }
                }
                if (curPath != "")
                {
                    try
                    {
                        var compare = md5(DateTime.UtcNow.DayOfYear + sharedSecret);
                        if (Mirror.serviceTryAccessCache(compare, identity + '/' + curPath, baseUrl + curPath, false, null))
                        {
                            lock (Mirror.mirrorCachedPath)
                            {
                                Mirror.mirrorCachedPath.Add(curPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnError("Mirror error:" + ex.Message);
                    }
                }
                Thread.Sleep(100);
            }
            Mirror.threadStopping = false;
        }

        public static event Action<string> OnError;
        public static void HandleHttpRequest(string absPath,bool redirectToTarget)
        {
            absPath = absPath.Trim('/').ToLower();
            if(dontCache.Contains(absPath)){
                return;
            }
            lock (Mirror.mirrorCachedPath)
            {
                if (Mirror.mirrorCachedPath.Contains(absPath))
                {
                    if (redirectToTarget)
                    {
                        HttpContext.Current.Response.Redirect(Mirror.MirrorBaseUrl + identity + '/' + absPath, true);
                    }
                    return;
                }
            }
            lock (Mirror.pendingPath)
            {
                Mirror.pendingPath.Enqueue(absPath);
            }
            //new Thread(() =>
            //{
            //    try
            //    {

            //        MFLMirrorServices.WebServicesSoapClient c = new MFLMirrorServices.WebServicesSoapClient();
            //        if (c.TryAccessCache(compare, path, "http://masonrygallery.com" + absurl, false, null))
            //        {
            //            lock (Global.MirrorCachedPath)
            //            {
            //                Global.MirrorCachedPath.Add(path);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log("Mirror error:" + ex.Message);
            //    }
            //}).Start();
        }
        public static void Expires(string path)
        {

            path = path.Trim('/').ToLower();
            lock (Mirror.mirrorCachedPath)
            {
                if (Mirror.mirrorCachedPath.Contains(path))
                {
                    Mirror.mirrorCachedPath.Remove(path);
                }
            }
            try
            {
                var compare = md5(DateTime.UtcNow.DayOfYear + sharedSecret);
                Mirror.serviceExpires(compare, Mirror.identity + '/' + path);
            }
            catch (Exception ex)
            {
                OnError("Mirror error:" + ex.Message);
            }
        }
        public static void Uninstall()
        {
            Mirror.threadStopping = true;
            while (Mirror.threadStopping)
            {
                Thread.Sleep(10);
            }
        }

        private static string md5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = get_md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            get_md5.Dispose();
            return nyteArrayToHexString(bytes, 0, 16);
        }
        private static string nyteArrayToHexString(byte[] buf, int offset, int len)
        {
            len += offset;
            var sb = new StringBuilder();
            for (int i = offset; i < len; i++)
            {
                sb.Append(buf[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
