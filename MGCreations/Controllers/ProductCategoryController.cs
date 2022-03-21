﻿using System;
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Product_Category(product_category product_Category)
        {
            product_category pc = new product_category();
            pc.Category_Name = product_Category.Category_Name;
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

            [HttpGet]
        public ActionResult Update_Product_Category(int? pc_id)
        {
            if(pc_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product_category pc = db.product_category.Find(pc_id);
            if(pc == null)
            {
                return HttpNotFound();
            }

            return View(pc);
        }

        [HttpPost]
        public ActionResult Update_Product_Category(product_category product_Category)
        {
            product_category pc = new product_category();
            pc.Category_Name = product_Category.Category_Name;
            db.Entry(product_Category).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Product_Category_List");
        }

        [HttpGet]
        public ActionResult Delete_Product_Category(int? pc_id)
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

        [HttpPost, ActionName("Delete_Product_Category")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int pc_id)
        {

            product_category product_Category = db.product_category.Find(pc_id);
            db.product_category.Remove(product_Category);
            db.SaveChanges();
            return RedirectToAction("Product_Category_List");
        }

    }
}