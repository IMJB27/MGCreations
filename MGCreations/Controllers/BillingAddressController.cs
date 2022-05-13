using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;

namespace MGCreations.Controllers
{
 
    public class BillingAddressController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();

        [HttpGet]
        public ActionResult Add_Billing_Address()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Billing_Address(billing_address ba)
        {
            try
            {
                if (Session["User_ID"] == null)
                {
                    return RedirectToAction("User_Login", "User");
                }
                else
                {
                    billing_address billing_Address = new billing_address();

                    billing_Address.User_ID = Convert.ToInt32(Session["User_ID"].ToString());
                    billing_Address.Billing_Address_Line1 = ba.Billing_Address_Line1;
                    billing_Address.Billing_Address_Line2 = ba.Billing_Address_Line2;
                    billing_Address.Billing_Address_City = ba.Billing_Address_City;
                    billing_Address.Billing_Address_County = ba.Billing_Address_County;
                    billing_Address.Billing_Address_Country = ba.Billing_Address_Country;
                    billing_Address.Billing_Address_Postcode = ba.Billing_Address_Postcode;
                    

                    db.billing_address.Add(billing_Address);
                    db.SaveChanges();
                    TempData["BillingAddressID"] = billing_Address.Billing_Address_ID;
                    TempData.Keep();
                    ModelState.Clear();
                    return RedirectToAction("Place_Order", "Order");

                }
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        [HttpGet]
        public ActionResult Billing_Address_List()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                List<billing_address> customer_Addresses = db.billing_address.Where(x => x.User_ID.Equals(userid)).ToList();
                if (customer_Addresses.Count() > 0)
                {
                    return View(customer_Addresses);
                }
                else
                {
                    return RedirectToAction("Add_Billing_Address", "BillingAddress");
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select_Billing_Address(int? ba_id)
        {
            billing_address billing_Address = db.billing_address.Find(ba_id);
            TempData["BillingAddress"] = billing_Address;
            TempData["BillingAddressID"] = billing_Address.Billing_Address_ID;
            TempData.Keep();
            return RedirectToAction("View_Cart", "Cart");
        }
    }
}