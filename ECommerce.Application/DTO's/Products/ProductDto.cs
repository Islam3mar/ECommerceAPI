using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO_s.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }


        public string BrandName { get; set; } = null!;
        public string TypeName { get; set; } = null!;
    }
}
