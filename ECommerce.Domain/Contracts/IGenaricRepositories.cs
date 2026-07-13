using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Contracts
{
    public interface IGenaricRepositories<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);


        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);

        Task<TEntity?> GetByIdWithSpecificationsAsync(ISpecefications<TEntity, TKey> specifications, CancellationToken ct = default);

        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);

        // Gets all entities with the specified specifications
        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationsAsync(ISpecefications<TEntity, TKey> specifications, CancellationToken ct = default);

        Task<int> GetProductCountWithSpecificationsAsync(ISpecefications<TEntity, TKey> specifications, CancellationToken ct = default);

    }
}
