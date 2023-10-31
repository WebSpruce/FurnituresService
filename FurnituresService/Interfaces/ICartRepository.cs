using FurnituresService.Models;

namespace FurnituresService.Interfaces
{
	public interface ICartRepository
	{
		Task<Cart> GetByUserId(string id);
		bool Insert(Cart cart);
		Task<bool> InsertFurnitureToCart(string id, Furniture furniture);
		Task<bool> RemoveFurnitureToCart(Cart cart, Furniture furniture);
		bool Save();

		IEnumerable<Furniture> GetAddedFurnitures(string id);
	}
}
