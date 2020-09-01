using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Project.Data
{
    /// <inheritdoc/>
    public class DataRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// The Db Context of the repository
        /// </summary>
        protected readonly DbContext Db;

        /// <summary>
        /// The DataRepository constructor
        /// </summary>
        /// <param name="db">The db context that can be injected</param>
        public DataRepository(DbContext db)
        {
            Db = db;
        }

        /// <inheritdoc/>
        public virtual async Task<int> CountBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return await secondaryResult.CountAsync(spec.Criteria, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<int> Create(TEntity entity, string userEmail = null, 
            string userDisplayName = null, bool fillUpdatedInfo = false, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Created = DateTime.UtcNow;
            entity.CreatedBy = GetModifier(userEmail, userDisplayName);

            if (fillUpdatedInfo)
            {
                entity.Updated = entity.Created;
                entity.UpdatedBy = entity.CreatedBy;
            }

            Db.Set<TEntity>().Add(entity);
            await Db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        /// <inheritdoc/>
        public virtual async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbSet = Db.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await Db.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> GetById(int id, CancellationToken cancellationToken = default)
        {
            return Db.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken).AsTask();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // add order by to query
            if (spec.OrderBy != null)
            {
                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);
            } 
            else if (spec.OrderByList.Count > 0)
            {
                IOrderedQueryable<TEntity> orderedResult = null;
                foreach (var orderBy in spec.OrderByList)
                {
                    if (orderedResult == null)
                        orderedResult = secondaryResult.OrderBy(orderBy);

                    orderedResult = orderedResult.ThenBy(orderBy);
                }

                secondaryResult = orderedResult;
            }
            else if (spec.OrderByDescendingList.Count > 0)
            {
                IOrderedQueryable<TEntity> orderedResult = null;
                foreach (var orderByDescending in spec.OrderByDescendingList)
                {
                    if (orderedResult == null)
                        orderedResult = secondaryResult.OrderByDescending(orderByDescending);

                    orderedResult = orderedResult.ThenByDescending(orderByDescending);
                }

                secondaryResult = orderedResult;
            }

            // apply criteria and paging values
            secondaryResult = secondaryResult.Where(spec.Criteria);

            if (spec.Selector != null)
                secondaryResult = secondaryResult.Select(spec.Selector);

            // apply paging values
            if (spec.Skip.HasValue && spec.Take.HasValue)
                secondaryResult = secondaryResult.Skip(spec.Skip.Value).Take(spec.Take.Value);

            // return the result of the query
            return await secondaryResult.ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // add order by to query
            if (spec.OrderBy != null)
            {
                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);
            }
            else if (spec.OrderByList.Count > 0)
            {
                IOrderedQueryable<TEntity> orderedResult = null;
                foreach (var orderBy in spec.OrderByList)
                {
                    if (orderedResult == null)
                        orderedResult = secondaryResult.OrderBy(orderBy);

                    orderedResult = orderedResult.ThenBy(orderBy);
                }

                secondaryResult = orderedResult;
            }
            else if (spec.OrderByDescendingList.Count > 0)
            {
                IOrderedQueryable<TEntity> orderedResult = null;
                foreach (var orderByDescending in spec.OrderByDescendingList)
                {
                    if (orderedResult == null)
                        orderedResult = secondaryResult.OrderByDescending(orderByDescending);

                    orderedResult = orderedResult.ThenByDescending(orderByDescending);
                }

                secondaryResult = orderedResult;
            }

            if (spec.Selector != null)
                secondaryResult = secondaryResult.Select(spec.Selector);

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .FirstOrDefaultAsync(spec.Criteria, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task Update(TEntity entity, string userEmail = null, string userDisplayName = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Updated = DateTime.UtcNow;
            entity.UpdatedBy = GetModifier(userEmail, userDisplayName);
            entity.ConcurrencyStamp = Guid.NewGuid().ToString();

            Db.Entry(entity).State = EntityState.Modified;
            await Db.SaveChangesAsync(cancellationToken);
        }

        private string GetModifier(string userEmail, string userDisplayName)
        {
            if (string.IsNullOrEmpty(userEmail) && string.IsNullOrEmpty(userDisplayName))
                return "";

            if (string.IsNullOrEmpty(userDisplayName))
                return $"{userEmail} | {userEmail}";

            return $"{userDisplayName} | {userEmail}";
        }
    }
}
