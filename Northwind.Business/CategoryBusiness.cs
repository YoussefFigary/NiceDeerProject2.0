using Northwind.DAL.Models;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Business
{
    public class CategoryBusiness : BaseBusiness
    {
   
        public RequestResponse GetCategory(int id)
        {
            var response = new RequestResponse();
            var category = DBContext.Categories.Find(id);

            if (category == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                response.Message = "Category not found.";
                return response;
            }

            var categoryVM = new CategoryVM()
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Description = category.Description
            };

            response.JsonData = categoryVM;
            response.Success = true;
            return response;
        }

        
        public RequestResponse GetCategories()
        {
            var response = new RequestResponse();
            var categories = DBContext.Categories
                                      .OrderBy(c => c.CategoryID)
                                      .Select(c => new CategoryVM
                                      {
                                          CategoryID = c.CategoryID,
                                          CategoryName = c.CategoryName,
                                          Description = c.Description
                                      })
                                      .ToList();

            response.JsonData = categories;
            response.Success = true;
            return response;
        }

       
        public RequestResponse AddEditCategory(CategoryVM categoryVM)
        {
            var response = new RequestResponse();

            if (categoryVM.CategoryID == 0) // Add
            {
                var category = new Category()
                {
                    CategoryName = categoryVM.CategoryName,
                    Description = categoryVM.Description
                };

                DBContext.Categories.Add(category);
            }
            else // Edit
            {
                var category = DBContext.Categories
                                        .SingleOrDefault(c => c.CategoryID == categoryVM.CategoryID);

                if (category == null)
                {
                    response.Success = false;
                    response.HttpStatus = "404";
                    response.Message = "Category not found.";
                    return response;
                }

                category.CategoryName = categoryVM.CategoryName;
                category.Description = categoryVM.Description;

                DBContext.Entry(category).State = System.Data.Entity.EntityState.Modified;
            }

            var result = DBContext.SaveChanges();
            response.Success = result > 0;
            response.HttpStatus = "200";
            return response;
        }

 
        public RequestResponse RemoveCategory(CategoryVM categoryVM)
        {
            var response = new RequestResponse();

            var category = DBContext.Categories.Find(categoryVM.CategoryID);
            if (category == null)
            {
                response.Success = false;
                response.HttpStatus = "404";
                response.Message = "Category not found.";
                return response;
            }

            DBContext.Categories.Remove(category);
            var result = DBContext.SaveChanges();

            response.Success = result > 0;
            response.HttpStatus = "200";
            return response;
        }
    }
}
