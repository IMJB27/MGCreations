using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;
using MySql.Data;
using System.Net;
using System.Web.Security;
namespace MGCreations.Controllers
{
    public class UserController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        [HttpGet]
        public ActionResult User_Login()
        {
            if (Session["User_ID"] != null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User_Login(user User)
        {
            try 
            {
                user u = db.users.Where(x => x.User_Username == User.User_Username && x.User_Password == User.User_Password).SingleOrDefault();
                if (u != null)
                {
                    Session["User_ID"] = u.User_ID.ToString();
                    Session["User_Type"] = u.User_Type.ToString();
                    FormsAuthentication.SetAuthCookie(u.User_Username, false);
                    return Redirect("~/Home/Index");
                }
                else
                {
                    ViewBag.error = "Username or Password is Incorrect!";
                    return View();
                }
            }
            catch(Exception ex)
            {
                return ViewBag(ex);
            }
        }

        [HttpGet]
        public ActionResult Register_User()
        {
            if (Session["User_ID"] != null)
            {
                return Redirect("~/Home/Index");
            }
            else {
                ViewBag.DefaultValue = "Customer"; 
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register_User(user User)
        {
            user u = new user();
            u.User_Username = User.User_Username;
            u.User_Password = User.User_Password;
            u.User_FirstName = User.User_FirstName;
            u.User_LastName = User.User_LastName;
            u.User_Email = User.User_Email;
            u.User_ContactNo = User.User_ContactNo;
            u.User_DOB = User.User_DOB;
            u.User_Type = User.User_Type;

            db.users.Add(u);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "New"+ u.User_Type + "Created Successfully";

            return View();
        }

        [HttpGet]
        public ActionResult User_Logout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User_Logout(user User)
        {
            return View();
        }

        [HttpGet]
        public ActionResult User_List()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User_List(user User)
        {
            return View();
        }

        [HttpGet]
        public ActionResult View_User()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult View_User(user User)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update_User()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_User(user User)
        {
            return View();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_User()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_User(user User)
        {
            return View();
        }




    }
}