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
        /// <summary>
        /// Initiate the search specification
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        public Specification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Initiate the search specification
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <param name="orderBy">The fields that will be used to sort the result</param>
        /// <param name="orderDesc">Whether to sort the result descendingly based on the defined order fields</param>
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
        /// Initiate the search specification
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <param name="orderBy">The fields that will be used to sort the result</param>
        /// <param name="orderDesc">Whether to sort the result descendingly based on the defined order fields</param>
        /// <param name="skip">The number of items to skip</param>
        /// <param name="take">The maximum number of items to return</param>
        public Specification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy, bool orderDesc, int skip, int take)
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

            Skip = skip;
            Take = take;
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
        /// The number of items to skip
        /// </summary>
        public int? Skip { get; private set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        public int? Take { get; private set; }

        /// <summary>
        /// Adds include expression to the specification
        /// </summary>
        /// <param name="includeExpression">The include expression to be added</param>
        public virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Adds include in string format to the specification
        /// </summary>
        /// <param name="includeString">The include in string format</param>
        public virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
