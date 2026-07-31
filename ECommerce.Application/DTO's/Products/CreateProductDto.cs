using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.DTO_s.Products
{
    public class CreateProductDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(500)]
        public string Description { get; set; } = default!;

        [Required, Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int TypeId { get; set; }

        // Comes from multipart/form-data — the ProductController endpoint uses [FromForm] to bind it.
        [Required]
        public IFormFile Image { get; set; } = default!;
    }
}
