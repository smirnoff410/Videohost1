﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Videohost.Models;

namespace Videohost.Providers
{
    public class CustomRoleProvider : RoleProvider
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
            string[] roles = new string[] { };
            using (VideohostEntities db = new VideohostEntities())
            {
                User user = db.Users.FirstOrDefault(p => p.Email == username);
                if(user != null)
                {
                    Role userRole = db.Roles.Find(user.role);
                    if (userRole != null)
                        roles = new string[] { userRole.name };
                }
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;

            using(VideohostEntities db = new VideohostEntities())
            {
                User user = db.Users.FirstOrDefault(p => p.Email == username);
                if(user != null)
                {
                    Role userRole = db.Roles.Find(user.role);
                    if (userRole != null && userRole.name == roleName)
                        outputResult = false;
                }
            }
            return outputResult;
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