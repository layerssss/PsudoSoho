using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MFLJson;
using System.Drawing;
using System.Linq.Expressions;
namespace GoClassing.Internal
{
    public class GCAuthenticated:GCVisitor
    {
        public gc_localtestEntities D;
        public gc_user U;
        public GCAuthenticated(gc_localtestEntities d)
        {
            this.D = d;
        }
        public IEnumerable<gc_user> AllFriends
        {
            get
            {
                return this.U.gc_friendship.Where(tf => tf.accepted).Select(tf => tf.gc_user1).Concat(this.U.gc_friendship1.Where(tf => tf.accepted).Select(tf => tf.gc_user));
            }
        }
        public JsonArray GetFeeds(int startId,string ftypes)
        {
            if (startId == -1)
            {
                startId = int.MaxValue;
            }
            return Json.Array(this.U.gc_feed.Where(tf => ftypes.Contains(tf.ftype)).OrderByDescending(tf => tf.id).Where(tf => tf.id < startId).Take(10).Select(tf => Json.Object(
                    "id", Json.Number(tf.id),
                    "ftype", Json.String(tf.ftype),
                    "text", Json.String(tf.text),
                    "time", Json.String(GCCommon.GetTime(tf.time))
                )).ToArray());
        }
        public GCCourse VisitCourse(int courseId)
        {
            var c=new GCCourse(courseId, this.D);
            c.IsCreatorOf = this.U.gc_course.Contains(c.C);
            c.IsMemberOf = c.IsMemberOf || this.U.gc_paticipate.Any(tp => tp.approved && tp.gc_course == c.C);
            return c;
        }
        public GCUser VisitUser(string username)
        {
            var u = new GCUser(this.D, username);
            u.IsFriend = this.D.gc_friendship.Any(tf => (
                (tf.user_id1 == u.U.id && tf.user_id2 == this.U.id) ||
                (tf.user_id2 == u.U.id && tf.user_id1 == this.U.id)) && tf.accepted);
            u.IsSalutting = this.D.gc_friendship.Any(tf =>
                (tf.user_id1 == u.U.id && tf.user_id2 == this.U.id) && !tf.accepted);
            u.IsSalutted = this.D.gc_friendship.Any(tf =>
                (tf.user_id1 == this.U.id && tf.user_id2 == u.U.id) && !tf.accepted);
            u.IsSelf = this.U.id == u.U.id;
            return u;
        }
        public JsonObject GetMyCourses(string page,string ctypes1,string ctypes2)
        {
            return GCCommon.GetPagedList(this.U.gc_course.Where(tc => ctypes1.Contains(tc.type1)
                && ctypes2.Contains(tc.type2)
                ).AsQueryable(), tg => tg.id, GCCourse.GetJson, page, 5);
        }
        public JsonObject GetMyPaticipatedCourses(string page, string ctypes1, string ctypes2)
        {
            return GCCommon.GetPagedList(this.U.gc_paticipate
                .Where(tp => tp.approved)
                .Select(tp => tp.gc_course)
                .Where(tc => ctypes1.Contains(tc.type1)
                && ctypes2.Contains(tc.type2)
                ).AsQueryable(), tg => tg.id, GCCourse.GetJson, page, 5);
        }
        public JsonArray GetMyCtypes1()
        {
            return Json.Array(
                this.U.gc_paticipate
                .Where(tp => tp.approved)
                .Select(tp => tp.gc_course)
                .Union(this.U.gc_course)
                .Select(tc => tc.type1).Distinct().Select(tctype => Json.Object(
                    "type", Json.String(tctype)
                    )));
        }
        public JsonArray GetMyCtypes2(string ctypes1)
        {
            return Json.Array(
                this.U.gc_paticipate
                .Where(tp => tp.approved)
                .Select(tp => tp.gc_course)
                .Union(this.U.gc_course)
                .Where(tc => ctypes1.Contains(tc.type1))
                .Select(tc => tc.type2).Distinct().Select(tctype => Json.Object(
                    "type", Json.String(tctype)
                    )));
        }
        public int CreateCourse(string name, string ctype1, string ctype2)
        {
            if (!GCSearch.GetAllCtypes1().Any(tjs => ((tjs as JsonObject)["type"] as JsonString).Text == ctype1))
            {
                throw (new GCException("ctype1",GCExceptionType.FieldReqired));
            }

            if (!this.D.gccon_ctype2.Any(tt=>tt.ctype1_type==ctype1&&tt.type==ctype2))
            {
                throw (new GCException("ctype2", GCExceptionType.FieldReqired));
            }
            var c = new gc_course()
            {
                gc_user = this.U,
                name = name,
                type1 = ctype1,
                type2 = ctype2,
                canJoin=true
            };
            this.D.gc_course.AddObject(c);
            this.D.SaveChanges();
            foreach(var u in this.AllFriends){
                this.D.gc_msg.AddObject(new gc_msg()
                {
                    content=string.Format(UI.MsgFriendCreateCourse,this.U.truename,c.name),
                    fromid=this.U.id,
                    user_id=u.id,
                    nexturl="/C/"+c.id.ToString("D"+GCCommon.CourseIdLen),
                    read=false,
                    time=DateTime.Now
                });
            }
            this.D.SaveChanges();
            this.D.gc_tag.AddObject(new gc_tag()
            {
                canGuestView=true,
                canReply=false,
                course_id=c.id,
                leftcol=true,
                sort=1,
                tag="课程介绍"
            });
            this.D.gc_tag.AddObject(new gc_tag()
            {
                canGuestView = true,
                canReply = true,
                course_id = c.id,
                leftcol = true,
                sort = 2,
                tag = "在线作业"
            });
            this.D.gc_tag.AddObject(new gc_tag()
            {
                canGuestView = true,
                canReply = false,
                course_id = c.id,
                leftcol = true,
                sort = 3,
                tag = "参考资料"
            });
            this.D.gc_tag.AddObject(new gc_tag()
            {
                canGuestView = true,
                canReply = false,
                course_id = c.id,
                leftcol = false,
                sort = 1,
                tag = "开放讨论"
            });
            this.D.gc_tag.AddObject(new gc_tag()
            {
                canGuestView = true,
                canReply = false,
                course_id = c.id,
                leftcol = false,
                sort = 1,
                tag = "资源分享"
            });
            this.D.SaveChanges();
            return c.id;
        }
        void Set(GCUserFlag flag, bool value)
        {
            if (value)
            {
                this.U.flagsettings |= (1 << (int)flag);
            }
            else
            {
                this.U.flagsettings &= ~(1 << (int)flag);
            }
        }
        public void UpdataProfiles(HttpRequest r)
        {
            gc_profile p;
            while ((p = this.U.gc_profile.FirstOrDefault())!=null)
            {
                this.D.gc_profile.DeleteObject(p);
            }
            var n = Convert.ToInt32(r["num"]);
            for (int i = 0; i < n; i++)
            {
                var name=r["name" + i];
                if (!this.D.gccon_profilename.Any(tp => tp.name == name))
                {
                    GCException.GCStopMessage("数据无效");
                }
                this.D.gc_profile.AddObject(new gc_profile()
                {
                    sort=i,
                    gc_user=this.U,
                    name = name,
                    value = r["value" + i]
                });
            }
            this.D.SaveChanges();
        }
        public void UpdateSettings(HttpRequest request)
        {
            foreach (var a in Enum.GetNames(typeof(GCUserFlag)))
            {
                if (request[a] != null)
                {
                    this.Set((GCUserFlag)Enum.Parse(typeof(GCUserFlag), a), request[a]=="true");
                }
            }
            this.D.SaveChanges();
        }
        public bool Salut(string username)
        {
            var u = this.VisitUser(username);
            if (u.IsSelf )
            {
                GCException.GCStopMessage("抱歉，无法将自己加为好友。");
            }
            if ( u.IsSalutted)
            {
                GCException.GCStopMessage("您已经向该用户发送过了好友请求。");
            }
            if (u.IsFriend)
            {
                GCException.GCStopMessage("该用户已经是您的好友。");
            }
            if (u.IsSalutting)
            {
                this.D.gc_friendship.First(tf =>
                    tf.user_id1 == u.U.id && tf.user_id2 == this.U.id).accepted = true;
                this.D.gc_msg.AddObject(new gc_msg()
                {
                    content = string.Format(UI.MsgFriendAccepted, this.U.truename),
                    fromid = this.U.id,
                    user_id = u.U.id,
                    nexturl = "/T/" + this.U.originalusername,
                    read = false,
                    time = DateTime.Now
                });
                this.D.SaveChanges();
                return true;
            }
            else
            {
                if (!u.Can(GCUserFlag.AllowAddFriend))
                {
                    GCException.GCStopMessage(UI.NotAllowAddFriend);
                }
                this.D.gc_friendship.AddObject(new gc_friendship()
                {
                    accepted = false,
                    user_id1 = this.U.id,
                    user_id2 = u.U.id,
                });

                this.D.gc_msg.AddObject(new gc_msg()
                {
                    content = string.Format(UI.MsgFriendSalutting, this.U.truename),
                    fromid = this.U.id,
                    user_id = u.U.id,
                    nexturl = "/T/" + this.U.originalusername,
                    read = false,
                    time = DateTime.Now
                });
                this.D.SaveChanges();
                return false;
            }
        }
        public void Deny(string username)
        {

            var u = this.VisitUser(username);
            if (u.IsSalutted)
            {
                GCException.GCConfirm("确定要取消对" + u.U.truename + "发送的好友请求吗？");
                this.D.gc_friendship.DeleteObject(this.D.gc_friendship.First(tf => (tf.user_id1 == this.U.id && tf.user_id2 == u.U.id) && !tf.accepted));

            }
            if (u.IsSalutting)
            {
                GCException.GCConfirm("确定要拒绝来自" + u.U.truename + "的好友请求吗？");
                this.D.gc_friendship.DeleteObject(this.D.gc_friendship.First(tf => (tf.user_id1 == u.U.id && tf.user_id2 == this.U.id) && !tf.accepted));

                this.D.gc_msg.AddObject(new gc_msg()
                {
                    content = string.Format(UI.MsgFriendDenied, this.U.truename),
                    fromid = this.U.id,
                    user_id = u.U.id,
                    nexturl = "/T/" + this.U.originalusername,
                    read = false,
                    time = DateTime.Now
                });
            }
            if (u.IsFriend)
            {
                GCException.GCConfirm("确定删除与" + u.U.truename + "的好友关系吗？");
                this.D.gc_friendship.DeleteObject(this.D.gc_friendship.First(tf => ((tf.user_id1 == u.U.id && tf.user_id2 == this.U.id) ||
                (tf.user_id2 == u.U.id && tf.user_id1 == this.U.id)) && tf.accepted));

            }
            this.D.SaveChanges();
        }
        public void UpdataProfile(HttpRequest httpRequest)
        {
            this.U.truename = httpRequest["truename"];
            this.U.ucity = httpRequest["ucity"];
            this.U.uprovince = httpRequest["uprovince"];
            this.U.sex = httpRequest["sex"] == ""?null:(Nullable<bool>)(httpRequest["sex"]=="m");
            this.D.SaveChanges();
        }
        public void UpdateAvatar(JsonObject obj)
        {
            GCCommon.CreateUploadTicket("/Avatars/" + this.U.id + ".jpg", 10000000, this.U.id, obj);
        }
        public JsonArray GetSaluttingUsers()
        {
            return Json.Array(this.U.gc_friendship1.Where(tf=>!tf.accepted).Select(tf => tf.gc_user).AsEnumerable().Select(tu => GCUser.GetJson(tu)));
        }
        public MFLJson.JsonObject GetMyFriends(string pagestr, string search, bool online)
        {
            var ss = search.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            var D = new gc_localtestEntities();
            var us = this.U.gc_friendship.Where(tf => tf.accepted).Select(tf => tf.gc_user1).Concat(this.U.gc_friendship1.Where(tf => tf.accepted).Select(tf => tf.gc_user)).AsQueryable();//D.gc_user.AsQueryable();
            if (online)
            {
                us = us.Where(tu => (DateTime.Now - tu.timeLastActivity).TotalMinutes < 5);
            }
            var partu = Expression.Parameter(typeof(gc_user));
            Expression exp = Expression.Constant(true, typeof(bool));
            if (ss.Any())
            {
                var contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                Expression nexp = Expression.Constant(false, typeof(bool));
                foreach (var f in ss)
                {
                    nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partu, "truename"), contains, Expression.Constant(f, typeof(string))));
                    if (D.gccon_province.Any(tp => tp.province.Contains(f)))
                    {
                        nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partu, "uprovince"), contains, Expression.Constant(f, typeof(string))));
                    }
                    if (D.gccon_city.Any(tp => tp.city.Contains(f)))
                    {
                        nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partu, "ucity"), contains, Expression.Constant(f, typeof(string))));
                    }
                }
                exp = Expression.And(exp, nexp);
            }
            return GCCommon.GetPagedList(us.Where(Expression.Lambda<Func<gc_user, bool>>(exp, partu)), tu => tu.truename, tu => GCUser.GetJson(tu), pagestr, 8, true);
        }
        public JsonObject GetMyFriends(string pagestr, string search)
        {
            return this.GetMyFriends(pagestr, search, false);
        }
        public JsonString GetMyPwdQuestion()
        {
            return Json.String(this.U.pwdQuestion);
        }
        public void UpdatePwdQuestion(string pwdQuestion, string pwdAnswer,string pwdAnswer2)
        {
            GCCommon.VerifyOldPwdAnser();
            if (pwdAnswer != pwdAnswer2)
            {
                GCException.GCStopFieldError("pwdAnswer2", "两次输入的新安全问题答案不同。");
            }
            this.U.pwdQuestion = pwdQuestion;
            this.U.pwdAnswer = pwdAnswer;
            this.D.SaveChanges();

        }
        public void UpdatePassword(string oldPassword, string password1, string password2)
        {
            if (password1 != password2)
            {
                GCException.GCStopFieldError("password2", "两次输入的新密码不同。");
            }
            if (this.U.password != oldPassword)
            {
                GCException.GCStopFieldError("oldPassword", "当前密码错误。");
            }
            if (password1.Length < 6)
            {
                GCException.GCStopFieldError("password1", "新密码的最小长度为6。");
            }
            this.U.password = password1;
            this.D.SaveChanges();
            
        }
        public void UpdateEmail(string oldEmail, string email,string email2)
        {
            GCCommon.VerifyOldPwdAnser();
            if (email != email2)
            {
                GCException.GCStopFieldError("email2", "两次输入的新电子邮箱地址不同。");
            }
            if (this.U.email != oldEmail)
            {
                GCException.GCStopFieldError("oldEmail", "当前电子邮箱地址错误。");
            }
            if(this.D.gc_user.Any(tu=>tu.email==email&&tu.id!=U.id)){
                GCException.GCStopFieldError("email", "这个电子邮件地址已经被他人使用。");
            }
            var ar = new[] { '.', '_', '-', '@' };
            if (!email.All(tc => char.IsLetterOrDigit(tc) || ar.Contains(tc)) || email.Count(tc => tc == '@') != 1)
            {
                GCException.GCStopFieldError("email", "您输入的新电子邮件地址格式不正确。");
            }
            this.U.email = email;
            this.U.mailverified = false;
            GCCommon.Logout(HttpContext.Current);
            this.D.SaveChanges();
        }


        public JsonObject GetMyCourses(string page, string filter)
        {
            filter = filter ?? "";
            var D = new gc_localtestEntities();
            var filters = filter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var partc = Expression.Parameter(typeof(gc_course), "tc");
            var contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            Expression nexp = Expression.Constant(false, typeof(bool));
            foreach (var f in filters)
            {
                nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partc, "name"), contains, Expression.Constant(f, typeof(string))));
                nexp = Expression.Or(nexp, Expression.Call(Expression.Property(Expression.Property(partc, "gc_user"), "truename"), contains, Expression.Constant(f, typeof(string))));
            }
            nexp = Expression.And(nexp, nexp);
            return GCCommon.GetPagedList(
                this.U.gc_course.Where(Expression.Lambda<Func<gc_course, bool>>(
                nexp
                , partc).Compile())
                .Concat(
                this.U.gc_paticipate.Select(tp => tp.gc_course).Where(Expression.Lambda<Func<gc_course, bool>>(
                nexp
                , partc).Compile())
                )
                .AsQueryable()
                , tg => tg.name, GCCourse.GetJson, page, 10, true);
        }




        public JsonArray GetMessages(string startDate, string stopDate, bool showOld)
        {
            var msgs = this.U.gc_msg.AsQueryable().Where(tg => !tg.read || showOld);
            DateTime d, d2;
            if (DateTime.TryParse(startDate, out d))
            {
                d2 = DateTime.Parse(stopDate).AddDays(1);
                msgs = msgs.Where(tm => tm.time > d && tm.time < d2);
            }
            return Json.Array(msgs.OrderByDescending(tm=>tm.time).AsEnumerable().Select(GetMessage));
        }
        public static JsonObject GetMessage(gc_msg tm)
        {
            var D = new gc_localtestEntities();
            var obj = Json.Object(
                       "fromJsonMachine", Json.True(),
                   "id", Json.Number(tm.id),
                   "content", Json.String(tm.content),
                   "time", Json.String(GCCommon.GetTime(tm.time)));
            obj.Embed(GCUser.GetJson(D.gc_user.First(tu => tu.id == tm.fromid)), "from");
            return obj;
        }


        public JsonObject GetMyMsgSetting()
        {
            return Json.Object(
                "type", Json.String(this.U.msgType.ToString()),
                "clock", Json.String(this.U.msgClock.ToString()));
        }


        public void UpdateMailing(int msgType, int msgClock)
        {
            this.U.msgType = msgType;
            this.U.msgClock = msgClock;
            this.D.SaveChanges();
        }
    }
}