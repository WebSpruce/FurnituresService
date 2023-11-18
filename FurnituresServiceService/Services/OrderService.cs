using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;

namespace FurnituresServiceService.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrdersRepository _ordersRepository;
		public OrderService(IOrdersRepository ordersRepository)
		{
			_ordersRepository = ordersRepository;
		}
		public async Task<IEnumerable<Order>> GetAllAsync()
		{
			return await _ordersRepository.GetAllAsync();
		}

		public async Task<Order> GetByIdAsync(int id)
		{
			return await _ordersRepository.GetByIdAsync(id);
		}
		public async Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId)
		{
			return await _ordersRepository.GetByCustomerIdAsync(customerId);
		}
		public async Task<IEnumerable<Furniture>> GetOrderedFurnitures(int id)
		{
			return await _ordersRepository.GetOrderedFurnitures(id);
		}
        public bool Insert(Order order)
		{
			return _ordersRepository.Insert(order);
		}
		public bool Update(Order order)
		{
			return _ordersRepository.Update(order);
		}
		public bool Delete(Order order)
		{
			return _ordersRepository.Delete(order);
		}
		public bool Save()
		{
			return _ordersRepository.Save();
		}
	}
}
