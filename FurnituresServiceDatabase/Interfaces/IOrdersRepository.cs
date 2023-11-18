using FurnituresServiceModels.Models;

namespace FurnituresServiceDatabase.Interfaces
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Furniture>> GetOrderedFurnitures(int id);
        bool Insert(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
