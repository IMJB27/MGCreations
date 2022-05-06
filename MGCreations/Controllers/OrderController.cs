using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;
using MGCreations.Controllers;

namespace MGCreations.Controllers
{
    public class OrderController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        CartController cartController = new CartController();
        // GET: Order
        public ActionResult View_Order()
        {
            return View(db.orders.ToList());
        }

        [HttpGet]
        public ActionResult Place_Order()
        {
            int userid = Convert.ToInt32(Session["User_ID"].ToString());
            List<cart> Cart = new List<cart>();

            Cart = db.carts.Where(x => x.User_ID.Equals(userid) && x.Cart_Status.Equals(1)).ToList();

            foreach (var item in Cart)
            {
                order Order = new order();
                Order.User_ID = item.User_ID;
                Order.Cart_ID = item.Cart_ID;
                Order.Order_TotalAmount = cartController.GetSubTotal(userid);
                Order.Order_Status = "Order Placed";
                Order.Order_Date = System.DateTime.Today;
                db.orders.Add(Order);
                item.Cart_Status = 0;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("View_Order", "Order");
        }
    }
}