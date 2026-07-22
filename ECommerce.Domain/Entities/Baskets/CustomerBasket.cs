using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities.Baskets
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public ICollection<BasketItem> Items { get; set; } = [];

        #region Payment

        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
        #endregion
    }
}
