using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        IGenaricRepositories<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
