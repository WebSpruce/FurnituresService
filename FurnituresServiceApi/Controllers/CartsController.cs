using AutoMapper;
using FurnituresServiceApi.Dto;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using FurnituresServiceService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FurnituresServiceApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        public CartsController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        // GET api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetByUserId(string id)
        {
            var cart = _mapper.Map<CartDto>(await _cartService.GetByUserId(id));
            return Ok(cart);
        }
    }
}
