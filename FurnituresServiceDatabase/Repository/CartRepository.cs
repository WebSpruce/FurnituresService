using FurnituresServiceDatabase.Data;
using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FurnituresServiceDatabase.Repository
{
	public class CartRepository : ICartRepository
	{
		private readonly ApplicationDbContext _context;
		public CartRepository(ApplicationDbContext context) 
		{
			_context = context;
		}

		public async Task<Cart> GetByUserId(string id)
		{
			return await _context.Carts.Where(c=>c.UserId == id).FirstOrDefaultAsync();
		}

		public bool Insert(Cart cart)
		{
			_context.Carts.AddAsync(cart);
			return Save();
		}
		public async Task<bool> InsertFurnitureToCart(string id, Furniture furniture)
		{
			var cart = await GetByUserId(id);
			_context.CartFurnitures.AddAsync(new CartFurniture { Furniture = furniture , Cart = cart });
			return Save();
		}
		public async Task<bool> RemoveFurnitureToCart(IdentityUser user, Furniture furniture)
		{
			var cart = _context.Carts.Where(c => c.UserId == user.Id).FirstOrDefault();
			var cf = _context.CartFurnitures.Where(cf => cf.Cart == cart && cf.Furniture == furniture).FirstOrDefault();
			_context.CartFurnitures.Remove(cf);
			return Save();
		}
        public async Task<bool> RemoveAllFurnituresFromCart(IdentityUser user)
        {
            var cart = _context.Carts.Where(c => c.UserId == user.Id).FirstOrDefault();
			Trace.WriteLine($"user: {user.UserName}, cart: {cart.Id}");

            var cartFurnitureToRemove = _context.CartFurnitures.Where(cf => cf.Cart == cart).ToList();

            if (cartFurnitureToRemove.Any())
            {
                _context.CartFurnitures.RemoveRange(cartFurnitureToRemove);
            }
            return Save();
        }
        public bool Update(Cart cart)
		{
            _context.Carts.Update(cart);
            return Save();
        }
		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

        public IEnumerable<Furniture> GetAddedFurnitures(string id)
		{
			IEnumerable<Furniture> furnitures = Enumerable.Empty<Furniture>();
			furnitures = _context.Carts.Where(c => c.UserId == id).SelectMany(c => c.CartFurnitures.Select(cf => cf.Furniture)).ToList();
			return furnitures;
		}

    }
}
