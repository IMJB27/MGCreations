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

        [HttpPost]
        public ActionResult AddToCart(int p_id, string Quantity)
        {
            product Product = db.products.Find(p_id);
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("User_Login", "User");
            }
            else
            {
                try
                {
                    int userid = Convert.ToInt32(Session["User_ID"].ToString());
                    cart Cart = new cart();
                    if ((db.carts.Any(x => x.User_ID == userid && x.Product_ID == p_id && x.Cart_Status.Equals(1))))
                    {
                        Cart = db.carts.Single(x => x.User_ID == userid && x.Product_ID == p_id);
                        Cart.Cart_Quantity = Cart.Cart_Quantity + Convert.ToInt32(Quantity);
                        Cart.Cart_Total = Product.Product_Price * Cart.Cart_Quantity;

                        db.Entry(Cart).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Cart.User_ID = Convert.ToInt32(Session["User_ID"].ToString());
                        Cart.Product_ID = Product.Product_ID;
                        Cart.Cart_Quantity = Convert.ToInt32(Quantity);
                        Cart.Cart_Total = Product.Product_Price * Convert.ToInt32(Quantity);
                        Cart.Cart_Status = 1;

                        db.carts.Add(Cart);
                    }
                    db.SaveChanges();
                    Response.Write("<script>alert('" + Product.Product_Name + " Added to Cart!');</script>");

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
                return RedirectToAction("View_Product_Details", "Product", new { p_id = Product.Product_ID });
            }
        }

        [HttpGet]
        public ActionResult View_Cart()
        {

             List<cart> Cart = new List<cart>();
            if(Session["User_ID"] == null)
            {  
                return RedirectToAction("User_Login","User");
            }
            else
            { 
                int userid = Convert.ToInt32(Session["User_ID"].ToString());
                Cart = db.carts.Where(x => x.User_ID.Equals(userid) && x.Cart_Status.Equals(1)).ToList();
                return View(Cart);
            }    
        }

        [HttpPost]
        public ActionResult Update_Cart(int c_id, int quantity)
        {
            if(quantity<=0)
            {
                return RedirectToAction("Delete_Cart","Cart", new { c_id = c_id } );
            }

            cart Cart = db.carts.Where(x => x.Cart_ID.Equals(c_id)).SingleOrDefault();
            product Product = db.products.Where(x => x.Product_ID.Equals(Cart.Product_ID)).SingleOrDefault();

            Cart.Cart_Quantity = quantity;
            Cart.Cart_Total = Product.Product_Price * Cart.Cart_Quantity;
            db.Entry(Cart).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var jsonresult = new
            {
                Cart_ID = Cart.Cart_ID,
                User_ID = Cart.User_ID,
                Product_ID = Cart.Product_ID,
                Product_Quantity = Cart.Cart_Quantity,
                Cart_Total = Cart.Cart_Total,
                Subtotal = GetSubTotal(Cart.User_ID)
            };
            return Json(jsonresult);
        }

        [HttpGet]
        public ActionResult Delete_Cart(int? c_id)
        {
            try
            {
                cart Cart = db.carts.Find(c_id);
                db.carts.Remove(Cart);
                db.SaveChanges();
                return Redirect("View_Cart");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return Redirect("View_Cart");
            }
            
        }

        [HttpGet]
        public decimal GetSubTotal(int UserID)
        {
            //int UserId = Convert.ToInt32(Session["User_ID"].ToString());
            List<cart> CartList = db.carts.Where(x => x.User_ID.Equals(UserID) && x.Cart_Status.Equals(1)).ToList();
            decimal SubTotal = 0;
            foreach(var item in CartList)
            {
                SubTotal = SubTotal + (item.product.Product_Price * item.Cart_Quantity);
            }
            return SubTotal;
        }
    }
}