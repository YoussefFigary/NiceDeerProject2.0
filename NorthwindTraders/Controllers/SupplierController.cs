using NorthwindTraders.Models;
using NorthwindTraders.Models.DTOs;
using System;
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
        public ActionResult Index()
        {
            return View(); 
        }
        public ActionResult Supplier()
        {
            var Suppliers = db.Suppliers.Where(t => t != null).OrderBy(x => x.SupplierID).ToList();
            return View(Suppliers);
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

            var record = db.Territories.Find(id);
            if (record == null)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Supplier not found." }, JsonRequestBehavior.AllowGet);
            }

            db.Territories.Remove(record);
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
    }
}