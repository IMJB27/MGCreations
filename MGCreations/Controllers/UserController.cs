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
                    Session["User_Name"] = u.User_Username.ToString();
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
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Customer"))
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
            if (Session["User_ID"] == null)
            {
                u.User_Type = "Customer";
            }
            else
            {
                u.User_Type = User.User_Type;
            }
            db.users.Add(u);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "New"+ u.User_Type + "Created Successfully";

            return View();
        }

        [HttpGet]
        public ActionResult User_Logout()
        {
            Session["User_ID"] = null;
            Session["User_Name"] = null;
            Session["User_Type"] = null;
            FormsAuthentication.SignOut();
            return Redirect("~/Home/Index");
        }


        [HttpGet]
     
        public ActionResult User_List()
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                return View(db.users.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult User_Details(int? u_id)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login");
            }
            else
            {
                if (u_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                
                user user1 = db.users.Find(u_id);

                if (user1 == null)
                {
                    return HttpNotFound();
                }
                else if ((Session["User_Type"].ToString() == "Customer") && (Session["User_ID"].ToString() != u_id.ToString()))
                {
                    return HttpNotFound();
                }
                else 
                {
                    
                }
                return View(user1);
            }
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