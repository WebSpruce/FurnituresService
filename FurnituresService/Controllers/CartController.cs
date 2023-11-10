using FurnituresServiceDatabase.Data;
using FurnituresServiceService.Interfaces;
using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartService _cartService;
		private readonly IUserService _usersService;
		private readonly IFurnitureService _furnituresService;
		private readonly IOrderService _ordersService;
		private readonly ICategoryService _categoriesService;
		private readonly ApplicationDbContext _context;
		public CartController(ICartService cartService,
			IUserService usersService,
			IFurnitureService furnituresService,
			IOrderService ordersService,
			ICategoryService categoriesService,
            ApplicationDbContext context)
		{
			_cartService = cartService;
			_usersService = usersService;
			_furnituresService = furnituresService;
			_ordersService = ordersService;
			_categoriesService = categoriesService;
            _context = context;
        }

		[HttpGet]
		public async Task<IActionResult> Show(string id)
		{
			if(await _cartService.GetByUserId(id) == null)
			{
				Cart newCart = new Cart()
				{
					UserId = id,
					User = await _usersService.GetByIdAsync(id)
				};
				_cartService.Insert(newCart);
			}
            var cart = await _cartService.GetByUserId(id);
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
            ViewData["AddedFurnitures"] = _cartService.GetAddedFurnitures(id);

            return View("Show", cart);
		}
        public async Task<FileResult> GetImage(int id)
        {
            var furniture = await _furnituresService.GetByIdAsync(id);
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
				var currentUser = await _usersService.GetByIdAsync(id);
				var clickedFurniture = await _furnituresService.GetByIdAsync(furnitureId);
                await _cartService.RemoveFurnitureToCart(currentUser, clickedFurniture);
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
				_ordersService.Insert(newOrder);

                var currentCart = await _cartService.GetByUserId(id);
                IEnumerable<Furniture> furnituresFromTheCart = _cartService.GetAddedFurnitures(id);
                List<OrderFurniture> allOrderedFurnitures = new List<OrderFurniture>();
                foreach (var item in furnituresFromTheCart)
                {
                    allOrderedFurnitures.Add(new OrderFurniture { FurnitureId=item.Id, });
                }
                newOrder.OrderFurnitures = allOrderedFurnitures;
                newOrder.Price = sum;
				_ordersService.Update(newOrder);


                var currentUser = await _usersService.GetByIdAsync(id);
                await _cartService.RemoveAllFurnituresFromCart(currentUser);
                currentCart.CouponId = null;
				_cartService.Update(currentCart);

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
            var theCart = await _cartService.GetByUserId(customerId);

            

            if (theCart.CouponId == null)
            {
               if (coupon != null)
               {
                   bool isCorrectProduct = false;
                   IEnumerable<Furniture> furnituresFromTheCart = _cartService.GetAddedFurnitures(customerId);
                   var category = await _categoriesService.GetByIdAsync(coupon.CouponCategoryId);
                   foreach (var item in furnituresFromTheCart)
                   {
                        var furnitureCategory = await _categoriesService.GetByIdAsync(item.CategoryId);
                        if (category.Name == furnitureCategory.Name)
                       {
                           isCorrectProduct = true; break;
                       }
                   }
                   if (isCorrectProduct)
                   {
                       theCart.CouponId = coupon.Id;
					   _cartService.Update(theCart);
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
