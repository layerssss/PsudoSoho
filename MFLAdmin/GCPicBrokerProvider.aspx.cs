using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace MFLAdmin
{
    public partial class GCPicBrokerProvider : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var p = new myProvider();
            p.HandleHttpContext(this.Context);
        }
        class myProvider : GCPicBroker.PicProvider
        {
            protected override void gettingImgUrl(out string imgUrl, out string imgFilename, out string contentType)
            {
                var files = Directory.GetFiles(Server.MapPath("~/static"), "*.png");
                if (files.Length != 0)
                {
                    imgFilename = files[0].Substring(files[0].LastIndexOf('\\') + 1);
                    imgUrl = MFL.SharedConfig.AdminBaseUrl + "static/" + imgFilename;
                    contentType = "image/png";
                    return;
                }
                imgUrl = null;
                imgFilename = null;
                contentType = null;
            }

            protected override void uploadComplete(string imgFilename, string imgYupooId, string imgUrl)
            {
                var prefix = imgFilename.Remove(imgFilename.Length - 4);
                File.WriteAllText(Server.MapPath("~/static/" + prefix + ".yupooid"), imgYupooId);
                File.WriteAllText(Server.MapPath("~/static/" + prefix + ".imgurl"), imgUrl);
                imgUrl = imgUrl.Remove(imgUrl.Length - 1);
                imgUrl = imgUrl.Remove(imgUrl.LastIndexOf('/') + 1);
                File.WriteAllText(Server.MapPath("~/static/" + prefix + ".imgurlbase"), imgUrl);
                File.Delete(Server.MapPath("~/static/" + imgFilename));
            }

            protected override void uploadError(string imgFilename, string errorMessage)
            {
                var prefix = imgFilename.Remove(imgFilename.Length - 4);
                File.Delete(Server.MapPath("~/static/" + imgFilename));
                File.Delete(Server.MapPath("~/static/" + prefix + ".length"));
                File.Delete(Server.MapPath("~/static/" + prefix + ".title"));
                File.Delete(Server.MapPath("~/static/" + prefix + ".content"));
                File.Delete(Server.MapPath("~/static/" + prefix + ".type"));
            }
        }
    }
}