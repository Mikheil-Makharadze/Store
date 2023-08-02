using API.DTO;
using API.Response;
using AutoMapper;
using Core.Entities;
using Core.Entities.Order;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService categoryService;
        private readonly IProduct_CategoryService product_Category;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService _categoryService, IProduct_CategoryService _product_Category, IMapper _mapper)
        {
            categoryService = _categoryService;
            product_Category = _product_Category;
            mapper = _mapper;
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            var category = await categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Category was not found" }
                });
            }

            var categoriesDTO = mapper.Map<CategoryDTO>(category);

            var products = await product_Category.GetByCategoryId(category.Id);

            categoriesDTO.Products = mapper.Map<List<ProductDTO>>(products);

            return Ok(new APIResponse { Result = categoriesDTO });
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllCategories()
        {
            var categories = await categoryService.GetAllAsync();

            return Ok(new APIResponse { Result = mapper.Map<List<CategoryDTO>>(categories) });
        }

        [HttpGet("AllDetails")]
        public async Task<ActionResult<APIResponse>> GetAllCategoriesDetails(SearchString searchString)
        {
            var categories = await categoryService.GetAllAsync();

            if (searchString.Search != null)
            {
                var search = searchString.Search.Trim();
                categories = categories.Where(n => n.Name!.Contains(search)).ToList();
            }

            var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);

            foreach (var category in categoriesDTO)
            {
                var products = await product_Category.GetByCategoryId(category.Id);

                category.Products = mapper.Map<List<ProductDTO>>(products);
            }

            return Ok(new APIResponse { Result = categoriesDTO });
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO createCategoryDTO)
        {
            var categories = await categoryService.GetAllAsync();
            if (categories.Any(n => n.Name!.ToLower() == createCategoryDTO.Name.ToLower()))
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Category with same name already exists" }
                });
            }

            var category = mapper.Map<Category>(createCategoryDTO);
            await categoryService.AddAsync(category);

            if (createCategoryDTO.ProductsId != null)
            {
                foreach (var productId in createCategoryDTO.ProductsId)
                {
                    await product_Category.CreateProduct_Category(productId, category.Id);
                }
            }   

            return Ok(new APIResponse { Result = mapper.Map<CategoryDTO>(category) });
        }


        [Authorize(Policy = "AdminOrEmployee")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            await categoryService.DeleteAsync(id);

            return Ok(new APIResponse { Result = id });
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateCategory([FromBody] CategoryUpdateDTO categoryDTO)
        {
            //await categoryService.UpdateAsync(mapper.Map<Category>(categoryDTO));

            //await product_Category.RemoveByCategoryId(categoryDTO.Id);

            //foreach (var productId in categoryDTO.ProductsId!)
            //{
            //    await product_Category.CreateProduct_Category(productId, categoryDTO.Id);
            //}

            return Ok(new APIResponse { Result = categoryDTO.Id });
        }
    }
}

