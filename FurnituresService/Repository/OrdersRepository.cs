using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnituresService.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o=>o.Id == id);
        }
        public bool Insert(Order order)
        {
            _context.Orders.AddAsync(order);
            return Save();
        }
        public bool Update(Order order)
        {
            _context.Orders.Update(order);
            return Save();
        }
        public bool Delete(Order order)
        {
            _context.Orders.Remove(order);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        
    }
}
