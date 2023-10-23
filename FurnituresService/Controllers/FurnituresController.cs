using FurnituresService.Data;
using FurnituresService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using FurnituresService.Interfaces;

namespace FurnituresService.Controllers
{
    public class FurnituresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFurnituresRepository _furnitureRepo;
        private readonly ICategoriesRepository _categoryRepo;
        public FurnituresController(ApplicationDbContext context, IFurnituresRepository furnitureRepo, ICategoriesRepository categoryRepo)
        {
            _context = context;
            _furnitureRepo = furnitureRepo;
            _categoryRepo = categoryRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var furnitures = await _furnitureRepo.GetAllAsync();
            return View("Show", furnitures);
        }
        public async Task<FileResult> GetImage(int id)
        {
            var furniture = await _furnitureRepo.GetByIdAsync(id);
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
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            var categories = await _categoryRepo.GetAllAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            return View("Insert");
        }
        [HttpPost]
        public async Task<IActionResult> Insert(IFormFile ImageData ,Furniture furniture)
        {
            try
            {
                var selectedCategory = await _categoryRepo.GetByIdAsync(furniture.CategoryId);
                furniture.Category = selectedCategory;

                if (ImageData != null && ImageData.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await ImageData.CopyToAsync(stream);
                        furniture.ImageData = stream.ToArray();
                    }
                }

                _furnitureRepo.Insert(furniture);
                return RedirectToAction("Show");
            }catch(Exception ex)
            {
                Trace.WriteLine($"Furniture Insert error: {ex}");
                return View(furniture);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var furniture = await _furnitureRepo.GetByIdAsync(id);
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

                _furnitureRepo.Update(furniture);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Update POST error: {ex}");
                return View(furniture);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var furniture = await _furnitureRepo.GetByIdAsync(id);

            return View(furniture);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var furniture = _context.Furnitures.Where(f=>f.Id == id).FirstOrDefault();
            Trace.WriteLine($"fur: {furniture.Name}");
            if(furniture == null)
            {
                return NotFound();
            }

            _furnitureRepo.Delete(furniture);
            return RedirectToAction("Show");
        }
    }
}
