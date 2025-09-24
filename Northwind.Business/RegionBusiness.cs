using Northwind.DAL.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Business
{
    public class RegionBusiness : BaseBusiness
    {
        // ✅ Get single Region
        public RequestResponse GetRegion(int id)
        {
            var response = new RequestResponse();
            var region = DBContext.Regions.Find(id);

            if (region == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                response.Message = "Region not found.";
                return response;
            }

            var regionVM = new RegionVM()
            {
                RegionID = region.RegionID,
                RegionDescription = region.RegionDescription
            };

            response.JsonData = regionVM;
            response.Success = true;
            return response;
        }

        // ✅ Get all Regions
        public RequestResponse GetRegions()
        {
            var response = new RequestResponse();
            var regions = DBContext.Regions
                                   .OrderBy(r => r.RegionID)
                                   .Select(r => new RegionVM
                                   {
                                       RegionID = r.RegionID,
                                       RegionDescription = r.RegionDescription
                                   })
                                   .ToList();

            response.JsonData = regions;
            response.Success = true;
            return response;
        }

        // ✅ Add or Edit Region
        public RequestResponse AddEditRegion(RegionVM regionVM)
        {
            var response = new RequestResponse();

            if (regionVM.RegionID == 0) // Add
            {
               
                int newId = DBContext.Regions.Any()
                            ? DBContext.Regions.Max(r => r.RegionID) + 1
                            : 1;

                var region = new Region()
                {
                    RegionID = newId,
                    RegionDescription = regionVM.RegionDescription
                };

                DBContext.Regions.Add(region);
            }
            else // Edit
            {
                var region = DBContext.Regions
                                      .SingleOrDefault(r => r.RegionID == regionVM.RegionID);

                if (region == null)
                {
                    response.Success = false;
                    response.HttpStatus = "404";
                    response.Message = "Region not found.";
                    return response;
                }

                region.RegionDescription = regionVM.RegionDescription;

                DBContext.Entry(region).State = System.Data.Entity.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result > 0;
            response.HttpStatus = "200";
            return response;
        }

        // ✅ Remove Region
        public RequestResponse RemoveRegion(RegionVM regionVM)
        {
            var response = new RequestResponse();

            var region = DBContext.Regions.Find(regionVM.RegionID);
            if (region == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                response.Message = "Region not found.";
                return response;
            }

            DBContext.Regions.Remove(region);
            var result = DBContext.SaveChanges();

            response.Success = result > 0;
            response.HttpStatus = "200";
            return response;
        }
    }
}
