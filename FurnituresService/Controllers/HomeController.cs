using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IFurnituresRepository _furnitureRepo;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IFurnituresRepository furnituresRepo)
        {
            _logger = logger;
            _context = context;
            _furnitureRepo = furnituresRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
			var furnitures = await _furnitureRepo.GetAllAsync();

			return View("Index", furnitures);
        }
		public async Task<FileResult> GetImage(int id)
		{
			var furniture = await _furnitureRepo.GetByIdAsync(id);
			if (furniture != null && furniture.ImageData != null)
			{
				return File(furniture.ImageData, "image/jpeg");
			}
			else
			{
				var furnitureFromDb = _context.Furnitures.Where(f => f.Name == "DEFAULT").FirstOrDefault();
				return File(furnitureFromDb.ImageData, "image/png");
			}
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}