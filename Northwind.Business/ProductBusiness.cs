﻿using Northwind.DAL;
using Northwind.DAL.Models;
using Northwind.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business
{
    public class ProductBusiness : BaseBusiness
    {
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
            response.Success = true;
            response.JsonData = productsVM;
            return response;
        }

        public RequestResponse GetProductDetails(int id)
        {
            var response = new RequestResponse();
            Product product = new Product();
            product = DBContext.Products.Find(id);
            ProductVM productVM = new ProductVM
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

            response.JsonData = productVM;
            return response;
        }

        public RequestResponse SaveProduct(ProductVM productVM)
        {
            Product product;
            var response = new RequestResponse();

            if (productVM.ProductID == 0)
            {
                // ADD
                product = new Product()
                {
                    ProductName = productVM.ProductName,
                    SupplierID = productVM.SupplierID,
                    CategoryID = productVM.CategoryID,
                    QuantityPerUnit = productVM.QuantityPerUnit,
                    UnitPrice = productVM.UnitPrice,
                    UnitsInStock = productVM.UnitsInStock,
                    UnitsOnOrder = productVM.UnitsOnOrder,
                    ReorderLevel = productVM.ReorderLevel,
                    Discontinued = productVM.Discontinued
                };
                DBContext.Products.Add(product);
            }
            else
            {
                // EDIT
                product = DBContext.Products.Find(productVM.ProductID);

                product.ProductName = productVM.ProductName;
                product.SupplierID = productVM.SupplierID;
                product.CategoryID = productVM.CategoryID;
                product.QuantityPerUnit = productVM.QuantityPerUnit;
                product.UnitPrice = productVM.UnitPrice;
                product.UnitsInStock = productVM.UnitsInStock;
                product.UnitsOnOrder = productVM.UnitsOnOrder;
                product.ReorderLevel = productVM.ReorderLevel;
                product.Discontinued = productVM.Discontinued;

                DBContext.Entry(product).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.JsonData = product;
            return response;
        }

        public RequestResponse DeleteProduct(int id)
        {
            var response = new RequestResponse();
            Product product = DBContext.Products.Find(id);
            if (product == null)
            {
                response.Success = false;
                response.JsonData = null;
                response.Message = "Product not found.";
                return response;
            }

            DBContext.Products.Remove(product);
            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.JsonData = product;
            response.Message = "Product deleted successfully.";
            return response;
        }

        public RequestResponse hasOrders(int id)
        {
            var response = new RequestResponse();
            var product = DBContext.Products.Find(id);

            if (product.Order_Details.Any())
            {
                response.Success = false;
                response.Message = "Cannot delete this product because it has related Orders.";
            }
            else
            {
                response.Success = true;

            }
            return response;
        }
        //public RequestResponse hasOrders(int id)
        //{
        //    var response = new RequestResponse();
        //    response.Success = DBContext.Order_Details.Any(o => o.ProductID == id);
        //    response.Message = "Cannot delete this product because it has related Orders.";
        //    return response;
        //}

        //dropdown
        public RequestResponse GetSuppliers()
        {
            var response = new RequestResponse();
            List<Supplier> suppliers = DBContext.Suppliers
                .Where(s => s != null)
                .OrderBy(s => s.CompanyName)
                .ToList();

            List<SupplierVM> suppliersVM = new List<SupplierVM>();
            foreach (var s in suppliers)
            {
                suppliersVM.Add(new SupplierVM
                {
                    SupplierID = s.SupplierID,
                    CompanyName = s.CompanyName,
                    ContactName = s.ContactName,
                    ContactTitle = s.ContactTitle,
                    Address = s.Address,
                    City = s.City,
                    Region = s.Region,
                    PostalCode = s.PostalCode,
                    Country = s.Country,
                    Phone = s.Phone,
                    Fax = s.Fax,
                    HomePage = s.HomePage
                });
            }

            response.JsonData = suppliersVM;
            response.Success = true;
            return response;
        }

        public RequestResponse GetCategories()
        {
            var response = new RequestResponse();
            List<Category> categories = DBContext.Categories
                .Where(c => c != null)
                .OrderBy(c => c.CategoryName)
                .ToList();

            List<CategoryVM> categoriesVM = new List<CategoryVM>();
            foreach (var c in categories)
            {
                categoriesVM.Add(new CategoryVM
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    Picture = c.Picture
                });
            }

            response.JsonData = categoriesVM;
            response.Success = true;
            return response;
        }


    }
}