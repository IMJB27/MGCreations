using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;
using MGCreations.Controllers;
using System.Net;

namespace MGCreations.Controllers
{
    public class OrderController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        CartController cartController = new CartController();
    
        [HttpGet]
        public ActionResult View_Order()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else if (TempData["OrderReference"] == null)
            {
                return HttpNotFound();
            }
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                string orderref = TempData["OrderReference"].ToString();
                TempData.Keep();
                List<order> orders = db.orders.Where(x => x.Order_Reference_No.Equals(orderref) && x.Order_Status.Equals("Paid")).ToList();
                //TempData.Remove("OrderReference");
                //TempData.Remove("DeliveryAddressID");
                //TempData.Remove("DeliveryAddress");
                //TempData.Remove("BillingAddress");
                //TempData.Remove("BillingAddressID");
                return View(orders);
            }
        }

        [HttpGet]
        public ActionResult Place_Order()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
           
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                var ordertotal = cartController.GetSubTotal(userid);

                List<cart> Cart = new List<cart>();

                Cart = db.carts.Where(x => x.User_ID.Equals(userid) && x.Cart_Status.Equals(1)).ToList();
                Random generator = new Random();
                String randomNum = null;
                do
                {

                    randomNum = generator.Next(0, 100000).ToString("000000");
                } while (db.orders.Where(x => x.Order_Reference_No == randomNum).Count() != 0);


                foreach (var item in Cart)
                {
                    order Order = new order();
                    Order.User_ID = item.User_ID;
                    Order.Cart_ID = item.Cart_ID;
                    Order.Delivery_Address_ID = Convert.ToInt32(TempData["DeliveryAddressID"].ToString());
                    Order.Billing_Address_ID = Convert.ToInt32(TempData["BillingAddressID"].ToString());
                    Order.Order_Reference_No = "MG" + randomNum;
                    Order.Order_TotalAmount = ordertotal;
                    Order.Order_Status = "Not Paid";
                    Order.Order_Date = System.DateTime.Now;
                    db.orders.Add(Order);

                    db.SaveChanges();
                }
                TempData["OrderReference"] = "MG" + randomNum;
                TempData.Keep();
                return RedirectToAction("PaymentWithPaypal", "PayPal");
            }
        }

        [HttpGet]
        public ActionResult Confirm_Order()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else if (TempData["OrderReference"] == null)
            {
                return HttpNotFound();
            }
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                List<cart> Cart = new List<cart>();

                Cart = db.carts.Where(x => x.User_ID.Equals(userid) && x.Cart_Status.Equals(1)).ToList();
                foreach (var item in Cart)
                {
                    item.Cart_Status = 0;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    product Product = db.products.Find(item.Product_ID);

                    Product.Product_Quantity = Product.Product_Quantity - item.Product_Quantity;
                    db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
                }

                string orderref = TempData["OrderReference"].ToString();
                List<order> Order = db.orders.Where(x => x.Order_Reference_No.Equals(orderref) && x.Order_Status.Equals("Not Paid")).ToList();
                foreach (var item in Order)
                {
                    item.Order_Status = "Paid";
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("View_Order");
            }
        }

        [HttpGet]
        public ActionResult View_Order_List()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(db.orders.ToList());
            }
        }

        [HttpGet]
        public ActionResult User_Manage_Order(int? u_id)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(db.orders.ToList());
            }
        }

    }
}