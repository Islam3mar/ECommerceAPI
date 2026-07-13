using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Baskets;

namespace ECommerce.Application.DTO_s.Basket
{
    public class BasketDto
    {
        public string Id { get; set; }
        public ICollection<BasketItemDto> Items { get; set; } = [];
    }
}
