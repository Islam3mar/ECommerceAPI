using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity<int>
    {
        public ProductItemOrdered product { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
