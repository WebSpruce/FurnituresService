using FurnituresServiceDatabase.Data;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FurnituresService.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _usersService;
        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserService usersService)
        {
            _context = context;
            _userManager = userManager;
			_usersService = usersService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var users = await _usersService.GetAllAsync();
            return View("Show", users);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            return View("Insert");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Insert(IdentityUser user)
        {
            try
            {
                Trace.WriteLine($"new user: {user.Email} - {user.PasswordHash}");
                user.UserName = user.Email;
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = _usersService.Insert(user).Result;
                    if (result)
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(IdentityUser user)
        {
            try
            {
                var oldSettings = await _usersService.GetByIdAsync(user.Id);
                user.PasswordHash = oldSettings.PasswordHash;
                user.UserName = user.Email;
				_usersService.Update(user);
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"User Update error: {ex}");
                return View(user);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

			_usersService.Delete(user);
            return RedirectToAction("Show");
        }
    }
}
