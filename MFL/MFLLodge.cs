using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MFLJson;
using System.Web;
using System.IO;
using System.Drawing;
using YupooAPI.NET;
namespace MFL
{
    public class MFLLodge
    {
        public MFLEntities D;
        public MFL_lodge MFL_lodge;
        public string Root;
        public string LodgeName
        {
            get
            {
                return this.GetProperty("lodgeName", "无名旅馆");
            }
        }
        public MFLLodge(string root,string lodgeName)
        {
            this.D = new MFLEntities();
            this.Root = root;
            this.MFL_lodge = D.MFL_lodge.SingleOrDefault(tl => tl.ident == lodgeName);
            if (this.MFL_lodge == null)
            {
                throw (new Exception("找不到该旅社:"+lodgeName));
            }
            if (!this.MFL_lodge.enabled)
            {
                throw (new Exception("该旅社暂时不可用，详情请查询您的账户通知。"));
            }
        }
        public virtual JsonArray GetAttributes()
        {
            return Json.Array(this.MFL_lodge.MFL_attribute.Select(ta =>
            {
                var obj = Json.Object(
                 "attributeName", Json.String(ta.name),
                 "attributeId", Json.Number(ta.id),
                 "attributeIcon", Json.String(ta.icon));
                obj["attributeOptionNameTags"] = Json.String(this.GetOptions(ta.id).Select(tj => "<span class=\"mfl-roomAttributeOptionName optionIcon" 
                    + ((tj as JsonObject)["optionIcon"] as JsonString).Text 
                    + "\">" + ((tj as JsonObject)["optionName"] as JsonString).Text 
                    + "</span>").Aggregate((s1, s2) => s1 + s2));
                //obj["attributeOptions"] = this.GetOptions(ta.id);
                return obj;
            }));
        }
        public virtual int NewAttribute(string name, string icon)
        {
            var a = new MFL_attribute()
            {
                name = name,
                icon = icon,
                MFL_lodge = this.MFL_lodge
            };
            this.D.MFL_attribute.AddObject(a);
            this.D.SaveChanges();
            this.LodgeChanged = true;
            return a.id;
        }
        public virtual int NewRoom(string name, string icon, string prize)
        {
            this.LodgeChanged = true;
            var a = new MFL_room()
            {
                name = name,
                icon = icon,
                prize = prize,
                MFL_lodge = this.MFL_lodge
            };
            this.D.MFL_room.AddObject(a);
            this.D.SaveChanges();
            return a.id;
        }
        public virtual int NewOption(int attributeId, string name, string icon)
        {
            this.LodgeChanged = true;
            var a = new MFL_attributeOption()
            {
                name = name,
                icon = icon,
                MFL_attribute = this.MFL_lodge.MFL_attribute.First(ta => ta.id == attributeId)
            };
            this.D.MFL_attributeOption.AddObject(a);
            this.D.SaveChanges();
            for (var i = 0; i < a.MFL_attribute.MFL_attributeOption.Count; i++)
            {
                a.MFL_attribute.MFL_attributeOption.ElementAt(i).icon = (i - 4) * 32 + "px";
            }
            this.D.SaveChanges();
            return a.id;
        }
        public virtual void DelAttribute(int id)
        {
            this.LodgeChanged = true;
            this.D.MFL_attribute.DeleteObject(this.MFL_lodge.MFL_attribute.First(ta => ta.id == id));
            this.D.SaveChanges();
        }
        public virtual void DelRoom(int id)
        {
            this.LodgeChanged = true;
            var photos = this.RoomGetPhotos(id);
            foreach (JsonObject photo in photos)
            {
                this.RoomDelPhoto(id, (int)(photo["roomPhotoId"] as JsonNumber).Value);
            }
            this.D.MFL_room.DeleteObject(this.MFL_lodge.MFL_room.First(ta => ta.id == id));
            this.D.SaveChanges();
        }
        public virtual void DelOption(int id)
        {
            this.LodgeChanged = true;
            if (id == 0)
            {
                throw (new Exception("该选项为默认选项，不可以删除"));
            }
            
            this.D.MFL_attributeOption.DeleteObject(
            this.MFL_lodge.MFL_attribute.SelectMany(ta => ta.MFL_attributeOption.Where(to => to.id == id)).First());
            this.D.SaveChanges();
        }
        public virtual void UpdateAttribute(int id, string name, string icon)
        {
            this.LodgeChanged = true;
            var a = this.MFL_lodge.MFL_attribute.First(ta => ta.id == id);
            a.name = name;
            a.icon = icon;
            D.SaveChanges();
        }
        public virtual void UpdateRoom(int id, string name, string icon, string prize)
        {
            this.LodgeChanged = true;
            var a = this.MFL_lodge.MFL_room.First(ta => ta.id == id);
            a.name = name;
            a.icon = icon;
            a.prize = prize;
            D.SaveChanges();
        }
        public virtual void UpdateOption(int id, string name, string icon)
        {
            if (id == 0)
            {
                throw (new Exception("该选项为默认选项，不可以修改"));
            }
            var a = this.MFL_lodge.MFL_attribute.SelectMany(ta => ta.MFL_attributeOption.Where(to => to.id == id)).First();
            a.name = name;
            a.icon = icon;
            D.SaveChanges();
            this.LodgeChanged = true;
        }
        public virtual void UpdateRoomAttribute(int roomId, int optionId, int attributeId)
        {
            var room = this.MFL_lodge.MFL_room.First(tr => tr.id == roomId);
            var attr = this.MFL_lodge.MFL_attribute.First(ta => ta.id == attributeId);
            var ras = room.MFL_roomAttribute.Where(tra => tra.MFL_attributeOption.MFL_attribute==attr).ToList();
            foreach (var ra in ras)
            {
                this.D.MFL_roomAttribute.DeleteObject(ra);
            }
            if (optionId != 0)
            {
                this.D.MFL_roomAttribute.AddObject(new MFL_roomAttribute()
                {
                    MFL_attributeOption=attr.MFL_attributeOption.First(tao=>tao.id==optionId),
                    MFL_room=room
                });
            }
            this.D.SaveChanges();
            this.LodgeChanged = true;
        }
        public virtual JsonArray GetOptions(int id)
        {
            var arr = Json.Array(
                this.MFL_lodge.MFL_attribute.First(ta => ta.id == id).MFL_attributeOption.Select(
                (to) => Json.Object(
                    "optionId", Json.Number(to.id),
                    "optionName", Json.String(to.name),
                    "optionIcon", Json.String(to.icon))).ToArray());
            arr.Add(Json.Object(
                "optionId", Json.Number(0),
                "optionName", Json.String("无"),
                "optionIcon", Json.String("-160px")));
            return arr;
        }
        public virtual JsonArray GetRooms()
        {
            return Json.Array(this.MFL_lodge.MFL_room.Select(troom =>
            {
                var obj = Json.Object(
                    "roomName", Json.String(troom.name),
                    "roomPrize", Json.String(troom.prize),
                    "roomId", Json.Number(troom.id),
                    "roomIcon", Json.String(troom.icon));
                obj["roomAttributes"] = Json.Object("listItems", this.GetRoomAttributes(troom.id,true));
                return obj;
            }).ToArray());
        }
        public virtual JsonArray GetRoomAttributes(int id,bool showEnabled)
        {
            var arr = Json.Array();
            var room = this.MFL_lodge.MFL_room.First(tr => tr.id == id);

            foreach (var attr in this.MFL_lodge.MFL_attribute)
            {
                var opti = room.MFL_roomAttribute.FirstOrDefault(tra => tra.MFL_attributeOption.MFL_attribute == attr);
                var attrIcon = attr.icon;
                if (showEnabled)
                {
                    attrIcon = attrIcon.Remove(attrIcon.Length - 7);
                    attrIcon = opti == null ? attrIcon + " -160px;" : attrIcon + ' ' + opti.MFL_attributeOption.icon;
                }
                var obj = Json.Object(
                    "attributeName", Json.String(attr.name),
                    "attributeIcon", Json.String(attrIcon),
                    "attributeId", Json.Number(attr.id)
                    );
                
                if (opti == null)
                {
                    obj.Add("optionName", Json.String("无"));
                    obj.Add("optionIcon", Json.String("-160px"));
                    obj.Add("optionId", Json.Number(0));
                    obj.Add("attributeEnabled", Json.False());
                }
                else
                {
                    obj.Add("optionName", Json.String(opti.MFL_attributeOption.name));
                    obj.Add("optionIcon", Json.String(opti.MFL_attributeOption.icon));
                    obj.Add("optionId", Json.Number(opti.MFL_attributeOption.id));
                    obj.Add("attributeEnabled", Json.True());
                }
                arr.Add(obj);
            }
            return arr;
        }
        public virtual string GetProperty(string key, string defaultValue)
        {
            var p = this.MFL_lodge.MFL_lodgeProperty.FirstOrDefault(tp => tp.name == key);
            if (p == null)
            {
                return defaultValue;
            }
            return p.value;
        }
        public virtual JsonObject GetAllProperties()
        {
            return Json.Object(
                            "lodgeName", Json.String(GetProperty("lodgeName", "无名旅馆")),
                            "lodgeLongtitude", Json.Number(GetProperty("lodgeLongtitude", 0)),
                            "lodgeLatitude", Json.Number(GetProperty("lodgeLatitude", 0)),
                            "lodgeTraffic", Json.String(GetProperty("lodgeTraffic", "远方而来的客人怎样到达我的旅馆呢？在这里描述一下吧。")),
                            "lodgeContact", Json.String(GetProperty("lodgeContact", "XX省XX市XX区XX路X号\r\n电话:0XXX-XXXXXXX & 13XX XXXX XXXX")),
                            "lodgeDescription", Json.String(GetProperty("lodgeDescription", "用简短的几段话向访客介绍一下我的旅馆吧。\r\n这段话会放在首页。")),
                            "lodgeOrder", Json.String(GetProperty("lodgeOrder", "客人应该怎样订房呢？在这里说明一下吧。")),
                            "lodgeTemplateName", Json.String(GetTemplateName())
                            );
        }
        public virtual double GetProperty(string key, double defaultValue)
        {
            return Convert.ToDouble(this.GetProperty(key, defaultValue.ToString()));
        }
        public virtual void UpdateProperty(string key, string value)
        {
            if (value == null)
            {
                return;
            }
            var p = this.MFL_lodge.MFL_lodgeProperty.FirstOrDefault(tp => tp.name == key);
            if (p == null)
            {
                p = new MFL_lodgeProperty()
                {
                    MFL_lodge=this.MFL_lodge,
                    name=key
                };
                this.D.MFL_lodgeProperty.AddObject(p);
            }
            p.value = value;
            this.D.SaveChanges();
            this.LodgeChanged = true;
        }
        public virtual void UpdatePropertyNumber(string key, string value)
        {
            if (value == null)
            {
                return;
            }
            double v;
            if (!double.TryParse(value, out v))
            {
                throw (new Exception("属性“"+key+"”必须是一个数字。"));
            }
            this.UpdateProperty(key, value);
            this.LodgeChanged = true;
        }
        public virtual int LodgeNewPhoto(string title, string content, string type,HttpPostedFile file)
        {
            int i = 0;
            var str = "";
            do
            {
                i++;
                str = string.Format("static\\{0}-album-{1:D4}.length", this.MFL_lodge.ident, i);
                if (i == 1000)
                {
                    throw (new Exception("您上传的照片数已经达到系统限制数目。"));
                }
            } while (File.Exists(Root + str));
            var img=Image.FromStream(file.InputStream);
            var w=img.Width;
            var h=img.Height;
            if (w > 1024)
            {
                h *= 1024;
                h /= w;
                w = 1024;
            }
            if (h > 1024)
            {
                w *= 1024;
                w /= h;
                h = 1024;
            }
            var aaa = new Bitmap(w,h);
            var g = Graphics.FromImage(aaa);
            g.DrawImage(img, 0, 0, w, h);
            g.Dispose();
            aaa.Save(Root + string.Format("static\\{0}-album-{1:D4}.png", this.MFL_lodge.ident, i), System.Drawing.Imaging.ImageFormat.Png);
            aaa.Dispose();
            img.Dispose();
            File.WriteAllText(Root + string.Format("static\\{0}-album-{1:D4}.title", this.MFL_lodge.ident, i), title);
            File.WriteAllText(Root + string.Format("static\\{0}-album-{1:D4}.content", this.MFL_lodge.ident, i), content);

            try
            {
                type = type.Remove(type.IndexOf('('));
            }
            catch { }
            File.WriteAllText(Root + string.Format("static\\{0}-album-{1:D4}.type", this.MFL_lodge.ident, i), type);
            this.MFL_lodge.avaPhoto -= new FileInfo(Root + string.Format("static\\{0}-album-{1:D4}.png", this.MFL_lodge.ident, i)).Length;
            File.WriteAllText(Root + string.Format("static\\{0}-album-{1:D4}.length", this.MFL_lodge.ident, i), (new FileInfo(Root + string.Format("static\\{0}-album-{1:D4}.png", this.MFL_lodge.ident, i)).Length).ToString());
            this.D.SaveChanges();

            return i;
        }
        public virtual JsonArray LodgeGetPhotos()
        {
            return Json.Array(Directory.GetFiles(Root + "static\\", this.MFL_lodge.ident + "-album-*.length").Select(ts =>
            {
                var prefix = ts.Remove(ts.LastIndexOf('.'));
                var id = prefix.Substring(prefix.LastIndexOf('-') + 1);
                var title = File.ReadAllText(prefix + ".title");
                var content = File.ReadAllText(prefix + ".content");
                var type = File.ReadAllText(prefix + ".type");
                var size = Convert.ToInt32(File.ReadAllText(prefix + ".length"));
                var sizeS="";
                if (size / 1024 != 0)
                {
                    if (size / (1024 * 1024) != 0)
                    {
                        sizeS += size / (1024 * 1024) + "MB";
                    }
                    else
                    {
                        sizeS +=size / 1024+ "KB";
                    }
                }
                else
                {
                    sizeS += size+ "B";
                }
                string url, medium, large, small,thumb;
                if (File.Exists(Root + "/static/" + this.MFL_lodge.ident + "-album-" + id + ".png"))
                {
                    thumb = medium = large = small = url = SharedConfig.AdminBaseUrl + "Style/noPreview.png";
                }
                else
                {
                    url = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurl");
                    medium = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "medium/";
                    large = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "large/";
                    small = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "small/";
                    thumb = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "thumb/";
                }
                return Json.Object(
                    "lodgePhotoId", Json.Number(Convert.ToInt32(id)),
                    "lodgePhotoUrl", Json.String(url),
                    "lodgePhotoUrlSmall", Json.String(small),
                    "lodgePhotoUrlMedium", Json.String(medium),
                    "lodgePhotoUrlThumb", Json.String(thumb),
                    "lodgePhotoUrlLarge", Json.String(large ),
                    "lodgePhotoTitle", Json.String(title),
                    "lodgePhotoContent", Json.String(content),
                    "lodgePhotoType", Json.String(type),
                    "lodgePhotoSize",Json.String(sizeS)
                    );
            }).ToArray());
        }
        public virtual JsonArray LodgeGetPhotos(string type)
        {
            return Json.Array(this.LodgeGetPhotos().Where(tjson => ((tjson as JsonObject)["lodgePhotoType"] as JsonString).Text == type));
        }
        public virtual void LodgeDelPhoto(int id)
        {
            var prefix = Root+string.Format("static\\{0}-album-{1:D4}", this.MFL_lodge.ident, id);
            if (!File.Exists(prefix + ".length"))
            {
                throw (new Exception("找不到该照片"));
            }

            this.MFL_lodge.avaPhoto += Convert.ToInt64(File.ReadAllText(prefix + ".length"));
            File.Delete(prefix + ".png");
            File.Delete(prefix + ".title");
            File.Delete(prefix + ".content");
            File.Delete(prefix + ".imgurl");
            File.Delete(prefix + ".type");
            File.Delete(prefix + ".error");
            File.Delete(prefix + ".length");
            File.Delete(prefix + ".imgurlbase");
            if (File.Exists(prefix + ".yupooid"))
            {
                var yid = File.ReadAllText(prefix + ".yupooid");
                YPhoto u = new YPhoto("de89c7232cad62b6", "4f34a3335937ad3cc7714ac5603166b1", "http://v.yupoo.com/");
                try
                {
                    u.Delete("c9fc5cde774deef50596e11a6b0a344c", yid);
                }
                catch { }
                File.Delete(prefix + ".yupooid");
            }
            this.D.SaveChanges();
            this.LodgeChanged = true;
        }
        public virtual void LodgeUpdatePhoto(int id, string title, string content,string type)
        {
            var prefix = Root + string.Format("static\\{0}-album-{1:D4}", this.MFL_lodge.ident, id);
            if (!File.Exists(prefix + ".length"))
            {
                throw (new Exception("找不到该照片"));
            }
            File.WriteAllText(prefix + ".title", title);
            File.WriteAllText(prefix + ".content", content);
            try
            {
                type = type.Remove(type.IndexOf('('));
            }
            catch { }
            File.WriteAllText(prefix + ".type", type);
            this.LodgeChanged = true;

        }
        public virtual int RoomNewPhoto(int roomId, string title, string content, HttpPostedFile file)
        {
            if (!this.MFL_lodge.MFL_room.Any(tr => tr.id == roomId))
            {
                throw (new Exception("找不到该房间"));
            }
            int i = 0;
            var prefix = "";
            do
            {
                i++;
                prefix = string.Format("static\\{0}-room{2:D4}-album-{1:D4}", this.MFL_lodge.ident, i, roomId);
                if (i == 1000)
                {
                    throw (new Exception("您上传的照片数已经达到系统限制数目。"));
                }
            } while (File.Exists(Root + prefix + ".length"));
            var img = Image.FromStream(file.InputStream);
            var w = img.Width;
            var h = img.Height;
            if (w > 1024)
            {
                h *= 1024;
                h /= w;
                w = 1024;
            }
            if (h > 1024)
            {
                w *= 1024;
                w /= h;
                h = 1024;
            }
            var aaa = new Bitmap(w, h);
            var g = Graphics.FromImage(aaa);
            g.DrawImage(img, 0, 0, w, h);
            g.Dispose();
            aaa.Save(Root + prefix + ".png", System.Drawing.Imaging.ImageFormat.Png);
            aaa.Dispose();
            img.Dispose();
            File.WriteAllText(Root + prefix + ".title", title);
            File.WriteAllText(Root + prefix + ".content", content);
            this.MFL_lodge.avaPhoto -= new FileInfo(Root + prefix + ".png").Length;
            File.WriteAllText(Root + prefix + ".length", (new FileInfo(Root + prefix + ".png").Length).ToString());
            this.D.SaveChanges();
            this.LodgeChanged = true;
            return i;
        }
        public virtual JsonArray RoomGetPhotos(int roomId)
        {
            if (!this.MFL_lodge.MFL_room.Any(tr => tr.id == roomId))
            {
                throw (new Exception("找不到该房间"));
            }
            return Json.Array(Directory.GetFiles(Root + "static\\", string.Format("{0}-room{1:D4}-album-*.length", this.MFL_lodge.ident, roomId)).Select(ts =>
            {
                var prefix = ts.Remove(ts.LastIndexOf('.'));
                var id = prefix.Substring(prefix.LastIndexOf('-') + 1);
                var title = File.ReadAllText(prefix + ".title");
                var content = File.ReadAllText(prefix + ".content");
                var size = Convert.ToInt32(File.ReadAllText(prefix + ".length"));
                var sizeS = "";
                if (size / 1024 != 0)
                {
                    if (size / (1024 * 1024) != 0)
                    {
                        sizeS += size / (1024 * 1024) + "MB";
                    }
                    else
                    {
                        sizeS += size / 1024 + "KB";
                    }
                }
                else
                {
                    sizeS += size + "B";
                }
                string url, medium, large, small,thumb;
                if (File.Exists(Root + string.Format("/static/{0}-room{1:D4}-album-{2}.png", this.MFL_lodge.ident, roomId, id)))
                {
                    thumb = medium = large = small = url = SharedConfig.AdminBaseUrl + "Style/noPreview.png";
                }
                else
                {
                    url = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurl");
                    medium = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "medium/";
                    large = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "large/";
                    small = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "small/";
                    thumb = "http://pic.yupoo.com" + File.ReadAllText(prefix + ".imgurlbase") + "thumb/";
                }
                return Json.Object(
                    "roomPhotoId", Json.Number(Convert.ToInt32(id)),
                    "roomPhotoUrl", Json.String(url),
                    "roomPhotoUrlSmall", Json.String(small),
                    "roomPhotoUrlMedium", Json.String(medium),
                    "roomPhotoUrlThumb", Json.String(thumb),
                    "roomPhotoUrlLarge", Json.String(large),
                    "roomPhotoTitle", Json.String(title),
                    "roomPhotoContent", Json.String(content),
                    "roomPhotoSize", Json.String(sizeS)
                    );
            }).ToArray());
        }
        public virtual void RoomDelPhoto(int roomId, int id)
        {
            if (!this.MFL_lodge.MFL_room.Any(tr => tr.id == roomId))
            {
                throw (new Exception("找不到该房间"));
            }
            var prefix = Root + string.Format("static\\{0}-room{1:D4}-album-{2:D4}", this.MFL_lodge.ident, roomId, id);
            if (!File.Exists(prefix + ".length"))
            {
                throw (new Exception("找不到该照片"));
            }

            this.MFL_lodge.avaPhoto += Convert.ToInt64(File.ReadAllText(prefix + ".length"));
            File.Delete(prefix + ".png");
            File.Delete(prefix + ".title");
            File.Delete(prefix + ".content");
            File.Delete(prefix + ".type");
            File.Delete(prefix + ".error");
            File.Delete(prefix + ".imgurl");
            File.Delete(prefix + ".length");
            File.Delete(prefix + ".imgurlbase");
            if (File.Exists(prefix + ".yupooid"))
            {
                var yid = File.ReadAllText(prefix + ".yupooid");
                YPhoto u = new YPhoto("de89c7232cad62b6", "4f34a3335937ad3cc7714ac5603166b1", "http://v.yupoo.com/");
                try
                {
                    u.Delete("c9fc5cde774deef50596e11a6b0a344c", yid);
                }
                catch { }
                File.Delete(prefix + ".yupooid");
            }
            this.LodgeChanged = true;
            this.D.SaveChanges();
        }
        public virtual void RoomUpdatePhoto(int roomId, int id, string title, string content)
        {
            if (!this.MFL_lodge.MFL_room.Any(tr => tr.id == roomId))
            {
                throw (new Exception("找不到该房间"));
            }
            var prefix = Root + string.Format("static\\{0}-room{1:D4}-album-{2:D4}", this.MFL_lodge.ident, roomId, id);
            if (!File.Exists(prefix + ".length"))
            {
                throw (new Exception("找不到该照片"));
            }
            File.WriteAllText(prefix + ".title", title);
            File.WriteAllText(prefix + ".content", content);
            this.LodgeChanged = true;

        }
        public virtual JsonObject GetAlbumStatus()
        {
            return Json.Object(
                "avaPhoto", Json.String((this.MFL_lodge.capPhoto-this.MFL_lodge.avaPhoto) / (1024 * 1024) + "MB"),
                "capPhoto", Json.String(this.MFL_lodge.capPhoto / (1024 * 1024) + "MB"),
                "percentage", Json.Number((this.MFL_lodge.capPhoto - this.MFL_lodge.avaPhoto) * 100 / this.MFL_lodge.capPhoto)
                );
        }
        public virtual string GetTemplateName()
        {
            var f=this.Root + "static\\" + this.MFL_lodge.ident + ".templateName";
            if (!File.Exists(f))
            {
                return "NotSelected";
            }
            return File.ReadAllText(f);
        }
        public virtual void UpdateTemplateName(string templateName)
        {
            if (templateName == null)
            {
                return;
            }
            if (!Directory.Exists(this.Root + "templates\\" + templateName))
            {
                throw (new Exception(string.Format("模版“{0}”不存在或者已近被删除。", templateName)));
            }
            File.WriteAllText(this.Root + "static\\" + this.MFL_lodge.ident + ".templateName", templateName);
            this.LodgeChanged = true;
        }
        public virtual JsonObject GetLodgeStatus()
        {
            return Json.Object("lodgeRoomsNum", Json.Number(this.MFL_lodge.MFL_room.Count),
                "lodgeVisitted", Json.Number(1024),
                "lodgeVisittedToday", Json.Number(1024),
                "lodgeVisittedThisMonth", Json.Number(1024),
                "lodgeVisittedThisWeek", Json.Number(1024),
                "lodgeRegisterdTime", Json.String(DateTime.Now.ToShortDateString()));
        }
        public virtual JsonArray LodgeGetPhotoTypes()
        {
            var list=this.LodgeGetPhotos().Select(tjson => ((tjson as JsonObject)["lodgePhotoType"] as JsonString).Text).ToList();
            
            foreach (var line in File.ReadAllLines(this.Root+"templates\\"+this.GetTemplateName()+"\\specialPhotoTypes.txt"))
            {
                var ar = line.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (ar.Length == 3)
                {
                    var i = Convert.ToInt32(ar[2]);
                    var type = ar[1];
                    try
                    {
                        type = type.Remove(type.IndexOf('('));
                    }
                    catch { }
                    list.Add(ar[1] + "(" + (i == 0 ? "无限制" : (i < 0 ? "至少" + (-i) : i.ToString())) + "张，现在已有" + this.LodgeGetPhotos(type).Count + "张)");
                    list.RemoveAll(ts => ts == type);
                }
            }
            list.RemoveAll(ts => ts == "");
            list = list.OrderByDescending(ts => ts).ToList();
            if (list.Contains("照片展示"))
            {
                list.RemoveAll(ts => ts == "照片展示");
            }
            list.Add("照片展示");
            return Json.Array(list.Distinct().Reverse().Select(str => Json.String(str)));
        }
        public virtual JsonArray GetTemplateProperties(string tname)
        {
            var lines = File.ReadAllLines(this.Root + "templates\\" + tname + "\\properties.txt");
            var arr = Json.Array();
            for (var i = 0; i < lines.Length; i += 3)
            {
                var key = lines[i];
                var name = lines[i + 1];
                var def = lines[i + 2];
                arr.Add(Json.Object(
                    "templatePropertyName", Json.String(name),
                    "templatePropertyKey", Json.String("TemplateProperty" + key),
                    "templatePropertyValue", Json.String(this.GetProperty("TemplateProperty" + key, def))));

            }
            return arr;
        }
        public virtual void UpdateTemplateProperties(HttpContext c)
        {
            var tname = this.GetTemplateName();
            var lines = File.ReadAllLines(this.Root + "templates\\" + tname + "\\properties.txt");
            for (var i = 0; i < lines.Length; i += 3)
            {
                var key = lines[i];
                var name = lines[i + 1];
                var def = lines[i + 2];
                var newv=c.Request["TemplateProperty" + key];
                if (newv == "")
                {
                    newv = def;
                }
                this.UpdateProperty("TemplateProperty" + key,newv );
            }
            this.LodgeChanged = true;
        }
        public virtual JsonArray SearchAttributeNames()
        {
            var all=this.D.MFL_attribute.Select(ta => ta.name).Distinct();
            return Json.Array(all.AsEnumerable().Select(ts=>Json.String(ts)));

        }
        public virtual JsonArray SearchOptionNames(string attributeNane, string term)
        {
            var all=this.D.MFL_attributeOption.Where(to=>to.MFL_attribute.name==attributeNane).Select(to=>to.name).Where(ts=>ts.Contains(term)).Distinct();
            return Json.Array(all.AsEnumerable().Select(ts => Json.String(ts)));
        }
        public virtual void ChangeAdminPwd(string pwd)
        {

            this.MFL_lodge.adminPwd = pwd;
            this.D.SaveChanges();
        }
        public bool LodgeChanged
        {
            set
            {
                var fi = new FileInfo(this.Root + "static\\" + this.MFL_lodge.ident + ".changed");
                if (value && !fi.Exists)
                {
                    fi.CreateText().Close();
                }
                if (!value && fi.Exists)
                {
                    fi.Delete();
                }
            }
            get
            {
                var fi = new FileInfo(this.Root + "static\\" + this.MFL_lodge.ident + ".changed");
                return fi.Exists;
            }
        }
        public virtual JsonArray LodgeGetDetailPhotoTypes()
        {
            return Json.Array(this.LodgeGetPhotoTypes().Select(tj =>
            {
                var type = (tj as JsonString).Text;
                var i = type.LastIndexOf("(");
                var percentage = -1;
                if (i != -1)
                {
                    type = type.Remove(i);
                    var lines = File.ReadAllLines(Root + "templates\\" + this.GetTemplateName() + "\\specialPhotoTypes.txt").Select(ts => ts.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries)).Where(tsr => tsr.Length == 3);
                    var first = lines.FirstOrDefault(tline => tline[1] == type);
                    if (first != null)
                    {
                        var count = Math.Abs(Convert.ToInt32(first[2]));
                        if (count != 0)
                        {
                            try
                            {
                                type = type.Remove(type.LastIndexOf("("));
                            }
                            catch { }
                            percentage = this.LodgeGetPhotos(type).Count * 100 / count;
                            if (percentage > 100)
                            {
                                percentage = 100;
                            }
                            i = Convert.ToInt32(first[2]);
                            type = type + "(<span class=\"ui-widget-content ui-corner-all need\">" + (i < 0 ? "至少" + (-i) : i.ToString()) + "张</span>，现在<span class=\"ui-widget-header ui-corner-all already\">已有" + this.LodgeGetPhotos(type).Count + "张</span>)";
                        }
                    }
                }
                if (!type.EndsWith("</span>)"))
                {
                    type = (tj as JsonString).Text;
                    type += "(现在<span class=\"ui-widget-header ui-corner-all already\">已有" + this.LodgeGetPhotos(type).Count + "张</span>)";
                }
                return Json.Object("type", Json.String(type), "percentage", Json.Number(percentage));
            }));
        }

        public virtual int StaGetHistory()
        {
            this.staCheckToday();
            return this.MFL_lodge.staHistory;
        }
        public virtual int StaGetToday()
        {
            this.staCheckToday();
            return this.MFL_lodge.staToday;
        }
        protected virtual void staCheckToday()
        {
            if (this.MFL_lodge.staToday != 0)
            {
                if (this.MFL_lodge.staTodayDate != DateTime.Today)
                {
                    this.MFL_lodge.staHistory += this.MFL_lodge.staToday;
                    this.MFL_lodge.staToday = 0;
                    this.MFL_lodge.staTodayDate = DateTime.Today;
                    this.D.SaveChanges();
                }
            }
        }
        public virtual void StaVisit()
        {
            this.staCheckToday();
            this.MFL_lodge.staToday++;
            this.MFL_lodge.staTodayDate = DateTime.Today;
            this.D.SaveChanges();
        }
    }
}
