using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO_s.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
