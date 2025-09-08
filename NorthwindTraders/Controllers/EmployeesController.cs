using NorthwindTraders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class EmployeesController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        // GET: Employees
        public ActionResult Index()
        {
            var Employee = db.Employees.Where(t => t != null).OrderBy(x => x.EmployeeID).ToList();
            return View(Employee);
        }

        // needs validation
        public int ManagerName(String  name)
        {
            var managerid = db.Employees.FirstOrDefault(t => t.FirstName +" "+t.LastName == name);

            return managerid.EmployeeID;

            
        }
        public String GetReportsTo(int id)
        {
            var manager = db.Employees.FirstOrDefault(t => t.EmployeeID == id);
            var managerName = manager.FirstName +" "+manager.LastName;
            return managerName ?? "He is the Boss";
            /*
            var managerName = db.Employees
           .Where(e => e.EmployeeID == id && e.ReportsTo != null)
           .Join(
               db.Employees,
               emp => emp.ReportsTo,
               mgr => mgr.EmployeeID,
               (emp, mgr) => mgr.FirstName + " " + mgr.LastName
           )
           .FirstOrDefault();

            return managerName ?? "He is the Boss"; 
            */
        }
        public ActionResult AddOrEditPopup(int id)
        {
            ViewBag.TitleOfCourtes = db.Employees.Select(c => c.TitleOfCourtesy).Distinct().ToList();
            ViewBag.Titles = db.Employees.Select(c => c.Title).Distinct().ToList();
            ViewBag.Cities = db.Employees.Select(c => c.City).Distinct().ToList();
            ViewBag.Countries = db.Employees.Select(c => c.Country).Distinct().ToList();
            if (id == 0)
                return PartialView("_EmployeeAddOrEdit", new Employee()); // Empty model for Add
            else
            {
                var employee = db.Employees.Find(id);
                return PartialView("_EmployeeAddOrEdit", employee); // Filled model for Edit
            }
        }

        [HttpPost]
        public ActionResult SaveEdit(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            // Try to find existing Supplier
            var existing = db.Employees.Find(employee.EmployeeID);


            if (employee.EmployeeID == 0)
            {
                //ADD


                db.Employees.Add(employee);
            }
            else
            {
                // EDIT

                db.Entry(existing).CurrentValues.SetValues(employee);
                //  db.Entry(supplier).State = EntityState.Modified;

            }

            db.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult Details(int id)
        {
            var employee = db.Employees.Find(id);
            return PartialView("_DetailsPopup", employee);
        }

    }
}

