using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoriesRepository _categoryRepo;
        public CategoriesController(ApplicationDbContext context, ICategoriesRepository categoryRepo)
        {
            _context = context;
            _categoryRepo = categoryRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View("Show", categories);
        }
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            return View("Insert");
        }
        [HttpPost]
        public async Task<IActionResult> Insert(Category category)
        {
            try
            {
                _categoryRepo.Insert(category);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Category Insert error: {ex}");
                return View(category);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
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
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            try
            {
                _categoryRepo.Update(category);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Category update error: {ex}");
                return View(category);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            _categoryRepo.Delete(category);
            return RedirectToAction("Show");
        }
    }
}
