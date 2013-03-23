using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YupooAPI.NET
{
    public class YAuth:Abstract.YAPI
    {

        public YCheckTokenResponse CheckToken(string token)
        {
            var req = new yGetRequest();
            this.MakeRequest(req);
            req.Method = "yupoo.auth.checkToken";
            req.Params["auth_token"] = token;
            var res = new YCheckTokenResponse();
            req.MakeResponse(res);
            return res;
        }
        public YGetTokenResponse GetToken(string frob)
        {
            var req = new yGetRequest();
            this.MakeRequest(req);
            req.Method = "yupoo.auth.getToken";
            req.Params["frob"] = frob;
            var res = new YGetTokenResponse();
            req.MakeResponse(res);
            return res;
        }
        public string GetLoginUrl(string perms)
        {
            var req = new yRedirectRequest();
            this.MakeRequest(req);
            req.Params["perms"] = perms;
            return req.GetRedirectUrl("services/Auth/");
        }
        public YAuth(string sharedKey, string apiKey, string endPointRootUrl)
            : base( sharedKey,  apiKey,  endPointRootUrl)
        {
        }
        public class YCheckTokenResponse : Abstract.YResponse
        {
            public string Perms
            {
                get
                {
                    return base.Values["perms"];
                }
            }
            public YUser User
            {
                get
                {
                    return new YUser(){
                        Id=Convert.ToInt32(base.Dics["user"]["id"]),
                        Username = base.Dics["user"]["username"],
                        Nickname = base.Dics["user"]["nickname"]
                    };
                }
            }
        }
        public class YGetTokenResponse : YCheckTokenResponse
        {
            public string Token
            {
                get
                {
                    return base.Values["token"];
                }
            }
        }
        public struct YUser
        {
            public int Id;
            public string Username;
            public string Nickname;
        }
    }
}
