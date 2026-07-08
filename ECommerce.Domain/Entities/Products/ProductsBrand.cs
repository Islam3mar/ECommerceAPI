using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities.Products
{
    public class ProductsBrand : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}
