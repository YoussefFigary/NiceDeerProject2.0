using System;
using System.Linq;
using System.Web.Mvc;
using NorthwindTraders.Models;

namespace NorthwindTraders.Controllers
{
    public class CategoryController : Controller
    {
        private NorthwindEntities _context;

        public CategoryController()
        {
            _context = new NorthwindEntities();
        }

        // =============== Main Page =============== //
        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // Partial view of the category table
        public PartialViewResult CategoryTable_()
        {
            var categories = _context.Categories.ToList();
            return PartialView("CategoryTable_", categories);
        }

        // =============== AJAX Actions =============== //

        // Create category with AJAX
        [HttpPost]
        public ActionResult CreateAjax(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
            var categories = _context.Categories.ToList();
            return PartialView("CategoryTable_", categories);

        }

        // Edit category with AJAX
        [HttpPost]
        public ActionResult EditAjax(Category category)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Categories.Find(category.CategoryID);
                if (existing != null)
                {
                    existing.CategoryName = category.CategoryName;
                    existing.Description = category.Description;
                    _context.SaveChanges();
                }
            }

            var categories = _context.Categories.ToList();
            return PartialView("CategoryTable_", categories);
        }

        // Delete category with AJAX
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            var categories = _context.Categories.ToList();
            return PartialView("CategoryTable_", categories);
        }

        // =============== Normal Actions (Non-AJAX) =============== //

        // Create page
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // Edit page
        public ActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Categories.Find(category.CategoryID);
                if (existing != null)
                {
                    existing.CategoryName = category.CategoryName;
                    existing.Description = category.Description;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // Normal delete
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}