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
    public class CustomerController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();

        [HttpGet]
        // GET: Customer
        public ActionResult Customer_Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customer_Login(customer_details customer)
        {
            customer_details c = db.customer_details.Where(x => x.Customer_Username == customer.Customer_Username && x.Customer_Password == customer.Customer_Password).SingleOrDefault();
            if (c != null)
            {
                Session["Customer_ID"] = c.Customer_ID.ToString();
                FormsAuthentication.SetAuthCookie(c.Customer_Username, false);
                return Redirect("~/Home/Index");
            }
            else
            {
                ViewBag.error = "Username or Password is Incorrect!";
                return View();
            }
        }
        
        [HttpGet]
        public ActionResult Customer_Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customer_Registration(customer_details customer)
        {
            customer_details c = new customer_details();
            c.Customer_Username = customer.Customer_Username;
            c.Customer_Password = customer.Customer_Password;
            c.Customer_Name = customer.Customer_Name;
            c.Customer_Surname = customer.Customer_Surname;
            c.Customer_DOB = customer.Customer_DOB;
            c.Customer_Email = customer.Customer_Email;
            c.Customer_ContactNo = customer.Customer_ContactNo;
            c.Customer_AddressLine1 = customer.Customer_AddressLine1;
            c.Customer_AddressLine2 = customer.Customer_AddressLine2;
            c.Customer_City = customer.Customer_City;
            c.Customer_County = customer.Customer_County;
            c.Customer_Country = customer.Customer_Country;
            c.Customer_PostCode = customer.Customer_PostCode;
            db.customer_details.Add(c);
            db.SaveChanges();
            ViewBag.Success = "New Customer Created Successfully";
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Customers_List()
        {
            return View(db.customer_details.ToList());
        }

        [HttpGet]
        public ActionResult Customer_Details(int? c_id)
        {
            if (c_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_details customer = db.customer_details.Find(c_id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public ActionResult Update_Customer_Details(int? c_id)
        {
            if (c_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_details customer = db.customer_details.Find(c_id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_Customer_Details(customer_details customer)
        {
            customer_details c = new customer_details();
            c.Customer_Username = customer.Customer_Username;
            c.Customer_Password = customer.Customer_Password;
            c.Customer_Name = customer.Customer_Name;
            c.Customer_Surname = customer.Customer_Surname;
            c.Customer_DOB = customer.Customer_DOB;
            c.Customer_Email = customer.Customer_Email;
            c.Customer_ContactNo = customer.Customer_ContactNo;
            c.Customer_AddressLine1 = customer.Customer_AddressLine1;
            c.Customer_AddressLine2 = customer.Customer_AddressLine2;
            c.Customer_City = customer.Customer_City;
            c.Customer_County = customer.Customer_County;
            c.Customer_Country = customer.Customer_Country;
            c.Customer_PostCode = customer.Customer_PostCode;
            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Customer_Details");
        }

        [HttpGet]
        public ActionResult Delete_Customer(int? c_id)
        {
            if (c_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_details customer = db.customer_details.Find(c_id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete_Customer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int c_id)
        {
            customer_details customer = db.customer_details.Find(c_id);
            db.customer_details.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Customers_List");
        }

        public ActionResult Customer_Logout()
        {
            Session["Customer_ID"] = null;
            FormsAuthentication.SignOut();
            return Redirect("~/Home/Index");
        }
    }
}