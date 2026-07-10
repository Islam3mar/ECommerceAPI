using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Params
{
    public class ProductQueryParams
    {
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string? searchValue { get; set; }

        public ProductSortingOptions? sort { get; set; }
      

    }
}
