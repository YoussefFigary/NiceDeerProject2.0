using NorthwindTraders.Models;
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
        
        private NorthwindEntities db = new NorthwindEntities();


        public ActionResult AddOREditPopup(int id)
        {
            ViewBag.Regions = db.Regions.ToList();
            if (id == 0)
                return PartialView("_TerritoryAddOrEdit", new Territory()); // Empty model for Add
            else
            {
                var territory = db.Territories.Find(id);
                return PartialView("_TerritoryAddOrEdit", territory); // Filled model for Edit
            }
        }

        [HttpPost]
        public JsonResult AddOREdit(Territory model)
        {
            if (model.TerritoryID == 0)
            {
                db.Territories.Add(model);
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new { success = true });
        }



        // to display the main Territory Table
        [HttpGet]
        public ActionResult Territory()
        {
            var territories = db.Territories.Where(t => t != null).OrderBy(x => x.TerritoryID).ToList();
            return View(territories); 
        }

        // to check if i can delete Territory
        public List<EmployeeInfo> CheckEmployees(int id)
        {
            var employees = db.Territories
                              .Where(t => t.TerritoryID == id)
                              .SelectMany(t => t.Employees)
                              .Select(e => new EmployeeInfo
                              {
                                  EmployeeID = e.EmployeeID,
                                  FullName = e.FirstName + " " + e.LastName
                              })
                              .ToList();

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

            var record = db.Territories.Find(id);
            if (record == null)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Territory not found." }, JsonRequestBehavior.AllowGet);
            }

            db.Territories.Remove(record);
            db.SaveChanges();

            return Json(new { success = true });
        }







        /*
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var territory = db.Territories.Find(id);
            if (territory == null)
            {
                return HttpNotFound();
            }

            db.Territories.Remove(territory);
            db.SaveChanges();
            // will change to a confermation pop up 
            return RedirectToAction("Index");
        }
        */

        [HttpGet]
        public JsonResult GetRegions()
        {
            var regions = db.Regions
                .Select(r => new { r.RegionID, r.RegionDescription })
                .ToList();

            return Json(regions, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // edits the Territory field 
        public ActionResult Edit(Territory updated)
        {
            if (updated == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var territory = db.Territories.Find(updated.TerritoryID);
            if (territory == null)
                return HttpNotFound();

            territory.TerritoryDescription = updated.TerritoryDescription;
            territory.RegionID = updated.RegionID;

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }

}