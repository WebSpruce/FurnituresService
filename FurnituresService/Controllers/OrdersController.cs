using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace FurnituresService.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUsersRepository _userRepository;
        private readonly IFurnituresRepository _furnituresRepository;
        public OrdersController(ApplicationDbContext context, IOrdersRepository ordersRepository, IUsersRepository usersRepository, IFurnituresRepository furnituresRepository)
        {
            _context = context;
            _ordersRepository = ordersRepository;
            _userRepository = usersRepository;
            _furnituresRepository = furnituresRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var orders = await _ordersRepository.GetAllAsync();
            return View("Show", orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _ordersRepository.GetByIdAsync(id);
            GetOrderedFurnitures(order, id);

            return View("Details", order);
        }
        private void GetOrderedFurnitures(Order order, int id)
        {
            List<OrderFurniture> orderFurnitures = new List<OrderFurniture>();
            orderFurnitures = _context.OrderFurnitures.Where(of => of.Order.Id == id).ToList();
            ICollection<Furniture> tempFurnitures = new Collection<Furniture>();
            foreach (var item in orderFurnitures)
            {
                var furniture = _context.Furnitures.Where(f => f.Id == item.FurnitureId).FirstOrDefault();
                tempFurnitures.Add(furniture);
            }
            ViewData["Furnitures"] = new SelectList(tempFurnitures, "Id", "Name");
            ViewData["SumPrice"] = order.Price;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            var users = await _userRepository.GetAllAsync();
            ViewData["Users"] = new SelectList(users, "Id", "Email");
            var furnitures = _context.Furnitures.Where(f => f.Name != "DEFAULT").ToList();
            ViewData["Furnitures"] = new SelectList(furnitures, "Id", "Name");
            return View("Insert");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Insert(Order order)
        {
            try
            {
                var selectedUser = await _userRepository.GetByIdAsync(order.UserId);
                order.User = selectedUser;

                var selectedFurnitureIds = Request.Form["OrderFurnitures"].Select(int.Parse).ToList();
                ICollection<OrderFurniture> tempOrderFurniture = new Collection<OrderFurniture>();
                foreach (var furnitureId in selectedFurnitureIds)
                {
                    var selectedFurniture = await _context.Furnitures.FindAsync(furnitureId);
                    _context.OrderFurnitures.Add(new OrderFurniture { Order = order, Furniture = selectedFurniture });
                }
                _ordersRepository.Insert(order);
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
            var order = await _ordersRepository.GetByIdAsync(id);

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

            _ordersRepository.Delete(order);
            return RedirectToAction("Show");
        }


        [HttpGet]
        public async Task<IActionResult> UserOrdersShow(string customerId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == customerId)
            {
                var orders = await _ordersRepository.GetByCustomerIdAsync(customerId);
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
                var order = await _ordersRepository.GetByIdAsync(id);
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
