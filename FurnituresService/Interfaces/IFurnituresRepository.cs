using FurnituresService.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FurnituresService.Interfaces
{
    public interface IFurnituresRepository
    {
        Task<IEnumerable<Furniture>> GetAllAsync();
        Task<Furniture> GetByIdAsync(int id);
        bool Insert(Furniture furniture);
        bool Update(Furniture furniture);
        bool Delete(Furniture furniture);
        bool Save();
    }
}
