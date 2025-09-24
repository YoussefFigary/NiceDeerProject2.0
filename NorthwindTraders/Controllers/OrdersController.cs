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
using System.Web.UI.WebControls.Expressions;

namespace NorthwindTraders.Controllers
{
    public class OrdersController : Controller
    {
        //BUSINESS LAYER INSTANCE
        OrderBusiness OrderBusiness = new OrderBusiness();

        //displaying table
        [HttpGet]
        public ActionResult Index()
        {
            var response = OrderBusiness.GetOrders();
            return View((List<OrderVM>)response.JsonData);
        }

        //details
        [HttpGet]
        public ActionResult Details(int id)
        {
            ViewBag.Customers = OrderBusiness.GetCustomers().JsonData as List<CustomerVM>;
            ViewBag.Employees = OrderBusiness.GetEmployees().JsonData as List<EmployeeVM>;
            ViewBag.Shippers = OrderBusiness.GetShippers().JsonData as List<ShipperVM>;
            ViewBag.Order_Details = OrderBusiness.GetOrderDetails().JsonData as List<Order_DetailVM>;

            var response = OrderBusiness.GetOrderDetails(id);
            return PartialView("_OrderDetails", (OrderVM)response.JsonData);
        }

        public ActionResult AddOrEditPopup(int id)
        {
            OrderVM order;

            if (id == 0)
            {
                order = new OrderVM(); // Empty model for Add
            }
            else
            {
                var response = OrderBusiness.GetOrderDetails(id);
                order = response.JsonData as OrderVM;
            }

            ViewBag.Customers = OrderBusiness.GetCustomers().JsonData as List<CustomerVM>;
            ViewBag.Employees = OrderBusiness.GetEmployees().JsonData as List<EmployeeVM>;
            ViewBag.Shippers = OrderBusiness.GetShippers().JsonData as List<ShipperVM>;
            ViewBag.Order_Details = OrderBusiness.GetOrderDetails().JsonData as List<Order_DetailVM>;

            return PartialView("_OrderAddOrEdit", order);

        }

        public ActionResult AddOrderDetailPopup(int orderId)
        {
            Order_DetailVM orderDetail = new Order_DetailVM
            {
                OrderID = orderId  // pre-fill order ID
            };
            ViewBag.Products = OrderBusiness.GetProducts().JsonData as List<ProductVM>;
            ViewBag.ProductID = OrderBusiness.GetProductIDs().JsonData as List<int>;
            return PartialView("_AddOrderDetails", orderDetail);
        }

        [HttpPost]
        public ActionResult SaveAddOrEdit(OrderVM order)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }
            var response = OrderBusiness.SaveOrder(order);

            return Json(new { response.Success });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var response = OrderBusiness.DeleteOrder(id);

            return Json(new { response.Success, response.Message });
        }

        [HttpPost]
        public ActionResult DeleteDetail(int orderID, int productID)
        {
            var response = OrderBusiness.DeleteDetail(orderID, productID);

            return Json(new { response.Success, response.Message });
        }
        [HttpPost]
        public ActionResult SaveDetail(Order_DetailVM detail)
        {
            var response = OrderBusiness.SaveDetail(detail);

            return Json(new { response.Success, response.Message });

        }

    }

}