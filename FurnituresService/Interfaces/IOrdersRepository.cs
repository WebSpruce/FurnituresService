using FurnituresService.Models;

namespace FurnituresService.Interfaces
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        bool Insert(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
