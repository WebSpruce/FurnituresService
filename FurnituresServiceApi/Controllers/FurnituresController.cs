using AutoMapper;
using FurnituresServiceApi.Dto;
using FurnituresServiceModels.Models;
using FurnituresServiceService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FurnituresServiceApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FurnituresController : ControllerBase
	{
		private readonly IFurnitureService _furnitureService;
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;
		public FurnituresController(IFurnitureService furnitureService, ICategoryService categoryService, IMapper mapper) 
		{
			_furnitureService = furnitureService;
			_categoryService = categoryService;
			_mapper = mapper;
        }


		// GET: api/Furnitures
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Furniture>>> GetAll()
		{
			var furnitures = await _furnitureService.GetAllAsync();
            var furnituresDto = _mapper.Map<IEnumerable<FurnitureDto>>(furnitures);
			return Ok(furnituresDto);
        }

		// GET api/Furnitures/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Furniture>> GetByIdAsync(int id)
		{
            var furniture = _mapper.Map<FurnitureDto>(await _furnitureService.GetByIdAsync(id));
            return Ok(furniture);
        }
        [HttpGet("{categoryId}")]
		public async Task<ActionResult<IEnumerable<Furniture>>> GetByCategoryAsync(int categoryId)
		{
            var furniture = _mapper.Map<IEnumerable<FurnitureDto>>(await _furnitureService.GetByCategory(categoryId));
            return Ok(furniture);
        }

		// POST api/Furnitures
		[HttpPost]
        public ActionResult<bool> Insert([FromBody] FurnitureDto furniture)
		{
			var furnitureMap = _mapper.Map<Furniture>(furniture);

			var isInserted = _furnitureService.Insert(furnitureMap);
			if (isInserted)
			{
				_furnitureService.Save();
				return Ok(true);
			}
			return BadRequest("Failed to insert Furniture.");
		}

		//PUT api/Furnitures/5
		[HttpPut]
		public ActionResult<bool> Update([FromBody] FurnitureDto furniture)
		{
			var furnitureToUpdate = _furnitureService.GetByIdAsync(furniture.Id).Result;
			if (furnitureToUpdate == null)
			{
				return NotFound();
			}
			_mapper.Map(furniture, furnitureToUpdate);
			var isUpdated = _furnitureService.Update(furnitureToUpdate);
			if (isUpdated)
			{
				_furnitureService.Save();
				return Ok(true);
			}
			return BadRequest("Failed to update Furniture.");
		}

		// DELETE api/Furnitures/5
		[HttpDelete("{id}")]
		public ActionResult<bool> Delete(int id)
		{
            var furnitureToDelete = _furnitureService.GetByIdAsync(id).Result;
            if (furnitureToDelete == null)
            {
                return NotFound();
            }

            var isDeleted = _furnitureService.Delete(furnitureToDelete);

            if (isDeleted)
            {
                _furnitureService.Save();
                return Ok(true);
            }

            return BadRequest("Failed to delete furniture.");

        }
	}
}
