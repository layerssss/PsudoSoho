using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MFLJson.JsonMachine;
using MFLJson;
namespace MasonryGallery
{
    public partial class Gallery : System.Web.UI.Page
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
            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["StaticBoost"]))
            {
                MFLJson.JsonMachine.JsonContext.TemplatesBuffer.Clear();
            }
            var m = JsonContext.Initial("Template", "MG", (str) => System.IO.File.ReadAllText(Server.MapPath(Global.ServerRoot) + "App_Data\\Template.html"));

            var galleryname = Request.QueryString["gallery"];
            var g = d.MG_gallery.First(tg => tg.username == galleryname);
            var langcode = Request.QueryString["langcode"];
            var l = g.MG_langusing.First(tl => tl.lang_code == langcode);
            m["Username"] = Json.String(galleryname);
            m["BaseUrl"] = Json.String(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]);
            m["Color1"] = Json.String(g.color1);
            m["Color2"] = Json.String(g.color2);
            m["Color3"] = Json.String(g.color3);
            m["Title"] = Json.String(l.titile == "" ? "无标题" : l.titile);
            m["Width"] = Json.Number(g.width);
            m["Opacity"] = Json.Number(g.opacity);
            m["BorderWidth"] = Json.Number(g.border);
            m["BorderWidthP"] = Json.String(g.border.ToString("D2"));
            m["BodyWidth"] = Json.String(g.bodyWidth.HasValue ? g.bodyWidth.Value.ToString() : "auto");
            m["Margin"] = Json.Number(g.margin);
            m["MarginP"] = Json.String(g.margin.ToString("D2"));
            m["LangCode"] = Json.String(langcode);
            m["LangString"] = Json.String(d.MG_lang.First(tl => tl.code == langcode).@string);
            var imageMap = Json.Array();
            
            m["ImageMap"] = imageMap;
            {
                try
                {
                    if (Request.Cookies["MGAUTHUSERNAME"].Value==g.username&&MasonryGallery.Data.GetGallery(d, Request.Cookies["MGAUTHUSERNAME"].Value, Request.Cookies["MGAUTHPASSWORDX"].Value)
                        != null)
                    {
                        m["Logon"] = Json.True();
                    }
                }
                catch { }
            }
            m["Themes"] = Json.Array(d.MG_theme.AsEnumerable().Select((tt) => Json.Object(
                "Color1", Json.String(tt.color1),
                "Color2", Json.String(tt.color2),
                "Color3", Json.String(tt.color3))));

            m["LangLinks"] = Json.Array(d.MG_lang.AsEnumerable().Select(tl =>
            {
                if (g.MG_langusing.Any(ttl => ttl.MG_lang == tl))
                {
                    return Json.Object("Html", Json.String(tl.code == langcode ?
                    "<span>" + tl.@string + "</span>" : "<a title=\"" + tl.@string + "\" href=\"http://" + g.domainname + '/' + tl.code + "\">" + tl.@string + "</a>"));
                }
                else
                {
                    return Json.Object("Html", Json.String("<a class=\"notavailable\" title=\"" + tl.@string + "\" href=\"http://" + g.domainname + '/' + tl.code + "\"><span class=\"btn btn-add\"></span>" + tl.@string + "</a>"));
                }
            }));
            m["Albums"] = Json.Array(g.MG_album.OrderBy(tp => tp.sort).Select(tp =>
            {
                var obj = Json.Object();
                obj["ThumbWidth"] = Json.Number(tp.thumbWidth);
                obj["ThumbHeight"] = Json.Number(tp.thumbWidth * tp.mainpicHeight / tp.mainpicWidth);
                obj["ShowWidth"] = Json.Number(tp.showWidth);
                obj["MainPicUrl"] = Json.String(tp.mainpicurl);
                imageMap.Add(Json.Object(
                    "Url", Json.String(tp.mainpicurl),
                    "UrlOrigin", Json.String(tp.mainpicurl_origin)));
                var de = tp.MG_description.FirstOrDefault(td => td.lang_code == langcode);
                if (de != null)
                {
                    obj["Title"] = Json.String(de.title);
                    obj["Description"] = Json.String(de.content);
                }
                else
                {
                    obj["Title"] = Json.String("");
                    obj["Description"] = Json.String("");
                }
                obj["TitleEncode"] = Json.String(Server.UrlEncode((obj["Title"] as JsonString).Text).Replace('%', '_'));
                obj["SubpicsNumber"] = Json.Number(tp.MG_subpic.Count);
                obj["Subpics"] = Json.String(tp.MG_subpic.Any() ?
                    tp.MG_subpic.OrderBy(ts => ts.sort).Select(ts =>
                    {

                        imageMap.Add(Json.Object(
                            "Url", Json.String(ts.url),
                            "UrlOrigin", Json.String(ts.url_origin)));
                        return string.Format(
                        "{0}\" style=\"width:{1}px;height:{2}px",
                        ts.url,
                        tp.subpicWidth,
                        tp.subpicWidth * ts.height / ts.width
                        );
                    }
                    ).Aggregate((s1, s2) => s1 + ',' + s2) : "");
                {
                    var str = "";
                    foreach (var c in tp.MG_comment)
                    {
                        str += GetCommentHtml(c);
                    }
                    obj["CommentsHtml"] = Json.String(str);
                }
                return obj;
            }));
            var data = m.Clone();
            data.Remove("Albums");
            data.Remove("LangLinks");
            m["data"] = Json.String((data as IJson).ToFormattedString(""));
            //System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(Response.OutputStream, System.IO.Compression.CompressionMode.Compress);
            //Response.AddHeader("Content-Encoding", "gzip");
            var gzip = Response.OutputStream;
            m.RenderToStream(gzip);
            gzip.Flush();
            //gzip.Close();
            //IGallery g = new LocalGallery();
            //Response.Write(string.Format("G:{0}<br/>",galleryname));
            //Response.Write(string.Format("L:{0}<br/>", langcode));
        }
        public static string GetCommentHtml(MG_comment c)
        {
            return string.Format("<div class=\"comment\"><span class=\"data-commentid notavailable\">{3}</span><img src=\""+System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]
                +"/avatars/{0}.jpg\" title=\"{1}\" alt=\"{1}\" /><p>{1}({4})</p><p>{2}</p></div>",
                            c.avatar,
                            c.name,
                            c.content,
                            c.id,
                            GoClassing.Internal.GCCommon.GetTime(c.time));
        }
        public static void GenerateSize(MG_album a)
        {
            var g = a.MG_gallery;
            var sW = a.mainpicWidth + (g.margin+g.border) * 2;
            if (sW > g.width + (g.margin + g.border) * 2)
            {
                sW -= sW % (g.width + (g.margin + g.border) * 2);
            }
            else
            {
                sW = g.width + (g.margin + g.border) * 2;
            }
            a.showWidth = sW - (g.margin + g.border) * 2;
            a.thumbWidth = g.width;
            GenetateThumbSize(a);
        }
        public static void GenetateThumbSize(MG_album a)
        {
            var l = Math.Min(a.mainpicWidth, a.thumbWidth);
            var newstr = "/thumb/";
            if (l > 100)
            {
                newstr = "/small/";
                if (l > 240)
                {
                    newstr = "/medium/";
                    if (l > 500)
                    {
                        newstr = "/big/";
                    }
                }
            }
            a.mainpicurl = a.mainpicurl
                .Replace("/thumb/", newstr)
                .Replace("/small/", newstr)
                .Replace("/medium/", newstr)
                .Replace("/big/", newstr);
        }
    }
}