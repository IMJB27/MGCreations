using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;
using MySql.Data;
using System.Net;

namespace MGCreations.Controllers
{
    //  [Authorize]
    public class AdminController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();

        [HttpGet]
        // GET: Admin
        public ActionResult Admin_Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_Login(admin admin1)
        {
            admin a = db.admins.Where(x => x.Admin_Username == admin1.Admin_Username && x.Admin_Password == admin1.Admin_Password).SingleOrDefault();
            if (a != null)
            {
                Session["Admin_ID"] = a.Admin_Id.ToString();
                return RedirectToAction("Admin_List");
            }
            else
            {
                ViewBag.error = "Username or Password is Incorrect!";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Admin_Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_Register(admin admin1)
        {
            admin a = new admin();
            a.Admin_Username = admin1.Admin_Username;
            a.Admin_Password = admin1.Admin_Password;
            a.Admin_Name = admin1.Admin_Name;
            a.Admin_Surname = admin1.Admin_Surname;
            a.Admin_Email = admin1.Admin_Email;
            a.Admin_ContactNo = admin1.Admin_ContactNo;
            db.admins.Add(a);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "New Admin Created Successfully";

            return View();
        }

        [HttpGet]
        public ActionResult Admin_List()
        {
            return View(db.admins.ToList());
        }

        [HttpGet]
        public ActionResult Admin_Details(int? a_id)
        {
            if (a_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin1 = db.admins.Find(a_id);
            if (admin1 == null)
            {
                return HttpNotFound();
            }

            return View(admin1);
        }

        [HttpGet]
        public ActionResult Update_Admin_Details(int? a_id)
        {
            if (a_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin1 = db.admins.Find(a_id);
            if (admin1 == null)
            {
                return HttpNotFound();
            }

            return View(admin1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_Admin_Details(admin admin1)
        {
            admin a = new admin();
            a.Admin_Username = admin1.Admin_Username;
            a.Admin_Password = admin1.Admin_Password;
            a.Admin_Name = admin1.Admin_Name;
            a.Admin_Surname = admin1.Admin_Surname;
            a.Admin_Email = admin1.Admin_Email;
            a.Admin_ContactNo = admin1.Admin_ContactNo;
            db.Entry(admin1).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Admin_List");
        }

        [HttpGet]
        public ActionResult Delete_Admin(int? a_id)
        {
            if (a_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin1 = db.admins.Find(a_id);
            if (admin1 == null)
            {
                return HttpNotFound();
            }
            return View(admin1);
        }

        [HttpPost, ActionName("Delete_Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int a_id)
        {
            admin admin1 = db.admins.Find(a_id);
            db.admins.Remove(admin1);
            db.SaveChanges();
            return RedirectToAction("Admin_List");
        }

    }
}