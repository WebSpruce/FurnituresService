using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnituresServiceService.Interfaces
{
    public interface IUserService
    {
		Task<IEnumerable<IdentityUser>> GetAllAsync();
		Task<IdentityUser> GetByIdAsync(string id);
		Task<bool> Insert(IdentityUser user);
		bool Delete(IdentityUser user);
		bool Update(IdentityUser user);
		bool Save();

	}
}
