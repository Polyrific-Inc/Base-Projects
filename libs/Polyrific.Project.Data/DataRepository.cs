using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Project.Data
{
    /// <summary>
    /// The base class of the repository
    /// </summary>
    /// <typeparam name="TEntity">Entity to be used in the repository</typeparam>
    public class DataRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// The Db Context of the repository
        /// </summary>
        protected readonly ApplicationDbContext Db;

        /// <summary>
        /// The DataRepository constructor
        /// </summary>
        /// <param name="db">The db context that can be injected</param>
        public DataRepository(ApplicationDbContext db)
        {
            Db = db;
        }

        /// <summary>
        /// Do count operation to the db
        /// </summary>
        /// <param name="spec">The specification of the count</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The count result</returns>
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

        /// <summary>
        /// Create an entry to the db
        /// </summary>
        /// <param name="entity">The entity to be created</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The new Id of the created entity</returns>
        public virtual async Task<int> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Created = DateTime.UtcNow;
            Db.Set<TEntity>().Add(entity);
            await Db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        /// <summary>
        /// Delete an entry in the db
        /// </summary>
        /// <param name="id">Id of the entity to be deleted</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        public virtual async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbSet = Db.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await Db.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Get an entry in the db using the entity id
        /// </summary>
        /// <param name="id">Id of the entity to be retrieved</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The entity object</returns>
        public virtual Task<TEntity> GetById(int id, CancellationToken cancellationToken = default)
        {
            return Db.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken).AsTask();
        }

        /// <summary>
        /// Get entries in the db using a specification
        /// </summary>
        /// <param name="spec">The specification of the get operation</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The list of the entity</returns>
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

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .Where(spec.Criteria)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get a single entry in the db using a specification
        /// </summary>
        /// <param name="spec">The specification</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>An entity object</returns>
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

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .FirstOrDefaultAsync(spec.Criteria, cancellationToken);
        }

        /// <summary>
        /// Update an entry in the db
        /// </summary>
        /// <param name="entity">The entity to be updated</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Updated = DateTime.UtcNow;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            Db.Entry(entity).State = EntityState.Modified;
            await Db.SaveChangesAsync(cancellationToken);
        }
    }
}
