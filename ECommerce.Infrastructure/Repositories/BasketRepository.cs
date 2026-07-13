using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Baskets;
using StackExchange.Redis;

namespace ECommerce.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        { 
            var json = JsonSerializer.Serialize(basket); // Convert From Object To String (JSON)
            
            var success = await _database.StringSetAsync(basket.Id, json, timeToLive ?? TimeSpan.FromDays(30));

            return success ? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string id, CancellationToken ct = default)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket?> GetBasketAsyc(string basketId, CancellationToken ct = default)
        {
            var basket = await _database.StringGetAsync(basketId);

            if(basket.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<CustomerBasket>(basket.ToString());
        }
    }
}
