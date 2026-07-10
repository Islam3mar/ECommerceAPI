using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Contracts
{
    public interface ISpecefications<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        Expression<Func<TEntity, bool>>? Criteria { get; }
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; }


    }
}
