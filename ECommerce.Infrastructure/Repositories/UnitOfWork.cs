using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork(StoreDbContext storeDbContext) : IUnitOfWork
    {
        public readonly Dictionary<string, object> _Repos = [];

        public IGenaricRepositories<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;

            if (_Repos.TryGetValue(typeName, out object oldRepos))   
                   return (IGenaricRepositories<TEntity, TKey>) oldRepos;

            var newRepos = new GenaricRepository<TEntity, TKey>(storeDbContext);

            _Repos[typeName] = newRepos;

            return newRepos;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await storeDbContext.SaveChangesAsync(ct);

    }
}
