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


        #region Payment

        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
        #endregion
    }
}
