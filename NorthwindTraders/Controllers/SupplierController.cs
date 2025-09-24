using Northwind.Business;
using Northwind.DTO;
using NorthwindTraders.Models;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{

    public class SupplierController : Controller
    {
        SupplierBusiness SupplierBusiness = new SupplierBusiness();
        // GET: Supplier

        public ActionResult Index()
        {
            var response = SupplierBusiness.GetSuppliers();
            if (response.Success)
            {
                return View((List<SupplierVM>)response.JsonData);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var supplier = (SupplierVM)SupplierBusiness.GetSupplier(id).JsonData;
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

            var record = (SupplierVM)SupplierBusiness.GetSupplier(id).JsonData;
            if (record == null)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Supplier not found." }, JsonRequestBehavior.AllowGet);
            }

            SupplierBusiness.RemoveSupplier(record);

            return Json(new { success = true });
        }
        public List<ProductInfo> CheckProducts(int id)
        {
            List<ProductInfo> Products = (List<ProductInfo>) SupplierBusiness.CheckProducts(id).JsonData;

            return Products;
        }

        public ActionResult AddOrEditPopup(int id)
        {
            ViewBag.ContactTitles = (List<String>)SupplierBusiness.GetContactTitle().JsonData;
            ViewBag.Cities = (List<String>)SupplierBusiness.GetCities().JsonData;
            ViewBag.Countries = (List<String>)SupplierBusiness.GetCountry().JsonData;
            if (id == 0)
                return PartialView("_SupplierAddOrEdit", new SupplierVM()); // Empty model for Add
            else
            {
                var Suppliers = (SupplierVM)SupplierBusiness.GetSupplier(id).JsonData;

                return PartialView("_SupplierAddOrEdit", Suppliers); // Filled model for Edit
            }
        }
    
        [HttpPost]
        public ActionResult SaveEdit(SupplierVM supplier)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            // Try to find existing Supplier
            var response = SupplierBusiness.AddEditSupplier(supplier);
           
            return Json(new { success = response.Success });
        }
    }
}