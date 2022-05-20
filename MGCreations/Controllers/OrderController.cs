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

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    Product.Product_Quantity = Product.Product_Quantity - item.Cart_Quantity;
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
                return RedirectToAction("Order_Confirmation");
            }
        }

        [HttpGet]
        public ActionResult Order_Confirmation()
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
                var Order = GetOrders().Where(x => x.Order.Order_Reference_No.Equals(TempData["OrderReference"]) && x.User.User_ID.Equals(userid)).ToList();
                TempData.Keep();
                TempData.Remove("DeliveryAddressID");
                TempData.Remove("DeliveryAddress");
                TempData.Remove("BillingAddress");
                TempData.Remove("BillingAddressID");
                return View(Order);
            }
        }

        [HttpGet]
        public ActionResult View_Order_List()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() != "Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var orders = GetOrders();
                return View(orders);
            }
        }

        [HttpGet]
        public ActionResult View_Order_Details(int? o_id)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                JoinModel Order = GetOrders().Where(x => x.Order.Order_ID.Equals(o_id)).SingleOrDefault();

                return View(Order);
            }
        }

        [HttpGet]
        public ActionResult User_Manage_Order(int? u_id)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                int UserID = Convert.ToInt32(Session["User_ID"].ToString());
                var Orders = GetOrders();
                List<JoinModel> User_Orders = Orders.Where(x => x.Order.User_ID.Equals(u_id) && x.Order.Order_Status != "Not Paid").ToList();
                return View(User_Orders);
            }
        }

        private List<JoinModel> GetOrders()
        {
            List<order> orders = db.orders.ToList();
            List<product> products = db.products.ToList();
            List<user> users = db.users.ToList();
            List<cart> carts = db.carts.ToList();
            List<delivery_address> delivery_Addresses = db.delivery_address.ToList();
            List<billing_address> billing_addressess = db.billing_address.ToList();

            var Order_Details = from o in orders
                       join u in users on o.User_ID equals u.User_ID into usertable
                       from u in usertable.ToList()
                       join da in delivery_Addresses on o.Delivery_Address_ID equals da.Delivery_Address_ID into deliveryaddresstable
                       from da in deliveryaddresstable.ToList()
                       join ba in billing_addressess on o.Billing_Address_ID equals ba.Billing_Address_ID into billingaddresstable
                       from ba in billingaddresstable.ToList()
                       join c in carts on o.Cart_ID equals c.Cart_ID into carttable
                       from c in carttable.ToList()
                       join p in products on c.Product_ID equals p.Product_ID into producttable
                       from p in producttable.ToList()
                       select new JoinModel
                       {
                           Order = o,
                           User = u,
                           delivery_Address = da,
                           billing_Address = ba,
                           Cart = c,
                           Product = p
                       };
            return Order_Details.ToList();
        }

        [HttpPost]
        public ActionResult Update_Order_Status(int? o_id, string Order_Status)
        {
            order Order = db.orders.Find(o_id);

            Order.Order_Status = Order_Status;
            db.Entry(Order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("View_Order_List", "Order");
        }

        [HttpGet]
        public ActionResult Delete_Order(int? o_id)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
  
            else
            {
                order Order = db.orders.Find(o_id);

                db.orders.Remove(Order);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}