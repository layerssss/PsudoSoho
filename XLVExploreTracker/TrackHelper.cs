using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace XLVExploreTracker
{
    public class TrackHelper
    {
        public TrackHelper(string format, params object[] args)
        {
            var c = new WebClient();
            Buffer = "";
            url = string.Format(format, args);
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    Buffer = c.DownloadString(url).Replace("\r\n", "").Replace("\t", "");
                    break;
                }
                catch
                {
                }
            }
            if (Buffer == "")
            {
                throw (new Exception(string.Format("下载“{0}”时出错", url)));
            }
        }
        public string Buffer = "";
        public int Position = 0;
        public string FetchString(string target)
        {
            var i = Buffer.IndexOf(target, Position);
            if (i == -1)
            {
                throw (new Exception(string.Format("在“{1}”没有找到“{0}”", target, url)));
            }
            var oldpos = Position;
            Position = i + target.Length;
            return Buffer.Substring(oldpos, i - oldpos);
        }
        public string url;
        public bool BufferTailedWith(string str)
        {
            return this.Buffer.Substring(this.Position, str.Length) == str;
        }
    }
}
