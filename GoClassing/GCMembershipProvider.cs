using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
namespace GoClassing
{
    public class GCMembershipProvider:System.Web.Security.MembershipProvider
    {

        gc_localtestEntities d = new gc_localtestEntities();
        public override string ApplicationName
        {
            get
            {
                return "GoClassing";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var u=d.gc_user.First(tu => tu.username == username && tu.password == oldPassword);
            u.password = newPassword;
            u.timeLastPasswordChange = DateTime.Now;
            d.SaveChanges();
            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            var u = d.gc_user.First(tu => tu.username == username && tu.password == password);
            u.pwdAnswer = newPasswordAnswer;
            u.pwdQuestion = newPasswordQuestion;
            d.SaveChanges();
            return true;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (this.d.gc_user.Any(tu => tu.username == username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }
            {
                var ar = new[] { '.', '_', '-' };
                if (!username.All(tc => char.IsLetterOrDigit(tc) || ar.Contains(tc)))
                {
                    status = MembershipCreateStatus.InvalidUserName;
                    return null;
                }
            }
            {
                var ar = new[] { '.', '_', '-', '@' };
                if (!email.All(tc => char.IsLetterOrDigit(tc) || ar.Contains(tc)) || email.Count(tc => tc == '@') != 1)
                {
                    status = MembershipCreateStatus.InvalidEmail;
                    return null;
                }
            }
            {
                if (password.Length < 6)
                {

                    status = MembershipCreateStatus.InvalidPassword;
                    return null;
                }
            }

            if (this.d.gc_user.Any(tu => tu.email == email))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var u = new gc_user()
            {
                username = username.ToLower(),
                originalusername=username,
                password = password,
                email = email,
                pwdQuestion = passwordQuestion,
                pwdAnswer = passwordAnswer,
                timeCreation = DateTime.Now,
                timeLastActivity = DateTime.Now,
                timeLastLogon = DateTime.Now,
                timeLastPasswordChange = DateTime.Now,
                admin=false,
                coop="",
                flagsettings=0,
                mailverified=false,
                spaceTotal=2000000000,
                truename="[匿名用户]",
                spaceUsed=0,
                msgType=2,
                msgClock=8
            };
            d.gc_user.AddObject(u);
            
            d.gc_feed.AddObject(new gc_feed()
            {
                ftype = "notify",
                gc_user = u,
                time = DateTime.Now,
                text = Internal.UI.FeedsWelcomeToGoClassing
            });
            var mu = this.GetUser(u);
            d.SaveChanges();
            status = MembershipCreateStatus.Success;
            return mu;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (!deleteAllRelatedData)
            {
                throw (new NotImplementedException());
            }
            d.gc_user.DeleteObject(d.gc_user.First(tu => tu.username == username));
            d.SaveChanges();
            return true;
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return true; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var c = new MembershipUserCollection();
            var us = d.gc_user.Where(tu => tu.email == emailToMatch).OrderBy(tu => tu.id).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var u in us)
            {
                c.Add(this.GetUser(u));
            }
            totalRecords = c.Count;
            return c;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var c = new MembershipUserCollection();
            var us = d.gc_user.Where(tu => tu.username == usernameToMatch).OrderBy(tu => tu.id).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var u in us)
            {
                c.Add(this.GetUser(u));
            }
            totalRecords = c.Count;
            return c;
            
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var c = new MembershipUserCollection();
            var us = d.gc_user.OrderBy(tu => tu.username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var u in us)
            {
                c.Add(this.GetUser(u));
            }
            totalRecords = c.Count;
            return c;
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            return d.gc_user.First(tu => tu.username == username && tu.pwdAnswer == answer).password;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return this.GetUser(d.gc_user.First(tu => tu.username == username));
        }
        MembershipUser GetUser(gc_user u)
        {
            return new MembershipUser("GCMembershipProvider",
                    u.originalusername,
                    u.id,
                    u.email,
                    u.pwdQuestion,
                    "",
                    true,
                    false,
                    u.timeCreation, u.timeLastLogon, u.timeLastActivity, u.timeLastPasswordChange, DateTime.Now);
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return this.GetUser(d.gc_user.First(tu => tu.id == (int)providerUserKey));
        }

        public override string GetUserNameByEmail(string email)
        {
            return email;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            var u = d.gc_user.First(tu => tu.id == (int)user.ProviderUserKey);
            u.email = user.Email;
            u.timeCreation = user.CreationDate;
            u.timeLastActivity = user.LastActivityDate;
            u.timeLastLogon = user.LastLoginDate;
            u.timeLastPasswordChange = user.LastPasswordChangedDate;
            d.SaveChanges();
        }

        public override bool ValidateUser(string username, string password)
        {
            return d.gc_user.Any(tu => tu.username == username && tu.password == password);
        }
    }
}