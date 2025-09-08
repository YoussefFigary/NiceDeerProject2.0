using NorthwindTraders.Models;
using NorthwindTraders.Models.DTOs;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class SupplierController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        // GET: Supplier
        
        public ActionResult Supplier()
        {
            var Suppliers = db.Suppliers.Where(t => t != null).OrderBy(x => x.SupplierID).ToList();
            return View(Suppliers);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var supplier = db.Suppliers.Find(id);
            return PartialView("_DetailsPopup", supplier);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var relatedRecords = CheckProducts(id);
            if (relatedRecords.Count != 0)
            {
                Response.StatusCode = 409;
                return Json(new
                {
                    error = "Cannot delete because this record is linked to Products.",
                    linkedProducts = relatedRecords
                }, JsonRequestBehavior.AllowGet);
            }

            var record = db.Suppliers.Find(id);
            if (record == null)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Supplier not found." }, JsonRequestBehavior.AllowGet);
            }

            db.Suppliers.Remove(record);
            db.SaveChanges();

            return Json(new { success = true });
        }
        public List<ProductInfo> CheckProducts(int id)
        {
            var Products = db.Suppliers
                              .Where(t => t.SupplierID == id)
                              .SelectMany(t => t.Products)
                              .Select(e => new ProductInfo
                              {
                                  ProductID = e.ProductID,
                                  ProductName = e.ProductName
                              })
                              .ToList();

            return Products;
        }

        public ActionResult AddOrEditPopup(int id)
        {
            ViewBag.ContactTitles = db.Suppliers.Select(c => c.ContactTitle).Distinct().ToList();
            ViewBag.Cities = db.Suppliers.Select(c => c.City).Distinct().ToList();
            ViewBag.Countries = db.Suppliers.Select(c => c.Country).Distinct().ToList();
            if (id == 0)
                return PartialView("_SupplierAddOrEdit", new Supplier()); // Empty model for Add
            else
            {
                var Suppliers = db.Suppliers.Find(id);
                return PartialView("_SupplierAddOrEdit", Suppliers); // Filled model for Edit
            }
        }
    
        [HttpPost]
        public ActionResult SaveEdit(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            // Try to find existing Supplier
            var existing = db.Suppliers.Find(supplier.SupplierID);


            if (supplier.SupplierID == 0)
            {
                //ADD
               

                db.Suppliers.Add(supplier);
            }
            else
            {
                // EDIT

                db.Entry(existing).CurrentValues.SetValues(supplier);
              //  db.Entry(supplier).State = EntityState.Modified;

            }

            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}