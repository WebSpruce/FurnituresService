using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Identity;

namespace FurnituresServiceService.Interfaces
{
	public interface ICartService
	{
		Task<Cart> GetByUserId(string id);
		bool Insert(Cart cart);
		IEnumerable<Furniture> GetAddedFurnitures(string id);
		Task<bool> InsertFurnitureToCart(string id, Furniture furniture);
		Task<bool> RemoveFurnitureToCart(IdentityUser user, Furniture furniture);
		Task<bool> RemoveAllFurnituresFromCart(IdentityUser user);
		bool Update(Cart cart);
		bool Save();

	}
}
