using FurnituresService.Models;

namespace FurnituresService.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        bool Insert(Category category);
        bool Update(Category category);
        bool Delete(Category category);
        bool Save();
    }
}
