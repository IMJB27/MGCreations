using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;


namespace MGCreations.Controllers
{
    public class ProductController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        // GET: ProductDetails
        [HttpGet]
        public ActionResult Product_List()
        {
            return View(db.products.ToList());
        } 
        
        [HttpGet]
        public ActionResult View_Product_Details()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Product()
        {
            product product_Details = new product();
            Get_Categories();
                return View();
        }

        [HttpPost]
        public ActionResult Add_Product(product product_Details)
        {
            product p = new product();

            p.Product_Name = product_Details.Product_Name;
            p.Category_ID = product_Details.Category_ID;
            p.Product_Description = product_Details.Product_Description;
            p.Product_Quantity = product_Details.Product_Quantity;
            p.Product_Price = product_Details.Product_Price;
            p.isPersonalisable = product_Details.isPersonalisable;

         
            db.products.Add(p);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "New Product Created Successfully";
           //Get_Categories();
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
        public ActionResult Delete_Product()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete_Product(product product_Details)
        {
            return View();
        }


        private void Get_Categories()
        {
            using (db)
            {
                List<product_category> Category_List = db.product_category.ToList<product_category>();
              TempData["Category_List"] = new SelectList(Category_List, "Category_ID", "Category_Name");
            }
        }
    }
}