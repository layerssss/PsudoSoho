using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ispJs;
namespace GebimaiService
{
    public class Error
    {
        /// <summary>
        /// 用来处理服务器错误的一个方法，仅仅把错误消息存到Cookie里然后跳转。
        /// </summary>
        /// <param name="message">message，出错信息传递过来的一个参数</param>
        /// <param name="from">from，出错信息传递过来的一个参数</param>
        [ispJs.Action]
        public void _(string message, string from)
        {
            WebApplication.Response.Cookies.Add(new HttpCookie("errormsg", message) { Path="/"});
            var refer = WebApplication.Request.UrlReferrer;
            WebApplication.Response.Redirect(from ?? (refer != null ? refer.AbsolutePath : "/"),true);
        }


    }
}