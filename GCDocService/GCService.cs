using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Configuration;
namespace GCDocService
{
    public partial class GCService : ServiceBase
    {
        public static string UploadFileEx(
            string uploadfile, 
            string url,  
            string fileFormName, 
            string contenttype,
            CookieContainer cookies)
        {
            if ((fileFormName == null) ||
             (fileFormName.Length == 0))
            {
                fileFormName = "file";
            }

            if ((contenttype == null) ||
             (contenttype.Length == 0))
            {
                contenttype = "application/octet-stream";
            }


            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.CookieContainer = cookies;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";


            // Build up the post message header
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileFormName);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(uploadfile));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(contenttype);
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            FileStream fileStream = new FileStream(uploadfile, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // Write out our post header
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // Write out the file contents
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);
            fileStream.Close();

            // Write out the trailing boundary
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            var str=sr.ReadToEnd();
            try{
                sr.Close();
            }catch{}
            return str;
        }
        public GCService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            this.mainThread = new Thread(this.mainLoop);
            this.mainThread.Start();
        }
        Thread mainThread;
        void mainLoop()
        {
            var services = new ServiceReference1.WebServicesSoapClient();
            var wc = new WebClient();
            string[] covertProces = ConfigurationManager.AppSettings["ConvertProcessNames"].Split(',');
            int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
            while (true)
            {
                try
                {
                    var repUrl = ConfigurationManager.AppSettings["RepositoryUrl"];
                    var fn = services.GetDoc();
                    if (fn == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    var path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + '\\' + fn;
                    wc.DownloadFile(repUrl + "Docs/" + fn, path);
                    Process.Start(ConfigurationManager.AppSettings["FlashPaperDir"] + "FlashPrinter.exe", " \"" + path + "\" -o \"" + path + ".flashpaper.swf\"").WaitForExit();
                    DateTime start = DateTime.Now;
                    bool hasTimeout = false;
                    while (true)
                    {
                        bool any = false;
                        if ((DateTime.Now - start).TotalSeconds > timeout)
                        {
                            hasTimeout = true;
                        }
                        foreach (var str in covertProces)
                        {
                            Process[] proces;
                            if ((proces = Process.GetProcessesByName(str)).Any())
                            {
                                if (!hasTimeout)
                                {
                                    Thread.Sleep(500);
                                    any = true;
                                    break;
                                }
                                proces.ToList().ForEach(tp =>
                                {
                                    try
                                    {
                                        tp.Kill();
                                        Thread.Sleep(500);
                                    }
                                    catch
                                    {
                                    }

                                });

                            }
                        }
                        if (any)
                        {
                            continue;
                        }
                        break;
                    }
                    Thread.Sleep(2000);
                    var fi = new FileInfo(path + ".flashpaper.swf");
                    if (fi.Exists)
                    {

                        try
                        {
                            UploadFileEx(path + ".flashpaper.swf",
                            ConfigurationManager.AppSettings["UploadApUrl"] + "?TicketId=" + fn.Remove(fn.LastIndexOf('.'))+"&FileSize="+fi.Length,
                            "Filedata",
                            "",
                            new CookieContainer());

                            fi.Delete();
                        }
                        catch { }
                    }
                    File.Delete(path);
                    services.DelDoc(fn);
                }
                catch {
                    Thread.Sleep(2000);
                }
            }
        }
        protected override void OnStop()
        {
            this.mainThread.Abort();
        }
    }
}
