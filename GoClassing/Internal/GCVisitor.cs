using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MFLJson;
using System.Web;
namespace GoClassing.Internal
{
    public interface GCVisitor
    {
        JsonArray GetFeeds(int startId, string ftypes);
        GCCourse VisitCourse(int courseId);
        GCUser VisitUser(string username);
        JsonObject GetMyCourses(string page, string ctypes1, string ctypes2);
        JsonObject GetMyCourses(string page, string search);
        JsonObject GetMyPaticipatedCourses(string page, string ctypes1, string ctypes2);
        JsonArray GetMyCtypes1();
        JsonArray GetMyCtypes2(string ctypes1);
        JsonObject GetMyFriends(string pagestr, string search);
        JsonObject GetMyFriends(string pagestr, string search,bool online);
        int CreateCourse(string name, string ctype1, string ctype2);
        void UpdataProfiles(HttpRequest request);
        void UpdateSettings(HttpRequest request);
        bool Salut(string p);
        void Deny(string username);
        void UpdataProfile(HttpRequest httpRequest);
        void UpdateAvatar(JsonObject obj);
        JsonArray GetSaluttingUsers();
        JsonString GetMyPwdQuestion();
        void UpdatePwdQuestion(string pwdQuestion, string pwdAnswer, string pwdAnswer2);
        void UpdatePassword(string oldPassword, string password1, string password2);
        void UpdateEmail( string oldEmail, string email,string email2);
        JsonArray GetMessages(string startDate, string stopDate, bool showOld);

        JsonObject GetMyMsgSetting();

        void UpdateMailing(int msgType, int msgClock);
    }
}
