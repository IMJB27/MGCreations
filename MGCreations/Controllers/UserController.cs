using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;
using MySql.Data;
using System.Net;
using System.Web.Security;
using System.Data.Entity.Validation;

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
                HttpCookie usercookie = new HttpCookie("Users");

                    if (User.User_RememberMe == true)
                    {
                        usercookie["Username"] = User.User_Username;
                        usercookie["password"] = User.User_Password;
                        usercookie.Expires = DateTime.Now.AddMinutes(120);
                        HttpContext.Response.Cookies.Add(usercookie);
                    }
                    else
                    {
                        usercookie.Expires = DateTime.Now.AddMinutes(-1);
                        HttpContext.Response.Cookies.Add(usercookie);
                    }
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
            try
            {
                int count = db.users.Where(x => x.User_Username.Contains(User.User_Username)).Count();
                if (count == 0)
                {
                    user u = new user();
                    
                    u.User_Username = User.User_Username;
                    u.User_Password = User.User_Password;
                    u.Confirm_Password = User.Confirm_Password;
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
                    ViewBag.Success = "New " + u.User_Type + " Created Successfully";
                }
                else
                {
                    ViewBag.Error = "Username Already Exist";
                }
                return View();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
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
                return View(user1);
            }
        }

        [HttpGet]
        public ActionResult Update_User(int? u_id)
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
                return View(user1);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_User(user User)
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
            db.Entry(User).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("User_Details", new { u_id = User.User_ID });
        }

        [HttpGet]
        public ActionResult Delete_User(int? u_id)
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
                return View(user1);
            }
        }

        [HttpPost, ActionName("Delete_User")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int u_id)
        {

            user User = db.users.Find(u_id);
            int admin_Count = db.users.Where(x => x.User_Type == "Admin").Count();
           
            if ((User.User_Type == "Admin")&&(admin_Count <= 1))
            {
                ViewBag.AlertMessage = "User Cant be Deleted";
                return View(db.users.Find(u_id));
            }
            else
            {
                db.users.Remove(User);
                db.SaveChanges();
                if (Session["User_Type"].ToString() == "Customer")
                {
                    return RedirectToAction("User_Logout");
                }
                return Redirect("~/Home/Index");
            }
           
        }

        [HttpGet]
        public ActionResult Admin_Dashboard()
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_Dashboard(user user1)
        {
            return View();
        }


    }
}