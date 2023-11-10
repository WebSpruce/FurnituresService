using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;

namespace FurnituresServiceService.Services
{
	public class FurnitureService : IFurnitureService
	{
		private readonly IFurnituresRepository _furnituresRepository;
		public FurnitureService(IFurnituresRepository furnituresRepository)
		{
			_furnituresRepository = furnituresRepository;
		}

		public async Task<IEnumerable<Furniture>> GetAllAsync()
		{
			return await _furnituresRepository.GetAllAsync();
		}
		public async Task<Furniture> GetByIdAsync(int id)
		{
			return await _furnituresRepository.GetByIdAsync(id);
		}
		public async Task<IEnumerable<Furniture>> GetByCategory(int id)
		{
			return await _furnituresRepository.GetByCategory(id);
		}
		public bool Insert(Furniture furniture)
		{
			return _furnituresRepository.Insert(furniture);
		}
		public bool Update(Furniture furniture)
		{
			return _furnituresRepository.Update(furniture);
		}
		public bool Delete(Furniture furniture)
		{
			return _furnituresRepository.Delete(furniture);
		}
		public bool Save()
		{
			return _furnituresRepository.Save();
		}
	}
}
