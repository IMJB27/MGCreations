using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;


namespace MGCreations.Controllers
{
    public class ProductDetailsController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        // GET: ProductDetails
        [HttpGet]
        public ActionResult Product_Details_List()
        {
            return View(db.products.ToList());
        } 
        
        [HttpGet]
        public ActionResult View_Product_Details()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Product_Details()
        {
            product product_Details = new product();
            using (db)
            {
                //product_Details.Category_List = db.product_category.ToList<product_category>();
            }
                return View(product_Details);
        }

        [HttpPost]
        public ActionResult Add_Product_Details(product product_Details, product_images product_Images)
        {

            return View();
        }

        [HttpGet]
        public ActionResult Update_Product_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update_Product_Details(product product_Details)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete_Product_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete_Product_Details(product product_Details)
        {
            return View();
        }
    }
}