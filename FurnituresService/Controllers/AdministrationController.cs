using FurnituresService.Data;
using FurnituresService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdministrationController(ApplicationDbContext context) 
        {
            _context= context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Show()
        {
            var furnitures = _context.Furnitures.ToList();
            return View("Show", furnitures);
        }
        public FileResult GetImage(int id)
        {
            var furniture = _context.Furnitures.Find(id);
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
        public IActionResult Insert()
        {
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            return View("Insert");
        }
        [HttpPost]
        public async Task<IActionResult> Insert(IFormFile ImageData ,Furniture furniture)
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

                _context.Furnitures.Add(furniture);
                await _context.SaveChangesAsync();
                return RedirectToAction("Show");
            }catch(Exception ex)
            {
                Trace.WriteLine($"error: {ex}");
                return View(furniture);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var furniture = _context.Furnitures.Where(c=>c.Id==id).FirstOrDefault();
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

                _context.Furnitures.Update(furniture);
                await _context.SaveChangesAsync();
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
            var furniture = _context.Furnitures.Where(f => f.Id == id).FirstOrDefault();

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

            _context.Furnitures.Remove(furniture);
            await _context.SaveChangesAsync();
            return RedirectToAction("Show");
        }
    }
}
