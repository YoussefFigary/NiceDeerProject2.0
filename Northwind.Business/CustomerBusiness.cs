﻿using Northwind.DAL;
using Northwind.DAL.Models;
using Northwind.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
namespace Northwind.Business
{
    public class CustomerBusiness : BaseBusiness
    {
        public RequestResponse GetCustomers()
        {
            var response = new RequestResponse();
            List<Customer> customers = new List<Customer>();
            customers = DBContext.Customers.Where(c => c != null).OrderBy(c => c.CustomerID).ToList();
            List<CustomerVM> customersVM = new List<CustomerVM>();

            customersVM = customers.Select(c => new CustomerVM()
            {
                CustomerID = c.CustomerID,
                CompanyName = c.CompanyName,
                ContactName = c.ContactName,
                ContactTitle = c.ContactTitle,
                Address = c.Address,
                City = c.City,
                Region = c.Region,
                PostalCode = c.PostalCode,
                Country = c.Country,
                Phone = c.Phone,
                Fax = c.Fax

            }).ToList();

            response.JsonData = customersVM;
            return response;
        }
        public RequestResponse GetCustomerDetails(string id)
        {
            var response = new RequestResponse();
            Customer customer = new Customer();
            customer = DBContext.Customers.Find(id);
            CustomerVM customerVM = new CustomerVM
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
            response.JsonData = customerVM;
            return response;
        }
        public RequestResponse SaveCustomer(CustomerVM customerVM)
        {

            Customer customer;
            var response = new RequestResponse();

            if (string.IsNullOrEmpty(customerVM.CustomerID))
            {
                customer = new Customer()
                {
                    CustomerID = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper(),
                    CompanyName = customerVM.CompanyName,
                    ContactName = customerVM.ContactName,
                    ContactTitle = customerVM.ContactTitle,
                    Address = customerVM.Address,
                    City = customerVM.City,
                    Region = customerVM.Region,
                    PostalCode = customerVM.PostalCode,
                    Country = customerVM.Country,
                    Phone = customerVM.Phone,
                    Fax = customerVM.Fax
                };
                DBContext.Customers.Add(customer);
            }
            else
            {
                // EDIT
                customer = DBContext.Customers.Find(customerVM.CustomerID);
                customer.CompanyName = customerVM.CompanyName;
                customer.ContactName = customerVM.ContactName;
                customer.ContactTitle = customerVM.ContactTitle;
                customer.Address = customerVM.Address;
                customer.City = customerVM.City;
                customer.Region = customerVM.Region;
                customer.PostalCode = customerVM.PostalCode;
                customer.Country = customerVM.Country;
                customer.Phone = customerVM.Phone;
                customer.Fax = customerVM.Fax;
                DBContext.Entry(customer).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;

            }

            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            return response;
        }

        public RequestResponse DeleteCustomer(string id)
        {
            var response = new RequestResponse();
            Customer customer = DBContext.Customers.Find(id);
            if (customer == null)
            {
                response.Success = false;
                response.JsonData = null;
                response.Message = "Customer not found.";
            }

            else
            {
                DBContext.Customers.Remove(customer);
                var result = DBContext.SaveChanges();
                response.Success = result != 0;
                response.JsonData = customer;
                response.Message = "Customer deleted successfully.";
            }

            return response;
        }

        public RequestResponse hasOrders(string id)
        {
            var response = new RequestResponse();
            var customer = DBContext.Customers.Find(id);

            if (customer.Orders.Any())
            {
                response.Success = false;
                response.Message = "Cannot delete this customer because it has related Orders.";
            }
            else
            {
                response.Success = true;

            }
            return response;
        }
        public RequestResponse GetContactTitles()
        {
            var response = new RequestResponse();
            response.JsonData = DBContext.Customers.Select(c => c.ContactTitle).Distinct().ToList();
            response.Success = true;
            return response;
        }

        public RequestResponse GetCities()
        {
            var response = new RequestResponse();
            response.JsonData = DBContext.Customers.Select(c => c.City).Distinct().ToList();
            response.Success = true;
            return response;
        }

        public RequestResponse GetCountries()
        {
            var response = new RequestResponse();
            response.JsonData = DBContext.Customers.Select(c => c.Country).Distinct().ToList();
            response.Success = true;
            return response;
        }
        //  DELETE ZAI YOUSSEF
        //public RequestResponse DeleteCustomer(string id)
        //{
        //    var response = new RequestResponse();
        //    var customer = DBContext.Customers.Find(id);
        //    var customerVM = new CustomerVM
        //    {
        //        CustomerID = customer.CustomerID,
        //        CompanyName = customer.CompanyName,
        //        ContactName = customer.ContactName,
        //        ContactTitle = customer.ContactTitle,
        //        Address = customer.Address,
        //        City = customer.City,
        //        Region = customer.Region,
        //        PostalCode = customer.PostalCode,
        //        Country = customer.Country,
        //        Phone = customer.Phone,
        //        Fax = customer.Fax
        //    };

        //    response.JsonData = customerVM;
        //    if (response.JsonData == null)
        //    {
        //        return response;
        //    }
        //    else
        //    {
        //        DBContext.Customers.Remove(customer);
        //        DBContext.SaveChanges();
        //        response.Success = true;
        //        return response;
        //    }


        //to check if orders exist
        //public List<OrderInfoVM> checkOrders(string id)
        //{
        //    var orders = DBContext.Orders
        //                 .Where(t => t.CustomerID == id)
        //                 .Select(e => new OrderInfoVM
        //                 {
        //                     OrderID = e.OrderID,
        //                     CompanyName = e.CustomerID
        //                 })
        //                 .ToList();

        //    return orders;
        //}

        //Populating dropdown lists


    }
}