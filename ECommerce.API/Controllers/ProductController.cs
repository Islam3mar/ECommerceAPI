using ECommerce.API.Attribute;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Products;
using ECommerce.Application.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class ProductController(IProductServices productServices) : ApiBaseController
    {
        [HttpGet]
        [Authorize]
        [RedisCache(1500)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams,CancellationToken ct)
        {
            var products = await productServices.GetAllProductsAsync(queryParams, ct);
            return ToActionResult<PaginatedResult<ProductDto>>(products);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
        {
            var brands = await productServices.GetAllProductBrandsAsync(ct);
            var result = ToActionResult(brands);

            return result;
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
        {
            var types = await productServices.GetAllProductTypesAsync(ct);
            var result = ToActionResult(types);

            return result;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken ct)
        {
            var product = await productServices.GetByIdAsync(id, ct);
            var result = ToActionResult(product);

            return result;
        }


        //--------------------------------------------------------------------------------------------------------


        // Admin only — [FromForm] because the request must be multipart/form-data to carry the image file.
        [HttpPost("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto createDto, CancellationToken ct)
        {
            var result = await productServices.CreateAsync(createDto, ct);
            return ToActionResult(result);
        }

        [HttpPut("{id}/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromForm] UpdateProductDto updateDto, CancellationToken ct)
        {
            var result = await productServices.UpdateAsync(id, updateDto, ct);
            return ToActionResult(result);
        }

        [HttpDelete("{id}/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteProduct(int id, CancellationToken ct)
        {
            var result = await productServices.DeleteAsync(id, ct);
            return ToActionResult(result);
        }


    }
}
