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
    public class ProductCategoryController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        // GET: ProductCategory
        [HttpGet]
        public ActionResult Add_Product_Category()
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Product_Category(product_category product_Category)
        {
            product_category pc = new product_category();
            pc.Category_Name = product_Category.Category_Name;
            pc.is_Active = 1;
            db.product_category.Add(pc);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "New Product Category is Added Successfully";
            return View();
        }

        [HttpGet]
        public ActionResult Product_Category_List()
        {
            return View(db.product_category.ToList());
        }

        [HttpGet]
        public ActionResult Product_Category_Details(int? pc_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (pc_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product_category pc = db.product_category.Find(pc_id);
                if (pc == null)
                {
                    return HttpNotFound();
                }

                return View(pc);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public ActionResult Update_Product_Category(int? pc_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (pc_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product_category pc = db.product_category.Find(pc_id);
                if (pc == null)
                {
                    return HttpNotFound();
                }

                return View(pc);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
        }

        [HttpPost]
        public ActionResult Update_Product_Category(product_category product_Category)
        {
            product_category pc = new product_category();
            pc = db.product_category.Find(product_Category.Category_ID);
            pc.Category_Name = product_Category.Category_Name;
            pc.is_Active = product_Category.is_Active;
            db.Entry(pc).State = System.Data.Entity.EntityState.Modified;
            if(pc.is_Active == 0)
            {
                List<product> products = db.products.Where(x => x.Category_ID.Equals(pc.Category_ID)).ToList();
                foreach(var item in products)
                {
                    item.is_Active = 0;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Product_Category_List");
        }

        [HttpGet]
        public ActionResult Delete_Product_Category(int? pc_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (pc_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product_category pc = db.product_category.Find(pc_id);
                if (pc == null)
                {
                    return HttpNotFound();
                }

                return View(pc);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost, ActionName("Delete_Product_Category")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int pc_id)
        {
            product_category product_Category = db.product_category.Find(pc_id);
            product_Category.Category_Name = product_Category.Category_Name;
            product_Category.is_Active = 0;
            db.Entry(product_Category).State = System.Data.Entity.EntityState.Modified;
            if (product_Category.is_Active == 0)
            {
                List<product> products = db.products.Where(x => x.Category_ID.Equals(product_Category.Category_ID)).ToList();
                foreach (var item in products)
                {
                    item.is_Active = 0;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Product_Category_List");
        }

    }
}