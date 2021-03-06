using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            List<product> product_List = new List<product>();
            if (Session["User_Type"] != null)
            {
                if (Session["User_Type"].ToString() == "Admin")
                {
                    product_List = db.products.ToList();
                }
                else
                {
                    product_List = db.products.Where(x => x.is_Active == 1).ToList();
                }
            }
            else
            {
                product_List = db.products.Where(x => x.is_Active == 1).ToList();
            }
            return View(product_List);
        }

        [HttpGet]
        public ActionResult View_Product_Details(int? p_id)
        {

            if (p_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product Product = db.products.Find(p_id);
            if (Product == null)
            {
                return HttpNotFound();
            }

            return View(Product);
        }

        [HttpGet]
        public ActionResult Add_Product()
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                product product_Details = new product();
                Get_Categories();
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Add_Product(product product_Details)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    product Product = new product();
                    Product.Product_Name = product_Details.Product_Name;
                    Product.Category_ID = product_Details.Category_ID;
                    Product.Product_Description = product_Details.Product_Description;
                    Product.Product_Quantity = product_Details.Product_Quantity;
                    Product.Product_Price = product_Details.Product_Price;
                    Product.isPersonalisable = product_Details.isPersonalisable;
                    Product.is_Active = 1;
                    db.products.Add(Product);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Success = "New Product Created Successfully";
                    //Get_Categories();
                    return RedirectToAction("Add_Product_Images", "ProductImages", new { p_id = Product.Product_ID });
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                    return View();
                }
               
            }
            else
            {
                return View();
            }        
        }

        [HttpGet]
        public ActionResult Update_Product_Details(int? p_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (p_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product Product = db.products.Find(p_id);
                Get_Categories();
                if (Product == null)
                {
                    return HttpNotFound();
                }
                return View(Product);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Update_Product_Details(product product_Details)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    product Product = new product();

                    Product.Product_Name = product_Details.Product_Name;
                    Product.Category_ID = product_Details.Category_ID;
                    Product.Product_Description = product_Details.Product_Description;
                    Product.Product_Quantity = product_Details.Product_Quantity;
                    Product.Product_Price = product_Details.Product_Price;
                    Product.isPersonalisable = product_Details.isPersonalisable;
                    Product.is_Active = product_Details.is_Active;
                    db.Entry(product_Details).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Product_List");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                    return View();
                }

            }
            else
            {
                return View();
            }
           
        }

        [HttpGet]
        public ActionResult Delete_Product(int? p_id)
        {
            if ((Session["User_ID"] != null) && (Session["User_Type"].ToString() == "Admin"))
            {
                if (p_id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product Product = db.products.Find(p_id);
                Get_Categories();
                if (Product == null)
                {
                    return HttpNotFound();
                }
                return View(Product);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost, ActionName("Delete_Product")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int p_id)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    product Product = db.products.Find(p_id);
                    Product.is_Active = 0;
                    db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Product_List");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                    return View();
                }
            }
            else
            {
                return View();
            }  
        }

        [HttpGet]
        public List<product_images> Get_Product_Images(int p_id)
        {
            List<product_images> Product_Image_List = db.product_images.Where(x => x.Product_ID.Equals(p_id)).ToList();

            return Product_Image_List;
        }

        [HttpGet]
        public List<product_category> Get_Categories()
        {
            List<product_category> Category_List = db.product_category.Where(x => x.is_Active == 1).ToList();
            TempData["Category_List"] = new SelectList(Category_List, "Category_ID", "Category_Name");
            return Category_List;                  
        }

        [HttpGet]
        public int Get_Product_Quantity(int p_id)
        {
            product Product = db.products.Where(x => x.Product_ID.Equals(p_id)).SingleOrDefault();
            int Quantity = Product.Product_Quantity;
            return Quantity;
        }



    }
}