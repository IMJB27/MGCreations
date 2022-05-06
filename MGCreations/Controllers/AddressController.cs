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
            return View();
        }

        [HttpPost]
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
        public ActionResult Add_Billing_Address()
        {
            return View();
        }

        [HttpPost]
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

    }
}