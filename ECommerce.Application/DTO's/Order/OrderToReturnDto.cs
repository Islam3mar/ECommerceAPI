using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ECommerce.Application.DTO_s.Identity;
using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.DTO_s.Order
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }

        public string BuyerEmail { get; set; } = default!;
        public DateTime OrderDate { get; set; } 

        public ICollection<OrderItemDto> Items { get; set; } = [];
        public AddressDto ShippingAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public string status { get; set; } = default!;

        public decimal SubTotal { get; set; }
        public decimal DeliveryCost { get; set; }

        public decimal Total {  get; set; }
    }
}
