using Northwind.Business;
using NorthwindTraders.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
    {

    public class TerritoryController : Controller
    {

        TerritoryBusiness TerritoryBusiness = new TerritoryBusiness();


        public ActionResult AddOREditPopup(int id)
        {
            ViewBag.Regions = (List<RegionVM>) TerritoryBusiness.GetRegions().JsonData;
            if (id == 0)
                return PartialView("_TerritoryAddOrEdit", new TerritoryVM()); // Empty model for Add
            else
            {
                var territory = (TerritoryVM)TerritoryBusiness.GetTerritory(id).JsonData;
                return PartialView("_TerritoryAddOrEdit", territory); // Filled model for Edit
            }
        }


        [HttpPost]
        public ActionResult SaveEdit(TerritoryVM territory)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            var response = TerritoryBusiness.AddEditTerritory(territory);

            return Json(new { success = response.Success });
        }



        // to display the main Territory Table
        [HttpGet]
        public ActionResult Index()
        {
            var territories = (List<TerritoryVM>) TerritoryBusiness.GetTerritories().JsonData;
            return View(territories); 
        }

        // to check if i can delete Territory
        public List<EmployeeInfo> CheckEmployees(int id)
        {
            var employees = (List<EmployeeInfo>)TerritoryBusiness.CheckEmployees(id).JsonData;

            return employees;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var relatedRecords = CheckEmployees(id);
            if (relatedRecords.Count != 0)
            {
                Response.StatusCode = 409;
                return Json(new
                {
                    error = "Cannot delete because this record is linked to employees.",
                    linkedEmployees = relatedRecords
                }, JsonRequestBehavior.AllowGet);
            }

            var record = (TerritoryVM)TerritoryBusiness.GetTerritory(id).JsonData;
            if (record == null)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Territory not found." }, JsonRequestBehavior.AllowGet);
            }

            TerritoryBusiness.RemoveTerritory(record);

            return Json(new { success = true });
        }


        [HttpGet]
        public JsonResult GetRegions()
        {
            var regions = (List<string>)TerritoryBusiness.GetRegions().JsonData;

            return Json(regions, JsonRequestBehavior.AllowGet);
        }


        
    }

}