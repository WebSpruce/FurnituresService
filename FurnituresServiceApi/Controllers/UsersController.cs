using AutoMapper;
using FurnituresServiceApi.Dto;
using FurnituresServiceService.Interfaces;
using FurnituresServiceService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FurnituresServiceApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public readonly IPasswordHasher<IdentityUser> _passwordHasher;
        public UsersController(IUserService userService, IMapper mapper, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userService = userService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        // GET api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityUser>> Get(string id)
        {
            var userDto = _mapper.Map<UserDto>(await _userService.GetByIdAsync(id));
            return Ok(userDto);
        }

        // POST api/Users
        [HttpPost]
        public ActionResult<bool> Insert([FromQuery] string password, [FromBody] UserDto userDto)
        {
            var newUser = new IdentityUser { Email = userDto.Email, UserName = userDto.Email, NormalizedEmail = userDto.Email.ToUpper(), NormalizedUserName = userDto.Email.ToUpper(), LockoutEnabled = true, EmailConfirmed = userDto.EmailConfirmed, PhoneNumber = userDto.PhoneNumber };
            var hashedPassword = _passwordHasher.HashPassword(newUser, password);
            newUser.PasswordHash = hashedPassword;
            var isInserted = _userService.Insert(newUser).Result;
            if (isInserted)
            {
                _userService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to insert User.");
        }

        //PUT api/Users/5
        [HttpPut]
        public ActionResult<bool> Update([FromQuery] string userId,[FromBody] UserDto userDto)
        {
            var userToUpdate = _userService.GetByIdAsync(userId).Result;
            if (userToUpdate == null)
            {
                return NotFound();
            }
            _mapper.Map(userDto, userToUpdate);
            var isUpdated = _userService.Update(userToUpdate);
            if (isUpdated)
            {
                _userService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to update Furniture.");
        }
        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(string id)
        {
            var userToDelete = _userService.GetByIdAsync(id).Result;
            if (userToDelete == null)
            {
                return NotFound();
            }

            var isDeleted = _userService.Delete(userToDelete);

            if (isDeleted)
            {
                _userService.Save();
                return Ok(true);
            }

            return BadRequest("Failed to delete furniture.");

        }
    }
}
