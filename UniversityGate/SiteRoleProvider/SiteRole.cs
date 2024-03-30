using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using UniversityGate.Models;

namespace UniversityGate.SiteRoleProvider
{
    public class SiteRole : RoleProvider
    {

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            UniGateDatabaseEntities db = new UniGateDatabaseEntities();

            string role3 = db.Students
                .Where(m => m.studentID == username)
                .Select(m => m.Role.RoleName)
                .FirstOrDefault();

            string role2 = db.Professors
                .Where(m => m.professorID == username)
                .Select(m => m.Role.RoleName)
                .FirstOrDefault();

            string role1 = "Admin";

            if (role3 != null)
            {
                string[] result = { role3 };
                return result;
            }
            else if (role2 != null)
            {
                string[] result = { role2 };
                return result;
            }
            else if (username == "admin")
            {
                string[] result = { role1 };
                return result;
            }
            else {
                return null;
            }
            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}