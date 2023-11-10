using FurnituresServiceModels.Models;

namespace FurnituresServiceService.Interfaces
{
	public interface IFurnitureService
	{
		Task<IEnumerable<Furniture>> GetAllAsync();
		Task<Furniture> GetByIdAsync(int id);
		Task<IEnumerable<Furniture>> GetByCategory(int id);
		bool Insert(Furniture furniture);
		bool Update(Furniture furniture);
		bool Delete(Furniture furniture);
		bool Save();
	}
}
