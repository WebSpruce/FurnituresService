using FurnituresServiceDatabase.Data;
using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using FurnituresServiceService.Interfaces;

namespace FurnituresService.Controllers
{
    public class FurnituresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFurnitureService _furnitureService;
        private readonly ICategoryService _categoryService;
        public FurnituresController(ApplicationDbContext context,
			IFurnitureService furnitureService,
			ICategoryService categoryService)
        {
            _context = context;
			_furnitureService = furnitureService;
			_categoryService = categoryService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var furnitures = await _furnitureService.GetAllAsync();
            return View("Show", furnitures);
        }
        public async Task<FileResult> GetImage(int id)
        {
            var furniture = await _furnitureService.GetByIdAsync(id);
            if(furniture!=null && furniture.ImageData != null)
            {
                return File(furniture.ImageData, "image/jpeg");
            }
            else
            {
                var furnitureFromDb = _context.Furnitures.Where(f => f.Name == "DEFAULT").FirstOrDefault();
                return File(furnitureFromDb.ImageData, "image/png");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            return View("Insert");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Insert(IFormFile ImageData ,Furniture furniture)
        {
            try
            {
                var selectedCategory = await _categoryService.GetByIdAsync(furniture.CategoryId);
                furniture.Category = selectedCategory;

                if (ImageData != null && ImageData.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await ImageData.CopyToAsync(stream);
                        furniture.ImageData = stream.ToArray();
                    }
                }

				_furnitureService.Insert(furniture);
                return RedirectToAction("Show");
            }catch(Exception ex)
            {
                Trace.WriteLine($"Furniture Insert error: {ex}");
                return View(furniture);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var furniture = await _furnitureService.GetByIdAsync(id);

            var categories = _context.Categories.ToList();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");

            return View("Details", furniture);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var furniture = await _furnitureService.GetByIdAsync(id);
            if (furniture == null)
            {
                await Response.WriteAsync("<script>alert('Couldn't edit this furniture.')</script>");
                return View();
            }

            var categories = _context.Categories.ToList();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            var oldFurniture = new Furniture()
            {
                Id = furniture.Id,
                Name = furniture.Name,
                Description = furniture.Description,
                Price = furniture.Price,
                ImageData = furniture.ImageData,
                CategoryId = furniture.CategoryId,
                Category = furniture.Category
            };

            return View(oldFurniture);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(IFormFile ImageData, Furniture furniture)
        {
            try
            {
                var selectedCategory = _context.Categories.Where(c => c.Id == furniture.CategoryId).FirstOrDefault();
                furniture.Category = selectedCategory;
                
                if (ImageData != null && ImageData.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await ImageData.CopyToAsync(stream);
                        furniture.ImageData = stream.ToArray();
                    }
                }
				_furnitureService.Update(furniture);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Update POST error: {ex}");
                return View(furniture);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var furniture = await _furnitureService.GetByIdAsync(id);

            return View(furniture);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var furniture = _context.Furnitures.Where(f=>f.Id == id).FirstOrDefault();
            if(furniture == null)
            {
                return NotFound();
            }

			_furnitureService.Delete(furniture);
            return RedirectToAction("Show");
        }
    }
}
