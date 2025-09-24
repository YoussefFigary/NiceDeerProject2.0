using Northwind.DAL.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business
{
    public class TerritoryBusiness : BaseBusiness
    {

        public RequestResponse GetTerritory(int id)
        {
            var response = new RequestResponse();
            Territory territory;
            TerritoryVM territorysVM;
            territory = DBContext.Territories.Find(id);
            territorysVM = new TerritoryVM()
            {
                RegionID = territory.RegionID,
                TerritoryDescription = territory.TerritoryDescription,
                TerritoryID = territory.TerritoryID
               
            };
            response.JsonData = territorysVM;
            response.Success = true;

            return response;
        }
        public RequestResponse GetRegions()
        {
            var response = new RequestResponse();
            List<Region> regions = DBContext.Regions.ToList();
            List<RegionVM> regionsVM = new List<RegionVM>();
            foreach (Region r in regions)
            {
                RegionVM regionVM = new RegionVM()
                {
                    RegionID = r.RegionID,
                    RegionDescription = r.RegionDescription

                };
                regionsVM.Add(regionVM);
            }


            response.Success = true;
            response.JsonData = regionsVM;
            return response;


        
        }

        public RequestResponse GetTerritories()
        {
            var response = new RequestResponse();
            List<Territory> territories = new List<Territory>();
            List<TerritoryVM> territoriesVM = new List<TerritoryVM>();
            territories = DBContext.Territories.Where(t => t != null).OrderBy(x => x.TerritoryID).ToList();
            foreach (Territory territory in territories)
            {
                TerritoryVM territoryVM = new TerritoryVM()
                {

                    RegionID = territory.RegionID,
                    TerritoryDescription = territory.TerritoryDescription,
                    TerritoryID = territory.TerritoryID,
                    RegionName = territory.Region.RegionDescription
                    

                };
                territoriesVM.Add(territoryVM);
            }
            response.JsonData = territoriesVM;
            response.Success = true;

            return response;
        }

        public RequestResponse AddEditTerritory(TerritoryVM territoryVM)
        {
            var response = new RequestResponse();
            Territory territory;
            if (territoryVM.TerritoryID == 0)
            {
                territory = new Territory()
                {
                    RegionID = territoryVM.RegionID,
                    TerritoryDescription = territoryVM.TerritoryDescription,
                    TerritoryID = territoryVM.TerritoryID
                };

                DBContext.Territories.Add(territory);
            }
            else
            {
                territory = DBContext.Territories.SingleOrDefault(c => c.TerritoryID == territoryVM.TerritoryID);

                territory.TerritoryID = territoryVM.TerritoryID;
                territory.RegionID = territoryVM.RegionID;
                territory.TerritoryDescription = territoryVM.TerritoryDescription;




                //  db.Entry(supplier).State = EntityState.Modified;

                DBContext.Entry(territory).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.HttpStatus = "200";
            return response;
        }

        public RequestResponse RemoveTerritory(TerritoryVM territoryVM)
        {
            var response = new RequestResponse();

            Territory territory = DBContext.Territories.Find(territoryVM.TerritoryID);
            
            DBContext.Territories.Remove(territory);
            DBContext.SaveChanges();
            response.Success = true;
            return response;

        }
        public RequestResponse CheckEmployees(int id)
        {
            var response = new RequestResponse();
            List<EmployeeInfo> employees;

            employees = DBContext.Territories
                              .Where(t => t.TerritoryID == id)
                              .SelectMany(t => t.Employees)
                              .Select(e => new EmployeeInfo
                              {
                                  EmployeeID = e.EmployeeID,
                                  FullName = e.FirstName + " " + e.LastName
                              })
                              .ToList();


            response.JsonData = employees;
            response.Success = true;

            return response;
        }
    }
}
