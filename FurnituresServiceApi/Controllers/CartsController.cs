using AutoMapper;
using FurnituresServiceApi.Dto;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using FurnituresServiceService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurnituresServiceApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IFurnitureService _furnitureService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public CartsController(ICartService cartService, IFurnitureService furnitureService, IUserService userService, IMapper mapper)
        {
            _cartService = cartService;
            _furnitureService = furnitureService;
            _userService = userService;
            _mapper = mapper;
        }

        // GET api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetByUserId(string id)
        {
            var cart = _mapper.Map<CartDto>(await _cartService.GetByUserId(id));
            return Ok(cart);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Furniture>>> GetAddedFurnituresByUserId(string id)
        {
            var cart = _mapper.Map<IEnumerable<FurnitureDto>>(_cartService.GetAddedFurnitures(id));
            return Ok(cart);
        }
        [HttpPost("{id}")]
        public ActionResult<bool> AddFurnitureToCartByUserId(string id, int furnitureId)
        {
            var furniture = _furnitureService.GetByIdAsync(furnitureId).Result;
            if(furniture == null)
            {
                return NotFound();
            }
            var isInserted = _cartService.InsertFurnitureToCart(id, furniture).Result;
            if(isInserted != null)
            {
                _cartService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to insert Furniture to Cart.");
        }
        [HttpDelete("{id}")]
        public ActionResult<bool> RemoveFurnitureFromCartByUserId(string id, int furnitureId)
        {
            var user = _userService.GetByIdAsync(id).Result;
            var furniture = _furnitureService.GetByIdAsync(furnitureId).Result;
            if(furniture == null || user == null)
            {
                return NotFound();
            }
            var isRemoved = _cartService.RemoveFurnitureToCart(user, furniture).Result;
            if(isRemoved != null)
            {
                _cartService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to remove Furniture from the Cart.");
        }
        [HttpDelete]
        public ActionResult<bool> RemoveAllFurnituresFromCartByUserId(string id)
        {
            var user = _userService.GetByIdAsync(id).Result;
            if(user == null)
            {
                return NotFound();
            }
            var isRemoved = _cartService.RemoveAllFurnituresFromCart(user).Result;
            if(isRemoved != null)
            {
                _cartService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to remove all Furnitures from the Cart.");
        }
    }
}
