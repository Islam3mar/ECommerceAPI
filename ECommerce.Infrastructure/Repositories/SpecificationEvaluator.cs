using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>
            (IQueryable<TEntity> inputQuery, ISpecefications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            #region Include
            if (specifications.IncludeExpressions.Count > 0)
            {
                // DbContext.Set<T>().Include(Brand).Include(Type)
                query = specifications.IncludeExpressions.Aggregate(query, (current, expression) => current.Include(expression));
            }
            #endregion
            #region Where

            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);
            #endregion
            #region Order By
            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            //------------------------------------------------------------------

            if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);
            #endregion
            #region Pagination
            if (specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }
            #endregion

            return query;
        }
    }
}
