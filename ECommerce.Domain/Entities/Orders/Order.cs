using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; } = default!;
        public DateTime OrderDate {  get; set; } = DateTime.Now;

        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderAddress ShippingAddress { get; set; } = default!;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;

        [ForeignKey("DeliveryMethod")]
        public int DeliveryMethodId { get; set; }

        public OrderStatus status { get; set; } = OrderStatus.Pending;

        public string? PaymentIntentId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Cost ?? 0);
    }
}
