using FurnituresServiceDatabase.Data;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly IFurnitureService _furnitureService;
		private readonly ICartService _cartService;
        private readonly ICategoryService _categoriesService;
        public HomeController(ApplicationDbContext context, 
			IFurnitureService furnitureService,
			ICartService cartService,
			ICategoryService categoriesService)
        {
            _context = context;
			_furnitureService = furnitureService;
			_cartService = cartService;
			_categoriesService = categoriesService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
			var furnitures = await _furnitureService.GetAllAsync();

			return View("Index", furnitures);
        }
		public async Task<FileResult> GetImage(int id)
		{
			var furniture = await _furnitureService.GetByIdAsync(id);
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
       
        public async Task<IActionResult> AddToCart(int furnitureId, string userId)
        {
            try {
				var clickedFurniture = await _furnitureService.GetByIdAsync(furnitureId);
                await _cartService.InsertFurnitureToCart(userId, clickedFurniture);
				return RedirectToAction("Show","Cart", new { id = userId });
			}
			catch(Exception ex)
            {
                Trace.WriteLine($"Home AddToCart error: {ex}");
                return View("Show");
	        }
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Catalog(string searchText)
        {
            var furnitures = await _furnitureService.GetAllAsync();
            ViewData["Categories"] = await _categoriesService.GetAllAsync();
            if (!String.IsNullOrEmpty(searchText))
            {
                furnitures = furnitures.Where(f=>f.Name.ToLower()!.Contains(searchText.ToLower()));
            }

            return View("Catalog", furnitures);
        }
        [HttpGet]
        public async Task<IActionResult> CatalogByCategory(int categoryId)
        {
            var furnitures = await _furnitureService.GetByCategory(categoryId);
			ViewData["Categories"] = await _categoriesService.GetAllAsync();
			return View("Catalog", furnitures);
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}