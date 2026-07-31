using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.DTO_s.Products
{
    public class UpdateProductDto
    {
        [MaxLength(50)]
        public string? Name { get; set; } 

        [MaxLength(500)]
        public string? Description { get; set; }

     
        public decimal? Price { get; set; }


        // Optional on purpose: admin might only change the name/price without re-uploading an image.
        // Null/empty => keep the existing PictureUrl as is.
        public IFormFile? Image { get; set; }
    }
}