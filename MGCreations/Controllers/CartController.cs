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
        public ActionResult View_Cart(cart Cart)
        {
            return View();
        }

        public List<cart> Update_Cart()
        {
            List<cart> Cart = new List<cart>();
            return Cart;
        }
    }
}