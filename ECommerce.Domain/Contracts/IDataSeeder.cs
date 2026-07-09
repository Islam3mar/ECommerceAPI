using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Contracts
{
    public interface IDataSeeder
    {
        Task SeedAsync(CancellationToken ct = default);
    }
}
