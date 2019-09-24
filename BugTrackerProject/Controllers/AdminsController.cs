using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerProject.Models;
using BugTrackerProject.Models.HelperClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerProject.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        RoleManager<IdentityRole> rolesManager;
        UserManager<ApplicationUser> usersManager;

        public AdminsController()
        {
            rolesManager = new RoleManager<IdentityRole>
          (new RoleStore<IdentityRole>(db));
            usersManager = new UserManager<ApplicationUser>
          (new UserStore<ApplicationUser>(db));
        }

        // GET: Admins
        public ActionResult Index()
        {
            return View();
        }

        //Assign Roles to Users from Admin
        public ActionResult AssignRoles()
        {
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "UserName");
            ViewBag.role = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AssignRoles(string userId, string role)
        {
            usersManager.AddToRole(userId.ToString(), role);
            return RedirectToAction("Index");
        }

        //unassign user from role
        public ActionResult RemoveUsers()
        {
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "UserName");
            ViewBag.role = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult RemoveUsers(string userId, string role)
        {
            UserRoleManager manager = new UserRoleManager(db);
            var result = manager.RemoveUserFromRole(userId, role);
            return View(result);
        }

        //admins and project managers need to have a list of all the projects
    }
}