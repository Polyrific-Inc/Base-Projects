using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Polyrific.Project.Core
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy, bool orderDesc = false)
        {
            Criteria = criteria;

            if (orderDesc)
            {
                OrderByDescending = orderBy;
            }
            else
            {
                OrderBy = orderBy;
            }
        }

        public Expression<Func<TEntity, bool>> Criteria { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
