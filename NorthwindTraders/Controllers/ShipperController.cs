using Northwind.Business;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class ShipperController : Controller
    {
        private readonly ShipperBusiness _shipperBusiness = new ShipperBusiness(); // Renamed to avoid ambiguity

        [HttpGet]
        public ActionResult Index()
        {
            var shippers = (List<ShipperVM>)_shipperBusiness.GetShippers().JsonData;
            return View(shippers); 
        }



        public ActionResult AddOrEditPopup(int id)
        {
            if (id == 0)
                return PartialView("_ShipperAddOrEdit", new ShipperVM()); // Add
            else
            {
                var shipper = (ShipperVM)_shipperBusiness.GetShipper(id).JsonData;
                return PartialView("_ShipperAddOrEdit", shipper); // Edit
            }
        }

        // ✅ Save or Edit
        [HttpPost]
        public ActionResult SaveEdit(ShipperVM shipper)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            var response = _shipperBusiness.AddEditShipper(shipper);

            return Json(new { success = response.Success });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var response = _shipperBusiness.RemoveShipper(id);

            if (!response.Success)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Shipper not found." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true });
        }
    }
}
