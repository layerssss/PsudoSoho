using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
namespace TCPUploadHandler
{
    public class UploadServer
    {
        public Dictionary<string, string> Logs = new Dictionary<string, string>();
        public int Port=81;
        public string Ip="0.0.0.0";
        public string Host = null;
        public int Timeout = 4;
        public int BufferSize = 10000;
        public bool CompatibleMode = false;
        private byte[] acceptContent;

        public string AcceptContent
        {
            set { acceptContent = Encoding.UTF8.GetBytes(value); }
        }
        private byte[] rejectContent;

        public string RejectContent
        {
            set { rejectContent = Encoding.UTF8.GetBytes(value); }
        }
        public UploadServer()
        {

        }
        public virtual void HandlingUploadStream(string str)
        {
        }
        public virtual bool ReceivingUpload(string post,string filname)
        {
            return true;
        }
        public virtual void UploadFinish()
        {
        }
        TcpListener tcp;
        private Thread listenThread;
        private void handleTcp(object obj)
        {
            var c = obj as TcpClient;
            l("{0} connected", c.Client.RemoteEndPoint);
            c.GetStream().ReadTimeout = this.Timeout * 1000;

            var buffer = new byte[this.BufferSize];
            int len;
            {
                string str;

                string host = null;
                string get = null;
                string post = null;
                string boundary = null;
                try
                {
                    this.readLine(c.GetStream(), buffer, 0, buffer.Length, out len);
                    str = Encoding.UTF8.GetString(buffer, 0, len);
                }
                catch(Exception ex)
                {

                    this.writeReject(c, "Cancled1");
                    return;
                }
                var curSize = 0;
                while (str!="\r\n")
                {
                    l("REQ:{0}", str);
                    if(str.StartsWith("Host:")){
                            host = str.Split(':')[1].Trim();
                    }
                    if(str.StartsWith("GET ")){
                            get = str.Split(' ')[1].Trim();
                    }
                    if (str.StartsWith("POST "))
                    {
                            post = str.Split(' ')[1].Trim();
                        try{
                            var curSizeS = post.Substring(post.IndexOf("FileSize=") + "FileSize=".Length);
                            curSize = Convert.ToInt32(curSizeS.IndexOf('&') == -1 ? curSizeS : curSizeS.Remove(curSizeS.IndexOf('&')));
                            l("FileSize:{0}", curSize);
                        }
                        catch(Exception ex){
                            l("Failed to retrieve FileSize:{0}", ex.Message);
                        }
                    }
                    if (str.StartsWith("Content-Type: multipart/form-data;"))
                    {
                        boundary = str.Substring(str.IndexOf("boundary=") + 9);
                        l("B:{0}", boundary);
                    }
                    if (str.StartsWith("Content-Disposition: ")&&str.Contains("name=\"Filedata\";"))
                    {
                        var filename = str.Substring(str.IndexOf("filename=\"") + 10);
                        filename = filename.Remove(filename.IndexOf('\"'));

                        if (!this.ReceivingUpload(post, filename))
                        {
                            this.writeReject(c,"rejected by server");
                            return;
                        }
                        do
                        {
                            try
                            {
                                this.readLine(c.GetStream(), buffer, 0, buffer.Length, out len);
                                str = Encoding.UTF8.GetString(buffer, 0, len);
                            }
                            catch
                            {
                                this.writeReject(c, "header");
                                return;
                            }
                        }
                        while (str != "\r\n");//文件内容开始

                        var b = Encoding.UTF8.GetBytes("--"+boundary.TrimEnd('\r','\n'));
                        var buffer2 = new byte[b.Length - 1];
                        var crlf = new byte[] { 13, 10 };
                        bool first = true;
                        if (curSize == 0)
                        {//兼容原始协议
                            if (!CompatibleMode)
                            {
                                this.writeReject(c, "deprecated method");
                                return;
                            }
                            while (true)
                            {
                                len = 0;
                                try
                                {
                                    this.readLine(c.GetStream(), buffer, 0, buffer.Length, out len);
                                }
                                catch (Exception ex)
                                {

                                    this.writeReject(c,"unexpected end");
                                    return;
                                }
                                if (len == 0)
                                {
                                    this.writeReject(c, "0 len end");
                                    return;
                                }
                                if (compareBytes(b, buffer, 0))
                                {
                                    break;
                                }
                                try
                                {
                                    if (!first)
                                    {
                                        this.HandlingUploadStream(crlf, 0, 2);
                                    }
                                    this.HandlingUploadStream(buffer, 0, len - 2);
                                }
                                catch
                                {
                                    this.writeReject(c,"error handling stream");
                                    return;
                                }
                                first = false;

                            }
                        }
                        else
                        {
                            var pos = 0;
                            while (pos < curSize)
                            {
                                try
                                {
                                    len = c.GetStream().Read(buffer, 0, Math.Min(buffer.Length, curSize - pos));
                                }
                                catch (Exception ex)
                                {
                                    this.writeReject(c, "NEW:error reading stream (" + ex.Message + ")");
                                    return;
                                }
                                if (len == 0)
                                {
                                    this.writeReject(c, "NEW:unexpected end");
                                    return;
                                }
                                pos += len;
                                try
                                {
                                    this.HandlingUploadStream(buffer, 0, len);
                                }
                                catch(Exception ex)
                                {
                                    this.writeReject(c, "NEW:error handling stream (" + ex.Message + ")");
                                    return;
                                }
                            }
                        }
                        {
                            if (this.writeOk(c, "OK"))
                            {
                                UploadFinish();
                            }
                            return;
                        }
                    }
                    try
                    {
                        this.readLine(c.GetStream(), buffer, 0, buffer.Length, out len);
                        str = Encoding.UTF8.GetString(buffer, 0, len);
                    }
                    catch (Exception ex)
                    {
                        this.writeReject(c, "Cancled1");
                        return;
                    }
                    if (boundary != null)
                    {
                        if (str != "\r\n")
                        {
                            if (str == "--" + boundary + "--\r\n")
                            {
                                l("BENDS");
                                break;
                            }
                            if (str == "--" + boundary+"\r\n")
                            {
                                l("Bcontinue");
                            }
                        }
                        else
                        {
                            str = "[MOREFORMS]";
                        }
                    }
                }
                if (get!=null||post!=null)
                {
                    this.writeOk(c, "OK");
                }
            }
        }

        public virtual void UploadCancel()
        {
        }
        void writeReject(TcpClient c,string msg)
        {

            this.UploadCancel();
            try
            {
                var writer = new StreamWriter(c.GetStream());
                writer.WriteLine("HTTP/1.0 403");
                writer.WriteLine("Server: GCFileServer/1.0");
                writer.WriteLine("Content-Length: " + this.rejectContent.Length.ToString());
                writer.WriteLine("Content-Type: text/html");
                writer.WriteLine();
                writer.Flush();
                c.GetStream().Write(this.rejectContent, 0, this.rejectContent.Length);
                c.GetStream().Flush();
                writer.Flush();
                l("Rejected in {0}",msg);
            }
            catch
            {
            }
            try
            {
                c.Close();
            }
            catch { }
        }
        bool writeOk(TcpClient c, string msg)
        {
            try
            {

                l("StartResponse");
                var writer = new StreamWriter(c.GetStream());
                writer.WriteLine("HTTP/1.0 200 OK");
                writer.WriteLine("Server: GCFileServer/1.0");
                writer.WriteLine("Content-Length: " + this.acceptContent.Length.ToString());
                writer.WriteLine("Content-Type: text/html; charset=utf-8");
                writer.WriteLine();
                writer.Flush();
                c.GetStream().Write(this.acceptContent, 0, this.acceptContent.Length);
                c.GetStream().Flush();
                writer.Flush(); l("EndResponse");
            }
            catch
            {
            }
            try
            {
                l("ok");
                c.Close();
            }
            catch { }
            return true;
        }
        public virtual void HandlingUploadStream(byte[] buffer, int p, int len)
        {
        }
        void myswitch<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
        public bool compareBytes(byte[] b, byte[] buffer, int offset)
        {
            for (var i = 0; i < b.Length; i++)
            {
                if (b[i] != buffer[offset + i])
                {
                    return false;
                }
            }
            return true;
        }
        public bool readLine(NetworkStream stream, byte[] buffer, int offset, int count, out int len)
        {
            len = 0;
            int i,j;
            while (true)
            {
                try
                {
                    i = stream.ReadByte();
                }
                catch
                {
                    Thread.Sleep(2000);
                    try
                    {
                        i = stream.ReadByte();
                    }
                    catch
                    {
                        if (len == 0)
                        {
                            throw (new Exception());
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                if (i == -1)
                {
                    return false;
                }
                if (i == 13)
                {
                    j = stream.ReadByte();
                    if (j == 10)
                    {

                        buffer[offset + len] = (byte)i;
                        len++;
                        buffer[offset + len] = (byte)j;
                        len++;
                        return true;
                    }
                    else
                    {
                        buffer[offset + len] = (byte)i;
                        len++;
                        buffer[offset + len] = (byte)j;
                        len++;
                        continue;
                    }
                }

                buffer[offset + len] = (byte)i;
                len++;
                if (count == len)
                {
                    return false;
                }
            }
        }
        void l(string s, params object[] args)
        {
            lock (this.Logs)
            {
                if (s == null)
                {
                    s = "";
                }
                if (!this.Logs.ContainsKey(Thread.CurrentThread.Name))
                {
                    this.Logs[Thread.CurrentThread.Name] = "";
                }
                this.Logs[Thread.CurrentThread.Name] += string.Format(s, args) + "\r\n";
            }
        }
        private void listenThreadStart()
        {
            var count=0;
            while (true)
            {
                var c=this.tcp.AcceptTcpClient();
                Thread t = new Thread(this.handleTcp);
                t.Name = count.ToString("D2");
                t.Start(c);
                count++;
            }
        }
        public void Start()
        {
            this.tcp = new TcpListener(IPAddress.Parse(this.Ip),this.Port);
            this.tcp.Start(10);

            this.listenThread = new Thread(this.listenThreadStart);
            this.listenThread.Start();
        }
        public void Stop()
        {
            this.listenThread.Abort();
            this.tcp.Stop();
        }
        
    }
}
