using Northwind.Business;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class RegionController : Controller
    {
        private readonly RegionBusiness _regionBusiness = new RegionBusiness();

       
        [HttpGet]
        public ActionResult Index()
        {
            var regions = (List<RegionVM>)_regionBusiness.GetRegions().JsonData;

            regions = regions.OrderBy(r => r.RegionID).ToList();

            return View(regions);
        }


        [HttpPost]
        public ActionResult CreateAjax(RegionVM region)
        {
            
            if (string.IsNullOrWhiteSpace(region.RegionDescription) || region.RegionDescription.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Description must be at least 3 characters.");
            }

            var response = _regionBusiness.AddEditRegion(region);
            if (!response.Success)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = response.Message });
            }

            var regions = (List<RegionVM>)_regionBusiness.GetRegions().JsonData;
            regions = regions.OrderBy(r => r.RegionID).ToList();

            return PartialView("_RegionTable", regions);
        }

        [HttpPost]
        public ActionResult EditAjax(RegionVM region)
        {
            if (string.IsNullOrWhiteSpace(region.RegionDescription) || region.RegionDescription.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Description must be at least 3 characters.");
            }

            var response = _regionBusiness.AddEditRegion(region);
            if (!response.Success)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = response.Message });
            }

            var regions = (List<RegionVM>)_regionBusiness.GetRegions().JsonData;
            regions = regions.OrderBy(r => r.RegionID).ToList();

            return PartialView("_RegionTable", regions);
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var record = (RegionVM)_regionBusiness.GetRegion(id).JsonData;
            if (record == null)
            {
                return Json(new { success = false, message = "Region not found." });
            }

            var response = _regionBusiness.RemoveRegion(record);
            if (!response.Success)
            {
                return Json(new { success = false, message = response.Message });
            }

            return Json(new { success = true });
        }

    }
}
