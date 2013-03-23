using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
namespace MasonryGallery
{
    public partial class Data : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObject obj = Json.Object();
            var action = Request["action"];
            var argsInt = new List<string>();
            var argsString = new List<string>();
            var actions = new List<string>();
            bool acted = false;
            Action<string, Action> regAction = (string actName, Action act) =>
            {
                if (Request.QueryString["action"] == actName)
                {
                    act();
                    acted = true;
                }
                actions.Add(actName);
            };
            var argError = "";
            Func<string, int> pi = (p) =>
            {
                if (Request.QueryString[p] == null||Request.Form[p]==null)
                {
                    argError += "缺少整数参数“" + p + "”；";
                }
                argsInt.Add(p);
                return (int)(Convert.ToDouble(Request.QueryString[p] ?? Request.Form[p])+0.5);
            };
            Func<string, string> ps = (p) =>
            {
                if (Request.QueryString[p] == null || Request.Form[p] == null)
                {
                    argError += "缺少字符串参数“" + p + "”；";
                }
                argsString.Add(p);
                return Request.QueryString[p] ?? Request.Form[p];
            };
            try
            {
                System.Threading.Thread.Sleep(700);
                var d = new MGEntities();
                regAction("Login", () =>
                {
                    var u = ps("username");
                    var pX = md5(ps("password") + System.Configuration.ConfigurationManager.AppSettings["PasswordSalt"]);
                    if (GetGallery(d, u, pX) != null)
                    {
                        Response.SetCookie(new HttpCookie("MGAUTHUSERNAME")
                        {
                            Value = u
                        });
                        Response.SetCookie(new HttpCookie("MGAUTHPASSWORDX")
                        {
                            Value = pX
                        });
                    }
                    else
                    {
                        throw (new Exception("Authentication failed!"));
                    }
                });
                regAction("Logout", () =>
                {
                    Response.SetCookie(new HttpCookie("MGAUTHUSERNAME")
                    {
                        Expires = DateTime.Now.AddDays(-10)
                    });
                    Response.SetCookie(new HttpCookie("MGAUTHPASSWORDX")
                    {
                        Expires = DateTime.Now.AddDays(-10)
                    });
                });
                regAction("LeaveComment", () =>
                {
                    var avatar = md5(ps("email").Trim()).ToLower();
                    var name = ps("name");
                    var content = ps("content");
                    var url = ps("url");
                    var afile = Server.MapPath(Global.ServerRoot + "avatars/" + avatar + ".jpg");
                    if (!System.IO.File.Exists(afile))
                    {
                        var c = new System.Net.WebClient();
                        try
                        {
                            c.DownloadFile("http://www.gravatar.com/avatar/" + avatar + ".jpg?s=50", afile);
                        }
                        catch
                        {
                            c.Dispose();
                            throw (new Exception());
                        }
                        c.Dispose();
                    }
                    var cc = new MG_comment()
                    {
                        album_id = d.MG_album.First(ta => ta.mainpicurl_origin == url).id,
                        avatar = avatar,
                        content = content,
                        name = name,
                        time = DateTime.Now
                    };
                    d.MG_comment.AddObject(cc);
                    obj["Html"] = Json.String(Gallery.GetCommentHtml(cc));
                });
                MG_gallery g = null;
                if (Request.Cookies["MGAUTHUSERNAME"] != null)
                {
                    g = GetGallery(d, Request.Cookies["MGAUTHUSERNAME"].Value, Request.Cookies["MGAUTHPASSWORDX"].Value);
                    if (g == null)
                    {
                        obj["messageType"] = Json.Number(6);
                        throw (new Exception("|location.reload()"));
                    }
                }
                Action checkRoot = () =>
                {
                    if (g.username == "masonrygallery"&&bool.Parse(System.Configuration.ConfigurationManager.AppSettings["LockRoot"]))
                    {
                        throw (new Exception("You cannot modify this gallery. :(  Press the register button to get your own one :)"));
                    }
                };
                regAction("DelAlbum", () =>
                {
                    checkRoot();
                    GoClassing.Internal.GCException.GCConfirm("Are you sure to delete this album?");
                    var yPhoto = new YupooAPI.NET.YPhoto(Global.YSharedSecret, Global.YAPIKey, Global.YEndPointUrlBase);
                    var url = Request["url"];
                    var a = g.MG_album.First(ta => ta.mainpicurl_origin == url);
                    foreach (var s in a.MG_subpic)
                    {
                        try
                        {
                            yPhoto.Delete(Global.YAuthToken, s.YUPOO_photoId);
                        }
                        catch { }
                    }
                        try
                        {
                            yPhoto.Delete(Global.YAuthToken, a.YUPOO_photoId);
                        }
                        catch { }
                    d.MG_album.DeleteObject(a);
                });
                regAction("DelComment", () =>
                {
                    checkRoot();
                    var id = pi("id");
                    var a = d.MG_comment.First(tc => tc.id == id && tc.MG_album.gallery_id == g.id);
                    d.MG_comment.DeleteObject(a);
                });
                regAction("DelSubpic", () =>
                {
                    checkRoot();
                    var yPhoto = new YupooAPI.NET.YPhoto(Global.YSharedSecret, Global.YAPIKey, Global.YEndPointUrlBase);
                    var url = ps("url");
                    var a = d.MG_subpic.First(tc => tc.url_origin == url && tc.MG_album.gallery_id == g.id);
                    yPhoto.Delete(Global.YAuthToken, a.YUPOO_photoId);
                    d.MG_subpic.DeleteObject(a);
                });

                regAction("SetTitle", () =>
                {
                    checkRoot();
                    var url = ps("url");
                    var langcode = ps("langCode");
                    var a = d.MG_album.First(tc => tc.mainpicurl_origin == url && tc.gallery_id == g.id);
                    var des = a.MG_description.FirstOrDefault(td => td.lang_code == langcode);
                    if (des == null)
                    {
                        des = new MG_description()
                        {
                            album_id = a.id,
                            lang_code = langcode,
                            content = ""
                        };
                        d.MG_description.AddObject(des);
                    }
                    des.title = ps("title");
                });
                regAction("SetDescription", () =>
                {
                    checkRoot();
                    var url = ps("url");
                    var langcode = ps("langCode");
                    var a = d.MG_album.First(tc => tc.mainpicurl_origin == url && tc.gallery_id == g.id);
                    var des = a.MG_description.FirstOrDefault(td => td.lang_code == langcode);
                    if (des == null)
                    {
                        des = new MG_description()
                        {
                            album_id = a.id,
                            lang_code = langcode,
                            title = ""
                        };
                        d.MG_description.AddObject(des);
                    }
                    des.content = ps("description");
                });

                regAction("SetThumbWidth", () =>
                {
                    checkRoot();
                    var url = ps("url");
                    var a = d.MG_album.First(tc => tc.mainpicurl_origin == url && tc.gallery_id == g.id);
                    a.thumbWidth = pi("width");
                    Gallery.GenetateThumbSize(a);

                });
                regAction("SetShowWidth", () =>
                {
                    checkRoot();
                    var url = ps("url");
                    var a = d.MG_album.First(tc => tc.mainpicurl_origin == url && tc.gallery_id == g.id);
                    a.showWidth = pi("width");
                });
                regAction("OrderSubpics", () =>
                {
                    checkRoot();
                    var url = ps("url");
                    var str = ps("str").Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var a = d.MG_album.First(tc => tc.mainpicurl_origin == url && tc.gallery_id == g.id);
                    foreach (var subpic in a.MG_subpic)
                    {
                        var i = str.IndexOf(subpic.url_origin);
                        subpic.sort = i == -1 ? 9999 : i;
                    }
                });
                regAction("OrderAlbums", () =>
                {
                    checkRoot();
                    var str = ps("str").Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var album in g.MG_album)
                    {
                        var i = str.IndexOf(album.mainpicurl_origin);
                        album.sort = i == -1 ? 9999 : i;
                    }
                });
                regAction("SetColors", () =>
                {
                    checkRoot();
                    g.color1 = ps("color1");
                    g.color2 = ps("color2");
                    g.color3 = ps("color3");
                });
                regAction("SetSize", () =>
                {
                    checkRoot();
                    g.margin = pi("margin");
                    g.border = pi("borderWidth");
                    g.width = pi("width");
                    if (bool.Parse(ps("fullscreen")))
                    {
                        g.bodyWidth = null;
                    }
                    else
                    {
                        g.bodyWidth = pi("bodyWidth");
                    }
                    foreach (var a in g.MG_album)
                    {
                        Gallery.GenerateSize(a);
                    }

                });
                regAction("SetOptions", () =>
                {
                    checkRoot();
                    var langCode = ps("langCode");
                    var l = g.MG_langusing.First(tl => tl.lang_code == langCode);
                    l.titile = ps("title");
                });
                regAction("SetPassword", () =>
                {
                    checkRoot();
                    if (ps("newPassword") != ps("newPassword2"))
                    {
                        GoClassing.Internal.GCException.GCStopMessage("Two passwords do not match! ");
                    }
                    g.password = ps("newPassword");
                });
                regAction("Register", () =>
                {
                    if (ps("password") != ps("password2"))
                    {
                        GoClassing.Internal.GCException.GCStopMessage("Two passwords do not match! ");
                    }
                    var username = ps("username").ToLower();
                    {
                        var ar = new[] { '.', '_', '-' };
                        if (!username.All(tc => char.IsLetterOrDigit(tc) || ar.Contains(tc)))
                        {
                            GoClassing.Internal.GCException.GCStopMessage("Username must contain only alphabets, numbers, ',', '-', '_'! ");
                        }
                        if (username.Length ==0)
                        {
                            GoClassing.Internal.GCException.GCStopMessage("Username must contain only alphabets, numbers, ',', '-', '_'! ");
                        }
                        if (ps("password").Length < 6)
                        {
                            GoClassing.Internal.GCException.GCStopMessage("Password must be at least 6 chars. ");
                        }
                    }
                    lock (Global.RegisterLock)
                    {
                        {
                            if (d.MG_gallery.Any(tg => tg.username == username))
                            {
                                GoClassing.Internal.GCException.GCStopMessage("This username has already been taken. Try another one. ");
                            }
                        }
                        var defTheme = d.MG_theme.OrderBy(tt => tt.sort).First();

                        var yAlbum = new YupooAPI.NET.YAlbum(Global.YSharedSecret, Global.YAPIKey, Global.YEndPointUrlBase);
                        var res = yAlbum.Create(Global.YAuthToken, username, Request.ServerVariables["REMOTE_ADDR"]);
                        var newG = new MG_gallery()
                        {
                            bodyWidth = 900,
                            border = 1,
                            color1 = defTheme.color1,
                            color2 = defTheme.color2,
                            color3 = defTheme.color3,
                            default_lang_code = "en-us",
                            domainname = "masonrygallery.com",
                            margin = 9,
                            opacity = 0.75,
                            username = username,
                            password = ps("password"),
                            type = "LocalGallery",
                            width = 160,
                            YUPOO_albumId = res.Id
                        };
                        d.MG_gallery.AddObject(newG);
                        d.SaveChanges();
                        var newL = new MG_langusing()
                        {
                            gallery_id = newG.id,
                            lang_code = "en-us",
                            titile = "A Gallery Without Its Title! lol"
                        };
                        d.MG_langusing.AddObject(newL);
                        d.SaveChanges();
                    }
                    obj["newUrl"] = Json.String(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"] + username);
                });
                regAction("ActiveLang", () =>
                {
                    checkRoot();
                    var langCode = ps("code");
                    lock (Global.RegisterLock)
                    {
                        if (g.MG_langusing.Any(tl => tl.lang_code == langCode))
                        {
                            GoClassing.Internal.GCException.GCStopMessage("Language already in use! ");
                        }
                        if (!d.MG_lang.Any(tl => tl.code == langCode))
                        {
                            GoClassing.Internal.GCException.GCStopMessage("Language does not exist! ");
                        }
                        d.MG_langusing.AddObject(new MG_langusing()
                        {
                            gallery_id = g.id,
                            lang_code = langCode,
                            titile = "A Gallery Without Its Title! lol"
                        });
                        d.SaveChanges();
                        obj["newUrl"] = Json.String(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"] + g.username + "/" + langCode);
                    }
                });
                regAction("DeactiveLang", () =>
                {
                    checkRoot();
                    var langCode = ps("code");
                    GoClassing.Internal.GCException.GCConfirm("All data asociated with this language will be deleted! Are you sure? ");
                    lock (Global.RegisterLock)
                    {
                        var l = g.MG_langusing.First(tl => tl.lang_code == langCode);
                        d.MG_langusing.DeleteObject(l);
                        MG_description dd = null;
                        d.SaveChanges();
                        while ((dd = d.MG_description.FirstOrDefault(td => td.MG_album.gallery_id == g.id && td.lang_code == langCode)) != null)
                        {
                            d.MG_description.DeleteObject(dd);
                            d.SaveChanges();
                        }
                        if (g.default_lang_code == langCode)
                        {
                            g.default_lang_code = g.MG_langusing.First().lang_code;
                            d.SaveChanges();
                        }
                    }
                });
                regAction("获取操作", () =>
                {
                    obj["actions"] = Json.Array(actions.Select(ta => Json.String(ta)));
                });
                if (!acted)
                {
                    throw (new Exception(string.Format("操作“{0}”未实现。", Request.QueryString["action"])));
                }
                obj["success"] = Json.True();
                d.SaveChanges();
            }
            catch (GoClassing.Internal.GCException ex)
            {

                obj["success"] = Json.False();
                obj["messageType"] = Json.Number((int)ex.Type);
                obj["message"] = Json.String(ex.Message);
            }
            catch (Exception ex)
            {
                obj["success"] = Json.False();
                obj["message"] = Json.String(ex.Message);
            }

            if (Request.QueryString["listArgs"] == "true")
            {
                if (argError != "")
                {
                    obj["argErrorMessage"] = Json.String(argError);
                }
                obj["argInt"] = Json.Array(argsInt.Select(ta => Json.String(ta)));
                obj["argString"] = Json.Array(argsString.Select(ta => Json.String(ta)));
            }
            if (Request["formatted"] != "true")
            {
                if (Request["cType"] == "html")
                {
                    Response.Write("<html><head></head><body>");
                    Response.Write((obj as IJson).ToString());
                    Response.Write("</body></html>");
                    Response.ContentType = "text/html";
                }
                else
                {
                    Response.Write((obj as IJson).ToString());
                    Response.ContentType = "application/json";
                }
            }
            else
            {
                Response.ContentType = "text/plain";
                Response.Write((obj as IJson).ToFormattedString(""));//.Replace(" ", " ").Replace("\r\n", "<br />"));
            }

        }
        public static MG_gallery GetGallery(MGEntities d, string username, string passwordX)
        {
            var tg = d.MG_gallery.FirstOrDefault(ttg => ttg.username == username);
            if (tg == null)
            {
                return null;
            }
            if (md5(tg.password + System.Configuration.ConfigurationManager.AppSettings["PasswordSalt"]) != passwordX)
            {
                return null;
            }
            return tg;
        }
        static string md5(string str)
        {
            
            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes=get_md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            get_md5.Dispose();
            return ByteArrayToHexString(bytes, 0, 16);
            
        }
        static string ByteArrayToHexString(byte[] buf, int offset, int len)
        {
            len += offset;
            var sb = new System.Text.StringBuilder();
            for (int i = offset; i < len; i++)
            {
                sb.Append(buf[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}