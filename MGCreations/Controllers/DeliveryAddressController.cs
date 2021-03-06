using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;

namespace MGCreations.Controllers
{
    public class DeliveryAddressController : Controller
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
        public ActionResult Add_Delivery_Address(delivery_address da)
        {
            try
            {
                if(Session["User_ID"] == null)
                {
                    return RedirectToAction("User_Login", "User");
                }
                else
                {
                    delivery_address delivery_Address = new delivery_address();

                    delivery_Address.User_ID = Convert.ToInt32(Session["User_ID"].ToString());
                    delivery_Address.Delivery_Address_Line1 = da.Delivery_Address_Line1;
                    delivery_Address.Delivery_Address_Line2 = da.Delivery_Address_Line2;
                    delivery_Address.Delivery_Address_City = da.Delivery_Address_City;
                    delivery_Address.Delivery_Address_County = da.Delivery_Address_County;
                    delivery_Address.Delivery_Address_Country = da.Delivery_Address_Country;
                    delivery_Address.Delivery_Address_Postcode = da.Delivery_Address_Postcode;

                    db.delivery_address.Add(delivery_Address);
                    TempData["DeliveryAddressID"] = delivery_Address.Delivery_Address_ID;
                    TempData["DeliveryAddress"] = delivery_Address;
                    TempData.Keep();
                    db.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("View_Cart", "Cart");
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
                List<delivery_address> delivery_Addresses = db.delivery_address.Where(x => x.User_ID.Equals(userid)).ToList();
                if (delivery_Addresses.Count() > 0)
                {
                    return View(delivery_Addresses);
                }
                else
                {
                    return RedirectToAction("Add_Delivery_Address", "DeliveryAddress");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select_Delivery_Address(int? da_id)
        {
            delivery_address delivery_Address = db.delivery_address.Find(da_id);
            TempData["DeliveryAddress"] = delivery_Address;
            TempData["DeliveryAddressID"] = delivery_Address.Delivery_Address_ID;
            TempData.Keep();
            return RedirectToAction("View_Cart", "Cart");
        }

        [HttpPost]
        public ActionResult GetDeliveryAddress()
        {
           delivery_address Address = TempData["DeliveryAddress"] as delivery_address;
            TempData["DeliveryAddressID"] = Address.Delivery_Address_ID;
           TempData.Keep();
    
            var jsonresult = new
            {
                AddressLine1 = Address.Delivery_Address_Line1,
                AddressLine2 = Address.Delivery_Address_Line2,
                City = Address.Delivery_Address_City,
                County = Address.Delivery_Address_County,
                Country = Address.Delivery_Address_Country,
                Postcode = Address.Delivery_Address_Postcode
            };

            return Json(jsonresult);
        }

    }
}