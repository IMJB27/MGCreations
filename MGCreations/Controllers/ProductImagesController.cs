using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGCreations.Models;
using MySql.Data;
using System.Net;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.IO;

namespace MGCreations.Controllers
{
    public class ProductImagesController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        public int? Product_ID = 0;
        // GET: ProductImages
        [HttpGet]
        public ActionResult Add_Product_Images(int? p_id)
        {

            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (p_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Product_ID = p_id;
                Get_Product(p_id);
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Product_Images(product_images product_Images, HttpPostedFileBase Image_Path)
        {
            product_images pi = new product_images();
            if (ModelState.IsValid)
            {
                try
                {
                    string filePath = Upload_Image(Image_Path);
                    if(filePath == null)
                    {
                        ViewBag.Error = "Error While Uploading Image";
                    }
                    else 
                    {
                        pi.Product_ID = (int) TempData["Product_ID"];
                        pi.Product_Image_Name = product_Images.Product_Image_Name;
                        pi.Product_Image_URL = filePath;
                        pi.isPersonalisable = product_Images.isPersonalisable;
                        db.product_images.Add(pi);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Success = "New Image for " + TempData["Product_Name"] + " Uploaded Successfully";
                    }    
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");                 
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        public string Upload_Image(HttpPostedFileBase Image_Path)
        {
            string filePath = null;
            int count = 1;
            if(Image_Path != null && Image_Path.ContentLength > 0)
            {
                string imageExtension = Path.GetExtension(Image_Path.FileName);
                if((imageExtension.ToLower().Equals(".jpg")) || (imageExtension.ToLower().Equals(".jpeg")) || (imageExtension.ToLower().Equals(".png")))
                {
                    try
                    {
                        filePath = Path.Combine(Server.MapPath("~/Content/ProductImages"), TempData["Product_Name"].ToString() + count + Path.GetFileName(Image_Path.FileName));
                        Image_Path.SaveAs(filePath);
                        filePath = "~/Content/ProductImages/" + TempData["Product_Name"].ToString() + count + Path.GetFileName(Image_Path.FileName);
                    }
                    catch (Exception ex)
                    {
                        filePath = null;
                        Response.Write("<script>alert('"+ ex.Message +"');</script>");
                    }
                }
                else
                {
                    ViewBag.Error = "File Type Not Supported!";
                    filePath = null;
                }
            }

            return filePath;
        }

        private void Get_Product(int? p_id)
        {
            using (db)
            {
                product Product = db.products.Where(x => x.Product_ID == p_id).SingleOrDefault();
                TempData["Product_ID"] = Product.Product_ID;
                TempData["Product_Name"] = Product.Product_Name;
            }
        }

        [HttpGet]
        public ActionResult Personalise_Product()
        {
            product_images ProductImage = db.product_images.Where(x => x.Product_ID == 25 && x.isPersonalisable == 1).SingleOrDefault();
            return View(ProductImage);
        }

        [HttpGet]
        public ActionResult Product_Images_List(int p_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (p_id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                TempData["ProductID"] = p_id;
                List<product_images> product_Images = db.product_images.Where(x => x.Product_ID.Equals(p_id)).ToList();
                return View(product_Images);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Delete_Product_Image(int? pi_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (pi_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

               product_images product_Image = db.product_images.Find(pi_id);
               db.product_images.Remove(product_Image);
               db.SaveChanges();
               return RedirectToAction("Product_Images_List", new { p_id = TempData["ProductID"] });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}