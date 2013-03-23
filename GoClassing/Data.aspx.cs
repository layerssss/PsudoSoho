using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoClassing.Internal;
using MFLJson;

namespace GoClassing
{
    public partial class Data : System.Web.UI.Page
    {
        public static int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObject obj = Json.Object("success", Json.True());
            var u = GCCommon.GetVisitor(this.Context);
            //var str = "";
            //if (i % 3 == 0&&!Internal.GCCommon.VeryfyRecaptcha(this.Context,ref str))
            //{
            //        obj["success"] = Json.False();
            //        obj["message"] = Json.String( "reCaptcha");
            //}
            //else
            //{
            //    i++;
            try
            {
                try
                {
                    switch (Request.QueryString["action"])
                    {
                        case "GetVariables":
                            obj["sessionId"] = Json.String(this.Session.SessionID);
                            obj["repositoryUrl"] = Json.String(System.Configuration.ConfigurationManager.AppSettings["repositoryUrl"]);
                            obj["swfuTypesDescription"] = Json.Object(
                                "swf", Json.String("文档/演示文稿"),
                                "mp4", Json.String("视频录像"),
                                "mp3", Json.String("音频片段"));
                            obj["swfuTypes"] =
                                Json.Object(
                                "swf", Json.String(GCCommon.DocTypes.Aggregate("", (str1, str2) => str1 + "*." + str2 + ";")),
                                "mp4", Json.String(GCCommon.MediaTypes.Aggregate("", (str1, str2) => str1 + "*." + str2 + ";")),
                                "mp3", Json.String(GCCommon.SoundTypes.Aggregate("", (str1, str2) => str1 + "*." + str2 + ";")));
                            break;
                        case "GetPlayerVariebles":
                            obj["jplayerVideoTemplate"] = Json.String(GCTemplateReader.Get("JplayerVideo.txt"));
                            obj["jplayerAudioTemplate"] = Json.String(GCTemplateReader.Get("JplayerAudio.txt"));
                            obj["docTemplate"] = Json.String(GCTemplateReader.Get("Doc.txt"));
                            obj["baseUrl"]=Json.String(System.Configuration.ConfigurationManager.AppSettings["baseUrl"]);
                            obj["baseUrlMirror"] = Json.String("http://tangzx.cl14.53dns.net/goclassing/");
                            break;
                        case "UpdateMailing":
                            u.UpdateMailing(Convert.ToInt32(Request.QueryString["msgType"]), Convert.ToInt32(Request.QueryString["msgClock"]));
                            break;
                        case "GetMessages":
                            obj["listItems"] = u.GetMessages(Request.QueryString["dateStart"], Request.QueryString["dateStop"], Request.QueryString["showOld"] == "true");
                            break;
                        case "AddTag":
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).AddTag(GCCommon.NotNull("tag"), Request.QueryString["leftCol"]);
                            break;
                        case "AddPost":
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).AddPost(GCCommon.NotNull("title"),Request.QueryString["tag"]);
                            break;
                        case "GetCoursePost":
                            obj.Embed(u.VisitCourse(Convert.ToInt32(Request["courseId"])).GetPost(Convert.ToInt32(Request["postId"])));
                            break;
                        case "CreateReply":
                            u.VisitCourse(Convert.ToInt32(Request["courseId"])).CreateReply(Convert.ToInt32(Request["postId"]), Request["content"], Request["selectedFilename"], obj);
                            break;
                        case "CreateNote":
                            u.VisitUser(Request["username"]).CreateNote(Request["content"],Convert.ToBoolean( Request["private"]));
                            break;
                        case "DelNote":
                            u.VisitUser(Request["username"]).DelNote(Convert.ToInt32(Request["noteId"]));
                            break;
                        case "GetCourseMembers":
                            obj["members"] = u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).GetMembers(Request.QueryString["page"], 30);
                            obj["newMembers"] = u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).GetNewMembers();
                            break;
                        case "JoinCourse":
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).Join();
                            break;
                        case "AproveCourseJoin":{
                            DateTime t;
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).Aprove(Convert.ToInt32(Request.QueryString["userId"]), DateTime.TryParse(Request.QueryString["due"], out t) ? t as Nullable<DateTime> : null);
                            break;
                        }
                        case "DelCourseMember":
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).DelMember(Convert.ToInt32(Request.QueryString["userId"]));
                            break;
                        case "DelCourseReply":
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).DelReply(Convert.ToInt32(Request.QueryString["postId"]), Convert.ToInt32(Request.QueryString["replyId"]));
                            break;

                        case "DeproveCourseJoin":
                            u.VisitCourse(Convert.ToInt32(Request.QueryString["courseId"])).Deprove(Convert.ToInt32(Request.QueryString["userId"]));
                            break;
                        case "UpdateCourse":
                            u.VisitCourse(Convert.ToInt32(Request["courseId"])).Update(Request["left"], Request["right"], bool.Parse(Request["canJoin"]));
                            break;
                        case "QuickSearch":
                            obj.Embed(GCSearch.QuickSearch(Request.QueryString["page"],Request.QueryString["search"]));
                            obj["friends"] = u.GetMyFriends(null, Request.QueryString["search"]);
                            obj["courses"] = u.GetMyCourses(null, Request.QueryString["search"]);
                            break;
                        case "GetMyPwdQuestion":
                            obj["pwdQuestion"] = u.GetMyPwdQuestion();
                            obj["msg"] = u.GetMyMsgSetting();
                            break;
                        case "UpdatePassword":
                            u.UpdatePassword(GCCommon.NotNull("oldPassword"), GCCommon.NotNull("password1"), GCCommon.NotNull("password2"));
                            break;
                        case "UpdatePwdQuestion":
                            u.UpdatePwdQuestion(GCCommon.NotNull("pwdQuestion"),GCCommon.NotNull("pwdAnswer"),GCCommon.NotNull("pwdAnswer2"));
                            break;
                        case "UpdateEmail":
                            u.UpdateEmail(GCCommon.NotNull("oldEmail"), GCCommon.NotNull("email"), GCCommon.NotNull("email2"));
                            break;
                        case "GetMyFriends":
                            obj["friends"] = u.GetMyFriends(Request.QueryString["page"],Request.QueryString["search"]);
                            break;
                        case "GetSaluttingUsers":
                            obj["listItems"] = u.GetSaluttingUsers();
                            break;
                        case "UpdateAvatar":
                            u.UpdateAvatar(obj);
                            break;
                        case "GetFeeds":
                            obj["listItems"] = u.GetFeeds(Convert.ToInt32(Request.QueryString["startid"]), Request.QueryString["ftypes"]);
                            break;
                        case "GetCtypes1":
                            obj["listItems"] = u.GetMyCtypes1();
                            break;
                        case "GetCtypes2":
                            obj["listItems"] = u.GetMyCtypes2(Request.QueryString["ctypes1"]);
                            break;
                        case "GetAllCtypes1":
                            obj["listItems"] = GCSearch.GetAllCtypes1();
                            break;
                        case "GetAllCtypes2":
                            obj.Embed(GCSearch.GetAllCtypes2(Request.QueryString["ctypes1"], Request["page"]));
                            break;
                        case "GetAllCourses":
                            obj["courses"] = GCSearch.GetAllCourses(Request.QueryString["page"],
                                Request.QueryString["ctype1"], 
                                Request.QueryString["ctypes2"], 
                                Request.QueryString["filter"]);
                            break;
                        case "GetAllUsers":
                            obj["users"] = GCSearch.GetAllUsers(Request.QueryString["page"], Request.QueryString["search"]);
                            break;
                        case "创建课程":
                            obj["showId"] = Json.String(u.CreateCourse(GCCommon.NotNull("cname"),
                                GCCommon.NotNull("ctype1"),
                                GCCommon.NotNull("ctype2")).ToString("D" + GCCommon.CourseIdLen));
                            break;
                        case "GetMyCourses":
                            obj["courses"] = u.GetMyCourses(Request.QueryString["page"], Request.QueryString["ctypes1"], Request.QueryString["ctypes2"]);
                            break;
                        case "GetMyPaticipatedCourses":
                            obj["paticipatedCourses"] = u.GetMyPaticipatedCourses(Request.QueryString["page"], Request.QueryString["ctypes1"], Request.QueryString["ctypes2"]);
                            break;
                        case "GetUserProfiles":
                            obj["moreprofiles"] = u.VisitUser(Request.QueryString["username"]).GetMoreProfiles();
                            obj["profiles"] = u.VisitUser(Request.QueryString["username"]).GetProfiles();
                            break;
                        case "GetUserNotes":
                            obj["notes"] = u.VisitUser(Request.QueryString["username"]).GetNotes(Request["page"], 10);
                            break;
                        case "GetUserFriends":
                            obj["friends"] = u.VisitUser(Request.QueryString["username"]).GetFriends(Request.QueryString["page"], 24);
                            break;
                        case "GetUserCourses":
                            obj["courses"] = u.VisitUser(Request.QueryString["username"]).GetCourses(Request.QueryString["page"],10);
                            break;
                        case "GetUprovince":
                            obj["listItems"] = GCSearch.GetUprovince();
                            break;
                        case "GetUcity":
                            obj["listItems"] =GCSearch.GetCity(Request.QueryString["uprovince"]);
                            break;
                        case "GetSex":
                            obj["listItems"] = GCSearch.GetSex();
                            break;
                        case "GetUserPaticipatedCourses":
                            obj["paticipatedCourses"] = u.VisitUser(Request.QueryString["username"]).GetPaticipatedCourses(Request.QueryString["page"], 10);
                            break;
                        case "Login":
                            GCException.GCReCaptcha();
                            GCCommon.Login(GCCommon.NotNull("username"), GCCommon.NotNull("password"), Request.QueryString["remember"] != null, this.Context, Request.QueryString["redirect"]);
                            break;
                        case "Logout":
                            GCCommon.Logout(this.Context);
                            break;
                        case "Register":
                            GCCommon.Register(GCCommon.NotNull("regusername"), GCCommon.NotNull("regemail"), GCCommon.NotNull("password1"), GCCommon.NotNull("password2"), Request.QueryString["agreement"] != null, Request.QueryString["redirect"], this.Context);
                            break;
                        case "VerifyMail":
                            GCCommon.VerifyMail(GCCommon.NotNull("mail"), GCCommon.NotNull("hash"));
                            break;
                        case "GetPwdQuestion":
                            obj["pwdQuestion"] = Json.String(GCCommon.GetPwdQuestion(GCCommon.NotNull("rloginname")));
                            break;
                        case "RetrievePassword":
                            obj["mail"] = Json.String(GCCommon.RetrievePassword(GCCommon.NotNull("r2loginname"), GCCommon.NotNull("oldPwdAnser")));
                            break;
                        case "UpdateProfiles":
                            u.UpdataProfiles(this.Request);
                            break;
                        case "UpdateProfile":
                            u.UpdataProfile(this.Request);
                            break;
                        case "UpdateSettings":
                            u.UpdateSettings(this.Request);
                            break;
                        case "Salut":
                            if (!u.Salut(Request.QueryString["username"]))
                            {
                                obj["salutting"] = Json.True();
                            }
                            break;
                        case "Deny":
                            u.Deny(Request.QueryString["username"]);
                            break;
                        case "TestReCaptcha":
                            GCException.GCReCaptcha();
                            break;
                        default:
                            GCException.GCStopMessage("操作“" + Request.QueryString["action"] + "”未实现。");
                            break;
                    }
                }
                catch (NotImplementedException)
                {
                    GCException.GCNeedLogin();
                    obj["success"] = Json.False();
                    obj["message"] = Json.String("权限不够。");
                }
            }
            catch (Internal.GCException ex)
            {
                obj["success"] = Json.False();
                obj["messageType"] = Json.Number((int)ex.Type);
                obj["message"] = Json.String(ex.Message);
            }
            catch (Exception ex)
            {
                obj["success"] = Json.False();
                obj["message"] = Json.String("执行“" + Request.QueryString["action"] + "”操作时发生错误，错误原因是：" + ex.ToString());
            }
            //}
            if (Request.QueryString["formatted"] != "true")
            {
                if (Request.QueryString["cType"] == "html")
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
    }
}