using Northwind.DAL.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business
{
    public class SupplierBusiness : BaseBusiness
    {
        public RequestResponse GetSuppliers()
        {
            var response = new RequestResponse();
            List<Supplier> suppliers = new List<Supplier>();
            List<SupplierVM> suppliersVM = new List<SupplierVM>();
            suppliers = DBContext.Suppliers.Where(t => t != null).OrderBy(x => x.SupplierID).ToList();
            foreach (Supplier supplier in suppliers)
            {
                SupplierVM suppliervm = new SupplierVM()
                {
                    SupplierID = supplier.SupplierID,
                    CompanyName = supplier.CompanyName,
                    ContactName = supplier.ContactName,
                    ContactTitle = supplier.ContactTitle,
                    Address = supplier.Address,
                    Region = supplier.Region,
                    PostalCode = supplier.PostalCode,
                    Phone = supplier.Phone,
                    Fax = supplier.Fax,
                    HomePage = supplier.HomePage,
                    City = supplier.City,
                    Country = supplier.Country
                };
                suppliersVM.Add(suppliervm);
            }
            response.JsonData = suppliersVM;
            response.Success = true;

            return response;
        }

        public RequestResponse GetSupplier(int id)
        {
            var response = new RequestResponse();
            Supplier supplier;
            SupplierVM suppliersVM;
            supplier = DBContext.Suppliers.Find(id);
            suppliersVM = new SupplierVM()
            {
                SupplierID = supplier.SupplierID,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle,
                Address = supplier.Address,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                HomePage = supplier.HomePage,
                City = supplier.City,
                Country = supplier.Country
            };
            response.JsonData = suppliersVM;
            response.Success = true;

            return response;
        }

        public RequestResponse RemoveSupplier(SupplierVM supplierVM)
        {
            var response = new RequestResponse();

            Supplier supplier = DBContext.Suppliers.Find(supplierVM.SupplierID);

            DBContext.Suppliers.Remove(supplier);

            DBContext.Suppliers.Remove(supplier);
            DBContext.SaveChanges();
            response.Success = true;
            return response;

        }

        public RequestResponse CheckProducts(int id)
        {
            var response = new RequestResponse();
            List<ProductInfo> products;

            products = DBContext.Suppliers
                              .Where(t => t.SupplierID == id)
                              .SelectMany(t => t.Products)
                              .Select(e => new ProductInfo
                              {
                                  ProductID = e.ProductID,
                                  ProductName = e.ProductName
                              })
                              .ToList();


            response.JsonData = products;
            response.Success = true;

            return response;
        }

        public RequestResponse GetContactTitle()
        {
            var response = new RequestResponse();
            List<String> ContactTitles = new List<String>();
            ContactTitles.Add("Purchasing Manager");
            ContactTitles.Add("Order Administrator");
            ContactTitles.Add("Sales Representative");
            ContactTitles.Add("Marketing Manager");
            ContactTitles.Add("Export Administrator");
            ContactTitles.Add("Marketing Representative");
            ContactTitles.Add("Sales Agent");
            ContactTitles.Add("Sales Manager");
            ContactTitles.Add("International Marketing Mgr.");
            ContactTitles.Add("Coordinator Foreign Markets");
            ContactTitles.Add("Regional Account Rep.");
            ContactTitles.Add("Wholesale Account Agent");
            ContactTitles.Add("Owner");
            ContactTitles.Add("Accounting Manager");
            ContactTitles.Add("Product Manager");
            ContactTitles.Add("Order Administrator");

            response.Success = true;
            response.JsonData = ContactTitles;
            return response;
        }

        public RequestResponse GetCountry()
        {
            var response = new RequestResponse();
            List<String> Countries = new List<String>();

            Countries.Add("UK");
            Countries.Add("USA");
            Countries.Add("Japan");
            Countries.Add("Spain");
            Countries.Add("Australia");
            Countries.Add("Sweden");
            Countries.Add("Brazil");
            Countries.Add("Germany");
            Countries.Add("Italy");
            Countries.Add("Norway");
            Countries.Add("France");
            Countries.Add("Singapore");
            Countries.Add("Denmark");
            Countries.Add("Netherlands");
            Countries.Add("Finland");
            Countries.Add("Canada");


            response.Success = true;
            response.JsonData = Countries;
            return response;
        }


        public RequestResponse GetCities()
        {
            var response = new RequestResponse();
            List<String> Cities = new List<String>();

            Cities.Add("Manchester");
            Cities.Add("New Orleans");
            Cities.Add("Ann Arbor");
            Cities.Add("Tokyo");
            Cities.Add("Oviedo");
            Cities.Add("Osaka");
            Cities.Add("Melbourne");
            Cities.Add("Göteborg");
            Cities.Add("Sao Paulo");
            Cities.Add("Berlin");
            Cities.Add("Frankfurt");
            Cities.Add("Cuxhaven");
            Cities.Add("Ravenna");
            Cities.Add("Sandvika");
            Cities.Add("Bend");
            Cities.Add("Stockholm");
            Cities.Add("Paris");
            Cities.Add("Boston");
            Cities.Add("Singapore");
            Cities.Add("Lyngby");
            Cities.Add("Zaandam");
            Cities.Add("Lappeenranta");
            Cities.Add("Sydney");
            Cities.Add("Montréal");
            Cities.Add("Salerno");
            Cities.Add("Montceau");
            Cities.Add("Annecy");
            Cities.Add("Ste-Hyacinthe");

            response.Success = true;
            response.JsonData = Cities;
            return response;
        }

        public RequestResponse AddEditSupplier(SupplierVM supplierVM)
        {
            var response = new RequestResponse();
            Supplier supplier;
            if (supplierVM.SupplierID == 0)
            {
                supplier = new Supplier()
                {
                    CompanyName = supplierVM.CompanyName,
                    ContactName = supplierVM.ContactName,
                    ContactTitle = supplierVM.ContactTitle,
                    Address = supplierVM.Address,
                    Region = supplierVM.Region,
                    PostalCode = supplierVM.PostalCode,
                    Phone = supplierVM.Phone,
                    Fax = supplierVM.Fax,
                    HomePage = supplierVM.HomePage,
                    City = supplierVM.City,
                    Country = supplierVM.Country

                };

                DBContext.Suppliers.Add(supplier);
            }
            else
            {
                supplier = DBContext.Suppliers.SingleOrDefault(c => c.SupplierID == supplierVM.SupplierID);

                supplier.CompanyName = supplierVM.CompanyName;
                supplier.ContactName = supplierVM.ContactName;
                supplier.ContactTitle = supplierVM.ContactTitle;
                supplier.Address = supplierVM.Address;
                supplier.Region = supplierVM.Region;
                supplier.PostalCode = supplierVM.PostalCode;
                supplier.Phone = supplierVM.Phone;
                supplier.Fax = supplierVM.Fax;
                supplier.HomePage = supplierVM.HomePage;
                supplier.City = supplierVM.City;
                supplier.Country = supplierVM.Country;




                //  db.Entry(supplier).State = EntityState.Modified;

                DBContext.Entry(supplier).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.HttpStatus = "200";
            return response;
        }

    }
}
