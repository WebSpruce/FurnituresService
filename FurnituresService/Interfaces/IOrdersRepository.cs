using FurnituresService.Models;

namespace FurnituresService.Interfaces
{
    public interface IOrdersRepository
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
