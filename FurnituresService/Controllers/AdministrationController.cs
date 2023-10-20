using FurnituresService.Data;
using FurnituresService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
        public IActionResult Insert()
        {
            return View("Insert");
        }
        [HttpPost]
        public async Task<IActionResult> Insert(Furniture furniture)
        {
            if (!ModelState.IsValid)
            {
                return View(furniture);
            }
            _context.Furnitures.Add(furniture);
            return RedirectToAction("Show");
        }

    }
}
