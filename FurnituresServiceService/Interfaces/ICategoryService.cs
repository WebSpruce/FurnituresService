using FurnituresServiceModels.Models;

namespace FurnituresServiceService.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<Category>> GetAllAsync();
		Task<Category> GetByIdAsync(int id);
		bool Insert(Category category);
		bool Update(Category category);
		bool Delete(Category category);
		bool Save();
	}
}
