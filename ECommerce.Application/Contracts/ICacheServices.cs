using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface ICacheServices
    {
        Task<string?> GetAsync(string cacheKey, CancellationToken ct = default);
        Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive, CancellationToken ct = default);
    }
}
