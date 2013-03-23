using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace MasonryGallery.Uploader
{
    public partial class Default : System.Web.UI.Page
    {

        public override void Dispose()
        {
            base.Dispose();
            try
            {
                this.d.Dispose();
            }
            catch
            {
            }
        }
        MGEntities d;
        protected void Page_Load(object sender, EventArgs e)
        {
                d = new MGEntities();
                if (Request.HttpMethod == "GET")
                {
                    try
                    {
                        var g = MasonryGallery.Data.GetGallery(d, Request.Cookies["MGAUTHUSERNAME"].Value, Request.Cookies["MGAUTHPASSWORDX"].Value);
                        if (g == null)
                        {
                            throw (new Exception());
                        }
                    }
                    catch
                    {
                        Response.StatusCode = 403;
                        Response.End();
                    }
                }
                if (Request.HttpMethod == "POST")
                {
                    try
                    {
                        var g = MasonryGallery.Data.GetGallery(d, Request["MGAUTHUSERNAME"], Request["MGAUTHPASSWORDX"]);
                        if (g == null)
                        {
                            Response.StatusCode = 403;
                            Response.End();
                            return;
                        }
                        var url = Request["url"];
                        var file = Request.Files["Filedata"];
                        var filename = file.FileName;
                        if (filename.Contains('\\'))
                        {
                            filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                        }
                        filename = Server.MapPath("temp") + "\\" + g.username + "." + filename;
                        file.SaveAs(filename);
                        var img = System.Drawing.Bitmap.FromFile(filename) as System.Drawing.Bitmap;
                        var imgW = img.Width;
                        var imgH = img.Height;
                        if (imgW > 800)
                        {
                            var newImg = new System.Drawing.Bitmap(img, 800, 800 * imgH / imgW);
                            imgW = newImg.Width;
                            imgH = newImg.Height;
                            img.Dispose();
                            System.IO.File.Delete(filename);
                            img = newImg;
                            img.Save(filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            filename += ".jpg";
                        }
                        if (imgH > 800)
                        {
                            var newImg = new System.Drawing.Bitmap(img, 800 * imgW / imgH, 800);
                            imgW = newImg.Width;
                            imgH = newImg.Height;
                            img.Dispose();
                            System.IO.File.Delete(filename);
                            img = newImg;
                            img.Save(filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            filename += ".jpg";
                        }
                        img.Dispose();
                        var yPhoto = new YupooAPI.NET.YPhoto(Global.YSharedSecret, Global.YAPIKey, Global.YEndPointUrlBase);
                        var uploadRes = yPhoto.UploadPhoto(Global.YAuthToken,
                            filename,
                            file.ContentType,
                            g.YUPOO_albumId,
                            DateTime.Now.ToString(),
                            "");
                        System.IO.File.Delete(filename);


                        var album = g.MG_album.FirstOrDefault(ta => ta.mainpicurl_origin == url);
                        if (album == null)
                        {
                            album = new MG_album()
                            {
                                gallery_id = g.id,
                                mainpicHeight = imgH,
                                mainpicWidth = imgW,
                                sort = 100,
                                subpicWidth = 100,
                                mainpicurl = "http://pic.yupoo.com" + uploadRes.AbsoluteUrlSmall,
                                mainpicurl_origin = "http://pic.yupoo.com" + uploadRes.AbsoluteUrl,
                                YUPOO_photoId = uploadRes.Id
                            };
                            d.MG_album.AddObject(album);
                            Gallery.GenerateSize(album);
                        }
                        else
                        {
                            d.MG_subpic.AddObject(new MG_subpic()
                            {
                                height = imgH,
                                width = imgW,
                                album_id = album.id,
                                sort = 999,
                                url = "http://pic.yupoo.com" + uploadRes.AbsoluteUrlSmall,
                                url_origin = "http://pic.yupoo.com" + uploadRes.AbsoluteUrl,
                                YUPOO_photoId = uploadRes.Id
                            });
                        }
                        d.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText(Server.MapPath("temp/errors.log"), DateTime.Now.ToString() + "\r\n" + ex.Message);
                    }
                    Response.StatusCode = 200;
                    Response.End();
                }
        }
        
    }
}