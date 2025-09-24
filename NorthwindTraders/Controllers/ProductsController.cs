﻿using Northwind.Business;
using Northwind.DTO;
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
        //BUSINESS LAYER INSTANCE
        ProductBusiness ProductBusiness = new ProductBusiness();

        //displaying table
        [HttpGet]
        public ActionResult Index()
        {
            var response = ProductBusiness.GetProducts();
            return View((List<ProductVM>)response.JsonData);
        }

        //details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var response = ProductBusiness.GetProductDetails(id);
            return PartialView("_ProductDetails", (ProductVM)response.JsonData);
        }

        public ActionResult AddOrEditPopup(int id)
        {
            ProductVM product;

            if (id == 0)
            {
                product = new ProductVM(); // Empty model for Add
            }
            else
            {
                var response = ProductBusiness.GetProductDetails(id);
                product = response.JsonData as ProductVM;
            }

            // Always populate dropdowns
            var suppliers = ProductBusiness.GetSuppliers();
            var categories = ProductBusiness.GetCategories();

            ViewBag.Suppliers = suppliers.JsonData as List<SupplierVM>;
            ViewBag.Categories = categories.JsonData as List<CategoryVM>;


            return PartialView("_ProductAddOrEdit", product);

        }

        [HttpPost]
        public ActionResult SaveEdit(ProductVM product)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }
            var response = ProductBusiness.SaveProduct(product);

            return Json(new { success = response.Success });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            // Check relationships
            var checkRelationResponse = ProductBusiness.hasOrders(id);
            if (!checkRelationResponse.Success)
            {
                return Json(new
                {
                    checkRelationResponse.Success,
                    checkRelationResponse.Message
                });
            }
            var response = ProductBusiness.DeleteProduct(id);
            return Json(new { response.Success, response.Message });
        }

    }

}