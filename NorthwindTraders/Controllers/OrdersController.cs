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
using System.Web.UI.WebControls.Expressions;

namespace NorthwindTraders.Controllers
{
    public class OrdersController : Controller
    {
        //database
        private NorthwindEntities db = new NorthwindEntities();

        //displaying table
        [HttpGet]
        public ActionResult Index()
        {
            var orders = db.Orders.Where(o => o != null).OrderBy(x => x.OrderDate).ToList();
            return View(orders);
        }

        //details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order_Details= db.Order_Details.ToList();

            return PartialView("_OrderDetails", order);
        }
        [HttpGet]
        public ActionResult AddOrEditPopup(int id)
        {
            Order order;

            if (id == 0)
            {
                // New order
                order = new Order();
            }
            else
            {
                // Existing order
                order = db.Orders.Find(id);
            }

            // Populate dropdown lists
            ViewBag.Customers = db.Customers.ToList();
            ViewBag.Employees = db.Employees.ToList();
            ViewBag.Shippers = db.Shippers.ToList();
            ViewBag.Order_Details = db.Order_Details.ToList();
            return PartialView("_OrderAddOrEdit", order);
        }

        public ActionResult AddOrderDetailPopup()
        {
            Order_Detail orderDetail = new Order_Detail();
            ViewBag.Products = db.Products.ToList();
            ViewBag.ProductID = db.Order_Details.Select(c => c.ProductID).Distinct().ToList();
            return PartialView("_AddOrderDetails", orderDetail);
        }

        [HttpPost]
        public ActionResult SaveAddOrEdit(Order order)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }            

            if ( order.OrderID==0)
            {
                //ADD
                db.Orders.Add(order);
            }
            else
            {
                // EDIT
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);
            if ( id==0 )
            {
                return Json(new { success = false, message = "Order not found." });
            }
            var orderDetails = db.Order_Details.Where(o => o.OrderID == id).ToList();

            foreach (var item in orderDetails)
            {
                db.Order_Details.Remove(item);
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return Json(new { success = true, message = "Order deleted successfully." });
        }


    }

}