using FurnituresServiceModels.Models;

namespace FurnituresServiceDatabase.Interfaces
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
