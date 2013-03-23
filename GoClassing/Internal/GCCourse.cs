using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MFLJson;
namespace GoClassing.Internal
{
    public class GCCourse
    {
        public gc_localtestEntities D;
        public gc_course C;
        public bool IsMemberOf;
        public bool IsCreatorOf;
        public GCCourse(int courseId,gc_localtestEntities d)
        {
            this.D = d;
            this.C = this.D.gc_course.First(tc => tc.id == courseId);
        }
        public JsonObject GetMembers(string pagestr,int pageSize)
        {
            return GCCommon.GetPagedList(this.C.gc_paticipate.AsQueryable().Where(tp => tp.approved), t => t.gc_user.truename, tp => {
                var obj=GCUser.GetJson(tp.gc_user);
                obj["due"] = Json.String(tp.overdue.HasValue ? tp.overdue.Value.ToString("yyyy-MM-dd") : "无");
                return obj;
            }, pagestr, pageSize, true);
        }
        public JsonArray GetNewMembers()
        {
            return Json.Array(this.C.gc_paticipate.Where(tp => !tp.approved).Select(tp => GCUser.GetJson(tp.gc_user)));
        }
        public void Update(string left, string right, bool canJoin)
        {
            if (this.IsCreatorOf)
            {
                Dictionary<string, object[]> tags = new Dictionary<string, object[]>();
                try
                {
                    var tagstrs = left.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tagstrs)
                    {
                        var ar = tag.Split(',');
                        tags.Add(
                            HttpUtility.UrlDecode(ar[0]),
                            new object[]{
                                true,
                                bool.Parse(ar[1]),
                                bool.Parse(ar[2]),
                                ar.Skip(3).Select(tstr=>Convert.ToInt32(tstr))
                            });
                    }
                    tagstrs = right.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tagstrs)
                    {
                        var ar = tag.Split(',');
                        tags.Add(
                            HttpUtility.UrlDecode(ar[0]),
                            new object[]{
                                false,
                                bool.Parse(ar[1]),
                                bool.Parse(ar[2]),
                                ar.Skip(3).Select(tstr=>Convert.ToInt32(tstr))
                            });
                    }
                }
                catch(Exception ex)
                {
                    GCException.GCStopMessage("传入数据格式不正确：" + ex.Message);
                }
                this.C.canJoin = canJoin;
                if (left == "" && right == "")
                {
                    this.D.SaveChanges();
                    return;
                }
                while (this.C.gc_tag.Any())
                {
                    this.D.gc_tag.DeleteObject(this.C.gc_tag.First());
                }
                foreach (var p in this.C.gc_post)
                {
                    p.tag = null;
                }
                var i = 0;
                foreach (var tag in tags.Keys)
                {
                    this.D.gc_tag.AddObject(new gc_tag()
                    {
                        tag = tag,
                        leftcol = (bool)tags[tag][0],
                        canReply = (bool)tags[tag][1],
                        canGuestView = (bool)tags[tag][2],
                        course_id = this.C.id,
                        sort = i
                    });
                    var posts = tags[tag][3] as IEnumerable<int>;
                    int j = 0;
                    foreach (var pid in posts)
                    {
                        var p = this.C.gc_post.First(tp => tp.id == pid);
                        p.tag=tag;
                        p.sort = j;
                        j++;
                    }
                    i++;
                }
                while (this.C.gc_post.Any(tp => tp.tag == null))
                {
                    var p = this.C.gc_post.First(tp => tp.tag == null);
                    while (p.gc_reply.Any())
                    {
                        var r=p.gc_reply.First();
                        this.DelReply(p.id, r.id);
                    }
                    this.D.gc_post.DeleteObject(p);
                }
                this.D.SaveChanges();

            }
        }
        public static JsonObject GetJson(gc_course c){
            var obj = Json.Object(
                "showId",Json.String(string.Format("{0:D"+GCCommon.CourseIdLen+"}",c.id)),
                "id", Json.Number(c.id),
                "name", Json.String(c.name),
                "ctype1", Json.String(c.type1),
                "ctype2", Json.String(c.type2),
                "canJoinText",Json.String(c.canJoin?"是":"否"),
                "fromJsonMachine", Json.True(),
                //"numDoc", Json.Number(c.gc_post.Count(tp => tp.ftype == "doc")),
                //"numPpt", Json.Number(c.gc_post.Count(tp => tp.ftype == "ppt")),
                //"numVideo", Json.Number(c.gc_post.Count(tp => tp.ftype == "video")),
                //"numAssign", Json.Number(c.gc_post.Count(tp => tp.ftype == "assign")),
                //"numNotify", Json.Number(c.gc_post.Count(tp => tp.ftype == "notify")),
                //"numDiscuz", Json.Number(c.gc_post.Count(tp => tp.ftype == "discuz")),
                "numMembers", Json.Number(c.gc_paticipate.Count(tp => tp.approved)+1));
            obj.Embed(GCUser.GetJson(c.gc_user), "teacher");
            return obj;
        }
        public JsonArray GetPosts(gc_tag tag){
            return Json.Array(this.C.gc_post.Where(tp => tp.tag == tag.tag).OrderBy(tp => tp.sort).Select(tp => Json.Object(
                "ftype",Json.String(tp.ftype),
                "id", Json.Number(tp.id),
                "time",Json.String(GCCommon.GetTime(tp.time)),
                "title",Json.String(tp.title),
                "numReply",Json.Number(tp.gc_reply.Count(tr=>tr.ftype!=null))                
                )));
        }
        public void AddTag(string tag,string leftCol)
        {
            if (this.C.gc_tag.Any(tt => tt.tag == tag))
            {
                GCException.GCStopFieldError("tag", string.Format(UI.TagAlreadyInCourse, tag));
            }
            this.D.gc_tag.AddObject(new gc_tag()
            {
                course_id=this.C.id,
                leftcol=leftCol!=null,
                sort=1000,
                tag=tag,
                canGuestView=true,
                canReply=true
            });
            this.D.SaveChanges();
        }
        public void AddPost(string title, string tag)
        {
            if (this.C.gc_tag.Any(tt => tt.tag == tag))
            {
                this.D.gc_post.AddObject(new gc_post()
                {
                    course_id=this.C.id,
                    ftype="unknown",
                    sort=1000,
                    tag=tag,
                    time=DateTime.Now,
                    title=title
                });
                this.D.SaveChanges();
            }
        }
        public JsonObject GetJson()
        {
            var obj = GetJson(this.C);
            obj["isMemberOf"] = Json.Bool(this.IsMemberOf);
            obj["isCreatorOf"] = Json.Bool(this.IsCreatorOf);
            return obj;
        }

        public void Join()
        {
            var v = GCCommon.GetVisitor(HttpContext.Current);
            if (v is GCAuthenticated)
            {
                if (!this.IsCreatorOf)
                {
                    if (this.C.canJoin)
                    {

                        var uid = (v as GCAuthenticated).U.id;
                        if (this.C.gc_paticipate.Any(tp => tp.user_id == uid))
                        {
                            GCException.GCStopMessage(UI.AlreadyApplyingJoinCourse);
                        }
                        this.D.gc_paticipate.AddObject(new gc_paticipate()
                        {
                            approved = false,
                            overdue = null,
                            course_id = this.C.id,
                            user_id = uid
                        });
                        this.D.gc_msg.AddObject(new gc_msg()
                        {
                            content = string.Format(UI.MsgNewJoinApply, (v as GCAuthenticated).U.truename, this.C.name),
                            fromid = (v as GCAuthenticated).U.id,
                            nexturl = "/C/" + this.C.id.ToString("D" + GCCommon.CourseIdLen) + "/Members",
                            read = false,
                            user_id = this.C.user_id,
                            time = DateTime.Now
                        });
                        this.D.SaveChanges();
                    }
                    else
                    {
                        GCException.GCStopMessage(UI.NotAllowGuestJoin);
                    }
                }
            }
            else
            {
                throw (new NotImplementedException());
            }
        }
        public void Aprove(int userId, DateTime? dateTime)
        {
            if (this.IsCreatorOf)
            {
                this.C.gc_paticipate.First(tp => tp.user_id == userId).approved = true;
                this.C.gc_paticipate.First(tp => tp.user_id == userId).overdue = dateTime;

                this.D.gc_msg.AddObject(new gc_msg()
                {
                    content = string.Format(UI.MsgApprovedJoin, this.C.gc_user.truename, this.C.name),
                    fromid = this.C.user_id,
                    nexturl = "/C/" + this.C.id.ToString("D" + GCCommon.CourseIdLen)+"/Members",
                    read = false,
                    user_id = userId,
                    time = DateTime.Now
                });
                this.D.SaveChanges();
            }
        }

        public void DelMember(int userId)
        {
            if (this.IsCreatorOf)
            {
                try
                {
                    var p=this.C.gc_paticipate.First(tp => tp.user_id == userId);
                    
                    this.D.gc_msg.AddObject(new gc_msg()
                    {
                        content = string.Format(p.approved ? UI.MsgRemovedMembership : UI.MsgDeniedJoin, this.C.gc_user.truename, this.C.name),
                        fromid=this.C.user_id,
                        nexturl = "/C/" + this.C.id.ToString("D" + GCCommon.CourseIdLen) ,
                        read = false,
                        user_id = userId,
                        time = DateTime.Now

                    });
                    this.D.gc_paticipate.DeleteObject(p);
                    this.D.SaveChanges();
                }
                catch
                {
                    throw (new GCException("|location.reload", GCExceptionType.NeedLogin));
                }
            }
        }

        public void Deprove(int userId)
        {
            if (this.IsCreatorOf)
            {
                this.C.gc_paticipate.First(tp => tp.user_id == userId).approved = false;
                this.C.gc_paticipate.First(tp => tp.user_id == userId).overdue = null;

                this.D.gc_msg.AddObject(new gc_msg()
                {
                    content = string.Format(UI.MsgRemovedMembership, this.C.gc_user.truename, this.C.name),
                    fromid = this.C.user_id,
                    nexturl = "/C/" + this.C.id.ToString("D" + GCCommon.CourseIdLen),
                    read = false,
                    user_id = userId,
                    time = DateTime.Now
                });
                this.D.SaveChanges();
            }
        }

        public JsonObject GetPost(int pid)
        {
            var obj = Json.Object();
            var p = this.C.gc_post.First(tp => tp.id == pid);
            var t=this.C.gc_tag.First(tt => tt.tag == p.tag);
            if (!(this.IsCreatorOf || this.IsMemberOf||t.canGuestView))
            {
                GCException.GCStopMessage(UI.NotAllowGuestViewPostList);
            }
            obj["isCreatorOf"] = Json.Bool(this.IsCreatorOf);
            obj["tag"] = Json.String(p.tag);
            obj["canReply"] = Json.Bool((t.canReply && this.IsMemberOf && p.gc_reply.Count(tr => tr.ftype != null) > 0) || this.IsCreatorOf);
            obj["title"] = Json.String(p.title);
            obj["time"] = Json.String(GCCommon.GetTime(p.time));
            obj["limitationSwf"] = Json.String(GCCommon.FormatSpace(this.IsCreatorOf ?
                Global.ReplyLimitationOwner["swf"] :
                Global.ReplyLimitationMember["swf"]));
            obj["limitationMp4"] = Json.String(GCCommon.FormatSpace(this.IsCreatorOf ?
                Global.ReplyLimitationOwner["mp4"] :
                Global.ReplyLimitationMember["mp4"]));
            obj["limitationMp3"] = Json.String(GCCommon.FormatSpace(this.IsCreatorOf ?
                Global.ReplyLimitationOwner["mp3"] :
                Global.ReplyLimitationMember["mp3"]));
            obj["reply"] = Json.Array(p.gc_reply.Where(r => r.ftype != null).Select(tr =>
            {
                var o=Json.Object(
                    "ftype", Json.String(tr.ftype),
                    "fromJsonMachine", Json.True(),
                    "time", Json.String(GCCommon.GetTime(tr.time)),
                    "id",Json.Number(tr.id)
                    );
                o.Embed(GCUser.GetJson(tr.gc_user), "teacher");

                switch (tr.ftype)
                {
                    case "text":
                        o["content"] = Json.String(tr.content);
                        break;
                    case "ppt":
                        o["content"] = Json.String(
                            "<span class=\"content\"><a href=\"#\" onclick=\"playSwf(this,'/Drop/Reply" + tr.id + ".swf');return false;\">演示文稿:"+tr.filename+"</a><p class=\"info\">文件大小：" + GCCommon.FormatSpace(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + ".swf")) + "</p></span>");
                        break;
                    case "doc":
                        o["content"] = Json.String(
                            "<span class=\"content\"><a href=\"#\" onclick=\"playSwf(this,'/Drop/Reply" + tr.id + ".swf');return false;\">Word文档:" + tr.filename + "</a><p class=\"info\">文件大小：" + GCCommon.FormatSpace(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + ".swf")) + "</p></span>");
                        break;
                    case "pdf":
                        o["content"] = Json.String(
                            "<span class=\"content\"><a href=\"#\" onclick=\"playSwf(this,'/Drop/Reply" + tr.id + ".swf');return false;\">PDF文档:" + tr.filename + "</a><p class=\"info\">文件大小：" + GCCommon.FormatSpace(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + ".swf")) + "</p></span>");
                        break;
                    case "video":
                        o["content"] = Json.String("<span class=\"content\"><img class=\"preview\" title=\"视频预览\" src=\"/Drop/reply" + tr.id + ".mp4.jpg\" />"
                            + "<a href=\"#\" onclick=\"playVideo(this,'/Drop/Reply" + tr.id + ".mp4');return false;\">视频片段:" + tr.filename + "</a><p class=\"info\">文件大小：" + GCCommon.FormatSpace(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + ".mp4")) + "</p>"
                            + "<p class=\"info\">视频时间：" + GCCommon.FormatTimeSpan(Convert.ToInt32(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + ".mp4.length")))) + "</p>"
                            + "</span>");
                        break;
                    case "sound":
                        o["content"] = Json.String(
                            "<span class=\"content\"><a href=\"#\" onclick=\"playSound(this,'/Drop/Reply" + tr.id + '.' + tr.ext + "');return false;\">音频片段:" + tr.filename + "</a><p class=\"info\">文件大小：" + GCCommon.FormatSpace(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + "." + tr.ext)) + "</p>"
                            + "<p class=\"info\">音频时间：" + GCCommon.FormatTimeSpan(Convert.ToInt32(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Drop/reply" + tr.id + "." + tr.ext + ".length")))) + "</p>"
                            + "</span>");
                        break;
                    case "processing":
                        o["content"] = Json.String("该内容(" + tr.filename + ")正在被上传或处理,请稍候...<div class=\"info\" style=\"display:inline;border:none;\">(注：如果该内容已经上传失败而未被自动删除，请手动删除该内容。)</div>");
                        break;
                }
                return o;
            }));
            return obj;
        }

        public void CreateReply(int pid, string content, string selectedFilename, JsonObject obj)
        {
            var p = this.GetPost(pid);
            if (!(p["canReply"] as JsonBool).Value)
            {
                GCException.GCStopMessage(UI.NotAllowReply);
            }
            var a=(GCCommon.GetVisitor(HttpContext.Current) as GCAuthenticated);
            var r = new gc_reply()
            {
                post_id=pid,
                content =HttpUtility.HtmlEncode(content).Replace("\n", "<br />"),
                time=DateTime.Now,
                user_id=a.U.id,
                ftype="processing",
                @public=true,
                filename=""
            };
            this.D.gc_reply.AddObject(r);
            this.D.SaveChanges();
            if(selectedFilename==null){
                r.ftype = "text";
                r.filename = "";
                if (content.Contains(')'))
                {
                    try
                    {
                        var reply = content.Remove(content.IndexOf(')'));
                        if (reply.Remove(reply.IndexOf(' ')) == "(回复")
                        {
                            var t = reply.Substring(reply.IndexOf(' ') + 1);
                            var uid = r.gc_post.gc_reply.First(tr => tr.gc_user.truename == t).gc_user.id;
                            this.D.gc_msg.AddObject(new gc_msg()
                            {
                                content = string.Format(UI.MsgNewReplyInPost, a.U.truename, r.gc_post.title),
                                fromid = a.U.id,
                                user_id = uid,
                                nexturl = "/C/" + r.gc_post.gc_course.id.ToString("D" + GCCommon.CourseIdLen) + "/" + r.gc_post.id,
                                read = false,
                                time = DateTime.Now
                            });
                            r.content = r.content.Replace(reply, "<b>" + reply + "</b>");
                        }
                    }
                    catch { }

                }
                this.D.SaveChanges();
                return;
            }
            try
            {
                var ext = selectedFilename.Substring(selectedFilename.LastIndexOf('.') + 1).ToLower();
                if (GCCommon.DocTypes.Contains(ext))
                {
                    GCCommon.CreateUploadTicket("/Drop/reply" + r.id + ".swf", this.IsCreatorOf ? Global.ReplyLimitationOwner["swf"] : Global.ReplyLimitationMember["swf"], this.C.user_id, obj);
                    r.ext = "swf";
                }
                if (GCCommon.MediaTypes.Contains(ext))
                {
                    GCCommon.CreateUploadTicket("/Drop/reply" + r.id + ".mp4", this.IsCreatorOf ? Global.ReplyLimitationOwner["mp4"] : Global.ReplyLimitationMember["mp4"], this.C.user_id, obj);
                    r.ext = "mp4";
                }
                if (GCCommon.SoundTypes.Contains(ext))
                {
                    GCCommon.CreateUploadTicket("/Drop/reply" + r.id + ".mp3", this.IsCreatorOf ? Global.ReplyLimitationOwner["mp3"] : Global.ReplyLimitationMember["mp3"], this.C.user_id, obj);
                    r.ext = "mp3";
                }
                if (r.ext == null)
                {
                    GCException.GCStopMessage("不支持的格式");
                }
                r.filename = selectedFilename;
                this.D.SaveChanges();
            }
            catch (Exception ex)
            {
                this.D.gc_reply.DeleteObject(r);
                this.D.SaveChanges();
                GCException.GCStopMessage(ex.Message);
            }
        }
        public void DelReply(int pid,int replyId)
        {
            if (this.IsCreatorOf)
            {
                GCException.GCConfirm("您确定要删除这条内容/回复吗？");
                var r = this.C.gc_post.First(tp => tp.id == pid).gc_reply.First(tr => tr.id == replyId);
                if (r.ext != null)
                {
                    var fs = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("/Drop/reply" + r.id + "." + r.ext));
                    if (fs.Exists)
                    {
                        this.C.gc_user.spaceUsed -= fs.Length;
                        fs.Delete();
                    }
                    fs = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("/Drop/reply" + r.id + "." + r.ext) + ".jpg");
                    if (fs.Exists)
                    {
                        this.C.gc_user.spaceUsed -= fs.Length;
                        fs.Delete();
                    }
                    fs = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("/Drop/reply" + r.id + "." + r.ext) + ".length");
                    if (fs.Exists)
                    {
                        this.C.gc_user.spaceUsed -= fs.Length;
                        fs.Delete();
                    }
                    fs = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("/Drop/reply" + r.id + "." + r.ext) + ".big.jpg");
                    if (fs.Exists)
                    {
                        this.C.gc_user.spaceUsed -= fs.Length;
                        fs.Delete();
                    }
                }
                this.D.gc_reply.DeleteObject(r);
                this.D.SaveChanges();
            }
        }

        public void HandleFeeds(gc_reply r)
        {
            if(r.gc_user!=r.gc_post.gc_course.gc_user){
                return;
            }
            
            var isnew = r.gc_post.gc_reply.OrderBy(tr => tr.time).First() == r;
            var cname="“"+r.gc_post.gc_course.name+(isnew?"”中发布了新":"”中的");
            var str="";
            switch (r.ftype)
            {
                case "video":
                    str = string.Format(UI.FeedsNewVideo,
                        r.gc_post.gc_course.gc_user.truename,
                        cname,
                        r.gc_post.title,
                        r.gc_post.gc_course.id.ToString().PadLeft(GCCommon.CourseIdLen, '0'),
                        r.gc_post.id,
                        r.id);
                    break;
                case "sound":
                    str = string.Format(UI.FeedsNewAudio,
                        r.gc_post.gc_course.gc_user.truename,
                        cname, 
                        r.gc_post.title,
                        r.gc_post.gc_course.id.ToString().PadLeft(GCCommon.CourseIdLen, '0'),
                        r.gc_post.id);
                    break;
                case "doc":
                    str = string.Format(UI.FeedsNewDoc,
                        r.gc_post.gc_course.gc_user.truename,
                        cname,
                        r.gc_post.title,
                        r.gc_post.gc_course.id.ToString().PadLeft(GCCommon.CourseIdLen, '0'),
                        r.gc_post.id);
                    break;
                case "pdf":
                    str = string.Format(UI.FeedsNewPdf,
                        r.gc_post.gc_course.gc_user.truename,
                        cname,
                        r.gc_post.title,
                        r.gc_post.gc_course.id.ToString().PadLeft(GCCommon.CourseIdLen, '0'),
                        r.gc_post.id);
                    break;
                case "xls":
                    str = string.Format(UI.FeedsNewXls,
                        r.gc_post.gc_course.gc_user.truename,
                        cname,
                        r.gc_post.title,
                        r.gc_post.gc_course.id.ToString().PadLeft(GCCommon.CourseIdLen, '0'),
                        r.gc_post.id);
                    break;
                case "ppt":
                    str = string.Format(UI.FeedsNewPpt,
                        r.gc_post.gc_course.gc_user.truename,
                        cname,
                        r.gc_post.title,
                        r.gc_post.gc_course.id.ToString().PadLeft(GCCommon.CourseIdLen, '0'),
                        r.gc_post.id);
                    break;
                default:
                    return;
            }
            foreach (var p in this.C.gc_paticipate.Where(tp => tp.approved))
            {
                this.D.gc_feed.AddObject(new gc_feed()
                {
                    user_id=p.user_id,
                    ftype=r.ftype,
                    text=str,
                    time=DateTime.Now
                });
            }

            this.D.gc_feed.AddObject(new gc_feed()
            {
                user_id = this.C.user_id,
                ftype = r.ftype,
                text = str,
                time = DateTime.Now
            });
            this.D.SaveChanges(); ;
        }
    }
}