using Northwind.Business;
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
    public class CustomersController : Controller
    {
        //BUSINESS LAYER INSTANCE
        CustomerBusiness CustomerBusiness = new CustomerBusiness();

        //displaying table
        [HttpGet]
        public ActionResult Index()
        {
            var response = CustomerBusiness.GetCustomers();
            return View((List<CustomerVM>)response.JsonData);
        }


        //details
        [HttpGet]
        public ActionResult Details(string id)
        {
            var response = CustomerBusiness.GetCustomerDetails(id);
            return PartialView("_CustomerDetails", (CustomerVM)response.JsonData);
        }

        [HttpGet]
        public ActionResult AddOrEditPopup(string id)
        {
            CustomerVM customer;

            if (string.IsNullOrEmpty(id))
            {
                customer = new CustomerVM(); // Empty model for Add
            }
            else
            {
                var response = CustomerBusiness.GetCustomerDetails(id);
                customer = response.JsonData as CustomerVM;
            }

            // Always populate dropdowns
            var contactTitles = CustomerBusiness.GetContactTitles();
            var cities = CustomerBusiness.GetCities();
            var countries = CustomerBusiness.GetCountries();

            ViewBag.ContactTitles = contactTitles.JsonData as List<string>;
            ViewBag.Cities = cities.JsonData as List<string>;
            ViewBag.Countries = countries.JsonData as List<string>;

            return PartialView("_CustomerAddOrEdit", customer);

        }

        [HttpPost]
        public ActionResult SaveEdit(CustomerVM customer)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }
            var response = CustomerBusiness.SaveCustomer(customer);

            return Json(new { success = response.Success });
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var response = CustomerBusiness.DeleteCustomer(id);

            if (response.JsonData == null)
            {
                return Json(new { response.Success, response.Message });
            }

            // Check relationships
            var checkRelationResponse = CustomerBusiness.hasOrders(id);
            if (checkRelationResponse.Success)
            {
                return Json(new
                {
                    checkRelationResponse.Success,
                    checkRelationResponse.Message
                });
            }

            return Json(new { response.Success, response.Message });
        }


        //  DELETE ZAI YOUSSEF
        //[HttpPost]
        //public ActionResult Delete(string id)
        //{

        //    var relatedorders = CustomerBusiness.checkOrders(id);

        //    if (relatedorders.Count != 0)
        //    {
        //        Response.StatusCode = 409;
        //        return Json(new
        //        {
        //            error = "Cannot delete because an order is linked to this Customer",
        //            linkedorders = relatedorders
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    var response =CustomerBusiness.DeleteCustomer(id);
        //    if (response.JsonData == null)
        //    {
        //        Response.StatusCode = 404;
        //        return Json(new { success = false, message = "Customer not found." });
        //    }

        //    return Json(new { success = response.Success , message = "Customer deleted successfully." });
        //}
    }
}



