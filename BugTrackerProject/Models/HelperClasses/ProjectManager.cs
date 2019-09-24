using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerProject.Models.HelperClasses
{
    public class ProjectManager
    {
        ApplicationDbContext db;
        public ProjectManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        //this will give the project that was asked
        public Project CheckProjectId(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project != null)
            {
                return project;
            }
            return null;
        }

        //this will bring the given user
        public ApplicationUser CheckUserId(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    return user;
                }
            }   
            return null;
        }

        //assign user to the project
        //admins and PM's must assign projects to users
        public bool AssignUserToProject(string userId, int projectId)
        {
            var user = CheckUserId(userId);
            var project = CheckProjectId(projectId);
            //ApplicationDbContext ap = new ApplicationDbContext();
            //ap.ProjectUsers = project;
            
            if (user != null && project != null)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return true;

            }
            return false;
        }

        //Remove user from Project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            var user = CheckUserId(userId);
            var project = CheckProjectId(projectId);

            if (user != null && project != null)
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                return true;
            }
            return false;
        }


        //show the list of all the projects
        //only Admins and PM's can view this list
        public List<Project> GetAllProjects()
        {
            var allProjects = db.Projects.ToList();
            return allProjects;
        }

        //show the list of projects, users are assigned to
        public ICollection<ProjectUser> GetUserProjects(string userId)
        {
            var user = CheckUserId(userId);
            if (user != null)
            {
                return user.ProjectUsers;
            }
            return null;
        }
    }
}