using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace GCPicBroker
{
    public abstract class PicProvider
    {
        protected abstract void gettingImgUrl(out string imgUrl, out string imgFilename, out string contentType);
        protected abstract void uploadComplete(string imgFilename, string imgYupooId, string imgUrl);
        protected abstract void uploadError(string imgFilename, string errorMessage);
        protected HttpRequest Request;
        protected HttpResponse Response;
        protected HttpServerUtility Server;
        public void HandleHttpContext(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentEncoding = Encoding.UTF8;
            this.Request = context.Request;
            this.Response = context.Response;
            this.Server = context.Server;
            try
            {
                Response.StatusCode = 200;
                GCServiceBase.Validator.Validate(Request["hash"]);
                if (Request["name"] != null)
                {
                    if (Request["id"] != null)
                    {
                        uploadComplete(Request["name"], Request["id"], Request["url"]);
                    }
                    else
                    {
                        uploadError(Request["name"], Request["error"]);
                    }
                }
                else
                {
                    string imgUrl, imgFilename, contentType;
                    this.gettingImgUrl(out imgUrl, out imgFilename, out contentType);
                    if (imgUrl != null)
                    {
                        Response.Write(imgFilename + '|' + contentType + '|' + imgUrl);
                    }
                }
            }
            catch(Exception ex)
            {
                Response.Clear();
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
            Response.Flush();
            Response.End();
        }
    }
}
