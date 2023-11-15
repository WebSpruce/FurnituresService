using AutoMapper;
using FurnituresServiceApi.Dto;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FurnituresServiceApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            var ordersMap = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersMap);
        }

        // GET api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            var orderMap = _mapper.Map<OrderDto>(order);
            return Ok(orderMap);
        }

        // POST api/Orders
        [HttpPost]
        public ActionResult<bool> Insert([FromBody] OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);

            var isInserted = _orderService.Insert(order);
            if(isInserted)
            {
                _orderService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to insert Order.");
        }

        // PUT api/Orders>/5
        [HttpPut]
        public ActionResult<bool> Update([FromBody] OrderDto orderDto)
        {
            var orderToUpdate = _orderService.GetByIdAsync((int)orderDto.Id).Result;
            if(orderToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(orderDto, orderToUpdate);
            var isUpdated = _orderService.Update(orderToUpdate);
            if(isUpdated)
            {
                _orderService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to update Order.");
        }

        // DELETE api/Orders/5
        [HttpDelete]
        public ActionResult<bool> Delete(int id)
        {
            var orderToDelete = _orderService.GetByIdAsync(id).Result;
            if (orderToDelete == null)
            {
                return NotFound();
            }
            var isDeleted = _orderService.Delete(orderToDelete);
            if (isDeleted)
            {
                _orderService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to delete Order.");
        }
    }
}
