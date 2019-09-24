using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerProject.Models.HelperClasses
{
    public class UserRoleManager
    {
        ApplicationDbContext db = new ApplicationDbContext();

        UserManager<ApplicationUser> usersManager;

        public UserRoleManager(ApplicationDbContext db)
        {
            this.db = db;
            usersManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        //checks if the user is in any role
        public bool IsUserInRole(string userId, string roleName)
        {
            return usersManager.IsInRole(userId, roleName);

        }

        //adds users to role
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = usersManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        //removes users from the role
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = usersManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        //lists all the user in role
        public ICollection<ApplicationUser>UsersInRole(string roleName)
        {
            var result = new List<ApplicationUser>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    result.Add(user);
                }
            }
            return result;
        }
    }
}