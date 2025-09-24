﻿using Northwind.DAL;
using Northwind.DAL.Models;
using Northwind.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business
{
    public class OrderBusiness : BaseBusiness
    {
        public RequestResponse GetOrders()
        {
            var response = new RequestResponse();
            List<Order> orders = new List<Order>();
            orders = DBContext.Orders.Where(c => c != null).OrderBy(c => c.CustomerID).ToList();
            List<OrderVM> ordersVM = new List<OrderVM>();
            foreach (var order in orders)
            {
                OrderVM orderVM = new OrderVM()
                {
                    OrderID = order.OrderID,
                    CustomerID = order.CustomerID,
                    EmployeeID = order.EmployeeID,
                    OrderDate = order.OrderDate,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    ShipVia = order.ShipVia,
                    Freight = order.Freight,
                    ShipName = order.ShipName,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipRegion = order.ShipRegion,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipCountry = order.ShipCountry,

                };
                ordersVM.Add(orderVM);

            }
            response.JsonData = ordersVM;
            return response;
        }
        public RequestResponse GetOrderDetails(int id)
        {
            var response = new RequestResponse();
            Order order = new Order();
            order = DBContext.Orders.Find(id);
            OrderVM orderVM = new OrderVM
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipVia = order.ShipVia,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,
            };
            response.JsonData = orderVM;
            return response;
        }

        public RequestResponse SaveOrder(OrderVM orderVM)
        {


            Order order;
            var response = new RequestResponse();

            if (orderVM.OrderID == 0)
            {
                order = new Order()
                {
                    CustomerID = orderVM.CustomerID,
                    EmployeeID = orderVM.EmployeeID,
                    OrderDate = orderVM.OrderDate,
                    RequiredDate = orderVM.RequiredDate,
                    ShippedDate = orderVM.ShippedDate,
                    ShipVia = orderVM.ShipVia,
                    Freight = orderVM.Freight,
                    ShipName = orderVM.ShipName,
                    ShipAddress = orderVM.ShipAddress,
                    ShipCity = orderVM.ShipCity,
                    ShipRegion = orderVM.ShipRegion,
                    ShipPostalCode = orderVM.ShipPostalCode,
                    ShipCountry = orderVM.ShipCountry,
                };
                DBContext.Orders.Add(order);
            }
            else
            {
                // EDIT
                order = DBContext.Orders.Find(orderVM.OrderID);

                order.EmployeeID = orderVM.EmployeeID;
                order.OrderDate = orderVM.OrderDate;
                order.RequiredDate = orderVM.RequiredDate;
                order.ShippedDate = orderVM.ShippedDate;
                order.ShipVia = orderVM.ShipVia;
                order.Freight = orderVM.Freight;
                order.ShipName = orderVM.ShipName;
                order.ShipAddress = orderVM.ShipAddress;
                order.ShipCity = orderVM.ShipCity;
                order.ShipRegion = orderVM.ShipRegion;
                order.ShipPostalCode = orderVM.ShipPostalCode;
                order.ShipCountry = orderVM.ShipCountry;
                DBContext.Entry(order).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;

            }
            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            return response;
        }

        public RequestResponse DeleteOrder(int id)
        {
            var response = new RequestResponse();
            Order order = DBContext.Orders.Find(id);
            if (order == null)
            {
                response.Success = false;
                response.JsonData = null;
                response.Message = "Order not found.";
                return response;
            }
            var orderDetails = DBContext.Order_Details.Where(o => o.OrderID == id).ToList();
            foreach (var item in orderDetails)
            {
                DBContext.Order_Details.Remove(item);
            }
            DBContext.Orders.Remove(order);
            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.JsonData = order;
            response.Message = "Order deleted successfully.";
            return response;
        }

        public RequestResponse DeleteDetail(int orderID, int productID)
        {
            var response = new RequestResponse();
            Order_Detail orderDetail = DBContext.Order_Details.Find(orderID, productID);
            if (orderDetail == null)
            {
                response.Success = false;
                response.JsonData = null;
                response.Message = "Order Detail not found.";
                return response;
            }

            DBContext.Order_Details.Remove(orderDetail);
            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.JsonData = orderDetail;
            response.Message = "Order Detail deleted successfully.";
            return response;
        }
        public RequestResponse SaveDetail(Order_DetailVM detail)
        {
            var response = new RequestResponse();

            if (detail.OrderID <= 0 || detail.ProductID <= 0 || detail.Quantity <= 0)
            {
                response.Success = false;
                response.JsonData = null;
                response.Message = "Invalid input.";
                return response;
            }

            // Load UnitPrice from Products table
            var unitPrice = DBContext.Products
                .Where(p => p.ProductID == detail.ProductID)
                .Select(p => p.UnitPrice)
                .FirstOrDefault();

            if (unitPrice == null)
            {
                response.Success = false;
                response.JsonData = null;
                response.Message = "Product not found.";
                return response;
            }

            // Check if this detail already exists
            var existing = DBContext.Order_Details.Find(detail.OrderID, detail.ProductID);

            if (existing == null)
            {
                Order_Detail orderDetail = new Order_Detail
                {
                    OrderID = detail.OrderID,
                    ProductID = detail.ProductID,
                    Quantity = detail.Quantity,
                    UnitPrice = unitPrice.Value,
                    Discount = detail.Discount / 100
                };

                DBContext.Order_Details.Add(orderDetail);
                response.JsonData = orderDetail;
                response.Message = "Order detail added successfully.";
            }
            else
            {
                existing.Quantity += detail.Quantity;
                existing.Discount = detail.Discount;
                existing.UnitPrice = unitPrice.Value;

                DBContext.Entry(existing).State = System.Data.Entity.EntityState.Modified;
                response.Message = "Order detail updated successfully.";
                response.JsonData = existing;
            }
            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            return response;
        }

        //for the dropdown lists
        public RequestResponse GetCustomers()
        {
            var response = new RequestResponse();
            List<Customer> customers = new List<Customer>();
            customers = DBContext.Customers.Where(c => c != null).OrderBy(c => c.CustomerID).ToList();
            List<CustomerVM> customersVM = new List<CustomerVM>();
            foreach (var customer in customers)
            {
                CustomerVM customerVM = new CustomerVM()
                {
                    CustomerID = customer.CustomerID,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Address = customer.Address,
                    City = customer.City,
                    Region = customer.Region,
                    PostalCode = customer.PostalCode,
                    Country = customer.Country,
                    Phone = customer.Phone,
                    Fax = customer.Fax
                };
                customersVM.Add(customerVM);

            }
            response.JsonData = customersVM;
            return response;
        }

        public RequestResponse GetEmployees()
        {
            var response = new RequestResponse();
            List<Employee> employees = new List<Employee>();
            List<EmployeeVM> employeesVM = new List<EmployeeVM>();
            employees = DBContext.Employees.Where(t => t != null).OrderBy(x => x.EmployeeID).ToList();
            foreach (Employee employee in employees)
            {
                EmployeeVM employeevm = new EmployeeVM()
                {
                    EmployeeID = employee.EmployeeID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Title = employee.Title,
                    TitleOfCourtesy = employee.TitleOfCourtesy,
                    HireDate = employee.HireDate,
                    Region = employee.Region,
                    PostalCode = employee.PostalCode,
                    HomePhone = employee.HomePhone,
                    Extension = employee.Extension,
                    Photo = employee.Photo,
                    Notes = employee.Notes,
                    ReportsTo = employee.ReportsTo,
                    PhotoPath = employee.PhotoPath,
                    Address = employee.Address,
                    City = employee.City,
                    BirthDate = employee.BirthDate,
                    Country = employee.Country
                };
                employeesVM.Add(employeevm);
            }
            response.JsonData = employeesVM;
            response.Success = true;

            return response;
        }

        public RequestResponse GetShippers()
        {
            var response = new RequestResponse();
            List<Shipper> shippers = new List<Shipper>();
            shippers = DBContext.Shippers.Where(s => s != null).OrderBy(s => s.ShipperID).ToList();

            List<ShipperVM> shippersVM = new List<ShipperVM>();
            foreach (var shipper in shippers)
            {
                ShipperVM shipperVM = new ShipperVM()
                {
                    ShipperID = shipper.ShipperID,
                    CompanyName = shipper.CompanyName,
                    Phone = shipper.Phone
                };
                shippersVM.Add(shipperVM);
            }

            response.JsonData = shippersVM;
            return response;
        }


        public RequestResponse GetOrderDetails()
        {
            var response = new RequestResponse();
            List<Order_Detail> orderDetails = new List<Order_Detail>();
            orderDetails = DBContext.Order_Details.Where(o => o != null).OrderBy(o => o.OrderID).ToList();

            List<Order_DetailVM> orderDetailsVM = new List<Order_DetailVM>();
            foreach (var detail in orderDetails)
            {
                Order_DetailVM detailVM = new Order_DetailVM()
                {
                    OrderID = detail.OrderID,
                    ProductID = detail.ProductID,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount
                };
                orderDetailsVM.Add(detailVM);
            }

            response.JsonData = orderDetailsVM;
            return response;
        }

        public RequestResponse GetProducts()
        {
            var response = new RequestResponse();
            List<Product> products = new List<Product>();
            products = DBContext.Products.Where(p => p != null).OrderBy(p => p.ProductID).ToList();

            List<ProductVM> productsVM = new List<ProductVM>();
            foreach (var product in products)
            {
                ProductVM productVM = new ProductVM()
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    SupplierID = product.SupplierID,
                    CategoryID = product.CategoryID,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder,
                    ReorderLevel = product.ReorderLevel,
                    Discontinued = product.Discontinued
                };
                productsVM.Add(productVM);
            }

            response.JsonData = productsVM;
            return response;
        }

        public RequestResponse GetProductIDs()
        {
            var response = new RequestResponse();
            response.JsonData = DBContext.Products
                .Select(p => p.ProductID)
                .Distinct()
                .ToList();
            response.Success = true;
            return response;
        }


    }
}