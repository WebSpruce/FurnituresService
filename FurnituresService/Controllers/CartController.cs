using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
		private readonly ICategoriesRepository _categoriesRepository;
		private readonly ApplicationDbContext _context;
		public CartController(ICartRepository cartRepository, 
            IUsersRepository usersRepository, 
            IFurnituresRepository furnituresRepository, 
            IOrdersRepository ordersRepository, 
            ICategoriesRepository categoriesRepository,
            ApplicationDbContext context)
		{
			_cartRepository = cartRepository;
			_usersRepository = usersRepository;
			_furnituresRepository = furnituresRepository;
            _ordersRepository = ordersRepository;
            _categoriesRepository = categoriesRepository;
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
            var cart = await _cartRepository.GetByUserId(id);
            var coupon = _context.Coupons.SingleOrDefault(c => c.Id == cart.CouponId);
            if (coupon != null)
            {
                ViewData["couponValue"] = coupon.CouponValuePercentage;
            }
            else
            {
                decimal value = 0M;
                ViewData["couponValue"] = value;
            }
            ViewData["couponValidation"] = "Enter your coupon code if you have one.";
            ViewData["AddedFurnitures"] = _cartRepository.GetAddedFurnitures(id);

            return View("Show", cart);
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
		public async Task<IActionResult> Buy(string id, decimal sum)
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
                newOrder.Price = sum;
                _ordersRepository.Update(newOrder);


                var currentUser = await _usersRepository.GetByIdAsync(id);
                await _cartRepository.RemoveAllFurnituresFromCart(currentUser);
                currentCart.CouponId = null;
                _cartRepository.Update(currentCart);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Cart Buy error: {ex}");
                return View("Show");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(string customerId, string couponCode)
        {
            var coupon = _context.Coupons.SingleOrDefault(c => c.Code == couponCode);
            var theCart = await _cartRepository.GetByUserId(customerId);

            

            if (theCart.CouponId == null)
            {
               if (coupon != null)
               {
                   bool isCorrectProduct = false;
                   IEnumerable<Furniture> furnituresFromTheCart = _cartRepository.GetAddedFurnitures(customerId);
                   var category = await _categoriesRepository.GetByIdAsync(coupon.CouponCategoryId);
                   foreach (var item in furnituresFromTheCart)
                   {
                        var furnitureCategory = await _categoriesRepository.GetByIdAsync(item.CategoryId);
                        if (category.Name == furnitureCategory.Name)
                       {
                           isCorrectProduct = true; break;
                       }
                   }
                   if (isCorrectProduct)
                   {
                       theCart.CouponId = coupon.Id;
                       _cartRepository.Update(theCart);
                       ViewData["couponValue"] = coupon.CouponValuePercentage;
                       ViewData["couponValidation"] = "The coupon added.";
                   }
                   else
                   {
                       ViewData["couponValidation"] = "There is 0 products in the category for which there is a coupon.";
                   }
               }
               else
               {
                   ViewData["couponValidation"] = "The entered code is wrong.";
               }
                

            }
            else
            {
                ViewData["couponValidation"] = "You already used coupon.";
            }

            return RedirectToAction("Show", "Cart", new { id = customerId });
        }
    }
}
