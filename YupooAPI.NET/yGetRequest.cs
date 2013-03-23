using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace YupooAPI.NET
{
    class yGetRequest:Abstract.YRequest
    {
        public yGetRequest()
        {
        }
        public string Method
        {
            set
            {
                this.Params["method"] = value;
            }
        }
        public override void MakeResponse(Abstract.YResponse response)
        {
            var url = this.urlBase + "api/rest/";
            try
            {
                var req = HttpWebRequest.Create(url) as HttpWebRequest;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(this.makeQuery());
                req.ContentLength = bytes.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close(); 

                var res = req.GetResponse() as HttpWebResponse;
                if (res.ContentLength == 0)
                {
                    return;
                }
                response.ReadREST(res.GetResponseStream());
                try
                {
                    res.Close();
                }
                catch { }
            }
            catch(Exception ex)
            {
                throw (new Exception(string.Format("URL:{0} EX:{1}", url, ex.Message)));
            }
        }
    }
}
