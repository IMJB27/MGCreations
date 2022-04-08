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

namespace MGCreations.Controllers
{
    public class ProductImagesController : Controller
    {
        mgcreationsEntities db = new mgcreationsEntities();
        // GET: ProductImages
        [HttpGet]
        public ActionResult Add_Product_Images()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Product_Images(product_images product_Images)
        {

            return View();
        }
    }
}