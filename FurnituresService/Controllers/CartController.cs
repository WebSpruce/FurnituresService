using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartRepository _cartRepository;
		private readonly IUsersRepository _usersRepository;
		private readonly IFurnituresRepository _furnituresRepository;
		private readonly ApplicationDbContext _context;
		public CartController(ICartRepository cartRepository, IUsersRepository usersRepository, IFurnituresRepository furnituresRepository, ApplicationDbContext context)
		{
			_cartRepository = cartRepository;
			_usersRepository = usersRepository;
			_furnituresRepository = furnituresRepository;
			_context = context;
        }

		[HttpGet]
		public async Task<IActionResult> Show(string id)
		{
			if(await _cartRepository.GetByUserId(id) == null)
			{
				Cart newCart = new Cart()
				{
					UserId = id,
					User = await _usersRepository.GetByIdAsync(id)
				};
				_cartRepository.Insert(newCart);
			}

			ViewData["AddedFurnitures"] = _cartRepository.GetAddedFurnitures(id);

            return View("Show", await _cartRepository.GetByUserId(id));
		}
        public async Task<FileResult> GetImage(int id)
        {
            var furniture = await _furnituresRepository.GetByIdAsync(id);
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
		public async Task<IActionResult> RemoveFromCart(IdentityUser user, Furniture furniture)
		{
            try
            {
                await _cartRepository.RemoveFurnitureToCart(user, furniture);
                return RedirectToAction("Show", "Cart", new { id = user.Id });
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Cart RemoveFurniture error: {ex}");
                return View("Show");
            }
        }
    }
}
