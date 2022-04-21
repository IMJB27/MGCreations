using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;

namespace MGCreations.Controllers
{
    public class OrderController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        // GET: Order
        public ActionResult View_Order()
        {
            order Order = new order();
            List<cart> Cart = TempData["Order"] as List<cart>;

            foreach(var items in Cart)
            {
                Order.User_ID = items.User_ID;
                Order.Cart_ID = items.Cart_ID;
            }
            return View();
        }
    }
}