using System.Linq;
using System.Web.Mvc;
using NorthwindTraders.Models;

namespace NorthwindTraders.Controllers
{
    public class RegionController : Controller
    {
        private readonly NorthwindEntities _context;

        public RegionController()
        {
            _context = new NorthwindEntities();
        }

        // Index
        public ActionResult Index()
        {
            var regions = _context.Regions.ToList();
            return View(regions);
        }

        [HttpPost]
        public ActionResult CreateAjax(Region region)
        {
            if (string.IsNullOrWhiteSpace(region.RegionDescription) || region.RegionDescription.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Region description must be at least 3 characters.");
            }

            // معالجة مشكلة الـ PK لو مش Identity
            if (region.RegionID == 0)
            {
                region.RegionID = _context.Regions.Any()
                    ? _context.Regions.Max(r => r.RegionID) + 1
                    : 1;
            }

            _context.Regions.Add(region);
            _context.SaveChanges();

            var regions = _context.Regions.ToList();
            return PartialView("_RegionTable", regions);
        }

        [HttpPost]
        public ActionResult EditAjax(Region region)
        {
            if (string.IsNullOrWhiteSpace(region.RegionDescription) || region.RegionDescription.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Region description must be at least 3 characters.");
            }

            var existingRegion = _context.Regions.Find(region.RegionID);
            if (existingRegion == null)
            {
                return HttpNotFound();
            }

            existingRegion.RegionDescription = region.RegionDescription;
            _context.SaveChanges();

            var regions = _context.Regions.ToList();
            return PartialView("_RegionTable", regions);
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var region = _context.Regions.Find(id);
            if (region != null)
            {
                _context.Regions.Remove(region);
                _context.SaveChanges();

                var regions = _context.Regions.ToList();
                return PartialView("_RegionTable", regions);
            }
            return Json(new { success = false, message = "Not Found" });
        }
    }
}
