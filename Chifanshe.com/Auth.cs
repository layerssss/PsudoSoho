using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chifanshe.com
{
    public class Auth
    {
        public static void ValidateAdmin()
        {
            var ck=HttpContext.Current.Request.Cookies["cfsauth"];
            try
            {
                if (ck == null)
                {
                    throw (new Exception());
                }
                GCServiceBase.Validator.Validate(ck.Value,System.IO.File.ReadAllText(
                    HttpContext.Current.Server.MapPath("/App_Data/admin_hash.txt")));
            }
            catch
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("cfsauth") { Expires = DateTime.Now.AddDays(-1) });
                throw (new ispJs.AccessDeniedException("亲不行啊亲"));
            }

        }
        [ispJs.Action]
        public void Login(string password)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("cfsauth", GCServiceBase.Validator.GetHash(
                GCServiceBase.Validator.MD5(
                password))));
        }
        [ispJs.Action]
        public void ChangePassword(string password)
        {
            ValidateAdmin();
            System.IO.File.WriteAllText(
                HttpContext.Current.Server.MapPath("/App_Data/admin_hash.txt"),
                GCServiceBase.Validator.MD5(password));
        }
    }
}