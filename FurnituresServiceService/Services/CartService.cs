using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FurnituresServiceService.Services
{
	public class CartService : ICartService
	{
		private readonly ICartRepository _cartRepository;
		public CartService(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
		}

		public async Task<Cart> GetByUserId(string id)
		{
			return await _cartRepository.GetByUserId(id);
		}

		public bool Insert(Cart cart)
		{
			return _cartRepository.Insert(cart);
		}
		public async Task<bool> InsertFurnitureToCart(string id, Furniture furniture)
		{
			return await _cartRepository.InsertFurnitureToCart(id,furniture);
		}
		public async Task<bool> RemoveFurnitureToCart(IdentityUser user, Furniture furniture)
		{
			return await _cartRepository.RemoveFurnitureToCart(user, furniture);
		}
		public async Task<bool> RemoveAllFurnituresFromCart(IdentityUser user)
		{
			return await _cartRepository.RemoveAllFurnituresFromCart(user);
		}
		public bool Update(Cart cart)
		{
			return _cartRepository.Update(cart);
		}
		public bool Save()
		{
			return _cartRepository.Save();
		}

		public IEnumerable<Furniture> GetAddedFurnitures(string id)
		{
			return _cartRepository.GetAddedFurnitures(id);
		}
	}
}
