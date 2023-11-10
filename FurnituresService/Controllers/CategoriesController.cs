using FurnituresServiceDatabase.Data;
using FurnituresServiceService.Interfaces;
using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        public CategoriesController(ApplicationDbContext context, ICategoryService categoryService)
        {
            _context = context;
			_categoryService = categoryService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var categories = await _categoryService.GetAllAsync();
            return View("Show", categories);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            return View("Insert");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Insert(Category category)
        {
            try
            {
				_categoryService.Insert(category);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Category Insert error: {ex}");
                return View(category);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                await Response.WriteAsync("<script>alert('Couldn't edit this category.')</script>");
                return View();
            }

            var oldCategory = new Category()
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(oldCategory);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            try
            {
				_categoryService.Update(category);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Category update error: {ex}");
                return View(category);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return View(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

			_categoryService.Delete(category);
            return RedirectToAction("Show");
        }
    }
}
