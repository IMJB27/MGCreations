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
                List<String> UserType = new List<String>();

                UserType.Add("Admin");
                UserType.Add("Customer");

                TempData["UserType"] = new SelectList(UserType); //new SelectList(Category_List, "Category_ID", "Category_Name");

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register_User(user User)
        {
            try
            {
                string ErrorMessage = CheckData(User);
                if (ErrorMessage == "")
                {
                    user u = new user();
                    
                    u.User_Username = User.User_Username;
                    u.User_Password = User.User_Password;
                    u.Confirm_Password = User.Confirm_Password;
                    u.User_FirstName = ToUpperCase(User.User_FirstName);
                    u.User_LastName = ToUpperCase(User.User_LastName);
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
                    ViewBag.Errors = ErrorMessage;
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

        private string ToUpperCase(string str)
        {
            if(str.Equals(null))
            {
                return string.Empty;
            }
            return char.ToUpper(str[0]) + str.Substring(1);
        }
        
        private string CheckData(user User)
        {

            int countUserName = db.users.Where(x => x.User_Username.Equals(User.User_Username)).Count();
            
            int countEmail = db.users.Where(x => x.User_Email.Equals(User.User_Email)).Count();
            int countContactNo = db.users.Where(x => x.User_ContactNo.Equals(User.User_ContactNo)).Count();
            string ErrorMessage = "" ;

            if (get_age(User.User_DOB) < 16)
            {
                ErrorMessage = ErrorMessage + "Must be 16" + Environment.NewLine;
            }

            if (countUserName > 0)
            {
                ErrorMessage = ErrorMessage + "Username Already Exist" + Environment.NewLine;
            }
            if(countEmail > 0)
            {
                ErrorMessage = ErrorMessage  +"Email Already Exist" + Environment.NewLine;
            }
            if(countContactNo > 0)
            {
                ErrorMessage = ErrorMessage  + "Contact Number Already Exist";
            }
    
            return ErrorMessage;
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
                List<String> UserType = new List<String>();

                UserType.Add("Admin");
                UserType.Add("Customer");

                TempData["UserType"] = new SelectList(UserType); //new SelectList(Category_List, "Category_ID", "Category_Name");
                return View(user1);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_User(user User)
        {
            try
            {
                int UserID = Convert.ToInt32(Session["User_ID"].ToString());
          
                //if (db.users.Where(x => x.User_ID.Equals(UserID) && x.User_Password.Equals(User.User_Password)).Count() > 0)
                {
                    user u = db.users.Where(x => x.User_ID.Equals(User.User_ID)).SingleOrDefault();
                    u.User_Username = User.User_Username;
                    u.User_Password = u.User_Password;
                    u.Confirm_Password = u.User_Password;
                    u.User_FirstName = User.User_FirstName;
                    u.User_LastName = User.User_LastName;
                    u.User_Email = User.User_Email;
                    u.User_ContactNo = User.User_ContactNo;
                    u.User_DOB = User.User_DOB;
                    u.User_Type = User.User_Type;
                    db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("User_Details", new { u_id = User.User_ID });
                }
                //else
                //{
                //    return View();
                //}
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
                db.delivery_address.RemoveRange(db.delivery_address.Where(x => x.User_ID.Equals(User.User_ID)).ToList());
                db.billing_address.RemoveRange(db.billing_address.Where(x => x.User_ID.Equals(User.User_ID)).ToList());
                db.orders.RemoveRange(db.orders.Where(x => x.User_ID.Equals(User.User_ID)).ToList());
                db.carts.RemoveRange(db.carts.Where(x=>x.User_ID.Equals(User.User_ID)).ToList());
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

        [HttpGet]
        public int get_age(DateTime dob)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - dob.Month;
            int years = today.Year - dob.Year;

            if (today.Day < dob.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }
            return years;
        }

    }
}