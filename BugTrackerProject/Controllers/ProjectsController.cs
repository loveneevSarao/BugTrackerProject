using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerProject.Models;
using BugTrackerProject.Models.HelperClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerProject.Controllers
{
    public class ProjectsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        //[Authorize(Roles ="Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        //[Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Assign user to project
        public ActionResult AssignProjects()
        {
            ViewBag.userId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.projectId = new SelectList(db.Projects, "id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AssignProjects(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.ProjectUsers.Find(projectId);

            if (user.ProjectUsers == null)
                user.ProjectUsers = new List<ProjectUser>();
            user.ProjectUsers.Add(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //remove users from project
        public ActionResult RemoveUsersFromProjects()
        {
            ViewBag.userId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.projectId = new SelectList(db.Projects, "id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult RemoveUsersFromProjects(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.ProjectUsers.Find(projectId);

            if (user.ProjectUsers == null)
                user.ProjectUsers = new List<ProjectUser>();
            user.ProjectUsers.Add(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //show the list of userProjects
        //[Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        public ActionResult ShowUserProjects()
        {
            var userId = User.Identity.GetUserId();
            ProjectManager manager = new ProjectManager(db);
            var allProjectsForUsers = manager.GetUserProjects(userId).ToList();
            return View("Index", allProjectsForUsers);
        }

        //show all projects
        //only admin n pm is authenticated to it
        //[Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult GetAllProjects()
        {
            ProjectManager manager = new ProjectManager(db);
            var allProjects = manager.GetAllProjects().ToList();
            return View(allProjects);
        }
    }
}
