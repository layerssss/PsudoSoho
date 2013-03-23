using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using MFLJson;
namespace MFL
{
    public class MFLAdmin
    {
        private HttpContext context;
        public MFLAdmin(HttpContext context, string lodgeName)
        {
            this.context = context;
            this.LodgeName = lodgeName;
            this.D = new MFLEntities();
            try
            {
                var authx = context.Request.Cookies["MFLADMINAUTHX"].Value;
                var hash = getHash(this.D.MFL_lodge.First(tl => tl.ident == lodgeName).adminPwd);
                if (authx != hash)
                {
                    throw (new Exception());
                }
            }
            catch
            {
                context.Response.SetCookie(new HttpCookie("MFLADMINAUTHX")
                {
                    Expires = DateTime.Today.AddDays(-1)
                });
                throw (new Exception("登陆超时，请重新登陆"));
            }

        }
        public RoomInfo[] GetRoomsInfo()
        {
            var lodge = new MFLLodge(context.Server.MapPath("/"), LodgeName);
            var rooms = lodge.GetRooms();
            return rooms.Select(tjarr =>
            {
                var room = (tjarr as MFLJson.JsonObject);
                return new RoomInfo()
                {
                    Id = (int)(room["roomId"] as JsonNumber).Value,
                    Name = (room["roomName"] as JsonString).Text,
                    Prize = (room["roomPrize"] as JsonString).Text,
                    Attributes = ((room["roomAttributes"] as JsonObject)["listItems"] as JsonArray).Select(ta =>
                    {
                        var a = ta as JsonObject;
                        return new Attribute()
                        {
                            Enable = (a["attributeEnabled"] as JsonBool).Value,
                            Icon = (a["attributeIcon"] as JsonString).Text,
                            Name = (a["attributeName"] as JsonString).Text,
                            OptionName = (a["optionName"] as JsonString).Text
                        };
                    }).ToArray()
                };
            }).ToArray();
        }
        public RoomState GetRoomState(DateTime day, int id)
        {
            var prefix = context.Server.MapPath(string.Format("/static/{0}-room{1:D4}-state-{2:yyyyMMdd}", LodgeName, id, day));
            if (File.Exists(prefix + ".txt"))
            {
                var lines = File.ReadAllLines(prefix + ".state");
                return new RoomState()
                {
                    Contact = lines[0],
                    Email = lines[1],
                    Full = true,
                    Memo = context.Server.UrlEncode(lines[2]),
                    People = Convert.ToInt32(lines[3]),
                    QQ = lines[4],
                    Tel = lines[5]
                };
            }
            else
            {
                return new RoomState()
                {
                    Full = false,
                    Contact = "",
                    Email = "",
                    Memo = "",
                    People = 0,
                    QQ = "",
                    Tel = ""
                };
            }
        }
        public void SetRoomState(int year, int month, int day, int id, RoomState state)
        {
            var prefix = context.Server.MapPath(string.Format("/static/{0}-room{1:D4}-state-{2:D4}{3:D2}{4:D2}", LodgeName, id, year, month, day));

            if (state.Full)
            {
                File.WriteAllLines(prefix + ".state",
                    new[]{
                        state.Contact,
                        state.Email,
                        state.Memo,
                        state.People.ToString(),
                        state.QQ,
                        state.Tel});
                File.WriteAllText(prefix + ".txt", "true");
            }
            else
            {
                File.Delete(prefix + ".txt");
                File.Delete(prefix + ".state");
            }
            var m = context.Server.MapPath(string.Format("/static/{0}-state-{1:D4}-{2:D2}.state", LodgeName, year, month));
            
            var rooms = this.GetRoomsInfo();
            var list = new List<string>();
            for (var i = 0; i < 31; i++)
            {
                var count = 0;
                foreach (var r in rooms)
                {
                    if (File.Exists(context.Server.MapPath(string.Format("/static/{0}-room{1:D4}-state-{2:D4}{3:D2}{4:D2}.txt", LodgeName, r.Id, year, month, i + 1))))
                    {
                        count++;
                    }

                }
                list.Add(count.ToString());
            }
            File.WriteAllLines(m, list.ToArray());
        }
        public static void Logout(HttpContext context)
        {
            context.Response.SetCookie(new HttpCookie("MFLADMINAUTHX")
            {
                Expires = DateTime.Today.AddDays(-1)
            });
        }
        public static void Login(HttpContext context, string pwd)
        {
            context.Response.SetCookie(new HttpCookie("MFLADMINAUTHX", getHash(pwd)));
        }
        protected static string getHash(string pwd)
        {
            ;
            var salt = SharedConfig.PasswordSalt;
            var hash = common.MD5(DateTime.Today.ToShortDateString() + salt + pwd);
            return hash;

        }
        public string LodgeName;
        private MFLEntities D;
        public class RoomState
        {
            public bool Full;
            public int People;
            public string Contact;
            public string Tel;
            public string QQ;
            public string Email;
            public string Memo;
        }
        public class RoomInfo
        {
            public string Name;
            public string Prize;
            public int Id;
            public Attribute[] Attributes;
        }
        public class Attribute
        {
            public string Name;
            public string OptionName;
            public bool Enable;
            public string Icon;
        }
    }
}
