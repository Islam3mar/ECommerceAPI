using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Params;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Specifications
{
    public class ProductCountSpecefication : BaseSpecifications<Product,int>
    {
        public ProductCountSpecefication(ProductQueryParams queryParams)
            : base(p => (!queryParams.brandId.HasValue || p.BrandId == queryParams.brandId)
                     && (!queryParams.typeId.HasValue || p.TypeId == queryParams.typeId)
                     && (string.IsNullOrEmpty(queryParams.searchValue) || p.Name.ToLower().Contains(queryParams.searchValue.ToLower())))
        {
            
        }
    }
}
