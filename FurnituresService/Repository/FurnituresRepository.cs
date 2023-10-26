using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnituresService.Repository
{
    public class FurnituresRepository : IFurnituresRepository
    {
        private readonly ApplicationDbContext _context;
        public FurnituresRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Furniture>> GetAllAsync()
        {
            return await _context.Furnitures.Where(f=>f.Name!="DEFAULT").ToListAsync();
        }
        public async Task<Furniture> GetByIdAsync(int id)
        {
            return await _context.Furnitures.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        public bool Insert(Furniture furniture)
        {
            _context.Furnitures.AddAsync(furniture);
            return Save();
        }
        public bool Update(Furniture furniture)
        {
            _context.Furnitures.Update(furniture);
            return Save();
        }
        public bool Delete(Furniture furniture)
        {
            _context.Furnitures.Remove(furniture);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
