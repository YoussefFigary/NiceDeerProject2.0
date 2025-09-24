using Northwind.Business;
using Northwind.DTO;
using NorthwindTraders.Models.DTOs;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NorthwindTraders.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryBusiness _categoryBusiness = new CategoryBusiness();

        
        [HttpGet]
        public ActionResult Index()
        {
            var categories = (List<CategoryVM>)_categoryBusiness.GetCategories().JsonData;
            return View(categories);
        }

        
        [HttpPost]
        public ActionResult CreateAjax(CategoryVM category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Category name must be at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(category.Description))
            {
                return new HttpStatusCodeResult(400, "Description is required.");
            }

            var response = _categoryBusiness.AddEditCategory(category);
            if (!response.Success)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = response.Message });
            }

            var categories = (List<CategoryVM>)_categoryBusiness.GetCategories().JsonData;
            return PartialView("_CategoryTable", categories);
        }

        // ✅ تعديل كاتيجوري (Ajax)
        [HttpPost]
        public ActionResult EditAjax(CategoryVM category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Category name must be at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(category.Description))
            {
                return new HttpStatusCodeResult(400, "Description is required.");
            }

            var response = _categoryBusiness.AddEditCategory(category);
            if (!response.Success)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = response.Message });
            }

            var categories = (List<CategoryVM>)_categoryBusiness.GetCategories().JsonData;
            return PartialView("_CategoryTable", categories);
        }

        // ✅ حذف كاتيجوري (Ajax)
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var category = (CategoryVM)_categoryBusiness.GetCategory(id).JsonData;
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found." });
            }

            var response = _categoryBusiness.RemoveCategory(category);
            if (!response.Success)
            {
                return Json(new { success = false, message = response.Message });
            }

            return Json(new { success = true });
        }
    }
}
