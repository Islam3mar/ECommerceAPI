using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities.Products
{
    public class ProductsType : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
