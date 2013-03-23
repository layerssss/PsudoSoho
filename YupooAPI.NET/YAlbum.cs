using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YupooAPI.NET
{
    public class YAlbum:Abstract.YAPI
    {
        public YCreateResponse Create(string token,string title, string description)
        {
            var req = new yGetRequest();
            this.MakeRequest(req);
            req.Method = "yupoo.albums.create";
            req.Params["auth_token"] = token;
            req.Params["title"] = title;
            req.Params["description"] = description;
            var res = new YCreateResponse();
            req.MakeResponse(res);
            return res;
        }
        public class YCreateResponse : Abstract.YResponse
        {
            public string Id
            {
                get
                {
                    return base.Values["id"];
                }
            }
            public string Title
            {
                get
                {
                    return base.Values["title"];
                }
            }
            public string Description
            {
                get
                {
                    return base.Values["description"];
                }
            }

        }
        public YAlbum(string sharedKey, string apiKey, string endPointRootUrl)
            : base( sharedKey,  apiKey,  endPointRootUrl)
        {
        }
    }
}
