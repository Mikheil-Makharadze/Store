using API.DTO;
using API.Response;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace API.Controllers
{
    /// <summary>
    /// Product CRUD
    /// </summary>
    public class ProductController : BaseApiController
    {
        private readonly IProductService productService;
        private readonly IProduct_CategoryService product_Category;
        private readonly IMapper mapper;

        /// <summary>
        /// Injecting Services
        /// </summary>
        /// <param name="_productService"></param>
        /// <param name="_product_Category"></param>
        /// <param name="_mapper"></param>
        public ProductController(IProductService _productService, IProduct_CategoryService _product_Category, IMapper _mapper)
        {
            productService = _productService;
            product_Category = _product_Category;
            mapper = _mapper;
        }

        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(("{id:int}"))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            var product = await productService.GetByIdAsync(id, n => n.Producer!);

            var productDTO = mapper.Map<ProductDTO>(product);

            var categories = await product_Category.GetByProductId(product.Id);
            productDTO.Categories = mapper.Map<List<CategoryDTO>>(categories);

            return Ok(new APIResponse { Result = productDTO });
        }

        /// <summary>
        /// Get All Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllProducts()
        {
            return Ok(new APIResponse { Result = await productService.GetAllAsync() });
        }

        /// <summary>
        /// Get All Products Name and Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("SelectorItems")]
        public async Task<ActionResult<APIResponse>> GetAllSelectorOption()
        {
            return Ok(new APIResponse { Result = mapper.Map<List<SelectorOption>>( await productService.GetAllAsync())});
        }

        [HttpGet("AllDetails")]
        public async Task<ActionResult<APIResponse>> GetAllProductsDetails(SearchString searchString)
        {
            var products = await productService.GetAllAsync(n => n.Producer!);

            if(searchString.Search != null)
            {
                var search = searchString.Search.Trim();
                products = products.Where(n => n.Name!.Contains(search) || n.Description!.Contains(search)).ToList();
            }                

            var productsDTO = mapper.Map<List<ProductDTO>>(products);

            foreach (var product in productsDTO)
            {
                var categories = await product_Category.GetByProductId(product.Id);

                product.Categories = mapper.Map<List<CategoryDTO>>(categories);
            }

            return Ok(new APIResponse { Result = productsDTO });
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            var products = await productService.GetAllAsync();

            Product product = mapper.Map<Product>(productCreateDTO);
            await productService.AddAsync(product);

            foreach (var CategorieId in productCreateDTO.CategoriesId)
            {
                await product_Category.CreateProduct_Category(product.Id, CategorieId);
            }

            return Ok(new APIResponse { Result = mapper.Map<ProductDTO>(product) });
        }

        /// <summary>
        /// Delete Product By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOrEmployee")]
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int Id)
        {
            return Ok(new APIResponse{ Result = await productService.DeleteAsync(Id) });
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(ProductUpdateDTO productDTO)
        {
            var Product = await productService.GetByIdAsync(productDTO.Id, n => n.Product_Categories);

            Product.Product_Categories.Clear();

            mapper.Map(productDTO, Product);
            await productService.UpdateAsync(Product);

            foreach (var CategorieId in productDTO.CategoriesId)
            {
                await product_Category.CreateProduct_Category(productDTO.Id, CategorieId);
            }

            return Ok(new APIResponse {  Result = Product.Id });
        }
    }
}
