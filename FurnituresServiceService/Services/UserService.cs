using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FurnituresServiceService.Services
{
	public  class UserService : IUserService
	{
		private readonly IUsersRepository _usersRepository;
		public UserService(IUsersRepository usersRepository)
		{
			_usersRepository = usersRepository;
		}

		public async Task<IEnumerable<IdentityUser>> GetAllAsync()
		{
			return await _usersRepository.GetAllAsync();
		}
		public async Task<IdentityUser> GetByIdAsync(string id)
		{
			return await _usersRepository.GetByIdAsync(id);
		}
		public async Task<bool> Insert(IdentityUser user)
		{
			return await _usersRepository.Insert(user);
		}
		public bool Delete(IdentityUser user)
		{
			return _usersRepository.Delete(user);
		}
		public bool Update(IdentityUser user)
		{
			return _usersRepository.Update(user);
		}
		public bool Save()
		{
			return _usersRepository.Save();
		}

	}
}
