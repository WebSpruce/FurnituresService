using FurnituresService.Models;
using Microsoft.AspNetCore.Identity;

namespace FurnituresService.Interfaces
{
	public interface ICartRepository
	{
		Task<Cart> GetByUserId(string id);
		bool Insert(Cart cart);
		Task<bool> InsertFurnitureToCart(string id, Furniture furniture);
		Task<bool> RemoveFurnitureToCart(IdentityUser user, Furniture furniture);
		bool Save();

		IEnumerable<Furniture> GetAddedFurnitures(string id);
	}
}
