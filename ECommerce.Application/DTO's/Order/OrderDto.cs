using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ECommerce.Application.DTO_s.Identity;

namespace ECommerce.Application.DTO_s.Order
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; } = default!;

        [Required]
        public int DeliveryMethodId { get; set; }

        [Required]
        public AddressDto ShippingAddress { get; set; } = default!;
    }
}
