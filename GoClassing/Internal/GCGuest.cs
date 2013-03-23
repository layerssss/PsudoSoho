using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoClassing.Internal
{
    public class GCGuest : GCVisitor
    {

        gc_localtestEntities D;
        public GCGuest(gc_localtestEntities d)
        {
            this.D = d;
        }


        public GCUser VisitUser(string username)
        {
            var u = new GCUser(this.D, username);
            u.IsFriend = false;
            u.IsSelf = false;
            u.IsSalutting = false;
            return u;
        }

        public GCCourse VisitCourse(int courseId)
        {
            var c = new GCCourse(courseId, this.D);
            c.IsCreatorOf = false;
            c.IsMemberOf = false;
            return c;
        }


        public MFLJson.JsonArray GetSaluttingUsers()
        {
            return MFLJson.Json.Array();
        }

        #region GCVisitor 成员

        public MFLJson.JsonArray GetFeeds(int startId, string ftypes)
        {
            throw new NotImplementedException();
        }


        public MFLJson.JsonObject GetMyCourses(string page, string ctypes1, string ctypes2)
        {
            throw new NotImplementedException();
        }

        public MFLJson.JsonObject GetMyPaticipatedCourses(string page, string ctypes1, string ctypes2)
        {
            throw new NotImplementedException();
        }

        public MFLJson.JsonArray GetMyCtypes1()
        {
            throw new NotImplementedException();
        }

        public MFLJson.JsonArray GetMyCtypes2(string ctypes1)
        {
            throw new NotImplementedException();
        }

        public MFLJson.JsonArray GetMyCtypes3(string ctypes2)
        {
            throw new NotImplementedException();
        }

        public MFLJson.JsonObject GetMyFriends(string pagestr, string search)
        {
            return GCSearch.GetAllUsers(pagestr, search);
        }
        public MFLJson.JsonObject GetMyFriends(string pagestr, string search,bool online)
        {
            return MFLJson
                .Json.Object(
                "listItems",MFLJson.Json.Array(),
                "pages",MFLJson.Json.Array()
                );
        }

        public int CreateCourse(string name, string ctype1, string ctype2)
        {
            throw new NotImplementedException();
        }

        public void UpdataProfiles(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public void UpdateSettings(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public bool Salut(string p)
        {
            throw new NotImplementedException();
        }

        public void Deny(string username)
        {
            throw new NotImplementedException();
        }

        public void UpdataProfile(HttpRequest httpRequest)
        {
            throw new NotImplementedException();
        }

        public void UpdateAvatar(MFLJson.JsonObject obj)
        {
            throw new NotImplementedException();
        }

        public MFLJson.JsonString GetMyPwdQuestion()
        {
            throw new NotImplementedException();
        }

        public void UpdatePwdQuestion(string pwdQuestion, string pwdAnswer, string pwdAnswer2)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(string oldPassword, string password1, string password2)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmail(string oldEmail, string email, string email2)
        {
            throw new NotImplementedException();
        }

        #endregion


        public MFLJson.JsonObject GetMyCourses(string page, string search)
        {
            return GCSearch.GetAllCourses(page, "", "", search);
        }


        public MFLJson.JsonObject GetMessages(string p, string p_2, bool p_3)
        {
            throw new NotImplementedException();
        }


        MFLJson.JsonArray GCVisitor.GetMessages(string startDate, string stopDate, bool showOld)
        {
            throw new NotImplementedException();
        }




        MFLJson.JsonObject GCVisitor.GetMyMsgSetting()
        {
            throw new NotImplementedException();
        }


        public void UpdateMailing(int msgType, int msgClock)
        {
            throw new NotImplementedException();
        }
    }
}