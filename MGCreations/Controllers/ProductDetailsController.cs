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
            return View();
        } 
        
        [HttpGet]
        public ActionResult View_Product_Details()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Product_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_Product_Details(product_details product_Details)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update_Product_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update_Product_Details(product_details product_Details)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete_Product_Details()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete_Product_Details(product_details product_Details)
        {
            return View();
        }
    }
}