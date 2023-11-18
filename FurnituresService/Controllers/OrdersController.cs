using FurnituresServiceDatabase.Data;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Claims;

namespace FurnituresService.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IFurnitureService _furnitureService;
        public OrdersController(ApplicationDbContext context, 
            IOrderService orderService, 
            IUserService userService,
            IFurnitureService furnitureService)
        {
            _context = context;
			_orderService = orderService;
			_userService = userService;
            _furnitureService = furnitureService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var orders = await _orderService.GetAllAsync();
            return View("Show", orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            GetOrderedFurnitures(order, id);

            return View("Details", order);
        }
        private void GetOrderedFurnitures(Order order, int orderId)
        {
            var furnitures = _orderService.GetOrderedFurnitures(orderId).Result;
            ViewData["Furnitures"] = new SelectList(furnitures, "Id", "Name");
            ViewData["SumPrice"] = order.Price;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            var users = await _userService.GetAllAsync();
            ViewData["Users"] = new SelectList(users, "Id", "Email");
            var furnitures = _furnitureService.GetAllAsync().Result;
            ViewData["Furnitures"] = new SelectList(furnitures, "Id", "Name");
            return View("Insert");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Insert(Order order)
        {
            try
            {
                var selectedUser = await _userService.GetByIdAsync(order.UserId);
                order.User = selectedUser;

                var selectedFurnitureIds = Request.Form["OrderFurnitures"].Select(int.Parse).ToList();
                ICollection<OrderFurniture> tempOrderFurniture = new Collection<OrderFurniture>();
                foreach (var furnitureId in selectedFurnitureIds)
                {
                    var selectedFurniture = await _context.Furnitures.FindAsync(furnitureId);
                    _context.OrderFurnitures.Add(new OrderFurniture { Order = order, Furniture = selectedFurniture });
                }
				_orderService.Insert(order);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Order Insert error: {ex}");
                return View(order);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            return View(order);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var order = _context.Orders.Where(f => f.Id == id).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }

            var item = _context.OrderFurnitures.Where(of => of.Order == order).FirstOrDefault();
            _context.OrderFurnitures.Remove(item);

			_orderService.Delete(order);
            return RedirectToAction("Show");
        }


        [HttpGet]
        public async Task<IActionResult> UserOrdersShow(string customerId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == customerId)
            {
                var orders = await _orderService.GetByCustomerIdAsync(customerId);
                return View("UserOrdersShow", orders);
            }
            else
            {
                return NotFound("You have no access to this data.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> UserOrdersDetails(int id, string customerId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == customerId)
            {
                var order = await _orderService.GetByIdAsync(id);
                GetOrderedFurnitures(order, id);
                return View("UserOrdersDetails", order);
            }
            else
            {
                return NotFound("You have no access to this data.");
            }
        }
    }
}
