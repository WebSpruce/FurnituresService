using FurnituresServiceModels.Models;

namespace FurnituresServiceService.Interfaces
{
	public interface IOrderService
	{
		Task<IEnumerable<Order>> GetAllAsync();
		Task<Order> GetByIdAsync(int id);
		Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId);
		bool Insert(Order order);
		bool Update(Order order);
		bool Delete(Order order);
		bool Save();
	}
}
