using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MFLJson;
using System.Drawing;
namespace GoClassing.Internal
{
    public class GCUser
    {
        public gc_localtestEntities D;
        public bool IsSelf;
        public bool IsFriend;
        public bool IsSalutting;
        public gc_user U;
        public bool IsSalutted;
        public GCUser(gc_localtestEntities d, string username)
        {
            this.D = d;
            this.U = this.D.gc_user.First(tu => tu.username == username);
        }
        public static JsonObject GetJson(gc_user u)
        {
            return Json.Object(
                "id",Json.Number(u.id),
                "truename", Json.String(u.truename??"[匿名用户]"),
                "sex",Json.String(u.sex.HasValue?(u.sex.Value?"他":"她"):"他/她"),
                "sexprefix", Json.String(u.sex.HasValue ? (u.sex.Value ? "m" : "f") : ""),
                "sexchar", Json.String(u.sex.HasValue ? (u.sex.Value ? "男" : "女") : "保密"),
                "username",Json.String(u.originalusername),
                "uprovince",Json.String(u.uprovince),
                "ucity",Json.String(u.ucity),
                "coop",Json.String(u.coop??""),
                "createdCourses", Json.Number(u.gc_course.Count),
                "fromJsonMachine",Json.True(),
                "paticipatedCourses", Json.Number(u.gc_paticipate.Where(tp => tp.approved).Count()));
        }
        public JsonObject GetJson()
        {
            var o = GCUser.GetJson(this.U);
            if (this.IsSelf)
            {
                o["sex"] = Json.String("我");
            }
            o["isSelf"] = Json.Bool(this.IsSelf);
            o["isFriend"] = Json.Bool(this.IsFriend);
            o["isSalutted"] = Json.Bool(this.IsSalutted);
            o["isSalutting"] = Json.Bool(this.IsSalutting);

            return o;
        }
        public JsonObject GetCourses(string p, int pagesize)
        {
            if (IsSelf || IsFriend || this.Can(GCUserFlag.AllowGuestViewCourseList))
            {
                return GCCommon.GetPagedList(this.U.gc_course.AsQueryable(), tc => tc.id, GCCourse.GetJson, p, pagesize);
            }
            GCException.GCStopMessage(UI.NotAllowGuestViewCourses);
            return null;
        }
        public JsonObject GetPaticipatedCourses(string p, int pagesize)
        {
            if (IsSelf || IsFriend || this.Can(GCUserFlag.AllowGuestViewPaticipatedList))
            {
                return GCCommon.GetPagedList(this.U.gc_paticipate.Where(tp=>tp.approved).AsQueryable(), tc => tc.id, (tc)=>GCCourse.GetJson(tc.gc_course), p, pagesize);
            }
            GCException.GCStopMessage(UI.NotAllowGuestViewPaticipatedCourses);
            return null;
        }
        public bool Can(GCUserFlag flag)
        {
            return (this.U.flagsettings & (1 << (int)flag)) != 0;
        }
        public JsonArray GetProfiles()
        {
            var arr = Json.Array();
            if (this.IsFriend || this.IsSelf || this.Can(GCUserFlag.AllowGuestViewProfile))
            {
                foreach (var p in this.U.gc_profile.OrderBy(tp=>tp.sort))
                {
                    arr.Add(Json.Object(
                        "name", Json.String(p.name),
                        "value", Json.String(p.value)
                        ));
                }
                return arr;
            }
            GCException.GCStopMessage(UI.NotAllowGuestViewProfile);
            return null;
        }
        public JsonArray GetMoreProfiles()
        {
            var arr = Json.Array();
            if (this.IsFriend || this.IsSelf || this.Can(GCUserFlag.AllowGuestViewProfile))
            {
                foreach (var p in this.D.gccon_profilename.OrderBy(tp => tp.defaultSort).AsEnumerable().Where(tp=>!this.U.gc_profile.Any(ttp=>ttp.name==tp.name)))
                {
                    arr.Add(Json.Object(
                        "name", Json.String(p.name)
                        ));
                }
                return arr;
            }
            GCException.GCStopMessage(UI.NotAllowGuestViewProfile);
            return null;
        }

        public JsonObject GetFriends(string p,int psize)
        {
            if (this.IsSelf || this.IsFriend || this.Can(GCUserFlag.AllowGuestViewFriendsList))
            {
                return GCCommon.GetPagedList(this.D.gc_user.Where(tu => this.D.gc_friendship.Any(tf =>
                    ((tf.user_id1 == tu.id && tf.user_id2 == this.U.id) ||
                (tf.user_id2 == tu.id && tf.user_id1 == this.U.id)) && tf.accepted)), tu => tu.truename, GCUser.GetJson, p, psize);
            }
            GCException.GCStopMessage(UI.NotAllowGuestViewFriends);
            return null;
        }
        public JsonObject GetAllSettings()
        {
            var obj = Json.Object();
            foreach (var a in Enum.GetNames(typeof(GCUserFlag)))
            {
                obj[a] = Json.Bool(this.Can((GCUserFlag)Enum.Parse(typeof(GCUserFlag), a)));
            }
            return obj;
        }

        public JsonObject GetNotes(string p,int psize)
        {
            if (this.IsSelf || this.IsFriend || this.Can(GCUserFlag.AllowGuestViewNotesList))
            {
                var uid = -1;
                if (GCCommon.GetVisitor(HttpContext.Current) is GCAuthenticated)
                {
                    uid = (GCCommon.GetVisitor(HttpContext.Current) as GCAuthenticated).U.id;
                }
                return GCCommon.GetPagedList(
                    this.D.gc_note.Where(tn => tn.user_id == this.U.id).Where(tn => 
                        tn.privateuser_id==-1//不是悄悄话
                        ||tn.privateuser_id==uid//是回复给自己的
                        || this.IsSelf //是自己收到的
                        || tn.fromuser_id == uid),//是自己发出的
                    tn => tn.time,
                    tn =>
                    {
                        var obj = Json.Object(
                        "id", Json.Number(tn.id),
                        "content", Json.String(tn.content),
                        "time", Json.String(GCCommon.GetTime(tn.time)));
                        if (tn.privateuser_id != -1)
                        {
                            obj["content"] = Json.String("<div class=\"floatright\" style=\"font-size:0.9em;color:#999;\"><span class=\"ftype ftype-private\"></span>悄悄话</div>" + (obj["content"] as JsonString).Text);
                        }
                        obj["fromJsonMachine"] = Json.True();
                        obj.Embed(GCUser.GetJson(this.D.gc_user.First(tu => tu.id == tn.fromuser_id)), "teacher");
                        return obj;
                    },
                    p,
                    psize);
                        
            }
            GCException.GCStopMessage(UI.NotAllowGuestViewNotes);
            return null;
        }

        public void CreateNote(string content, bool priv)
        {
            if (this.IsFriend || this.IsSelf)
            {
                var a=(GCCommon.GetVisitor(HttpContext.Current) as GCAuthenticated);
                var replyId=-1;
                try
                {
                    var reply = content.Remove(content.IndexOf(')'));
                    if (reply.Remove(reply.IndexOf(' ')) == "(回复")
                    {
                        var t = reply.Substring(reply.IndexOf(' ') + 1);
                        replyId = this.U.gc_note.First(tn => this.D.gc_user.First(tu => tu.id == tn.fromuser_id).truename == t).fromuser_id;
                        content = content.Replace(reply, "<b>" + reply + "</b>");
                    }
                }
                catch { }
                if (replyId != -1)
                {
                    this.D.gc_msg.AddObject(new gc_msg()
                    {
                        content = string.Format(UI.MsgNewNoteReply, a.U.truename),
                        fromid = a.U.id,
                        user_id = replyId,
                        nexturl = "/T/" + this.U.originalusername+"#Notes",
                        read = false,
                        time = DateTime.Now
                    });
                }
                if (replyId != this.U.id)
                {
                    this.D.gc_msg.AddObject(new gc_msg()
                    {
                        content = string.Format(UI.MsgNewNote, a.U.truename),
                        fromid = a.U.id,
                        user_id = this.U.id,
                        nexturl = "/T/" + this.U.originalusername + "#Notes",
                        read = false,
                        time = DateTime.Now
                    });
                }
                this.D.gc_note.AddObject(new gc_note()
                {
                    content=content,
                    fromuser_id = a.U.id,
                    user_id=this.U.id,
                    time=DateTime.Now,
                    privateuser_id = priv ? (replyId == -1 ? this.U.id : replyId) : -1
                });
                this.D.SaveChanges();
            }
        }

        public void DelNote(int id)
        {
            if (this.IsSelf)
            {
                GCException.GCConfirm(UI.ConfirmDeleteNote);
                this.D.gc_note.DeleteObject(this.U.gc_note.First(tn => tn.id == id));
                this.D.SaveChanges();
            }
        }
    }
    public enum GCUserFlag : int
    {
        AllowGuestViewCourseList = 01,
        AllowGuestViewPaticipatedList = 02,
        AllowGuestViewShareList = 03,
        AllowGuestViewFriendsList = 04,
        AllowGuestViewNotesList = 06,
        AllowAddFriend = 07,
        AllowGuestViewProfile=08
    }
}