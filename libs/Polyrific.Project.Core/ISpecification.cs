using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Polyrific.Project.Core
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Search filter criteria
        /// </summary>
        Expression<Func<TEntity, bool>> Criteria { get; }

        /// <summary>
        /// Order by ascending criteria
        /// </summary>
        Expression<Func<TEntity, object>> OrderBy { get; }

        /// <summary>
        /// Order by descending criteria
        /// </summary>
        Expression<Func<TEntity, object>> OrderByDescending { get; }

        /// <summary>
        /// Related entities to be included in the query
        /// </summary>
        List<Expression<Func<TEntity, object>>> Includes { get; }

        /// <summary>
        /// Related entities to be included in the query
        /// </summary>
        List<string> IncludeStrings { get; }
    }
}
