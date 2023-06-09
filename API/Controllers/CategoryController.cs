using API.DTO;
using API.Response;
using AutoMapper;
using Core.Entities;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            try
            {
                var category = await categoryService.GetByIdAsync(id, n => n.Product_Categories);

                if (category == null)
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Category was not found" }
                    });
                }

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = mapper.Map<CategoryDTO>(category)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetAllCategories()
        {
            try
            {
                var categories = await categoryService.GetAllAsync();

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = mapper.Map<List<CategoryDTO>>(categories)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }
        [HttpGet("AllDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetAllCategoriesDetails()
        {
            try
            {
                var categories = await categoryService.GetAllAsync();

                var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);

                foreach (var category in categoriesDTO)
                {
                    var products = await product_Category.GetByCategoryId(category.Id);

                    category.Products = mapper.Map<List<ProductDTO>>(products);
                }


                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = mapper.Map<List<CategoryDTO>>(categoriesDTO)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO createCategoryDTO)
        {
            try
            {
                var categories = await categoryService.GetAllAsync();
                if (categories.Any(n => n.Name.ToLower() == createCategoryDTO.Name.ToLower()))
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

                return Created("Categories", new APIResponse
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Result = mapper.Map<CategoryDTO>(category)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }


        [Authorize(Policy = "AdminOrEmployee")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            try
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

                await categoryService.DeleteAsync(category);

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.NoContent,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateCategory([FromBody] CategoryUpdateDTO categoryDTO)
        {
            try
            {
                var existingCategory = await categoryService.GetByIdAsync(categoryDTO.Id);
                if (existingCategory == null)
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Category was not found" }
                    });
                }

                var updatedCategory = mapper.Map<Category>(categoryDTO);
                await categoryService.UpdateAsync(updatedCategory);

                await product_Category.RemoveByCategoryId(categoryDTO.Id);
                if (categoryDTO.ProductsId == null)
                {
                    foreach (var productId in categoryDTO.ProductsId)
                    {
                        await product_Category.CreateProduct_Category(productId, categoryDTO.Id);
                    }
                }

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.NoContent,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }
    }
}

