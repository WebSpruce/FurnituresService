using FurnituresServiceDatabase.Data;
using FurnituresServiceDatabase.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FurnituresServiceDatabase.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<IdentityUser> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<bool> Insert(IdentityUser user)
        {
            try
            {
				var result = await _userManager.CreateAsync(user, user.PasswordHash);
				await _userManager.AddToRoleAsync(user, "Customer");
				return result.Succeeded;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"UsersRepository Insert error: {ex}");
                return false;
            }
        }
        public bool Delete(IdentityUser user)
        {
            _context.Users.Remove(user);
            return Save();
        }
        public bool Update(IdentityUser user)
        {
            Trace.WriteLine($"user: {user.Id} - {user.Email} - {user.PhoneNumber} - {user.PasswordHash}");
            var existingUser = _context.Users.Find(user.Id);

            if (existingUser != null)
            {
                // Update the properties of the existing entity
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.PasswordHash = user.PasswordHash;

                // Set the entity state to Modified
                _context.Entry(existingUser).State = EntityState.Modified;

                return Save();
            }
            else
            {
                return false; // Entity with the given ID not found
            }
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
