using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Drawing;
using System.Threading;
namespace MasonryGallery
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Global.LogPath = Server.MapPath(ServerRoot + "Log.txt");
            Global.logLocker = new object();
            Global.RegisterLock = new object();
        }

        void Mirror_OnError(string obj)
        {
            Log(obj);
        }
        public static void Log(string msg)
        {
            try
            {
                lock (Global.logLocker)
                {
                    File.AppendAllText(Global.LogPath, msg + "\r\n");
                }
            }
            catch { }

        }
        static object logLocker;
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        public static string ServerRoot = System.Configuration.ConfigurationManager.AppSettings["ServerRoot"];
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var url = Request.Path.Substring(ServerRoot.Length).TrimEnd('/');
            if (url.ToLower() == "callback.php")
            {
                File.WriteAllText(Server.MapPath(ServerRoot + "Frob.txt"), Request.QueryString["frob"]);
                Response.StatusCode = 200;
                Response.End();
                return;
            }
        }
        public static string YSharedSecret
        {
            get{
                return "de89c7232cad62b6";
            }
        }
        public static string YAPIKey
        {
            get
            {
                return "4f34a3335937ad3cc7714ac5603166b1";
            }
        }
        public static string YAuthToken
        {
            get
            {
                return "c9fc5cde774deef50596e11a6b0a344c";
            }
        }
        public static string YEndPointUrlBase
        {
            get
            {
                return "http://v.yupoo.com/";
            }
        }
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
        static string[] staticMimeTypes = new[] { 
            ".css",
            ".png",
            ".js",
            ".html",
            ".gif",
            ".jpg",
            ".mp3",
            ".mp4",
            ".swf"
        };
        private static string LogPath;
        public static object RegisterLock;
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["StaticBoost"])&&Response.StatusCode == 200 && staticMimeTypes.Any(ts => this.Request.Url.AbsolutePath.ToLower().EndsWith(ts)))
            {
                if (Request.Headers["If-Modified-Since"] != null && (new FileInfo(Server.MapPath(Request.Url.AbsolutePath))).Exists)
                {
                    if (DateTime.Parse(Request.Headers["If-Modified-Since"]) == (new FileInfo(Server.MapPath(Request.Url.AbsolutePath))).LastWriteTime)
                    {
                        Response.StatusCode = 304;
                        Response.StatusDescription = "Not Modified";
                        Response.End();
                        return;
                    }
                }
            }

            if (Response.StatusCode == 404||Response.StatusCode==403)
            {
                if (Request.Path.Length < ServerRoot.Length)
                {
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]);
                    return;
                }
                var url = Request.Path.Substring(ServerRoot.Length);

                if (url.ToLower().StartsWith("colorstyles/"))
                {
                    if (url.Length == ("colorstyles/".Length + 22 + 4) && url.ToLower().EndsWith(".css"))
                    {
                        var colorstr = url.Substring("colorstyles/".Length, 22);
                        if (colorstr.All(tc => char.IsLetterOrDigit(tc)))
                        {
                            var filename = Server.MapPath(ServerRoot + url);
                            var m = MFLJson.JsonMachine.JsonContext.Initial("ColorStyles", "MG", (str) => System.IO.File.ReadAllText(Server.MapPath(Global.ServerRoot) + "App_Data\\ColorStyles.css"));
                            m["BaseUrl"] = MFLJson.Json.String(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]);
                            if (!File.Exists(filename))
                            {
                                var s = File.Create(filename);
                                m["BorderWidth"] = MFLJson.Json.String(colorstr.Substring(20, 2));
                                m["Margin"] = MFLJson.Json.String(colorstr.Substring(18, 2));
                                m["Color1"] = MFLJson.Json.String(colorstr.Substring(0, 6));
                                m["Color2"] = MFLJson.Json.String(colorstr.Substring(6, 6));
                                m["Color3"] = MFLJson.Json.String(colorstr.Substring(12, 6));
                                m.RenderToStream(s);
                                s.Flush();
                                s.Close();
                            }
                            Response.ContentType = "text/css";
                            Response.TransmitFile(filename);
                            Response.StatusCode = 200;
                            Response.End();
                            return;
                        }
                    }
                    if (url.Length == ("colorstyles/icon_sprite_".Length + 18 + 4) && url.ToLower().EndsWith(".png"))
                    {
                        var colorstr = url.Substring("colorstyles/icon_sprite_".Length, 18).ToUpper();
                        var r1 = Convert.ToInt32(colorstr.Substring(0, 2), 16);
                        var g1 = Convert.ToInt32(colorstr.Substring(2, 2), 16);
                        var b1 = Convert.ToInt32(colorstr.Substring(4, 2), 16);
                        var r2 = Convert.ToInt32(colorstr.Substring(6, 2), 16);
                        var g2 = Convert.ToInt32(colorstr.Substring(8, 2), 16);
                        var b2 = Convert.ToInt32(colorstr.Substring(10, 2), 16);
                        var r3 = Convert.ToInt32(colorstr.Substring(12, 2), 16);
                        var g3 = Convert.ToInt32(colorstr.Substring(14, 2), 16);
                        var b3 = Convert.ToInt32(colorstr.Substring(16, 2), 16);
                        var imgIconBlack = Image.FromFile(Server.MapPath(ServerRoot + "Styles/icon_sprite_icon_black.png")) as Bitmap;
                        var imgIconColor = Image.FromFile(Server.MapPath(ServerRoot + "Styles/icon_sprite_icon_color.png")) as Bitmap;
                        var imgBg = Image.FromFile(Server.MapPath(ServerRoot + "Styles/icon_sprite_bg.png")) as Bitmap;
                        var imgBorder = Image.FromFile(Server.MapPath(ServerRoot + "Styles/icon_sprite_border.png")) as Bitmap;
                        var img = new Bitmap(imgBg.Width, imgBg.Height);
                        for (int x = 0; x < imgBg.Width; x++)
                        {
                            for (int y = 0; y < imgBg.Height; y++)
                            {
                                int nr, ng, nb;
                                Color gc;
                                gc = imgBg.GetPixel(x, y);
                                nr = (int)gc.R  * r2 / 0xff;
                                ng = (int)gc.G * g2 / 0xff ;
                                nb = (int)gc.B  * b2 / 0xff ;
                                img.SetPixel(x, y, Color.FromArgb(
                                    gc.A == 255 ? 255 : 0,
                                    nr > 255 ? 255 : nr,
                                    ng > 255 ? 255 : ng,
                                    nb > 255 ? 255 : nb
                                    ));
                                var ba = gc.A;
                                gc = imgBorder.GetPixel(x, y);
                                if (gc.A != 0)
                                {
                                    var oldc = img.GetPixel(x, y);
                                    nr = (int)gc.R * gc.A * r1 / 0x77 + (int)oldc.R * (255 - gc.A);
                                    ng = (int)gc.G * gc.A * g1 / 0x77 + (int)oldc.G * (255 - gc.A);
                                    nb = (int)gc.B * gc.A * b1 / 0x77 + (int)oldc.B * (255 - gc.A);
                                    nr /= 255;
                                    ng /= 255;
                                    nb /= 255;
                                    img.SetPixel(x, y, Color.FromArgb(
                                        ba,
                                        nr > 255 ? 255 : nr,
                                        ng > 255 ? 255 : ng,
                                        nb > 255 ? 255 : nb
                                        ));
                                }
                                gc = imgIconBlack.GetPixel(x, y);
                                {
                                    if (gc.A != 0)
                                    {
                                        nr = (int)gc.R * r1 / 0x55;
                                        ng = (int)gc.G * g1 / 0x55;
                                        nb = (int)gc.B * b1 / 0x55;
                                        nr = nr > 255 ? 255 : nr;
                                        ng = ng > 255 ? 255 : ng;
                                        nb = nb > 255 ? 255 : nb;
                                        var oldc = img.GetPixel(x, y);
                                        var a = (oldc.A + gc.A);
                                        img.SetPixel(x, y, Color.FromArgb(
                                            a > 255 ? 255 : a,
                                            (oldc.R * (255 - gc.A) + nr * gc.A) / 255,
                                            (oldc.G * (255 - gc.A) + ng * gc.A) / 255,
                                            (oldc.B * (255 - gc.A) + nb * gc.A) / 255
                                            ));
                                    }
                                }
                                gc = imgIconColor.GetPixel(x, y);
                                {
                                    if (gc.A != 0)
                                    {
                                        nr = (int)gc.R * r3 / 0x55;
                                        ng = (int)gc.G * g3 / 0x55;
                                        nb = (int)gc.B * b3 / 0x55;
                                        nr = nr > 255 ? 255 : nr;
                                        ng = ng > 255 ? 255 : ng;
                                        nb = nb > 255 ? 255 : nb;
                                        var oldc = img.GetPixel(x, y);
                                        var a=(oldc.A + gc.A);
                                        img.SetPixel(x, y, Color.FromArgb(
                                            a > 255 ? 255 : a,
                                            (oldc.R * (255 - gc.A) + nr * gc.A) / 255,
                                            (oldc.G * (255 - gc.A) + ng * gc.A) / 255,
                                            (oldc.B * (255 - gc.A) + nb * gc.A) / 255
                                            ));
                                    }
                                }
                            }
                        }
                        var filename = Server.MapPath(ServerRoot + "ColorStyles/icon_sprite_" + colorstr + ".png");
                        img.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                        img.Dispose();
                        imgIconBlack.Dispose();
                        imgIconColor.Dispose();
                        imgBg.Dispose();
                        imgBorder.Dispose();
                        Response.ContentType = "image/png";
                        Response.TransmitFile(filename);
                        Response.StatusCode = 200;
                        Response.End();
                        return;
                    }
                }
                d = new MGEntities();
                var galleryname = "";
                var langcode = "";
                {
                    //parse
                    var i = url.IndexOf('/');
                    if (i == -1)
                    {
                        galleryname = url.ToLower();
                    }
                    else
                    {
                        galleryname = url.Remove(i).ToLower();
                        langcode = url.Substring(i + 1).ToLower();

                    }
                }
                {
                    if (d.MG_lang.Any(tl => tl.code == galleryname) && langcode == "")
                    {
                        langcode = galleryname;
                        galleryname = "";
                    }
                    if (galleryname == "")
                    {
                        galleryname = "masonrygallery";
                    }
                    if (!d.MG_gallery.Any(tg => tg.username == galleryname))
                    {
                        d.Dispose();
                        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]);
                        return;

                    }
                    if (url.ToLower() == galleryname)
                    {
                        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"] + galleryname + "/");
                        return;
                    }
                    var gallery = d.MG_gallery.First(tg => tg.username == galleryname);
                    
                    if (!gallery.MG_langusing.Any(tl => tl.lang_code == langcode))
                    {
                        langcode = "";
                        foreach (var l in Request.UserLanguages)
                        {
                            if (gallery.MG_langusing.Any(tl => tl.lang_code == l))
                            {
                                langcode = l;
                            }
                        }
                        if (langcode == "")
                        {
                            langcode = gallery.default_lang_code;
                        }
                    }
                }
                {
                    //Response.Write(string.Format("G:{0}<br/>", gallery));
                    //Response.Write(string.Format("L:{0}<br/>", langcode));
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.StatusCode = 200;
                    try
                    {
                        Server.Transfer(ServerRoot + "Gallery.aspx?gallery=" + Server.UrlEncode(galleryname) + "&langcode=" + Server.UrlEncode(langcode));
                    }
                    catch { }
                    return;

                }
            }
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}