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
    public class CustomerController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();

        [HttpGet]
        // GET: Admin
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
                return RedirectToAction("Customers_List");
            }
            else
            {
                ViewBag.error = "Username or Password is Incorrect!";
                return View();
            }
        }
    }
}