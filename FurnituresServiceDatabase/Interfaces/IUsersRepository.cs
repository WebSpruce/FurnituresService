using Microsoft.AspNetCore.Identity;

namespace FurnituresServiceDatabase.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync();
        Task<IdentityUser> GetByIdAsync(string id);
        Task<bool> Insert(IdentityUser user);
        bool Update(IdentityUser user);
        bool Delete(IdentityUser user);
        bool Save();
    }
}
