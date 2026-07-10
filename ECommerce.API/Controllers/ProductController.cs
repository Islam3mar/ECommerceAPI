using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Products;
using ECommerce.Application.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class ProductController(IProductServices productServices) : ApiBaseController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams,CancellationToken ct)
        {
            var products = await productServices.GetAllProductsAsync(queryParams, ct);
            var result = ToActionResult(products);

            return result;
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


    }
}
