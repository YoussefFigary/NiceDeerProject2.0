using Northwind.Business;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class EmployeesController : Controller
    {
        EmployeeBusiness EmployeeBusiness = new EmployeeBusiness();
        
        public ActionResult Index()
        {
            // var Employee = db.Employees.Where(t => t != null).OrderBy(x => x.EmployeeID).ToList();
            var response = EmployeeBusiness.GetEmployees();
            if (response.Success)
            {
                return View((List<EmployeeVM>)response.JsonData);
            }
            return View();
           
        }

       /*
        public int ManagerName(String  name)
        {
            var managerid = db.Employees.FirstOrDefault(t => t.FirstName +" "+t.LastName == name
            return managerid.EmployeeID
        }
       */
    /*
        public String GetReportsTo(int id)
        {
            var manager = db.Employees.FirstOrDefault(t => t.EmployeeID == id);
            var managerName = manager.FirstName +" "+manager.LastName;
            return managerName ?? "He is the Boss";
        
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
           
        } */
        public ActionResult AddOrEditPopup(int id)
        {
            ViewBag.TitleOfCourtes = (List<String>)EmployeeBusiness.GetTitlesOfCourte().JsonData;
            ViewBag.Titles = (List<String>)EmployeeBusiness.GetJobTitles().JsonData;
            ViewBag.Cities = (List<String>)EmployeeBusiness.GetCities().JsonData;
            ViewBag.Countries = (List<String>)EmployeeBusiness.GetCountry().JsonData;
            if (id == 0)
                return PartialView("_EmployeeAddOrEdit", new EmployeeVM()); // Empty model for Add
            else
            {
                var response= EmployeeBusiness.GetEmployee(id);


                return PartialView("_EmployeeAddOrEdit", response.JsonData); // Filled model for Edit
            }
        }

        [HttpPost]
        public ActionResult SaveEdit(EmployeeVM employee)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = new[] { "Invalid model state" } });
            }

            var response = EmployeeBusiness.AddEditEmployee(employee);
           
            return Json(new { success = response.Success });
        }
        public ActionResult Details(int id)
        {
            var employee = (EmployeeVM)EmployeeBusiness.GetEmployee(id).JsonData;
            return PartialView("_DetailsPopup", employee);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var relatedemployees = CheckEmployees(id);
            var relatedorders = checkOrders(id);
            if (relatedemployees.Count != 0)
            {
                Response.StatusCode = 409;
                return Json(new
                {
                    error = "Cannot delete because another Employee reports to this one",
                    linkedemployees = relatedemployees
                }, JsonRequestBehavior.AllowGet);
            }
            if (relatedorders.Count != 0)
            {
                Response.StatusCode = 409;
                return Json(new
                {
                    error = "Cannot delete because an order is linked to this Employee",
                    linkedorders = relatedorders
                }, JsonRequestBehavior.AllowGet); 
            }

            var record =(EmployeeVM) EmployeeBusiness.GetEmployee(id).JsonData;
            if (record == null)
            {
                Response.StatusCode = 404;
                return Json(new { error = "Employee not found." }, JsonRequestBehavior.AllowGet);
            }

            EmployeeBusiness.RemoveEmployee(record);

            return Json(new { success = true });
        }
        public List<EmployeeInfo> CheckEmployees(int id)
        {
            List<EmployeeInfo> employees = (List<EmployeeInfo>)EmployeeBusiness.CheckManager(id).JsonData;
                         

            return employees;
        }
        public List<OrderInfo> checkOrders(int id)
        {
            List<OrderInfo> orders =(List<OrderInfo>)EmployeeBusiness.CheckOrders(id).JsonData;

            return orders;
        }

    }
}

