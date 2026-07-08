using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities.Products
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public ProductsBrand Brand { get; set; } = null!;

        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        public ProductsType Type { get; set; } = null!;

        [ForeignKey("Type")]
        public int TypeId { get; set; }
    }
}
