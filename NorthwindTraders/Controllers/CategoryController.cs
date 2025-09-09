using System.Linq;
using System.Web.Mvc;
using NorthwindTraders.Models;

namespace NorthwindTraders.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NorthwindEntities _context;

        public CategoryController()
        {
            _context = new NorthwindEntities();
        }

        // Index
        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public ActionResult CreateAjax(Category category)
        {
            // ✅ Validation
            if (string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Category name must be at least 3 characters.");
            }

            if (category.CategoryName.Length > 50)
            {
                return new HttpStatusCodeResult(400, "Category name must not exceed 50 characters.");
            }

            if (!string.IsNullOrWhiteSpace(category.Description) && category.Description.Length > 200)
            {
                return new HttpStatusCodeResult(400, "Description must be less than 200 characters.");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            var categories = _context.Categories.ToList();
            return PartialView("_CategoryTable", categories);
        }

        [HttpPost]
        public ActionResult EditAjax(Category category)
        {
            // ✅ Validation
            if (string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Length < 3)
            {
                return new HttpStatusCodeResult(400, "Category name must be at least 3 characters.");
            }

            if (category.CategoryName.Length > 50)
            {
                return new HttpStatusCodeResult(400, "Category name must not exceed 50 characters.");
            }

            if (!string.IsNullOrWhiteSpace(category.Description) && category.Description.Length > 200)
            {
                return new HttpStatusCodeResult(400, "Description must be less than 200 characters.");
            }

            var existingCategory = _context.Categories.Find(category.CategoryID);
            if (existingCategory == null)
            {
                return HttpNotFound();
            }

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;

            _context.SaveChanges();

            var categories = _context.Categories.ToList();
            return PartialView("_CategoryTable", categories);
        }

        [HttpPost]
        public JsonResult DeleteAjax(int id)
        {
            try
            {
                var category = _context.Categories.Find(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Category not found." });
                }

                if (category.Products != null && category.Products.Any())
                {
                    return Json(new { success = false, message = "Cannot delete category with related products." });
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return Json(new { success = true, id = id });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
