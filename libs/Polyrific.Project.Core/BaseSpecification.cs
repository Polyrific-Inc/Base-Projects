using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Polyrific.Project.Core
{
    /// <summary>
    /// The specification base class (obsolete). Please use <see cref="Specification"/> instead which is not an abstract class.
    /// </summary>
    /// <typeparam name="TEntity">The entity class used in the specification</typeparam>
    [Obsolete("Please use \"Specification\" instead which is not an abstract class.", false)]
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy, bool orderDesc = false, Expression<Func<TEntity, TEntity>> selector = null)
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

            Selector = selector;
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
        /// List of "order by - ascending" criteria
        /// </summary>
        public List<Expression<Func<TEntity, object>>> OrderByList { get; }

        /// <summary>
        /// Order by descending expresion of the specification
        /// </summary>
        public Expression<Func<TEntity, object>> OrderByDescending { get; }

        /// <summary>
        /// List of "order by - descending" criteria
        /// </summary>
        public List<Expression<Func<TEntity, object>>> OrderByDescendingList { get; }

        /// <summary>
        /// Includes expression list
        /// </summary>
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

        /// <summary>
        /// Includes in string format
        /// </summary>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// The number of items to skip
        /// </summary>
        public int? Skip { get; private set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        public int? Take { get; private set; }

        /// <summary>
        /// Selector of the specification
        /// </summary>
        public Expression<Func<TEntity, TEntity>> Selector { get; }

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
