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
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);
    }
}
