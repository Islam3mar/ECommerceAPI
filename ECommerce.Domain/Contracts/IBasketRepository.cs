using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities.Baskets;

namespace ECommerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsyc(string basketId, CancellationToken ct = default);
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket,TimeSpan? timeToLive = null ,CancellationToken ct = default);
        Task<bool> DeleteBasketAsync(string id , CancellationToken ct = default);
    }
}
