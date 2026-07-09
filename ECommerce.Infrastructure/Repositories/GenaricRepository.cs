using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class GenaricRepository<TEntity, TKey> : IGenaricRepositories<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext storeDbContext;
        private readonly DbSet<TEntity> _set;

        public GenaricRepository(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
            _set = storeDbContext.Set<TEntity>();
        }

        public void Add(TEntity entity) => _set.Add(entity);

        public void Update(TEntity entity) => _set.Update(entity);

        public void Delete(TEntity entity) => _set.Remove(entity);


        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) 
            => await _set.AsNoTracking().ToListAsync(ct);


        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default) 
            => await _set.FindAsync(id, ct);
        

      
    }
}
