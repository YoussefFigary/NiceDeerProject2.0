using NorthwindTraders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class SupplierController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        // GET: Supplier
        public ActionResult Index()
        {
            return View(); 
        }
        public ActionResult Supplier()
        {
            var Suppliers = db.Suppliers.Where(t => t != null).OrderBy(x => x.SupplierID).ToList();
            return View(Suppliers);
        }
    }
}