using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities.Orders
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public string ShortName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string DeliveryTime { get; set; } = default!;
        public decimal Cost { get; set; } = default!;
    }
}
