using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using YupooAPI.NET;
using System.Web;
namespace GCPicBroker
{
    public class PicUploader : IDisposable
    {
        Dictionary<string,string> providers;
        public PicUploader(string tempPath,params string[] providerEndPoints)
        {
            this.tempPath = tempPath;
            this.providers = new Dictionary<string,string>();
            for (var i = 0; i < providerEndPoints.Length; i+=2)
            {
                this.providers.Add(providerEndPoints[i], providerEndPoints[i + 1]);
            }
            this.t = new Thread(this.tStart);
            this.t.Start();
        }
        Thread t;
        void tStart()
        {
            this.tStopping = false;
            c = new WebClient();
            c.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
            c.Encoding = Encoding.UTF8;
            this.Event += new EventHandler(PicUploader_Event);
            var str = "";
            do
            {
                foreach(var p in this.providers){
                    var ep=p.Value+"?hash="+GCServiceBase.Validator.GetHash();
                    try
                    {
                        str = c.DownloadString(ep);
                        if (str != "")
                        {
                            var arr=str.Split('|');
                            var name = arr[0];
                            var ctype=arr[1];
                            var url = arr[2];
                            try
                            {
                                c.DownloadFile(url, this.tempPath);
                            }
                            catch(Exception ex)
                            {
                                this.Event(p.Key, "下载错误("+url+"):" + ex.Message);
                                continue;
                            }
                            var aid = "2007655-3874562";
                            try
                            {
                                YPhoto u = new YPhoto("de89c7232cad62b6", "4f34a3335937ad3cc7714ac5603166b1", "http://v.yupoo.com/");
                                var newPhoto = u.UploadPhoto("c9fc5cde774deef50596e11a6b0a344c", this.tempPath, true, name, ctype, aid, "UploadedByMFLBroker", "haha", p.Key);
                                str = c.DownloadString(ep
                                    + "&name=" + HttpUtility.UrlEncode(name)
                                    + "&id=" + HttpUtility.UrlEncode(newPhoto.Id)
                                    + "&url=" + HttpUtility.UrlEncode(newPhoto.AbsoluteUrl)
                                    );
                                this.Event(p.Key, "上传成功:" + name);
                            }
                            catch (Exception ex)
                            {
                                str = c.DownloadString(ep
                                    + "&name=" + HttpUtility.UrlEncode(name)
                                    + "&error=" + HttpUtility.UrlEncode(ex.Message)
                                    );
                                this.Event(p.Key, "上传失败:" + ex.Message);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        this.Event(p.Key, "Provider错误("+ex.Message+"):" + str);
                    }
                }
                Thread.Sleep(5000);
            }
            while (!this.tStopping);
            this.c.Dispose();
            this.tStopping = false;
        }

        void PicUploader_Event(string providerName, string imgFilename)
        {
        }
        bool tStopping;
        WebClient c;
        private string tempPath;
        public void Dispose()
        {
            this.tStopping = true;
            do
            {
                Thread.Sleep(100);
            } while (this.tStopping);
        }
        public event EventHandler Event;
        public delegate void EventHandler(string providerName, string imgFilename);
    }

}
