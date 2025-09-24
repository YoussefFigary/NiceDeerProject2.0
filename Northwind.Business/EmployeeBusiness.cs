using Northwind.DAL.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;




namespace Northwind.Business
{
    public class EmployeeBusiness : BaseBusiness
    {
        public RequestResponse AddEditEmployee(EmployeeVM employeeVM)
        {
            var response = new RequestResponse();
            Employee emplyee;
            if (employeeVM.EmployeeID == 0)
            {
                emplyee = new Employee()
                {
                    FirstName = employeeVM.FirstName,
                    LastName = employeeVM.LastName,
                    Title = employeeVM.Title,
                    TitleOfCourtesy = employeeVM.TitleOfCourtesy,
                    HireDate = employeeVM.HireDate,
                    Region = employeeVM.Region,
                    PostalCode = employeeVM.PostalCode,
                    HomePhone = employeeVM.HomePhone,
                    Extension = employeeVM.Extension,
                    Photo = employeeVM.Photo,
                    Notes = employeeVM.Notes,
                    ReportsTo = employeeVM.ReportsTo,
                    PhotoPath = employeeVM.PhotoPath,
                    Address = employeeVM.Address,
                    City = employeeVM.City,
                    BirthDate = employeeVM.BirthDate,
                    Country = employeeVM.Country
                };

                DBContext.Employees.Add(emplyee);
            }
            else
            {
                emplyee = DBContext.Employees.SingleOrDefault(c => c.EmployeeID == employeeVM.EmployeeID);

                emplyee.FirstName = employeeVM.FirstName;
                emplyee.LastName = employeeVM.LastName;
                emplyee.Title = employeeVM.Title;
                emplyee.TitleOfCourtesy = employeeVM.TitleOfCourtesy;
                emplyee.HireDate = employeeVM.HireDate;
                emplyee.Region = employeeVM.Region;
                emplyee.PostalCode = employeeVM.PostalCode;
                emplyee.HomePhone = employeeVM.HomePhone;
                emplyee.Extension = employeeVM.Extension;
                emplyee.Photo = employeeVM.Photo;
                emplyee.Notes = employeeVM.Notes;
                emplyee.ReportsTo = employeeVM.ReportsTo;
                emplyee.PhotoPath = employeeVM.PhotoPath;
                emplyee.Address = employeeVM.Address;
                emplyee.City = employeeVM.City;
                emplyee.BirthDate = employeeVM.BirthDate;
                emplyee.Country = employeeVM.Country;




                //  db.Entry(supplier).State = EntityState.Modified;

                DBContext.Entry(emplyee).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result != 0;
            response.HttpStatus = "200"; 
            return response;
        }

        public RequestResponse GetEmployees()
        {
            var response = new RequestResponse();
            List<Employee> employees = new List<Employee>();
            List<EmployeeVM> employeesVM = new List<EmployeeVM>();
            employees = DBContext.Employees.Where(t => t != null).OrderBy(x => x.EmployeeID).ToList();
            foreach (Employee employee in employees) {
                EmployeeVM employeevm = new EmployeeVM()
                {
                    EmployeeID = employee.EmployeeID,
                    FirstName = employee.FirstName ,
                    LastName = employee.LastName,
                    Title = employee.Title,
                    TitleOfCourtesy = employee.TitleOfCourtesy,
                    HireDate = employee.HireDate,
                    Region =employee.Region,
                    PostalCode = employee.PostalCode,
                    HomePhone = employee.HomePhone,
                    Extension = employee.Extension,
                    Photo = employee.Photo,
                    Notes = employee.Notes,
                    ReportsTo = employee.ReportsTo,
                    ReportsToName = employee.Employee1?.FirstName,
                    PhotoPath = employee.PhotoPath,
                    Address = employee.Address,
                    City = employee.City,
                    BirthDate = employee.BirthDate,
                    Country = employee.Country
                };
                employeesVM.Add(employeevm);
            }
            response.JsonData = employeesVM;
            response.Success = true;

            return response;
        }

        public RequestResponse GetEmployee(int id)
        {
            var response = new RequestResponse();
            Employee employee;
            EmployeeVM employeesVM;
            employee = DBContext.Employees.Find(id);
            employeesVM = new EmployeeVM()
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.Title,
                TitleOfCourtesy = employee.TitleOfCourtesy,
                HireDate = employee.HireDate,
                Region = employee.Region,
                PostalCode = employee.PostalCode,
                HomePhone = employee.HomePhone,
                Extension = employee.Extension,
                Photo = employee.Photo,
                Notes = employee.Notes,
                ReportsTo = employee.ReportsTo,
                PhotoPath = employee.PhotoPath,
                Address = employee.Address,
                City = employee.City,
                BirthDate = employee.BirthDate,
                Country = employee.Country
            };
            response.JsonData = employeesVM;
            response.Success = true;

            return response;
        }

        public RequestResponse GetTitlesOfCourte()
        {
            var response = new RequestResponse();
            List<String> titles = new List<String>();
            titles.Add("Ms.");
            titles.Add("Mr.");
            titles.Add("Dr.");
            titles.Add("Mrs.");
            titles.Add("Miss.");
            titles.Add("Eng.");
            response.Success = true;
            response.JsonData = titles;
            return response;
        }

        public RequestResponse GetJobTitles()
        {
            var response = new RequestResponse();
            List<String> titles = new List<String>();
            titles.Add("Sales Representative");
            titles.Add("Vice President, Sales");
            titles.Add("Sales Manager");
            titles.Add("Inside Sales Coordinator");
            titles.Add("Team Leader, Support");
            titles.Add("Team Leader, Development");
            response.Success = true;
            response.JsonData = titles;
            return response;
        }
        public RequestResponse GetCountry()
        {
            var response = new RequestResponse();
            List<String> Countries = new List<String>();
            Countries.Add("USA");
            Countries.Add("UK");
            Countries.Add("Egypt");
            Countries.Add("Germany");
            Countries.Add("France");
            Countries.Add("Mexico");
            Countries.Add("Russia");
            Countries.Add("Italy");
            Countries.Add("Spain");
            Countries.Add("Japan");
            Countries.Add("China");
            Countries.Add("India");
            Countries.Add("Poland");
            Countries.Add("Australia");
            Countries.Add("Brazil");
            response.Success = true;
            response.JsonData = Countries;
            return response;

        }
        public RequestResponse GetCities()
        {
            var response = new RequestResponse();
            List<String> Cities = new List<String>();

            // USA
            Cities.Add("New York");
            Cities.Add("Los Angeles");
            Cities.Add("Chicago");

            // UK
            Cities.Add("London");
            Cities.Add("Manchester");
            Cities.Add("Birmingham");

            // Egypt
            Cities.Add("Cairo");
            Cities.Add("Alexandria");
            Cities.Add("Giza");

            // Germany
            Cities.Add("Berlin");
            Cities.Add("Munich");
            Cities.Add("Hamburg");

            // France
            Cities.Add("Paris");
            Cities.Add("Lyon");
            Cities.Add("Marseille");

            // Mexico
            Cities.Add("Mexico City");
            Cities.Add("Guadalajara");
            Cities.Add("Monterrey");

            // Russia
            Cities.Add("Moscow");
            Cities.Add("Saint Petersburg");
            Cities.Add("Novosibirsk");

            // Italy
            Cities.Add("Rome");
            Cities.Add("Milan");
            Cities.Add("Naples");

            // Spain
            Cities.Add("Madrid");
            Cities.Add("Barcelona");
            Cities.Add("Valencia");

            // Japan
            Cities.Add("Tokyo");
            Cities.Add("Osaka");
            Cities.Add("Kyoto");

            // China
            Cities.Add("Beijing");
            Cities.Add("Shanghai");
            Cities.Add("Guangzhou");

            // India
            Cities.Add("Mumbai");
            Cities.Add("Delhi");
            Cities.Add("Bangalore");

            // Poland
            Cities.Add("Warsaw");
            Cities.Add("Krakow");
            Cities.Add("Gdansk");

            // Australia
            Cities.Add("Sydney");
            Cities.Add("Melbourne");
            Cities.Add("Brisbane");

            // Brazil
            Cities.Add("São Paulo");
            Cities.Add("Rio de Janeiro");
            Cities.Add("Brasília");

            response.Success = true;
            response.JsonData = Cities;
            return response;
        }

        public RequestResponse CheckManager(int id)
        {
            var response = new RequestResponse();
            List<EmployeeInfo> employees;
            
            employees = DBContext.Employees
                .Where(t => t.ReportsTo == id)
                .Select(e => new EmployeeInfo{
                EmployeeID = e.EmployeeID,
                FullName = e.FirstName + " " + e.LastName
                }).ToList(); 
                         
          
            response.JsonData = employees;
            response.Success = true;

            return response;
        }
        public RequestResponse CheckOrders(int id)
        {
            var response = new RequestResponse();
            List<OrderInfo> orders;

            orders = DBContext.Orders
                         .Where(t => t.EmployeeID == id)
                         .Select(e => new OrderInfo
                         {
                             OrderID = e.OrderID,
                             CompanyName = e.CustomerID
                         })
                         .ToList();


            response.JsonData = orders;
            response.Success = true;

            return response;
        }
        public RequestResponse RemoveEmployee(EmployeeVM employeeVM)
        {
            var response = new RequestResponse();

            var employee = DBContext.Employees.Find(employeeVM.EmployeeID);
            if (employee == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                response.Message = "Employee not found.";
                return response;
            }

            DBContext.Employees.Remove(employee);
            DBContext.SaveChanges();

            response.Success = true;
            return response;
        }

    

    }
}
