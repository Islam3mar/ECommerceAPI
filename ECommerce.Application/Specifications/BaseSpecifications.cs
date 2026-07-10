using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;

namespace ECommerce.Application.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecefications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Include
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = [];
        public void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        } 
        #endregion

        #region Where
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteria = null)
        {
            Criteria = criteria;
        }
        #endregion

        #region Order By
        public Expression<Func<TEntity, object>>? OrderBy {  get; private set; }
        // set
        public void AddOrderBy(Expression<Func<TEntity, object>>? orderByExpression)
           => OrderBy = orderByExpression;
        
        public Expression<Func<TEntity, object>>? OrderByDesc {  get; private set; }
        // set
        public void AddOrderByDesc(Expression<Func<TEntity, object>>? orderByDescExpression)
           => OrderByDesc = orderByDescExpression;
        #endregion
    }
}
