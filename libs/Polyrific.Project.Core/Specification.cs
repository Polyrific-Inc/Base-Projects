using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Polyrific.Project.Core
{
    /// <summary>
    /// The specification base class.
    /// </summary>
    /// <typeparam name="TEntity">The entity class used in the specification</typeparam>
    public class Specification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        public Specification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Specification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy, bool orderDesc = false)
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

        /// <summary>
        /// Criteria of the specification
        /// </summary>
        public Expression<Func<TEntity, bool>> Criteria { get; }

        /// <summary>
        /// Order by expression of the specification
        /// </summary>
        public Expression<Func<TEntity, object>> OrderBy { get; }

        /// <summary>
        /// Order by descending expresion of the specification
        /// </summary>
        public Expression<Func<TEntity, object>> OrderByDescending { get; }

        /// <summary>
        /// Includes expression list
        /// </summary>
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

        /// <summary>
        /// Includes in string format
        /// </summary>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// Adds include expression to the specification
        /// </summary>
        /// <param name="includeExpression">The include expression to be added</param>
        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Adds include in string format to the specification
        /// </summary>
        /// <param name="includeString">The include in string format</param>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
