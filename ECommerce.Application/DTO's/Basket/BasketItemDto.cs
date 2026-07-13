using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Application.DTO_s.Basket
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        [Range(1,double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1,99)]
        public int Quantity { get; set; }
    }
}
