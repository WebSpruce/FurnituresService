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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesMap = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoriesMap);
        }

        // GET api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = _mapper.Map<CategoryDto>(await _categoryService.GetByIdAsync(id));
            return Ok(category);
        }

        // POST api/Categories
        [HttpPost]
        public ActionResult<bool> Insert([FromBody] CategoryDto categoryDto)
        {
            var categoryMap = _mapper.Map<Category>(categoryDto);

            var isInserted = _categoryService.Insert(categoryMap);
            if (isInserted)
            {
                _categoryService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to insert Furniture.");
        }

        // PUT api/Categories/5
        [HttpPut("{id}")]
        public ActionResult<bool> Update([FromBody] CategoryDto categoryDto)
        {
            var categoryToUpdate = _categoryService.GetByIdAsync(categoryDto.Id).Result;
            if (categoryToUpdate == null)
            {
                return NotFound();
            }
            _mapper.Map(categoryDto, categoryToUpdate);
            var isUpdated = _categoryService.Update(categoryToUpdate);
            if (isUpdated)
            {
                _categoryService.Save();
                return Ok(true);
            }
            return BadRequest("Failed to update Furniture.");
        }

        // DELETE api/Categories/5
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var cartToDelete = _categoryService.GetByIdAsync(id).Result;
            if (cartToDelete == null)
            {
                return NotFound();
            }

            var isDeleted = _categoryService.Delete(cartToDelete);

            if (isDeleted)
            {
                _categoryService.Save();
                return Ok(true);
            }

            return BadRequest("Failed to delete furniture.");
        }
    }
}
