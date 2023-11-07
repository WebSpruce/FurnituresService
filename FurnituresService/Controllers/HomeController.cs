using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using FurnituresService.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFurnituresRepository _furnitureRepo;
        private readonly ICartRepository _cartRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(ApplicationDbContext context, 
            IFurnituresRepository furnituresRepo, 
            ICartRepository cartRepository, 
            IUsersRepository usersRepository, 
            ICategoriesRepository categoriesRepository,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _furnitureRepo = furnituresRepo;
            _cartRepository = cartRepository;
            _usersRepository = usersRepository;
            _categoriesRepository = categoriesRepository;
            _signInManager = signInManager;
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
       
        public async Task<IActionResult> AddToCart(int furnitureId, string userId)
        {
            try {
				var clickedFurniture = await _furnitureRepo.GetByIdAsync(furnitureId);
                await _cartRepository.InsertFurnitureToCart(userId, clickedFurniture);
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
            var furnitures = await _furnitureRepo.GetAllAsync();
            ViewData["Categories"] = await _categoriesRepository.GetAllAsync();

            if (!String.IsNullOrEmpty(searchText))
            {
                furnitures = furnitures.Where(f=>f.Name.ToLower()!.Contains(searchText));
            }

            return View("Catalog", furnitures);
        }
        [HttpGet]
        public async Task<IActionResult> CatalogByCategory(int categoryId)
        {
            var furnitures = await _furnitureRepo.GetByCategory(categoryId);
			ViewData["Categories"] = await _categoriesRepository.GetAllAsync();
			return View("Catalog", furnitures);
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}