using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ECommerce.Application.Contracts;
using ECommerce.Domain.Contracts;

namespace ECommerce.Application.Services
{
    public class CacheServices : ICacheServices
    {
        private readonly ICacheRepository cacheRepository;

        public CacheServices(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            return cacheRepository.GetAsync(cacheKey, ct);
        }

        public  Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return cacheRepository.SetAsync(cacheKey, json, timeToLive, ct);
        }
    }
}
