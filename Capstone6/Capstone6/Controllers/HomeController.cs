using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone6.Models;

namespace Capstone6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Error = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Registration()
        {
            return View();

        }
        public ActionResult RegisterNewUser(User newUser)
        {
            TaskListEntities ORM = new TaskListEntities();

            ORM.Users.Add(newUser);

            ORM.SaveChanges();
            return View("Index");
        }
        public ActionResult SignIn(string UserName, string Password)
        {
            TaskListEntities ORM = new TaskListEntities();
            User currentUser = ORM.Users.Find(UserName);

            if (currentUser == null)
            {
                ViewBag.Error = "Username does not exist. are you registered?";
                return View("Index");
            }
            else if (currentUser.Password != Password)
            {
                ViewBag.Error = "Incorrect Password";
                return View("Index");

            }
            { 
            TempData.Add("AuthorizedUser", currentUser);
            TempData["AuthorizedUser"] = currentUser;

            return RedirectToAction("UserTask");
            }
        }
        public ActionResult UserTask()
        {
            TaskListEntities ORM = new TaskListEntities();

            User currentUser = (User)TempData["AuthorizedUser"];

            ViewBag.UserTasks = ORM.Users.Find(currentUser.UserName).Tasks;
            ViewBag.AuthorizedUser = currentUser;
            TempData["AuthorizedUser"] = currentUser;
            return View();

        }
        public ActionResult AddTask()
        {
            ViewBag.AuthorizedUser = TempData["AuthorizedUser"];
            TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
            return View();
        }
        public ActionResult AddNewTask(Task t)
        {
            ViewBag.AuthorizedUser = TempData["AuthorizedUser"];
            //1 create the ORM
            TaskListEntities ORM = new TaskListEntities();
            //2 Validation
            if (ModelState.IsValid)
            {
                //3 add the new object to the Task list
                ORM.Tasks.Add(t);
                //4 save changes to database
                ORM.SaveChanges();
                ViewBag.Message = "Task Added Succesfully";
                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }
            //return RedirectToAction("About");
            else
            {
                ViewBag.ErrorMessage = "Item Not Added";
                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }

        }
        public ActionResult DeleteTask(string TaskName)
        {
            ViewBag.AuthorizedUser = TempData["AuthorizedUser"];
            //1 Create ORM
            TaskListEntities ORM = new TaskListEntities();
            //2 Locate Task
            Task Found = ORM.Tasks.Find(TaskName);
            //3 Validate
            if (Found != null)
            {
                //4 Delete Task
                ORM.Tasks.Remove(Found);
                //5 Save to database
                ORM.SaveChanges();
                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }
            else
            {
                ViewBag.ErrorMessage = "Task Not Deleted";
                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }
        }
        public ActionResult EditTask(string TaskName)
        {

            TaskListEntities ORM = new TaskListEntities();

            Task toUpdate = ORM.Tasks.Find(TaskName);

            return View(toUpdate);

        }
        public ActionResult ChangeStatus(string Status,string TaskName)
        {
            ViewBag.AuthorizedUser = TempData["AuthorizedUser"];
            TaskListEntities ORM = new TaskListEntities();

            Task OldTaskRecord = ORM.Tasks.Find(TaskName);

            if (OldTaskRecord != null)
            {
                OldTaskRecord.Status = "Complete";
                ORM.Entry(OldTaskRecord).State = System.Data.Entity.EntityState.Modified;
                ORM.SaveChanges();

                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }
            else
            {
                ViewBag.ErrorMessage = "Task Not Updated";
                return RedirectToAction("UserTask");
            }

        }
        public ActionResult SaveEditedTask(Task editedTask)
        {
            ViewBag.AuthorizedUser = TempData["AuthorizedUser"];
            //1 ORM
            TaskListEntities ORM = new TaskListEntities();
        //2 Locate customer to update
        Task OldTaskRecord = ORM.Tasks.Find(editedTask.TaskName);
            if (OldTaskRecord != null && ModelState.IsValid)
            {
                //3 Update the Customer
                OldTaskRecord.TaskName = editedTask.TaskName;
                OldTaskRecord.TaskDescription = editedTask.TaskDescription;
                OldTaskRecord.DueDate = editedTask.DueDate;
                OldTaskRecord.Status = editedTask.Status;
                //4 This line tells ORM to update
                ORM.Entry(OldTaskRecord).State = System.Data.Entity.EntityState.Modified;
                //5 Save to database
                ORM.SaveChanges();
                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }
            else
            {
                ViewBag.ErrorMessage = "Task Not Updated";
                TempData["AuthorizedUser"] = TempData["AuthorizedUser"];
                return RedirectToAction("UserTask");
            }
        }
        public ActionResult Logout()
        {
            TempData = null;
            return RedirectToAction("Index");
        }
    }
    
}