using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;
namespace GoClassing
{
    public class GCRoleProvider:System.Web.Security.RoleProvider
    {
        gc_localtestEntities d = new gc_localtestEntities();


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var role in roleNames)
            {
                if (role != "管理员")
                {
                    throw new NotImplementedException();
                }
                var us=d.gc_user.Where(tu => usernames.Contains(tu.username));
                foreach (var u in us)
                {
                    u.admin = true;
                }
            }
            d.SaveChanges();
        }

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
        public override string Name
        {
            get
            {
                return "GCRoleProvider";
            }
        }

        public override void CreateRole(string roleName)
        {
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {

            if (roleName != "管理员")
            {
                throw new NotImplementedException();
            }
            return d.gc_user.Where(tu => tu.admin && tu.username == usernameToMatch).Select(tu => tu.username).ToArray();
        }

        public override string[] GetAllRoles()
        {
            return new[] { "管理员" };
        }

        public override string[] GetRolesForUser(string username)
        {
            return d.gc_user.First(tu => tu.username == username).admin ? new[] { "管理员" } : new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {

            if (roleName != "管理员")
            {
                throw new NotImplementedException();
            }
            return d.gc_user.Where(tu => tu.admin).Select(tu => tu.username).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                return d.gc_user.First(tu => tu.username == username).admin;
            }
            catch
            {
                return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {

            foreach (var role in roleNames)
            {
                if (role != "管理员")
                {
                    throw new NotImplementedException();
                }
                var us = d.gc_user.Where(tu => usernames.Contains(tu.username));
                foreach (var u in us)
                {
                    u.admin = false;
                }
            }
            d.SaveChanges();
        }

        public override bool RoleExists(string roleName)
        {
            return roleName == "管理员";
        }
    }
}