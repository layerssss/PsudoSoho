using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoClassing.Internal
{
    public class GCException:Exception
    {
        private string msg;
        public GCExceptionType Type;
        public GCException(string message, GCExceptionType type)
        {
            this.msg = message;
            this.Type = type;
        }
        public override string Message
        {
            get
            {
                return this.msg;
            }
        }
        public static void GCStopFieldError(string field, string message)
        {
            throw (new GCException(field + "|" + message, GCExceptionType.FieldError));
        }
        public static void GCStopMessage(string message)
        {
            throw (new GCException(message, GCExceptionType.Message));
        }
        public static void GCConfirm(string message)
        {
            if (HttpContext.Current.Request["gc_confirmed"] == null)
            {
                throw (new GCException(message, GCExceptionType.Confirmation));
            }
        }
        public static void GCNeedLogin()
        {
            throw (new GCException("您必须登录才可以继续进行当前的操作|openLogin", GCExceptionType.NeedLogin));
        }
        public static void GCReCaptcha()
        {
            var str = "";
            if (!Internal.GCCommon.VerifyRecaptcha(HttpContext.Current, ref str))
            {
                //throw (new GCException("", GCExceptionType.ReCaptcha));
            }
        }
    }
    public enum GCExceptionType:int
    {
        Message=0,
        ReCaptcha=1,
        Confirmation=2,
        FieldReqired=3,
        FieldInvalid=4,
        FieldError=5,
        NeedLogin=6
    }
}