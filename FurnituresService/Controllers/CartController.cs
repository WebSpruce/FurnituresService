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
		private readonly IOrdersRepository _ordersRepository;
		private readonly ApplicationDbContext _context;
		public CartController(ICartRepository cartRepository, IUsersRepository usersRepository, IFurnituresRepository furnituresRepository, IOrdersRepository ordersRepository, ApplicationDbContext context)
		{
			_cartRepository = cartRepository;
			_usersRepository = usersRepository;
			_furnituresRepository = furnituresRepository;
            _ordersRepository = ordersRepository;

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
		public async Task<IActionResult> RemoveFromCart(string id, int furnitureId)
		{
            try
            {
				var currentUser = await _usersRepository.GetByIdAsync(id);
				var clickedFurniture = await _furnituresRepository.GetByIdAsync(furnitureId);
                await _cartRepository.RemoveFurnitureToCart(currentUser, clickedFurniture);
                return RedirectToAction("Show", "Cart", new { id = id });
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Cart RemoveFurniture error: {ex}");
                return View("Show");
            }
        }
		public async Task<IActionResult> Buy(string id)
		{
            try
            {
                var newOrder = new Order { UserId = id };
                _ordersRepository.Insert(newOrder);

                var currentCart = await _cartRepository.GetByUserId(id);
                IEnumerable<Furniture> furnituresFromTheCart = _cartRepository.GetAddedFurnitures(id);
                List<OrderFurniture> allOrderedFurnitures = new List<OrderFurniture>();
                foreach (var item in furnituresFromTheCart)
                {
                    allOrderedFurnitures.Add(new OrderFurniture { FurnitureId=item.Id, });
                }
                newOrder.OrderFurnitures = allOrderedFurnitures;
                _ordersRepository.Update(newOrder);


                var currentUser = await _usersRepository.GetByIdAsync(id);
                await _cartRepository.RemoveAllFurnituresFromCart(currentUser);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Cart Buy error: {ex}");
                return View("Show");
            }
        }
    }
}
