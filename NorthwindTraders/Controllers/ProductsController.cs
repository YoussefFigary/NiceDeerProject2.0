using NorthwindTraders.Models;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class ProductsController : Controller
    {
        //database
        private NorthwindEntities db = new NorthwindEntities();

        //displaying table
        [HttpGet]
        public ActionResult Index()
        {
            var products = db.Products.Where(c => c != null).OrderBy(x => x.ProductID).ToList();
            return View(products);
        }

        //details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            return PartialView("_ProductDetails", product);
        }
        [HttpGet]
        public ActionResult AddOrEditPopup(int id)
        {
            if (id == 0)
                return PartialView("_ProductAddOrEdit", new Product()); // Empty model for Add
            else
            {
                var product = db.Products.Find(id);
                if (product == null)
                    product = new Product();

                // Supplier dropdown
                ViewBag.Suppliers = db.Suppliers
                    .Select(s => new SelectListItem
                    {
                        Value = s.SupplierID.ToString(),
                        Text = s.CompanyName
                    })
                    .ToList();

                // Category dropdown
                ViewBag.Categories = db.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryID.ToString(),
                        Text = c.CategoryName
                    })
                    .ToList();

                return PartialView("_ProductAddOrEdit", product);
            }
        }

        [HttpPost]
        public ActionResult SaveEdit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            if (product.ProductID == 0)
            {
                //ADD
                db.Products.Add(product);
            }
            else
            {
                // EDIT
                db.Entry(product).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new { success = true});
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            // Check relationships
            bool hasOrders = db.Order_Details.Any( o => o.ProductID == id);

            if (hasOrders)
            {
                return Json(new
                {
                    success = false,
                    message = "Cannot delete this product because it has related Orders."
                });
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Json(new { success = true, message = "Product deleted successfully." });
        }


    }

}