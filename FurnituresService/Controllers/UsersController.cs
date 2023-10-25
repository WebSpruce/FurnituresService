using FurnituresService.Data;
using FurnituresService.Interfaces;
using FurnituresService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsersRepository _usersRepository;
        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager,IUsersRepository usersRepository)
        {
            _context = context;
            _userManager = userManager;
            _usersRepository = usersRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var users = await _usersRepository.GetAllAsync();
            return View("Show", users);
        }
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            return View("Insert");
        }
        [HttpPost]
        public async Task<IActionResult> Insert(IdentityUser user)
        {
            try
            {
                Trace.WriteLine($"new user: {user.Email} - {user.PasswordHash}");
                user.UserName = user.Email;
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await _userManager.CreateAsync(user, user.PasswordHash);
                    await _userManager.AddToRoleAsync(user, "Customer");
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Show");
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Users Insert error: {ex}");
                return View(user);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            if (user == null)
            {
                await Response.WriteAsync("<script>alert('Couldn't edit this furniture.')</script>");
                return View();
            }

            var oldUser = new IdentityUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                NormalizedUserName = user.UserName.ToUpper(),
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = user.PasswordHash
            };
            return View(oldUser);
        }
        [HttpPost]
        public async Task<IActionResult> Update(IdentityUser user)
        {
            try
            {
                var oldSettings = await _usersRepository.GetByIdAsync(user.Id);
                user.PasswordHash = oldSettings.PasswordHash;
                user.UserName = user.Email;
                _usersRepository.Update(user);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"User Update error: {ex}");
                return View(user);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            _usersRepository.Delete(user);
            return RedirectToAction("Show");
        }
    }
}
