using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Specifications
{
    public class OrderSpecifications :BaseSpecifications<Order,Guid>
    {
        public OrderSpecifications(string email)
            :base(o => o.BuyerEmail == email) 
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);
        }

        public OrderSpecifications(Guid id , string email)
            :base(o => o.Id ==  id && o.BuyerEmail == email) 
        {

        }

    }
}
