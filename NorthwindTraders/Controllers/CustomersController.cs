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
        //database
        private NorthwindEntities db = new NorthwindEntities();

        //displaying table
        [HttpGet]
        public ActionResult Index()
        {
            var customers = db.Customers.Where(c => c != null).OrderBy(x => x.CustomerID).ToList();
            return View(customers);
        }

        //details
        [HttpGet]
        public ActionResult Details(string id)
        {
            var customer = db.Customers.Find(id);
            return PartialView("_CustomerDetails", customer);
        }
        [HttpGet]
        public ActionResult AddOrEditPopup(string id)
        {
            if (id == "")
                return PartialView("_CustomerAddOrEdit", new Customer()); // Empty model for Add
            else
            {
                var customer = db.Customers.Find(id);
                if (customer == null)
                    customer = new Customer();
                ViewBag.ContactTitles = db.Customers.Select(c => c.ContactTitle).Distinct().ToList();
                ViewBag.Cities = db.Customers.Select(c => c.City).Distinct().ToList();
                ViewBag.Countries = db.Customers.Select(c => c.Country).Distinct().ToList();

                return PartialView("_CustomerAddOrEdit", customer);
            }

        }
        [HttpPost]
        public ActionResult SaveEdit(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            // Try to find existing customer
            var existing = db.Customers.Find(customer.CustomerID);

            if (existing == null)
            {
                //ADD
                if (string.IsNullOrEmpty(customer.CustomerID))
                {
                    customer.CustomerID = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
                }

                db.Customers.Add(customer);
            }
            else
            {
                // EDIT
                if (existing.ContactName == null)
                
                existing.CompanyName = customer.CompanyName;
                existing.ContactName = customer.ContactName;
                existing.ContactTitle = customer.ContactTitle;
                existing.Address = customer.Address;
                existing.City = customer.City;
                existing.Region = customer.Region;
                existing.PostalCode = customer.PostalCode;
                existing.Country = customer.Country;
                existing.Phone = customer.Phone;
                existing.Fax = customer.Fax;
                
                
            }

            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            // Check relationships
            bool hasOrders = db.Orders.Any(o => o.CustomerID == id);

            if (hasOrders)
            {
                return Json(new
                {
                    success = false,
                    message = "Cannot delete this customer because it has related Orders or Customer Demographics."
                });
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Json(new { success = true, message = "Customer deleted successfully." });
        }


    }

}