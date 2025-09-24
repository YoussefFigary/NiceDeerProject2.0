using Northwind.DAL.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Business
{
    public class ShipperBusiness : BaseBusiness
    {
        // Get single shipper
        public RequestResponse GetShipper(int id)
        {
            var response = new RequestResponse();
            var shipper = DBContext.Shippers.Find(id);

            if (shipper == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                response.JsonData = null;
                return response;
            }

            var shipperVM = new ShipperVM
            {
                ShipperID = shipper.ShipperID,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone
            };

            response.Success = true;
            response.JsonData = shipperVM;
            return response;
        }

        // Get all shippers
        public RequestResponse GetShippers()
        {
            var response = new RequestResponse();
            var shippers = DBContext.Shippers.OrderBy(s => s.ShipperID).ToList();

            var shippersVM = shippers.Select(s => new ShipperVM
            {
                ShipperID = s.ShipperID,
                CompanyName = s.CompanyName,
                Phone = s.Phone
            }).ToList();

            response.Success = true;
            response.JsonData = shippersVM;
            return response;
        }

        // Add or Edit shipper
        public RequestResponse AddEditShipper(ShipperVM shipperVM)
        {
            var response = new RequestResponse();

            if (shipperVM.ShipperID == 0)
            {
                // Add
                var newShipper = new Shipper
                {
                    CompanyName = shipperVM.CompanyName,
                    Phone = shipperVM.Phone
                };

                DBContext.Shippers.Add(newShipper);
            }
            else
            {
                // Edit
                var existing = DBContext.Shippers.SingleOrDefault(s => s.ShipperID == shipperVM.ShipperID);
                if (existing == null)
                {
                    response.Success = false;
                    response.HttpStatus = "404";
                    response.JsonData = null;
                    return response;
                }

                existing.CompanyName = shipperVM.CompanyName;
                existing.Phone = shipperVM.Phone;

                DBContext.Entry(existing).State = System.Data.Entity.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result > 0;
            response.HttpStatus = "200";
            return response;
        }

        // Delete shipper
        public RequestResponse RemoveShipper(int id)
        {
            var response = new RequestResponse();

            var shipper = DBContext.Shippers.Find(id);
            if (shipper == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                return response;
            }

            DBContext.Shippers.Remove(shipper);
            DBContext.SaveChanges();

            response.Success = true;
            response.HttpStatus = "200";
            return response;
        }
    }
}
