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
            return View(db.products.ToList());
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
                    List<product_images> product_Images_List = Get_Product_Images(p_id);
                    db.product_images.RemoveRange(product_Images_List);
                    db.products.Remove(Product);
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
                    if ((db.carts.Any(x => x.User_ID == userid && x.Product_ID == p_id)))
                    {
                        
                        Cart = db.carts.Single(x => x.User_ID == userid && x.Product_ID == p_id);
                        Cart.Product_Quantity = Cart.Product_Quantity + Convert.ToInt32(Quantity);
                        Cart.Cart_Total = Product.Product_Price * Cart.Product_Quantity;

                        db.Entry(Cart).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Cart.User_ID = Convert.ToInt32(Session["User_ID"].ToString());
                        Cart.Product_ID = Product.Product_ID;
                        Cart.Product_Quantity = Convert.ToInt32(Quantity);
                        Cart.Cart_Total = Product.Product_Price * Convert.ToInt32(Quantity);

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
        public List<product_images> Get_Product_Images(int p_id)
        {
            List<product_images> Product_Image_List = db.product_images.Where(x => x.Product_ID.Equals(p_id)).ToList();

            return Product_Image_List;
        }

        [HttpGet]
        public List<product_category> Get_Categories()
        {
            List<product_category> Category_List = db.product_category.ToList<product_category>();
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