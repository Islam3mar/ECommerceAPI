using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Specifications
{
    public class ProductWithIdsSpecifications:BaseSpecifications<Product,int>
    {
        public ProductWithIdsSpecifications(IEnumerable<int> Ids)
            :base(p => Ids.Contains(p.Id))
        {
            
        }
    }
}
