using System.Linq;
using System.Web.Mvc;
using NorthwindTraders.Models;

namespace NorthwindTraders.Controllers
{
    public class ShipperController : Controller
    {
        private readonly NorthwindEntities _context;

        public ShipperController()
        {
            _context = new NorthwindEntities();
        }

        // Index
        public ActionResult Index()
        {
            var shippers = _context.Shippers.ToList();
            return View(shippers);
        }
        [HttpPost]
        public ActionResult CreateAjax(Shipper shipper)
        {
            if (string.IsNullOrWhiteSpace(shipper.CompanyName) || shipper.CompanyName.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Company name must be at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(shipper.Phone) ||
                !System.Text.RegularExpressions.Regex.IsMatch(shipper.Phone, @"^[0-9+\-() ]{5,}$"))
            {
                return new HttpStatusCodeResult(400, "Invalid phone number format.");
            }

            _context.Shippers.Add(shipper);
            _context.SaveChanges();

            var shippers = _context.Shippers.ToList();
            return PartialView("_ShipperTable", shippers);
        }

        [HttpPost]
        public ActionResult EditAjax(Shipper shipper)
        {
            if (string.IsNullOrWhiteSpace(shipper.CompanyName) || shipper.CompanyName.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Company name must be at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(shipper.Phone) ||
                !System.Text.RegularExpressions.Regex.IsMatch(shipper.Phone, @"^[0-9+\-() ]{5,}$"))
            {
                return new HttpStatusCodeResult(400, "Invalid phone number format.");
            }

            _context.Entry(shipper).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            var shippers = _context.Shippers.ToList();
            return PartialView("_ShipperTable", shippers);
        }


        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var shipper = _context.Shippers.Find(id);
            if (shipper != null)
            {
                _context.Shippers.Remove(shipper);
                _context.SaveChanges();

                var shippers = _context.Shippers.ToList();
                return PartialView("_ShipperTable", shippers);
            }
            return Json(new { success = false, message = "Not Found" });
        }
    }
}
