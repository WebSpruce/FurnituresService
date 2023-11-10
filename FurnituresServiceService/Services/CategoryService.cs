using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;

namespace FurnituresServiceService.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoriesRepository _categoryRepository;
		public CategoryService(ICategoriesRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}
		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			return await _categoryRepository.GetAllAsync();
		}
		public async Task<Category> GetByIdAsync(int id)
		{
			return await _categoryRepository.GetByIdAsync(id);
		}
		public bool Insert(Category category)
		{
			return _categoryRepository.Insert(category);
		}
		public bool Update(Category category)
		{
			return _categoryRepository.Update(category);
		}
		public bool Delete(Category category)
		{
			return _categoryRepository.Delete(category);
		}
		public bool Save()
		{
			return _categoryRepository.Save();
		}
	}
}
