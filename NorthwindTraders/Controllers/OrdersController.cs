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
            ViewBag.Order_Details = db.Order_Details.ToList();

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
        public ActionResult AddOrderDetailPopup(int orderId)
        {
            var orderDetail = new Order_Detail
            {
                OrderID = orderId  // pre-fill order ID
            };
            ViewBag.Products = db.Products.ToList();
            ViewBag.ProductID = db.Products.Select(p => p.ProductID).Distinct().ToList();
            return PartialView("_AddOrderDetails", orderDetail);
        }
        //public ActionResult AddOrderDetailPopup(int orderId)
        //{
        //    var orderDetail = new Order_Detail
        //    {
        //        OrderID = orderId  // pre-fill order ID
        //    };
        //    ViewBag.Products = db.Products.ToList();
        //    ViewBag.ProductID = db.Products.Select(p => p.ProductID).Distinct().ToList();
        //    return PartialView("_AddOrderDetails", orderDetail);
        //}


        //public ActionResult AddOrderDetailPopup()
        //{
        //    Order_Detail orderDetail = new Order_Detail();
        //    ViewBag.Products = db.Products.ToList();
        //    ViewBag.ProductID = db.Order_Details.Select(c => c.ProductID).Distinct().ToList();
        //    return PartialView("_AddOrderDetails", orderDetail);
        //}

        [HttpPost]
        public ActionResult SaveAddOrEdit(Order order)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            if (order.OrderID == 0)
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
            return Json(new { success = true, @OrderId = order.OrderID });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);
            if (id == 0)
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
        [HttpPost]
        public ActionResult DeleteDetail(int orderID, int productID)
        {
            var existing = db.Order_Details.Find(orderID, productID);
            if (existing == null)
            {
                return Json(new { success = false, message = "Detail not found." });
            }
            db.Order_Details.Remove(existing);
            db.SaveChanges();
            return Json(new { success = true, message = "Detail deleted successfully." });
        }

        [HttpPost]
        public ActionResult SaveDetail(Order_Detail detail)
        {
            
            if (detail.OrderID <= 0 || detail.ProductID <= 0 || detail.Quantity <= 0)
                return Json(new { success = false, message = "Invalid input." });

            // Load UnitPrice from Products table
            var unitPrice = db.Products.Where(p => p.ProductID == detail.ProductID).Select(p => p.UnitPrice).FirstOrDefault();

            if (unitPrice == null)
                return Json(new { success = false, message = "Product not found." });

            // Check if this detail already exists 
            var existing = db.Order_Details.Find(detail.OrderID, detail.ProductID);
            if (existing == null)
            {
                detail.UnitPrice = unitPrice.Value; // set server-side
                db.Order_Details.Add(detail);
            }
            else
            {
                existing.Quantity += detail.Quantity ;
                existing.Discount = detail.Discount;
                existing.UnitPrice = unitPrice.Value;
                db.Entry(existing).State = EntityState.Modified;
            }

            db.SaveChanges();
            return Json(new { success = true, orderId = detail.OrderID });
        }


    }

}