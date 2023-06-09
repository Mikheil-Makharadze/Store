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
    public class ProductController : BaseApiController
    {
        private readonly IProductService productService;
        private readonly IProduct_CategoryService product_Category;
        private readonly IMapper mapper;
        public ProductController(IProductService _productService, IProduct_CategoryService _product_Category,IMapper _mapper)
        {
            productService = _productService;
            product_Category = _product_Category;
            mapper = _mapper;
        }

        [HttpGet(("{id:int}"))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
            {
                var product = await productService.GetByIdAsync(id, n => n.Producer);

                if (product == null)
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Product was not found" }
                    });
                }
                var productDTO = mapper.Map<ProductDTO>(product);

                var categories = await product_Category.GetByProductId(product.Id);                
                productDTO.Categories = mapper.Map<List<CategoryDTO>>(categories);


                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = productDTO
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
        public async Task<ActionResult<APIResponse>> GetAllProducts()
        {
            try
            {
                var products = await productService.GetAllAsync();

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = products
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
        public async Task<ActionResult<APIResponse>> GetAllProductsDetails()
        {
            try
            {
                var products = await productService.GetAllAsync(n => n.Producer);

                var productsDTO = mapper.Map<List<ProductDTO>>(products);

                foreach (var product in productsDTO)
                {
                    var categories = await product_Category.GetByProductId(product.Id);

                    product.Categories = mapper.Map<List<CategoryDTO>>(categories);
                }

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = productsDTO
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
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            try
            {
                var products = await productService.GetAllAsync();
                if (products.Any(n => n.Name.ToLower() == productCreateDTO.Name.ToLower()))
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Product with same name already exists" }
                    });
                }

                Product product = mapper.Map<Product>(productCreateDTO);
                await productService.AddAsync(product);

                foreach (var CategorieId in productCreateDTO.CategoriesId)
                {
                    await product_Category.CreateProduct_Category(product.Id, CategorieId);
                }

                return Created("Products", new APIResponse
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Result = mapper.Map<ProductDTO>(product)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                var product = await productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Product was not found" }
                    });
                }

                await productService.DeleteAsync(product);
                await product_Category.removeByProductId(id);

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
        public async Task<ActionResult<APIResponse>> UpdateProduct(ProductUpdateDTO productDTO)
        {
            try
            {
                var Product = await productService.GetByIdAsync(productDTO.Id);
                if (Product == null)
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Product was not found" }
                    });
                }

                if(Product.Name != productDTO.Name)
                {
                    var products = await productService.GetAllAsync();
                    if (products.Any(n => n.Name.ToLower() == productDTO.Name.ToLower()))
                    {
                        return BadRequest(new APIResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            IsSuccess = false,
                            ErrorMessages = new List<string> { "Product with same name already exists" }
                        });
                    }
                }

                mapper.Map(productDTO, Product);
                await productService.UpdateAsync(Product);

                await product_Category.removeByProductId(productDTO.Id);
                foreach (var CategorieId in productDTO.CategoriesId)
                {
                    await product_Category.CreateProduct_Category(productDTO.Id, CategorieId);
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
