using System;
using System.Collections.Generic;
using System.Text;

namespace YupooAPI.NET.Abstract
{
    public abstract class YAPI
    {
        protected string sharedSecret;
        protected string apiKey;
        protected string endPointRootUrl;
        YAPI()
        {
        }
        public YAPI(string sharedSecret, string apiKey, string endPointRootUrl)
        {
            this.sharedSecret = sharedSecret;
            this.apiKey = apiKey;
            this.endPointRootUrl = endPointRootUrl;
        }
        public void MakeRequest(YRequest request)
        {
            request.UrlBase = this.endPointRootUrl;
            request.SharedSecret = this.sharedSecret;
            request.Params["api_key"] = this.apiKey;
        }
        public static int Timeout = 60000;
    }
}
