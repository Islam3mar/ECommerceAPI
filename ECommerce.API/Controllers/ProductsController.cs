using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Product> GetProduct()
        {
            var product = new Product
            {
                Id = 10,
                Name = "Laptop"
            };
            return Ok(product);
        }

        [HttpGet("Soly/{id}")]
        public ActionResult<Product> GetSoly(int id)
        {
            var product = new Product
            {
                Id = id,
                Name = "soly"
            };
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = new Product
            {
                Id = id,
                Name = "Keyboard"
            };
            return Ok(product);
        }
    }
}
