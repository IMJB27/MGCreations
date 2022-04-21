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
    public class CartController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
       
        // GET: Cart
        [HttpGet]
        public ActionResult View_Cart()
        {
             List<cart> Cart = new List<cart>();
            if(Session["User_ID"] == null)
            {
               
                return View(Cart);
            }
            else
            {
               
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                Cart = db.carts.Where(x => x.User_ID.Equals(userid)).ToList();

                return View(Cart);
            }
         
        }

        [HttpPost]
        public ActionResult Checkout(cart Cart)
        {
            int userid = Convert.ToInt32(Session["User_ID"].ToString());
            List<cart> Cart_List = new List<cart>();

            Cart_List = db.carts.Where(x => x.User_ID.Equals(userid)).ToList();

            TempData["Order"] = Cart_List;
            return RedirectToAction("View_Order", "Order");
        }

        [HttpPost]
        public ActionResult Update_Cart(int id, int quantity1)
        {
          
            cart Cart = db.carts.Where(x => x.Cart_ID.Equals(id)).SingleOrDefault();
            product Product = db.products.Where(x => x.Product_ID.Equals(Cart.Product_ID)).SingleOrDefault();

            Cart.Product_Quantity = quantity1;
            Cart.Cart_Total = Product.Product_Price * Cart.Product_Quantity;
           // db.Entry(cart).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var jsonresult = new cart
            {
                Cart_ID = Cart.Cart_ID,
                User_ID = Cart.User_ID,
                Product_ID = Cart.Product_ID,              
                Product_Quantity = Cart.Product_Quantity,
                Cart_Total = Cart.Cart_Total
            };
            return Json(jsonresult);
        }
    }
}