using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;

namespace MGCreations.Controllers
{
    public class AddressController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();

       [HttpGet]
        public ActionResult Add_Delivery_Address()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Delivery_Address(customer_address ca)
        {
            try
            {
                if(Session["User_ID"] == null)
                {
                    return RedirectToAction("User_Login", "User");
                }
                else
                {
                    customer_address customer_Address = new customer_address();

                    customer_Address.User_ID = Convert.ToInt32(Session["User_ID"].ToString());
                    customer_Address.Address_Line1 = ca.Address_Line1;
                    customer_Address.Address_Line2 = ca.Address_Line2;
                    customer_Address.Address_City = ca.Address_City;
                    customer_Address.Address_County = ca.Address_County;
                    customer_Address.Address_Country = ca.Address_Country;
                    customer_Address.Address_Postcode = ca.Address_Postcode;
                    customer_Address.Address_Type = "Delivery";

                    db.customer_address.Add(customer_Address);
                    TempData["DeliveryAddress"] = customer_Address;
                    db.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Add_Billing_Address", "Address");
                    
                }
            }
            catch (Exception ex)
            {
                return View();
            }
          
        }

        [HttpGet]
        public ActionResult Delivery_Address_List()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                List<customer_address> customer_Addresses = db.customer_address.Where(x => x.User_ID.Equals(userid) && x.Address_Type.Equals("Delivery")).ToList();
                if (customer_Addresses.Count() > 0)
                {
                    return View(customer_Addresses);
                }
                else
                {
                    return RedirectToAction("Add_Delivery_Address", "Address");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select_Delivery_Address(int? da_id)
        {
            customer_address customer_Address = db.customer_address.Find(da_id);
            TempData["DeliveryAddress"] = customer_Address;
            return RedirectToAction("Billing_Address_List", "Address");
        }

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
        public ActionResult Add_Billing_Address(customer_address ca)
        {
            try
            {
                if (Session["User_ID"] == null)
                {
                    return RedirectToAction("User_Login", "User");
                }
                else
                {
                    customer_address customer_Address = new customer_address();

                    customer_Address.User_ID = Convert.ToInt32(Session["User_ID"].ToString());
                    customer_Address.Address_Line1 = ca.Address_Line1;
                    customer_Address.Address_Line2 = ca.Address_Line2;
                    customer_Address.Address_City = ca.Address_City;
                    customer_Address.Address_County = ca.Address_County;
                    customer_Address.Address_Country = ca.Address_Country;
                    customer_Address.Address_Postcode = ca.Address_Postcode;
                    customer_Address.Address_Type = "Billing";

                    db.customer_address.Add(customer_Address);
                    db.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Place_Order","Order");

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
                List<customer_address> customer_Addresses =  db.customer_address.Where(x => x.User_ID.Equals(userid) && x.Address_Type.Equals("Billing")).ToList();
                if(customer_Addresses.Count() > 0)
                {
                    return View(customer_Addresses);
                }
                else
                {
                    return RedirectToAction("Add_Billing_Address", "Address");
                }
                 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select_Billing_Address(int? ba_id)
        {
            customer_address customer_Address = db.customer_address.Find(ba_id);
            TempData["BillingAddress"] = customer_Address;
            return RedirectToAction("Place_Order", "Order");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDeliveryAddress()
        {
           customer_address Address = TempData["DeliveryAddress"] as customer_address;
            TempData.Keep();
            
           // int UserID = Convert.ToInt32(Session["User_ID"].ToString());
           // Address = db.customer_address.Where(x => x.User_ID.Equals(UserID)).SingleOrDefault();
            var jsonresult = new
            {
                AddressLine1 = Address.Address_Line1,
                AddressLine2 = Address.Address_Line2,
                City = Address.Address_City,
                County = Address.Address_County,
                Country = Address.Address_Country,
                Postcode = Address.Address_Postcode
            };

            return Json(jsonresult);
        }

    }
}